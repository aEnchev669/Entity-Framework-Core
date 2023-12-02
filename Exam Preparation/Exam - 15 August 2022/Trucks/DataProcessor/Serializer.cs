namespace Trucks.DataProcessor
{
    using Data;
    using Newtonsoft.Json;
    using Trucks.DataProcessor.ExportDto;
    using Trucks.DataProcessor.ImportDto;
    using Trucks.Utilities;

    public class Serializer
    {
        private static XmlHelper xmlHelper;
        public static string ExportDespatchersWithTheirTrucks(TrucksContext context)
        {
            xmlHelper = new XmlHelper();

            var despatchers = context.Despatchers
                 .Where(d => d.Trucks.Any())
                 .ToArray()
                 .Select(d => new ExportDespatcherDto()
                 {
                     DespatcherName = d.Name,
                     Trucks = d.Trucks
                                .Select(t => new ExportTruckXmlDto()
                                {
                                    RegistrationNumber = t.RegistrationNumber,
                                    Make = (t.MakeType).ToString(),
                                })
                                .OrderBy(t => t.RegistrationNumber)
                                .ToArray(),
                     TrucksCount = d.Trucks.Count()
                 })
                 .OrderByDescending(d => d.Trucks.Count())
                 .ThenBy(d => d.DespatcherName)
                 .ToArray();

            return xmlHelper.Serialize(despatchers, "Despatchers");
        }

        public static string ExportClientsWithMostTrucks(TrucksContext context, int capacity)
        {

            var clients = context.Clients
                .Where(c => c.ClientsTrucks.Any(t => t.Truck.TankCapacity >= capacity))
                .ToArray()
                .Select(c => new
                {
                    Name = c.Name,
                    Trucks = c.ClientsTrucks
                                .Where(ct => ct.Truck.TankCapacity >= capacity)
                                .ToArray()
                                .Select(ct => new
                                {
                                    TruckRegistrationNumber = ct.Truck.RegistrationNumber,
                                    VinNumber = ct.Truck.VinNumber,
                                    TankCapacity = ct.Truck.TankCapacity,
                                    CargoCapacity = ct.Truck.CargoCapacity,
                                    CategoryType = (ct.Truck.CategoryType).ToString(),
                                    MakeType = ct.Truck.MakeType.ToString(),
                                })
                                .OrderBy(ct => ct.MakeType)
                                .ThenByDescending(ct => ct.CargoCapacity)
                                .ToArray()
                })
                .OrderByDescending(c => c.Trucks.Count())
                .ThenBy(c => c.Name)
                .Take(10)
                .ToArray();

            return JsonConvert.SerializeObject(clients, Formatting.Indented);


        }
    }
}
