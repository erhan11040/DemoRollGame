using System;
using System.Collections.Generic;

#nullable disable

namespace DemoRollGame.Models.Models
{
    public partial class User
    {
        public User()
        {
            UserMatches = new HashSet<UserMatch>();
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsAvailable { get; set; }

        public virtual ICollection<UserMatch> UserMatches { get; set; }
    }
}
