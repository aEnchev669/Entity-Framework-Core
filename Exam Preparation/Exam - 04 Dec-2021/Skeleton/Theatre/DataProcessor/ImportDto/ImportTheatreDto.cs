﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Theatre.Data.Models;

namespace Theatre.DataProcessor.ImportDto
{
    public class ImportTheatreDto
    {
        [Required]
        [MaxLength(30)]
        [MinLength(4)]
        public string Name { get; set; }

        [Required]
        [Range(1, 10)]
        public sbyte NumberOfHalls { get; set; }

        [Required]
        [MaxLength(30)]
        [MinLength(4)]
        public string Director { get; set; }

        public ImportTicketDto[] Tickets { get; set; }
    }
}
