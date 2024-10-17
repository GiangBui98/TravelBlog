using System;
using System.Collections.Generic;

namespace TravelBlogManagement.DataAccess
{
    public partial class User
    {
        public User()
        {
            Posts = new HashSet<Post>();
            UserComments = new HashSet<UserComment>();
            UserReactions = new HashSet<UserReaction>();
        }

        public int UserId { get; set; }
        public string Name { get; set; } = null!;
        public string Password { get; set; } = null!;

        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<UserComment> UserComments { get; set; }
        public virtual ICollection<UserReaction> UserReactions { get; set; }
    }
}
