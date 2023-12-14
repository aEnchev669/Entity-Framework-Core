using Footballers.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ImportDto
{
    [XmlType("Footballer")]
    public class ImportFootballerDto
    {
        [Required]
        [MaxLength(40)]
        [MinLength(2)]
        [XmlElement("Name")]
        public string Name { get; set; } = null!;

        [Required]
        [XmlElement("ContractStartDate")]
        public string ContractStartDate { get; set; }

        [Required]
        [XmlElement("ContractEndDate")]
        public string ContractEndDate { get; set; }

        [Required]
        [XmlElement("PositionType")]
        [Range(0,3)]
        public int PositionType { get; set; }

        [Required]
        [XmlElement("BestSkillType")]
        [Range(0, 4)]
        public int BestSkillType { get; set; }
    }
}
//Id – integer, Primary Key

//· Name – text with length [2, 40] (required)

//· ContractStartDate – date and time (required)

//· ContractEndDate – date and time (required)

//· Position - enumeration of type PositionType, with possible values (Goalkeeper, Defender, Midfielder, Forward) (required)

//· BestSkill – enumeration of type BestSkillType, with possible values (Defence, Dribble, Pass, Shoot, Speed) (required)

//· CoachId – integer, foreign key (required)

//· Coach – Coach

//· TeamsFootballers – collection of type TeamFootballer