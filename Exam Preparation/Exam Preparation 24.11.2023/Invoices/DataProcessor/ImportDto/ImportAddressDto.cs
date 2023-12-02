using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Invoices.DataProcessor.ImportDto
{
    [XmlType("Address")]
    public class ImportAddressDto
    {
        [Required]
        [MaxLength(20)]
        [MinLength(10)]
        public string StreetName { get; set; } = null!;

        [Required]
        public int StreetNumber { get; set; }

        [Required]
        public string PostCode { get; set; } = null!;


        [Required]
        [MaxLength(15)]
        [MinLength(5)]
        public string City { get; set; } = null!;

        [Required]
        [MaxLength(15)]
        [MinLength(5)]
        public string Country { get; set; } = null!;
    }
}
//· StreetName – text with length [10…20] (required)

//· StreetNumber – integer (required)

//· PostCode – text (required)

//· City – text with length [5…15] (required)

//· Country – text with length [5…15] (required)
