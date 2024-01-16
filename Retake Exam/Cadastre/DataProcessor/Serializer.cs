using Cadastre.Data;
using Cadastre.Data.Enumerations;
using Cadastre.DataProcessor.ExportDtos;
using Cadastre.Utilities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Globalization;
using System.Net;

namespace Cadastre.DataProcessor
{
    public class Serializer
    {
        public static string ExportPropertiesWithOwners(CadastreContext context)
        {
            DateTime date = DateTime.Parse("01/01/2000");
            var properties = context.Properties
                 .Where(p => p.DateOfAcquisition >= date)
                 .AsNoTracking()
                 .OrderByDescending(p => p.DateOfAcquisition)
                 .ThenBy(p => p.PropertyIdentifier)
                 .Select(p => new
                 {
                     PropertyIdentifier = p.PropertyIdentifier,
                     Area = p.Area,
                     Address = p.Address,
                     DateOfAcquisition = p.DateOfAcquisition.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                     Owners = p.PropertiesCitizens
                                 .Where(pc => pc.Property.PropertyIdentifier == p.PropertyIdentifier)
                                 .Select(pc => new
                                 {
                                     LastName = pc.Citizen.LastName,
                                     MaritalStatus = pc.Citizen.MaritalStatus.ToString(),
                                 })
                                 .OrderBy(pc => pc.LastName)
                                 .ToArray()
                 })
                 .ToArray();

            return JsonConvert.SerializeObject(properties, Formatting.Indented);
        }

        public static string ExportFilteredPropertiesWithDistrict(CadastreContext context)
        {
            XmlHelper xmlHelper = new XmlHelper();

            var properties = context.Properties
                 .Where(p => p.Area >= 100)
                 .OrderByDescending(p => p.Area)
                 .ThenBy(p => p.DateOfAcquisition)
                 .Select(p => new ExportPropertiesDto
                 {
                     PropertyIdentifier = p.PropertyIdentifier,
                     Area = p.Area,
                     DateOfAcquisition = p.DateOfAcquisition.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                     PostalCode = p.District.PostalCode,

                 })
                 .ToArray();

            return xmlHelper.Serialize(properties, "Properties");
        }
    }
}
