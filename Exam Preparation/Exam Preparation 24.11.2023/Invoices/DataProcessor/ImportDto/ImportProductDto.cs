﻿using Invoices.Data.Models.Enums;
using Invoices.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoices.DataProcessor.ImportDto
{
    public class ImportProductDto
    {
        

        [Required]
        [MaxLength(30)]
        [MinLength(9)]
        public string Name { get; set; } = null!;

        [Required]
        [Range(5.00, 1000.00)]
        public decimal Price { get; set; }
        [Required]
        [Range(0,4)]
        public CategoryType CategoryType { get; set; }

        public int[] Clients { get; set; }
    }
}

//· Name – text with length [9…30] (required)

//· Price – decimal in range [5.00…1000.00] (required)

//· CategoryType – enumeration of type CategoryType, with possible values (ADR, Filters, Lights, Others, Tyres) (required)

//· ProductsClients – collection of type ProductClient
