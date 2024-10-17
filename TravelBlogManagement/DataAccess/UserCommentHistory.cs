using System;
using System.Collections.Generic;

namespace TravelBlogManagement.DataAccess
{
    public partial class UserCommentHistory
    {
        public int UserCommentHistoryId { get; set; }
        public int? UserCommentId { get; set; }
        public string Content { get; set; } = null!;
        public DateTime? CreatedDate { get; set; }

        public virtual UserComment? UserComment { get; set; }
    }
}
