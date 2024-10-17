using System;
using System.Collections.Generic;

namespace TravelBlogManagement.DataAccess
{
    public partial class Post
    {
        public Post()
        {
            PostTags = new HashSet<PostTag>();
            UserComments = new HashSet<UserComment>();
            UserReactions = new HashSet<UserReaction>();
        }

        public int PostId { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? CreatedUserId { get; set; }

        public virtual User? CreatedUser { get; set; }
        public virtual ICollection<PostTag> PostTags { get; set; }
        public virtual ICollection<UserComment> UserComments { get; set; }
        public virtual ICollection<UserReaction> UserReactions { get; set; }
    }
}
