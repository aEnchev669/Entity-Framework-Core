using Medicines.Data.Models.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicines.DataProcessor.ImportDtos
{
    public class ImportPatientDto
    {
        [JsonProperty("FullName")]
        [Required]
        [MaxLength(100)]
        [MinLength(5)]
        public string FullName { get; set; } = null!;
        [Required]
        [Range(0, 2)]
        [JsonProperty("AgeGroup")]

        public int AgeGroup { get; set; }
        [Required]
        [Range(0, 1)]
        [JsonProperty("Gender")]

        public int Gender { get; set; }
        [JsonProperty("Medicines")]

        public virtual int[] Medicines { get; set; }
    }
}
