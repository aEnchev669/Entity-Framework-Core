﻿using Cadastre.Data.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Cadastre.DataProcessor.ImportDtos
{
    [XmlType("District")]
    public class ImportDistrictDto
    {
        [Required]
        [MaxLength(80)]
        [MinLength(2)]
        [XmlElement("Name")]
        public string Name { get; set; }

        [Required]
        [MaxLength(8)]
       [MinLength(8)]
        [RegularExpression(@"^[A-Z]{2}-\d{5}$")]
        [XmlElement("PostalCode")]
        public string PostalCode { get; set; }

        [Required]
        [XmlAttribute("Region")]
        public Region Region { get; set; }

        [XmlArray("Properties")]
        public  ImportPropertyDto[] Property { get; set; }
    }
}
