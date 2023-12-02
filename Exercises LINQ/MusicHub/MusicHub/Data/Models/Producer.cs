﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicHub.Data.Models
{
    public class Producer
    {

        public Producer()
        {
            this.Albums = new HashSet<Album>();
        }
        [Key]
        public int Id { get; set; }

        [MaxLength(30)]
        public string Name { get; set; } = null!;
        
        [MaxLength(30)]
        public string? Pseudonym { get; set; } = null!;
        
   
        public string? PhoneNumber { get; set; } = null!;

        public virtual ICollection<Album> Albums { get; set; } 
    }
}
