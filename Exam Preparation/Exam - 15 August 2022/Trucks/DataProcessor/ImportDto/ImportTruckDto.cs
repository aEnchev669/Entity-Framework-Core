﻿using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Trucks.Data.Models.Enums;

namespace Trucks.DataProcessor.ImportDto
{
    [XmlType("Truck")]
    public class ImportTruckDto
    {
        [MaxLength(8)]
        [MinLength(8)]
        [RegularExpression(@"[A-Z]{2}\d{4}[A-Z]{2}")]
        public string RegistrationNumber { get; set; }
        [MaxLength(17)]
        [MinLength(17)]
        public string VinNumber { get; set; }

        [Range(950, 1420)]
        public int TankCapacity { get; set; }
        [Range(5000,29000)]
        public int CargoCapacity { get; set; }

        [Range(0, 3)]
        public int CategoryType { get; set; }

        [Range(0, 4)]
        public int MakeType { get; set; }
    }
}