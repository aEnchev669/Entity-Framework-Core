using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models
{
    public class Bet
    {
        //BetId, Amount, Prediction, DateTime, UserId, GameId

        [Key]
        public int BetId { get; set; }

        public decimal Amount { get; set; } 
       
        public int Prediction { get; set; }
        
        public DateTime DateTime { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;

        [ForeignKey(nameof(Game))]       
        public int GameId { get; set; }

        public virtual Game Game { get; set; } = null!;
    }
}
