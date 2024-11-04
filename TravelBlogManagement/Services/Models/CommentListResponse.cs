using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBlogManagement.Services.Models
{
    public class CommentListResponse
    {
        public int? PostId {  get; set; }    

        public int? UserCommentId { get; set; }
    }
}
