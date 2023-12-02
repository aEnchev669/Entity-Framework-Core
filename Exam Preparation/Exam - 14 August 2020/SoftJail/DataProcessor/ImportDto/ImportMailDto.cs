using System.ComponentModel.DataAnnotations;

namespace SoftJail.DataProcessor.ImportDto
{
    public class ImportMailDto
    {
        public string Description { get; set; } = null!;
        [Required]
        public string Sender { get; set; } = null!;
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9\s]+ str\.$")]
        public string Address { get; set; } = null!;
    }
}