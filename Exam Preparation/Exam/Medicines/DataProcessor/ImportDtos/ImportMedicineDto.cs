using Medicines.Common;
using Medicines.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Medicines.DataProcessor.ImportDtos
{
    [XmlType("Medicine")]
    public class ImportMedicineDto
    {
        [XmlAttribute("category")]
        [Required]
        [Range(CommonConstraints.MedicineCategoryMinRange, CommonConstraints.MedicineCategoryMaxRange)]
        public int Category { get; set; }

        [XmlElement("Name")]
        [Required]
        [MinLength(CommonConstraints.MedicineNameMinLength)]
        [MaxLength(CommonConstraints.MedicineNameMaxLength)]
        public string Name { get; set; }

        [XmlElement("Price")]
        [Required]
        [Range(CommonConstraints.MedicinePriceMinRange, CommonConstraints.MedicinePriceMaxRange)]
        public decimal Price { get; set; }

        [XmlElement("ProductionDate")]
        [Required]
        public string ProductionDate { get; set; }

        [XmlElement("ExpiryDate")]
        [Required]
        public string ExpiryDate { get; set; }

        [XmlElement("Producer")]
        [Required]
        [MinLength(CommonConstraints.MedicineProducerMinLength)]
        [MaxLength(CommonConstraints.MedicineProducerMaxLength)]
        public string Producer { get; set; }
    }
}