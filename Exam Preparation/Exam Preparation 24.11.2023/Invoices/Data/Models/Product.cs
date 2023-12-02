﻿
using System;
using System.Collections.Generic;
using Castle.Components.DictionaryAdapter;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Invoices.Data.Models.Enums;

namespace Invoices.Data.Models
{
    public class Product
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; } = null!;

        [Required]
        public decimal Price { get; set; }
        [Required]
        public CategoryType CategoryType { get; set; }

        public virtual ICollection<ProductClient> ProductsClients { get; set; } = new HashSet<ProductClient>();
    }
}


//Id – integer, Primary Key

//· Name – text with length [9…30] (required)

//· Price – decimal in range [5.00…1000.00] (required)

//· CategoryType – enumeration of type CategoryType, with possible values (ADR, Filters, Lights, Others, Tyres) (required)

//· ProductsClients – collection of type ProductClient