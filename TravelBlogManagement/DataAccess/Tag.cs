using System;
using System.Collections.Generic;

namespace TravelBlogManagement.DataAccess
{
    public partial class Tag
    {
        public Tag()
        {
            PostTags = new HashSet<PostTag>();
        }

        public int TagId { get; set; }
        public string TagContent { get; set; } = null!;

        public virtual ICollection<PostTag> PostTags { get; set; }
    }
}
