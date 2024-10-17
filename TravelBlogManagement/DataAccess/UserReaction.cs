using System;
using System.Collections.Generic;

namespace TravelBlogManagement.DataAccess
{
    public partial class UserReaction
    {
        public int UserReactionId { get; set; }
        public int? UserId { get; set; }
        public int? PostId { get; set; }
        public int Reaction { get; set; }

        public virtual Post? Post { get; set; }
        public virtual User? User { get; set; }
    }
}
