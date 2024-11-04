using TravelBlogManagement.DataAccess;

namespace TravelBlogManagement.Services.Models
{
    public class NewPostResponse
    {
        public Post Post { get; set; }
        public List<int> TagList { get; set; }

    }
}
