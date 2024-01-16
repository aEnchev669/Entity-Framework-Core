using Cadastre.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cadastre.Data.Models
{
    public class PropertyCitizen
    {
        [Required]
        [ForeignKey(nameof(Property))]
        public int PropertyId { get; set; }
        public Property Property { get; set; }


        [Required]
        [ForeignKey(nameof(Citizen))]
        public int CitizenId { get; set; }
        public Citizen Citizen { get; set; }

    }
}

//PropertyId – integer, Primary Key, foreign key (required) 

//Property – Property 

//CitizenId – integer, Primary Key, foreign key (required) 

//Citizen – Citizen 