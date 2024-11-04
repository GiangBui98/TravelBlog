using TravelBlogManagement.DataAccess;
using TravelBlogManagement.DataAccess.DtAccess;
using TravelBlogManagement.Services.MessageResponse;
using TravelBlogManagement.Services.Models;

namespace TravelBlogManagement.Services
{
    public class PostService : IPostService
    {
        private readonly IPostDataAccess _postDataAccess;
        private readonly PostTagMessage _postTagMessage;

        public PostService(IPostDataAccess postDataAccess)
        {
            _postDataAccess = postDataAccess;
        }

        public Post CreatePost(string title, string content, string tags)
        {
            var tagList = tags.Split(';').ToList();
            foreach (var tag in tagList)
            {
                var tagLength = tag.Length;
                if ((tagLength > 0 && tagLength < 3) || tagLength > 10 || tagLength == 0)
                {
                    throw new Exception("Each tag must be between 3 and 10 characters.");
                }
            }

            return _postDataAccess.CreatedPost((int)SystemVariables.currentUserId, title, content, tags);
        }

        public Post CreateANewPost(string title, string content, string tags)
        {
            var tagList = tags.Split(';').ToList();
            foreach (var tag in tagList)
            {
                var tagLength = tag.Length;
                if ((tagLength > 0 && tagLength < 3) || tagLength > 10 || tagLength == 0)
                {
                    throw new Exception("Each tag must be between 3 and 10 characters.");
                }
            }

            return _postDataAccess.CreateANewPost((int)SystemVariables.currentUserId, title, content, tags);
        }

        public void AddComment(int postId, string comment)
        {
            _postDataAccess.AddComment((int)SystemVariables.currentUserId, postId, comment);
        }

        public void AddPostReaction(int postId, int reaction)
        {
            _postDataAccess.AddPostReaction((int)SystemVariables.currentUserId, postId, reaction);
        }

        public void OrderPostByPublishedDate()
        {
            _postDataAccess.OrderPostByPublishedDate();
        }

        public List<SearchForTagsOrTitleResponse> SearchForTagsOrTitle(string searchingText)
        {
             return _postDataAccess.SearchForTagsOrTitle(searchingText);
        }

        public void UpdateComment(int postId, int commentId, string comment)
        {
            _postDataAccess.UpdateComment((int)SystemVariables.currentUserId, postId, commentId, comment);
        }

        public Post UpdatePost(int postId, string title, string content)
        {
            return _postDataAccess.UpdatePost((int)SystemVariables.currentUserId, postId, title, content);
        }

        public List<UserCommentHistory> ViewCommentHistories(int commentId)
        {
            return _postDataAccess.ViewCommentHistories(commentId);
        }

        public List<CommentOfPostResponse> ViewCommentsInPost(int postId)
        {
            return _postDataAccess.ViewCommentsInPost(postId);
        }

        public ViewPostDetailsResponse ViewPostDetails(int postId)
        {
            return _postDataAccess.ViewPostDetails(postId);

        }

        public List<GetPostListResponse> GetPostList()
        {
            var listResult = _postDataAccess.GetPostList();

            return listResult;
        }

        public List<Post> GetPostListOfCurrentUser()
        {
            var listResult = _postDataAccess.GetPostListOfCurrentUser((int)SystemVariables.currentUserId);

            return listResult;
        }

        public List<Post> GetPostListExceptCurrentUser()
        {
            return _postDataAccess.GetPostListExceptCurrentUser((int)SystemVariables.currentUserId);
        }

        public List<int?> GetCommentIdList()
        {
            return _postDataAccess.GetCommentIdList();
        }

        public List<CommentListResponse> GetCommentListOfCurrentuser()
        {
            return _postDataAccess.GetCommentListOfCurrentuser();
        }
    }
}
