
using TravelBlogManagement.DataAccess;
using TravelBlogManagement.Services.Models;

namespace TravelBlogManagement.Services
{
    public interface IPostService
    {

        public Post CreatePost(string title, string content, string tagContent);

        public void ViewPostDetails(int postId);

        public void UpdatePost(int postId, string title, string content);

        public void SearchForTagsOrTitle(string searchingText);

        public void OrderPostByPublishedDate();

        public void AddComment(int postId, string comment);

        public void UpdateComment(int postId, int commentId, string comment);

        public List<CommentOfPostResponse> ViewCommentsInPost(int postId);

        public List<UserCommentHistory> ViewCommentHistories(int commentId);

        public void AddPostReaction(int postId, int reaction);

        public List<GetPostListResponse> GetPostList();
        public List<Post> GetPostListOfCurrentUser();
        public List<Post> GetPostListExceptCurrentUser();

        public List<UserCommentHistory> GetCommentList();
    } 
}
