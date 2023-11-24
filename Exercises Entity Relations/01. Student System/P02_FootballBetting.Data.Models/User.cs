using P02_FootballBetting.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models
{
    public class User
    {
        public User()
        {
            this.Bets = new HashSet<Bet>();
        }
        //UserId, Username, Password, Email, Name, Balanc
        [Key]
        public int UserId { get; set; }
        [Required]
        [MaxLength(ValidationConstants.UsernameMaxLenght)]
        public string Username { get; set; } = null!;
        [Required]
        [MaxLength(ValidationConstants.UserPasswordMaxLenght)]
        public string Password { get; set; } = null!;
        [Required]
        [MaxLength(ValidationConstants.UserEmailMaxLenght)]
        public string Email { get; set; } = null!;
        [Required]
        [MaxLength(ValidationConstants.UserNameMaxLenght)]
        public string Name { get; set; } = null!;
        [Required]
        public decimal Balance { get; set; }

        public virtual ICollection<Bet> Bets { get; set; } = null!;
    }
}
