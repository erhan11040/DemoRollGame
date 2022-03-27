using System;
using System.Collections.Generic;

#nullable disable

namespace DemoRollGame.Models.Models
{
    public partial class UserMatch
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime JoinedAt { get; set; }
        public int MatchId { get; set; }
        public int Roll { get; set; }
        public bool? IsWinner { get; set; }

        public virtual Match Match { get; set; }
        public virtual User User { get; set; }
    }
}
