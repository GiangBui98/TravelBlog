using TravelBlogManagement.Services.Models;

namespace TravelBlogManagement.DataAccess.DtAccess
{
    public interface IPostDataAccess
    {
        public Post CreatedPost(int currentUserId, string title, string content, string tag);

        public Post CreateANewPost(int currentUserId, string title, string content, string tag);
        
        public ViewPostDetailsResponse ViewPostDetails(int postId);

        public Post UpdatePost(int currentUserId, int postId, string title, string content);

        public List<SearchForTagsOrTitleResponse> SearchForTagsOrTitle(string searchingText);

        public void OrderPostByPublishedDate();

        public void AddComment(int currentUserId, int postId, string comment);

        public void UpdateComment(int currentUserId, int postId, int commentId, string comment);

        public List<CommentOfPostResponse> ViewCommentsInPost(int postId);

        public List<UserCommentHistory> ViewCommentHistories(int commentId);

        public void AddPostReaction(int userId, int postId, int reaction);

        public List<GetPostListResponse> GetPostList();

        public List<Post> GetPostListOfCurrentUser(int userId);

        public List<Post> GetPostListExceptCurrentUser(int userId);

        public List<int?> GetCommentIdList();

        public List<CommentListResponse> GetCommentListOfCurrentuser();
    }
}
