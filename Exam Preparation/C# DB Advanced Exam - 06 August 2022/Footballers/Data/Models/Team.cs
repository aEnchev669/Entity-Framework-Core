using AutoMapper;
using Footballers.Data.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Footballers.Data.Models
{
    public class Team
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
       // [RegularExpression(@"^[a-zA-Z0-9 .-]*$")]
        public string Name { get; set; }   =  null!;

        [Required]
        [MaxLength(40)]
        public string Nationality { get; set; } = null!;

        [Required]
        public int Trophies { get; set; }

        public ICollection<TeamFootballer> TeamsFootballers { get; set; } = new HashSet<TeamFootballer>();
    }
}

//Id – integer, Primary Key

//· Name – text with length [3, 40].Should contain letters(lower and upper case), digits, spaces, a dot sign ('.') and a dash ('-'). (required)

//· Nationality – text with length [2, 40] (required)

//· Trophies – integer (required)

//· TeamsFootballers – collection of type TeamFootballer
