using System;
using System.Collections.Generic;

namespace TravelBlogManagement.DataAccess
{
    public partial class UserComment
    {
        public UserComment()
        {
            UserCommentHistories = new HashSet<UserCommentHistory>();
        }

        public int UserCommentId { get; set; }
        public int? UserId { get; set; }
        public int? PostId { get; set; }
        public string Content { get; set; } = null!;

        public virtual Post? Post { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<UserCommentHistory> UserCommentHistories { get; set; }
    }
}
