using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Footballers.Data.Models
{
    public class Coach
    {
        //Properties
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string Name { get; set; }

        [Required]
        public string Nationality { get; set; }

        //Collections
        public virtual ICollection<Footballer> Footballers { get; set; } = new HashSet<Footballer>();
    }
}

//Id – integer, Primary Key

//· Name – text with length [2, 40] (required)

//· Nationality – text (required)

//· Footballers – collection of type Footballer