using System.Security.Cryptography.X509Certificates;
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

        public void CreatePost(string title, string content, string tagContent)
        {
            int tagLength = tagContent.Length;
            bool isTagContentNullOrSpace = string.IsNullOrWhiteSpace(tagContent);

            if (isTagContentNullOrSpace)
            {
                _postTagMessage.TagContentContainWhiteSpace();
            }
            else if ((tagLength > 0 && tagLength < 3) || tagLength > 10)
            {
                _postTagMessage.TagContentViolateLength();
            }
            else
            {
                _postDataAccess.CreatedPost((int)SystemVariables.currentUserId, title, content, tagContent);
            }

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

        public void SearchForTagsOrTitle(string searchingText)
        {
           _postDataAccess.SearchForTagsOrTitle(searchingText);

        }

        public void UpdateComment(int postId, int commentId, string comment)
        {
            _postDataAccess.UpdateComment((int)SystemVariables.currentUserId, postId, commentId, comment);
        }

        public void UpdatePost(int postId, string title, string content)
        {
            _postDataAccess.UpdatePost((int)SystemVariables.currentUserId, postId, title, content);
        }

        public void ViewCommentHistories(int commentId)
        {
           var listResult = _postDataAccess.ViewCommentHistories(commentId);

            if(listResult.Count == 1)
            {
                Console.WriteLine("'This is the latest comment");
                Console.WriteLine(listResult[0].Content + " - " + listResult[0].CreatedDate);
            } 
            else
            {
                foreach (var item in listResult)
                {
                    Console.WriteLine(item.Content + " - " + item.CreatedDate);
                }
            }
        }

        public void ViewCommentsInPost(int postId)
        {
            _postDataAccess.ViewCommentsInPost(postId);
        }

        public void ViewPostDetails(int postId)
        {
            _postDataAccess.ViewPostDetails(postId);
        }
        public void GetPostList()
        {
            var listResult = _postDataAccess.GetPostList();

            foreach (var item in listResult)
            {
                Console.WriteLine($"Post Id: {item.PostId} - Title: {item.Title}");
            }
        }

        public void GetPostListOfCurrentUser()
        {
            var listResult = _postDataAccess.GetPostListOfCurrentUser((int)SystemVariables.currentUserId);

            foreach (var item in listResult)
            {
                Console.WriteLine($"Id: {item.PostId}");
            }
        }
        public void GetPostListExceptCurrentUser()
        {
            var listResult = _postDataAccess.GetPostListOfCurrentUser((int)SystemVariables.currentUserId);

            foreach (var item in listResult)
            {
                Console.WriteLine($"Id: {item.PostId}");
            }
        }

    }
}
