using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using TravelBlogManagement.Services.MessageResponse;
using TravelBlogManagement.Services.Models;

namespace TravelBlogManagement.DataAccess.DtAccess
{
    public class PostDataAccess : IPostDataAccess
    {
        private readonly TravelBlogContext _context;
        private readonly PostTagMessage _postTagMessage;

        public PostDataAccess(TravelBlogContext context)
        {
            _context = context;
        }

        public Post CreatedPost(int currentUserId, string title, string content, string tag)
        {
            var newpost = new Post();
            newpost.Title = title;
            newpost.Content = content;
            newpost.CreatedDate = DateTime.Now;
            newpost.CreatedUserId = currentUserId;
            _context.Posts.Add(newpost);
            _context.SaveChanges();

            var lstTag = tag.Split(';');

            var lstTagIds = new List<int>();

            foreach (var tag1 in lstTag)
            {
                var check1 = _context.Set<Tag>().Where(t => t.TagContent.ToLower().Trim() == tag1.ToLower()).FirstOrDefault();

                if (check1 != null)
                {
                    lstTagIds.Add(check1.TagId);
                }
                else
                {
                    var newtag = new Tag();
                    newtag.TagContent = tag1;
                    _context.Tags.Add(newtag);
                    _context.SaveChanges();
                    lstTagIds.Add(newtag.TagId);
                }
            }
            foreach (var tagID in lstTagIds)
            {
                var postTag = new PostTag();
                postTag.TagId = tagID;
                postTag.PostId = newpost.PostId;
                _context.PostTags.Add(postTag);
                _context.SaveChanges();
            }

            return newpost;

            /*
                        int newPostTagId;

                        var inputTag = _context.Set<Tag>().FirstOrDefault(x => x.TagContent == tag);
                        //var inputTag1 = _context.Set<Tag>().Where()

                        if (inputTag == null)
                        {
                            var newTagObject = new Tag { TagContent = tag };

                            _context.Set<Tag>().Add(newTagObject);
                            _context.SaveChanges();

                            newPostTagId = newTagObject.TagId;
                        }
                        else
                        {
                            newPostTagId = inputTag.TagId;
                        }

                        var tagList = new List<PostTag>
                        {
                            new PostTag { TagId = newPostTagId }
                        };


                        var post = _context.Set<Post>().Add(new Post
                        {
                            Title = title,
                            Content = content,                
                            PostTags = tagList,                
                            CreatedDate = DateTime.Now,

                        }) ;

                        _context.SaveChanges();*/
        }
        public void AddComment(int currentUserId, int postId, string comment)
        {
            var getPost = _context.Set<Post>()
                .Where(p => p.PostId == postId)
                .FirstOrDefault();

            if (getPost != null)
            {
                var commentAdded = new UserComment();
                commentAdded.UserId = currentUserId;
                commentAdded.PostId = postId;
                commentAdded.Content = comment;

                _context.UserComments.Add(commentAdded);
                _context.SaveChanges();

                var userCommentHistoryAdded = new UserCommentHistory();
                userCommentHistoryAdded.UserCommentId = commentAdded.UserCommentId;
                userCommentHistoryAdded.CreatedDate = DateTime.Now;

                _context.UserCommentHistories.Add(userCommentHistoryAdded);
                _context.SaveChanges();
            }           
        }

        public void UpdateComment(int currentUserId, int postId, int userCommentId, string comment)
        {
            var postCommentOfCurrentUser = _context.Set<UserComment>()
                .Where(p => p.UserId == currentUserId && p.PostId == postId)                
                .ToList();

            if (postCommentOfCurrentUser != null)
            {
                var commentToUpdate = _context.Set<UserComment>()
                    .Where(p => p.UserCommentId == userCommentId)
                    .FirstOrDefault();

                commentToUpdate.Content = comment;
                
                _context.SaveChanges();

                var userCommentHistory = new UserCommentHistory();
                userCommentHistory.UserCommentId = userCommentId;
                userCommentHistory.Content = comment;
                userCommentHistory.CreatedDate = DateTime.Now;

                _context.Add(userCommentHistory);
                _context.SaveChanges();

            }
            else
            {
                throw new Exception("User has not have any comments for this post.");
            };
        }

        public void AddPostReaction(int currentUserId, int postId, int reaction)
        {
            var getPost = _context.Set<Post>()
                .Where(p => p.PostId == postId)
                .FirstOrDefault();

            if(getPost != null)
            {
                  UserReaction userReaction = new UserReaction();
                  userReaction.UserId = currentUserId;
                  userReaction.PostId = postId;
                  userReaction.Reaction = reaction;

                  _context.UserReactions.Add(userReaction); 
                  _context.SaveChanges();
            }           
        }
      
