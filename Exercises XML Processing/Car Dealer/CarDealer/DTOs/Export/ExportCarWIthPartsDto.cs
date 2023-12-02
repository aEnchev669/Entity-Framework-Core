﻿using CarDealer.DTOs.Import;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CarDealer.DTOs.Export
{
    [XmlType("car")]
    public class ExportCarWIthPartsDto
    {
        [XmlAttribute("make")]
        public string Make { get; set; } = null!;

        [XmlAttribute("model")]
        public string Model { get; set; } = null!;

        [XmlAttribute("traveledDistance")]
        public long TraveledDistance { get; set; }

        [XmlArray("parts")]
        public ExportPartCarsDto[] Parts { get; set; } = null!;
    }
}
