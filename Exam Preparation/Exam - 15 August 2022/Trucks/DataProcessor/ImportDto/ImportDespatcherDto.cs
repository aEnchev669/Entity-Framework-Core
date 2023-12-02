using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Trucks.Data.Models;

namespace Trucks.DataProcessor.ImportDto
{
    [XmlType("Despatcher")]
    public class ImportDespatcherDto
    {
        [MaxLength(40)]
        [MinLength(2)]
        public string Name { get; set; }

        public string Position { get; set; }

        [XmlArray("Trucks")]
        public virtual ImportTruckDto[] Trucks { get; set; }
    }
}
