namespace Cadastre.DataProcessor
{
    using Cadastre.Data;
    using Cadastre.Data.Enumerations;
    using Cadastre.Data.Models;
    using Cadastre.DataProcessor.ImportDtos;
    using Cadastre.Utilities;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Data.SqlTypes;
    using System.Globalization;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;

    public class Deserializer
    {
        private const string ErrorMessage =
            "Invalid Data!";
        private const string SuccessfullyImportedDistrict =
            "Successfully imported district - {0} with {1} properties.";
        private const string SuccessfullyImportedCitizen =
            "Succefully imported citizen - {0} {1} with {2} properties.";

        public static string ImportDistricts(CadastreContext context, string xmlDocument)
        {
            XmlHelper xmlHelper = new XmlHelper();
            StringBuilder sb = new StringBuilder();

            ImportDistrictDto[] districtDtos = xmlHelper
                .Deserialize<ImportDistrictDto[]>(xmlDocument, "Districts");

            ICollection<District> validDistricts = new HashSet<District>();

            foreach (var districtDto in districtDtos)
            {
                if (!IsValid(districtDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (context.Districts.Any(d => d.Name == districtDto.Name))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                District district = new District()
                {
                    Name = districtDto.Name,
                    PostalCode = districtDto.PostalCode,
                    Region = districtDto.Region,// validate in importDistrictDto
                };

                foreach (var propertyDto in districtDto.Property)
                {
                    if (!IsValid(propertyDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (context.Properties.Any(p => p.PropertyIdentifier == propertyDto.PropertyIdentifier))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    if (context.Districts.Any(p => p.Properties.Any( pt => pt.Address == propertyDto.Address)))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                   

                    if (district.Properties.Any(p => p.PropertyIdentifier == propertyDto.PropertyIdentifier))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    if (district.Properties.Any(p => p.Address == propertyDto.Address))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }


                    DateTime date = DateTime.ParseExact(propertyDto.DateOfAcquisition, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    Property property = new Property()
                    {
                        PropertyIdentifier = propertyDto.PropertyIdentifier,
                        Area = propertyDto.Area,
                        Details = propertyDto.Details,
                        Address = propertyDto.Address,
                        DateOfAcquisition = date,
                    };

                    district.Properties.Add(property);

                    

                }

                validDistricts.Add(district);

                sb.AppendLine($"Successfully imported district - {district.Name} with {district.Properties.Count} properties.");
            }
            context.AddRange(validDistricts);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportCitizens(CadastreContext context, string jsonDocument)
        {
            ImportCitizensDto[] citizenDtos = JsonConvert.DeserializeObject<ImportCitizensDto[]>(jsonDocument);

            ICollection<Citizen> validCitizens = new HashSet<Citizen>();

            StringBuilder sb = new StringBuilder();

            foreach (var citizenDto in citizenDtos)
            {
                if (!IsValid(citizenDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                MaritalStatus maritalStatus;

                if (!Enum.TryParse<MaritalStatus>(citizenDto.MaritalStatus, out maritalStatus))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                
                bool isValidDate = DateTime.TryParseExact(citizenDto.BirthDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date);

                if (isValidDate)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                //var validDate = DateTime.TryParse(citizenDto.BirthDate,  CultureInfo.InvariantCulture, out date);
                // DateTime date = DateTime.ParseExact(citizenDto.BirthDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                Citizen citizen = new Citizen()
                {
                    FirstName = citizenDto.FirstName,
                    LastName = citizenDto.LastName,
                    BirthDate = date,
                    MaritalStatus = maritalStatus,
                };

                foreach (var property in citizenDto.Properties)
                {
                    PropertyCitizen propertyCitizen = new PropertyCitizen()
                    {
                        PropertyId = property,
                        Citizen = citizen
                    };

                    citizen.PropertiesCitizens.Add(propertyCitizen);
                }

                validCitizens.Add(citizen);
                sb.AppendLine($"Succefully imported citizen - {citizen.FirstName} {citizen.LastName} with {citizen.PropertiesCitizens.Count} properties.");
            }

            context.AddRange(validCitizens);
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
