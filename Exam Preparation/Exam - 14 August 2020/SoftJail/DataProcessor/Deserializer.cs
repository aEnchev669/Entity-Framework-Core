namespace SoftJail.DataProcessor
{

    using Data;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.DataProcessor.ImportDto;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;

    public class Deserializer
    {
        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            ImportDepartmentsDto[] clientDtos = JsonConvert.DeserializeObject<ImportDepartmentsDto[]>(jsonString);

            ICollection<Department> departments = new HashSet<Department>();

            foreach (var clientDto in clientDtos)
            {

                if (clientDto.Name.Length < 3 || clientDto.Name.Length > 25)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }
                bool isValidData = false;

                var department = new Department()
                {
                    Name = clientDto.Name,
                };

                foreach (var cell in clientDto.Cells)
                {
                    if (cell.CellNumber > 1000 || cell.CellNumber < 1)
                    {
                        sb.AppendLine("Invalid Data");
                        isValidData = true;
                        break;
                    }

                    var validCell = new Cell()///may be wrong
                    {
                        CellNumber = cell.CellNumber,
                        HasWindow = cell.HasWindow,
                    };
                    department.Cells.Add(validCell);
                }

                if (isValidData)
                {
                    continue;
                }

                departments.Add(department);
                sb.AppendLine($"Imported {department.Name} with {department.Cells.Count} cells");
            }

            context.AddRange(departments);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        //public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    ImportPrisonerDto[] prisonerDtos = JsonConvert.DeserializeObject<ImportPrisonerDto[]>(jsonString);

        //    ICollection<Prisoner> prisoners = new HashSet<Prisoner>();

        //    foreach (var prisonerDto in prisonerDtos)
        //    {

        //        bool isValidIncarcerationDate = DateTime.TryParseExact(prisonerDto.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime validIncarcerationDate);
        //        bool isValidReleaseDate = DateTime.TryParseExact(prisonerDto.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime validReleaseDate);

        //        if (!IsValid(prisonerDto) || isValidIncarcerationDate)
        //        {
        //            sb.AppendLine("Invalid Data");
        //            continue;
        //        }
        //        var prisoner = new Prisoner()
        //        {
        //            FullName = prisonerDto.FullName,
        //            Nickname = prisonerDto.Nickname,
        //            Age = prisonerDto.Age,
        //            IncarcerationDate = validIncarcerationDate,
        //            ReleaseDate = isValidReleaseDate
        //            ? validReleaseDate
        //            : null,
        //            Bail = prisonerDto.Bail,
        //            CellId = prisonerDto.CellId
        //        };

        //        foreach (var mail in prisonerDto.Mails)
        //        {

        //        }
        //        prisoners.Add(prisoner);
        //    }
        //}

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
           
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
        }
    }
}