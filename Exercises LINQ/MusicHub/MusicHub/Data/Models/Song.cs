using MusicHub.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MusicHub.Data.Models
{
    public class Song
    {
        public Song()
        {
            this.SongPerformers = new HashSet<SongPerformer>();
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(20)]
        public string Name { get; set; } = null!;

        public TimeSpan Duration { get; set; }

        public DateTime CreatedOn { get; set; }


        public Genre Genre { get; set; }

        [ForeignKey(nameof(Album))]
        public int? AlbumId { get; set; }
        public virtual Album? Album { get; set; }


        [ForeignKey(nameof(Writer))]
        public int WriterId { get; set; }
        public virtual Writer Writer { get; set; } = null!;


        public decimal Price { get; set; }

        public virtual ICollection<SongPerformer> SongPerformers { get; set; } 

    }
}

//Id – integer, Primary Key

//· Name – text with max length 20 (required)

//· Duration – TimeSpan (required)

//· CreatedOn – date (required)

//· Genre – genre enumeration with possible values: "Blues, Rap, PopMusic, Rock, Jazz"(required)

//· AlbumId – integer, Foreign key

//· Album – the Song's Album

//· WriterId – integer, Foreign key (required)

//· Writer – the Song's Writer

//· Price – decimal (required)

//· SongPerformers – a collection of type SongPerformer
