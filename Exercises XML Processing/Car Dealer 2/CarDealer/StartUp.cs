using AutoMapper;
using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            CarDealerContext context = new CarDealerContext();
            string inputXml = File.ReadAllText("../../../Datasets/cars.xm");

            Console.WriteLine(ImportParts(context, inputXml));
        }

        private static Mapper GetMapper()
        {
            var cfg = new MapperConfiguration(c => c.AddProfile<CarDealerProfile>());
            return new Mapper(cfg);
        }

        //Problem 09
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            IMapper mapper = GetMapper();


            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportSupplierDto[]), new XmlRootAttribute("Suppliers"));
            using var reader = new StringReader(inputXml);

            ImportSupplierDto[] importSupplierDtos = (ImportSupplierDto[])xmlSerializer.Deserialize(reader);

            Supplier[] suppliers = mapper.Map<Supplier[]>(importSupplierDtos);

            context.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Length}";
        }

        //Problem 10
        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            IMapper mapper = GetMapper();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportPartDto[]), new XmlRootAttribute("Parts"));
            using var reader = new StringReader(inputXml);
            ImportPartDto[] importPartDtos = (ImportPartDto[])xmlSerializer.Deserialize(reader);

            var suppliersID = context.Suppliers
                .Select(s => s.Id)
                .ToArray();

            Part[] parts = mapper.Map<Part[]>(importPartDtos
                .Where(ip => suppliersID.Contains(ip.SupplierId)));

            context.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Length}";

        }

        //Problem 11
        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            IMapper mapper = GetMapper();


            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportCarDto[]), new XmlRootAttribute("Cars"));
            using var reader = new StringReader(inputXml);
            ImportCarDto[] importCarDtos = (ImportCarDto[])xmlSerializer.Deserialize(reader);

         

            foreach (var carDto in importCarDtos)
            {
                Car car = mapper.Map<Car>(carDto);

                int[] carPartsId = carDto.Parts
                    .Distinct()
                    .ToArray();

                foreach (var part in carPartsId)
                {

                }
            }

            
        }
    }
}