        public void ViewPostDetails(int postId)
        {
            var postDetail = _context.Set<Post>()
                .Where(p => p.PostId == postId).FirstOrDefault();

            var commentsOfPost = _context.Set<UserComment>()
                 .Where(p => p.PostId == postId)
                 .Select(p => new CommentOfPostResponse
                 {
                     Username = p.User.Name,
                     Comment = p.Content,

                 })
                 .ToList();

            var reactionsOfPost = _context.Set<UserReaction>()
                .Where(p => p.PostId == postId)
                .Select(p => new ReactionOfPostResponse
                {
                    Username = p.User.Name,                  
                    Reaction = p.Reaction,
                })
                .ToList();

            if (postDetail != null)
            {
                Console.WriteLine("Post Details:");
                Console.WriteLine($"Post ID: {postDetail.PostId}");
                Console.WriteLine($"Title: {postDetail.Title}");
                Console.WriteLine($"Content: {postDetail.Content}");
                Console.WriteLine($"Created Date: {postDetail.CreatedDate}");
                Console.WriteLine("All comments of post: ");
                foreach (var value in commentsOfPost)
                {
                    Console.WriteLine($"User: {value.Username}");
                    Console.WriteLine($"Comment: {value.Comment}");
                }

                Console.WriteLine("All reactions of post: ");
                foreach (var value in reactionsOfPost)
                {
                    Console.WriteLine($"User: {value.Username}");
                    Console.WriteLine($"Comment: {value.Reaction}");
                }
            }
            else
            {
                Console.WriteLine($"No post found with ID: {postId}");
            }
        }

        public void UpdatePost(int currentUserId, int postId, string title, string content)
        {
            var getPostToUpdate = _context.Set<Post>().Where(x => x.PostId == postId).FirstOrDefault();

            if (getPostToUpdate != null)
            {  
                getPostToUpdate.Title = title;
                getPostToUpdate.Content = content;
                getPostToUpdate.UpdatedDate = DateTime.Now;

                _context.Entry(getPostToUpdate).State = EntityState.Modified;
                _context.SaveChanges();                
            }
        }

        public void OrderPostByPublishedDate()
        {

            var result = _context.Set<Post>()
                .OrderBy(p => p.UpdatedDate.HasValue ? p.UpdatedDate : p.CreatedDate)
                .ToList();

            foreach (var item in result)
            {
                Console.WriteLine($"Post ID: {item.PostId}, Title: {item.Title}");
            }

        }

        public void SearchForTagsOrTitle(string searchingText)
        {

            var listPostIdViaSearchedResult = _context.Set<PostTag>()
                  .Where(p => p.Post.Title == searchingText || p.Tag.TagContent == searchingText)
                  .Select(p => new
                  {
                      p.Post.PostId,
                      p.Post.Title,
                      p.Post.Content,
                      p.Tag.TagContent
                  })
                 .ToList();

            if(listPostIdViaSearchedResult.Count == 0)
            {
                throw new Exception("No record found.");
            }
            else
            {
                foreach (var item in listPostIdViaSearchedResult)
                {
                    Console.WriteLine($"Post Id: {item.PostId}, Post Title: {item.Title} ,Tag Content: {item.TagContent}");
                }
            }            
        }

        public List<CommentOfPostResponse> ViewCommentsInPost(int postId)
        {
            var commentsOfPost = _context.Set<UserComment>()
                .Where(p => p.PostId == postId)
                .Select(p => new CommentOfPostResponse
                {
                    Username = p.User.Name,
                    Comment = p.Content

                })
                .ToList();

            return commentsOfPost;
        }

        public List<UserCommentHistory> ViewCommentHistories(int commentId)
        {
            var getCommentHistory = _context.Set<UserCommentHistory>()
                .Where(p => p.UserCommentId == commentId)
                .ToList() ;

            return getCommentHistory;
        }

        public List<GetPostListResponse> GetPostList()
        {
            var postList = _context.Set<Post>()
                .Select(p => new GetPostListResponse
                {
                    PostId = p.PostId,
                    Title = p.Title
                })
                .ToList();

            return postList;
        }

        public List<Post> GetPostListOfCurrentUser(int userId)
        {
            var postList = _context.Set<Post>()
                .Where(p => p.CreatedUserId == userId)
                .ToList();

            return postList;
        }

        public List<Post> GetPostListExceptCurrentUser(int userId)
        {
            var postList = _context.Set<Post>()
               .Where(p => p.CreatedUserId != userId)
               .ToList();

            return postList;
        }

        public List<UserCommentHistory> GetCommentList()
        {
            var getList = _context.Set<UserCommentHistory>()
                .Distinct()
                .ToList() ;
            return getList;
        }
    }
    
}
