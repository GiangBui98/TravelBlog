using Microsoft.EntityFrameworkCore;
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

        public void AddComment(int currentUserId, int postId, string comment)
        {

            //1. Check postId
            //2. if postId != null --> add cmt --> then add cmt in cmthistory tbl
            // elfe {error msg}

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
            else
            {
                throw new Exception($"Post with id {postId} not found.");
            }
        }

        public void UpdateComment(int currentUserId, int postId, int userCommentId, string comment)
        {

            //1. user select post
            //2. check if current has cmt 
            // if yes -> update cmt in UserComment tbl && create new record in UserCmtHistory tbl

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
                if (getPost.CreatedUserId == currentUserId)
                {
                    throw new Exception("Owner of the post cannot add reaction.");

                }
                else
                {
                    UserReaction userReaction = new UserReaction();
                    userReaction.UserId = currentUserId;
                    userReaction.PostId = postId;
                    userReaction.Reaction = reaction;

                    _context.UserReactions.Add(userReaction); 
                    _context.SaveChanges();
                }
            }
            else
            {
                throw new Exception($"Post id {postId} not existed.");
            }
        }

        public void CreatedPost(int currentUserId, string title, string content, string tag)
        {
            //Step
            //1. create post
            //2. check tag
            //add postTag into PostTag tbl
            

            //1. Tạo bài viết          
            var newpost = new Post();
            newpost.Title = title;
            newpost.Content = content;
            newpost.CreatedDate = DateTime.Now;
            newpost.CreatedUserId = currentUserId;
            _context.Posts.Add(newpost);
            _context.SaveChanges(); //lưu csdl => có PostId
            //2. Kiểm tra danh sách tag, chưa có thì lưu tag vào CSDL

            var lstTag = tag.Split(';');

            var lstTagIds = new List<int>();//khai báo để dùng ở step  3

            foreach(var tag1 in lstTag)
            {
                var check1 = _context.Set<Tag>().Where(t => t.TagContent.ToLower().Trim() == tag1.ToLower()).FirstOrDefault();
                //
                if(check1 != null)
                {
                    //đã có
                    lstTagIds.Add(check1.TagId);
                }
                else
                {
                    //chưa có, thêm mới tag
                    var newtag = new Tag();
                    newtag.TagContent = tag1;
                    _context.Tags.Add(newtag);//chưa save
                    _context.SaveChanges() ;//có tag id
                    lstTagIds.Add(newtag.TagId);
                }
            }

            //3. Liên kết bài viết với danh sách tag ban đầu.
            //lstTagIds

            foreach(var tagID in lstTagIds) {
                var postTag = new PostTag();
                postTag.TagId = tagID;
                postTag.PostId = newpost.PostId;
                _context.PostTags.Add(postTag);
                _context.SaveChanges();
            }

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

        public void ViewPostDetails(int postId)
        {
            //requirement: view all things relate to post: including interaction and comments

        }

        public void UpdatePost(int currentUserId, int postId, string title, string content)
        {
            var getPostToUpdate = _context.Set<Post>().Where(x => x.PostId == postId).FirstOrDefault();

            if (getPostToUpdate != null)
            {
                if(getPostToUpdate.CreatedUserId == currentUserId)
                {
                    getPostToUpdate.Title = title;
                    getPostToUpdate.Content = content;
                    getPostToUpdate.UpdatedDate = DateTime.Now;

                    _context.Entry(getPostToUpdate).State = EntityState.Modified;
                    _context.SaveChanges();
                }
                else
                {
                    Console.WriteLine("You're not owner of post.");
                }
                
            }
            else
            {
                _postTagMessage.PostIdNotExisted();
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

            foreach (var item in listPostIdViaSearchedResult)
            {
                Console.WriteLine($"Post Id: {item.PostId}, Post Title: {item.Title}, Tag Content: {item.TagContent}");
            }
        }

        public List<UserCommentHistory> ViewCommentHistories(int commentId)
        {
            var getCommentHistory = _context.Set<UserCommentHistory>()
                .Where(p => p.UserCommentId == commentId)
                .ToList() ;

            return getCommentHistory;
        }

        public void ViewCommentsInPost(int postId)
        {
            var commentsOfPost = _context.Set<UserComment>()
                .Where(p => p.PostId == postId)
                .Select(p => new
                {
                    p.User.Name,
                    p.Content
                    
                })
                .ToList();
            
            var reactionsOfPost = _context.Set<UserReaction>()
                .Where(p => p.PostId == postId)
                .Select(p => new
                {
                    p.User.Name,
                    p.Reaction
                })
                .ToList();

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

        
    }
    
}
