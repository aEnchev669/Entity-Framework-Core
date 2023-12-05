namespace Medicines.DataProcessor
{
    using Castle.DynamicProxy.Generators;
    using Medicines.Data;
    using Medicines.Data.Models;
    using Medicines.Data.Models.Enums;
    using Medicines.DataProcessor.ImportDtos;
    using Medicines.Utilities;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid Data!";
        private const string SuccessfullyImportedPharmacy = "Successfully imported pharmacy - {0} with {1} medicines.";
        private const string SuccessfullyImportedPatient = "Successfully imported patient - {0} with {1} medicines.";

        public static string ImportPatients(MedicinesContext context, string jsonString)
        {
            ImportPatientDto[] patientDtos = JsonConvert.DeserializeObject<ImportPatientDto[]>(jsonString);

            ICollection<Patient> validPatiens = new HashSet<Patient>();
            StringBuilder sb = new StringBuilder();

            foreach (var patientDto in patientDtos)
            {
                if (!IsValid(patientDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }


                Patient patient = new Patient()
                {
                    FullName = patientDto.FullName,
                    AgeGroup = (AgeGroup)(patientDto.AgeGroup),
                    Gender = (Gender)(patientDto.Gender),
                };

            
                foreach (var medicine in patientDto.Medicines)
                {
                    if (patient.PatientsMedicines.Any(pm => pm.MedicineId == medicine))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                  

                    patient.PatientsMedicines.Add(new PatientMedicine()
                    {
                        MedicineId = medicine,
                    });
                }

                validPatiens.Add(patient);

                sb.AppendLine(string.Format(SuccessfullyImportedPatient, patient.FullName, patient.PatientsMedicines.Count));
            }

            context.AddRange(validPatiens);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPharmacies(MedicinesContext context, string xmlString)
        {
            XmlHelper xmlHelper = new XmlHelper();
            StringBuilder sb = new StringBuilder();

            ImportPharmaciesDto[] pharmacyDtos = xmlHelper
                .Deserialize<ImportPharmaciesDto[]>(xmlString, "Pharmacies");

            ICollection<Pharmacy> validPharmacies = new HashSet<Pharmacy>();

            foreach (var pharmacyDto in pharmacyDtos)
            {

                if (!IsValid(pharmacyDto)
                    || (pharmacyDto.IsNonStop != "false" && pharmacyDto.IsNonStop != "true"))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Pharmacy pharmacy = new Pharmacy()
                {
                    IsNonStop = bool.Parse(pharmacyDto.IsNonStop),
                    Name = pharmacyDto.Name,
                    PhoneNumber = pharmacyDto.PhoneNumber,
                };

                foreach (var medicine in pharmacyDto.Medicines)
                {

                    if (!IsValid(medicine) || medicine.Producer == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    DateTime productionDate = DateTime.ParseExact(medicine.ProductionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    DateTime expiryDate = DateTime.ParseExact(medicine.ExpiryDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                    if (pharmacy.Medicines.Any(m => m.Name == medicine.Name && m.Producer == medicine.Producer))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (productionDate >= expiryDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    pharmacy.Medicines.Add(new Medicine()
                    {
                        Category = (Category)medicine.Category,
                        Name = medicine.Name,
                        Price = medicine.Price,
                        ProductionDate = productionDate,
                        ExpiryDate = expiryDate,
                        Producer = medicine.Producer
                    });
                }

                validPharmacies.Add(pharmacy);
                sb.AppendLine(String.Format(SuccessfullyImportedPharmacy, pharmacy.Name, pharmacy.Medicines.Count));

            }

            context.AddRange(validPharmacies);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
