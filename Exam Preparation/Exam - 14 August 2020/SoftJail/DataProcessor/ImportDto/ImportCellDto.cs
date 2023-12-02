using System.ComponentModel.DataAnnotations;

namespace SoftJail.DataProcessor.ImportDto
{
    public class ImportCellDto
    {
        [Required]
        public int CellNumber { get; set; }

        [Required]
        [Range(1, 1000)]
        public bool HasWindow { get; set; }
    }
}