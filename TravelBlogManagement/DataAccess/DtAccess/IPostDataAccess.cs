using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBlogManagement.Services.Models;

namespace TravelBlogManagement.DataAccess.DtAccess
{
    public interface IPostDataAccess
    {
        public Post CreatedPost(int currentUserId, string title, string content, string tag);
        public void ViewPostDetails(int postId);

        public void UpdatePost(int currentUserId, int postId, string title, string content);

        public void SearchForTagsOrTitle(string searchingText);

        public void OrderPostByPublishedDate();

        public void AddComment(int currentUserId, int postId, string comment);

        public void UpdateComment(int currentUserId, int postId, int commentId, string comment);

        public List<CommentOfPostResponse> ViewCommentsInPost(int postId);

        public List<UserCommentHistory> ViewCommentHistories(int commentId);

        public void AddPostReaction(int userId, int postId, int reaction);

        public List<GetPostListResponse> GetPostList();

        public List<Post> GetPostListOfCurrentUser(int userId);

        public List<Post> GetPostListExceptCurrentUser(int userId);

        public List<UserCommentHistory> GetCommentList();
    }
}
