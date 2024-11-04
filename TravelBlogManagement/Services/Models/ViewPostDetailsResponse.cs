namespace TravelBlogManagement.Services.Models
{
    public class ViewPostDetailsResponse
    {
        public int PostId { get; set; }

        public string PostTitle { get; set; }

        public string PostContent { get; set; }

        public string CreatedUser { get; set; }

        public DateTime? CreatedDate { get; set; }

        public List<CommentOfPostResponse> UserComment { get; set; }

        public List<ReactionOfPostResponse> UserReaction { get; set; }
    }
}
