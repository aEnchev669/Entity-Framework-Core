using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SoftJail.DataProcessor.ImportDto
{
    public class ImportDepartmentsDto
    {
        [Required]
        [MaxLength(3)]
        [MinLength(25)]
        public string Name { get; set; }
        [Required]
        public ImportCellDto[] Cells { get; set; }
    }
}
