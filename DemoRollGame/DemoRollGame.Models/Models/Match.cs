using System;
using System.Collections.Generic;

#nullable disable

namespace DemoRollGame.Models.Models
{
    public partial class Match
    {
        public Match()
        {
            UserMatches = new HashSet<UserMatch>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsComplated { get; set; }
        public int? WinnerRoll { get; set; }
        public string WinnerName { get; set; }

        public virtual ICollection<UserMatch> UserMatches { get; set; }
    }
}
