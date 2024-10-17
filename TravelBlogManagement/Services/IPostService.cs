
using TravelBlogManagement.Services.Models;

namespace TravelBlogManagement.Services
{
    public interface IPostService
    {

        public void CreatePost(string title, string content, string tagContent);

        public void ViewPostDetails(int postId);

        public void UpdatePost(int postId, string title, string content);

        public void SearchForTagsOrTitle(string searchingText);

        public void OrderPostByPublishedDate();

        public void AddComment(int postId, string comment);

        public void UpdateComment(int postId, int commentId, string comment);

        public void ViewCommentsInPost(int postId);

        public void ViewCommentHistories(int commentId);

        public void AddPostReaction(int postId, int reaction);

        public void GetPostList();
    } 
}
