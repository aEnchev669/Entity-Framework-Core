using SoftJail.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SoftJail.DataProcessor.ImportDto
{
    public class ImportPrisonerDto
    {
   

        [Required]
        [MaxLength(20)]
        [MinLength(3)]
        public string FullName { get; set; } = null!;

        [Required]
        [RegularExpression(@"^The\s[A-Z][a-z]+")]
        public string Nickname { get; set; } = null!;

        [Range(18,65)]
        public int Age { get; set; }

        public string IncarcerationDate { get; set; }

        public string? ReleaseDate { get; set; }

        [Range(0, (double)(decimal.MaxValue))]//??????????????????????????????????????????????????????????????????????????????????/
        public decimal? Bail { get; set; }

        public int? CellId { get; set; }


        public  ImportMailDto[] Mails { get; set; }
    }
}
