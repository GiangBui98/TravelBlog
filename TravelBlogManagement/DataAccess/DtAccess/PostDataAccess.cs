using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using TravelBlogManagement.Services.MessageResponse;
using TravelBlogManagement.Services.Models;

namespace TravelBlogManagement.DataAccess.DtAccess
{
    public class PostDataAccess : IPostDataAccess
    {
        private readonly TravelBlogContext _context;

        public PostDataAccess(TravelBlogContext context)
        {
            _context = context;
        }

        //todo: back later
        //G: write a new way --> Monday
        public Post CreatedPost(int currentUserId, string title, string content, string tag)
        {
            var newPost = new Post
            {
                Title = title,
                Content = content,
                CreatedDate = DateTime.Now,
                CreatedUserId = currentUserId,
            };
            _context.Posts.Add(newPost);
            _context.SaveChanges();

            var lstTag = tag.Split(';');

            var tagList = new List<int>();

            foreach (var item in lstTag)
            {
                var existingTag = _context.Set<Tag>().Where(t => t.TagContent.ToLower().Trim() == item.ToLower()).FirstOrDefault();

                if (existingTag != null)
                {
                    tagList.Add(existingTag.TagId);
                }
                else
                {
                    var newTag = new Tag
                    {
                        TagContent = item,
                    };
                    _context.Tags.Add(newTag);
                    _context.SaveChanges();
                    tagList.Add(newTag.TagId);
                }
            }
            foreach (var tagId in tagList)
            {
                var postTag = new PostTag
                {
                    TagId = tagId,
                    PostId = newPost.PostId,
                };
                _context.PostTags.Add(postTag);
                _context.SaveChanges();
            }

            return newPost;

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

        //CreatePost in a new way  --> Done
        public Post CreateANewPost(int currentUserId, string title, string content, string tag)
        {
            var newPost = new Post
            {
                Title = title,
                Content = content,
                CreatedDate = DateTime.Now,
                CreatedUserId = currentUserId,
                PostTags = new List<PostTag>()
            };

            var lstTag = tag.Split(';');

            foreach (var item in lstTag)
            {
                var existingTag = _context.Set<Tag>().Where(t => t.TagContent.ToLower().Trim() == item.ToLower()).FirstOrDefault();
                Tag tagToAdd;

                if (existingTag != null)
                {
                    tagToAdd = existingTag;
                }
                else
                {
                    tagToAdd = new Tag { TagContent = item.ToLower().Trim() };
                    _context.Tags.Add(tagToAdd);
                }

                newPost.PostTags.Add(new PostTag { Tag = tagToAdd });
            }
            _context.Posts.Add(newPost);
            _context.SaveChanges();

            return newPost;
        }

        //G: rewrite  --> Done
        public void AddComment(int currentUserId, int postId, string comment)
        {
            var existingPost = _context.Set<Post>()
                .Where(p => p.PostId == postId)
                .FirstOrDefault();

            if (existingPost != null)
            {
                var commentAdded = new UserComment
                {
                    UserId = currentUserId,
                    PostId = postId,
                    Content = comment,
                    UserCommentHistories = new List<UserCommentHistory>
                    {
                        new UserCommentHistory {
                            UserCommentId = currentUserId,
                            Content = comment,
                            CreatedDate = DateTime.Now,
                        }
                    }
                };

                _context.UserComments.Add(commentAdded);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception($"Post ID {postId} not correct.");
            }
        }

        //G: rewrite --> Done
        public void UpdateComment(int currentUserId, int postId, int userCommentId, string comment)
        {
            var existingComment = _context.Set<UserComment>()
                .Where(p => p.UserId == currentUserId && p.PostId == postId && p.UserCommentId == userCommentId)
                .FirstOrDefault();

            if (existingComment != null)
            {
                existingComment.Content = comment;

                var userCommentHistory = new UserCommentHistory
                {
                    UserCommentId = userCommentId,
                    Content = comment,
                    CreatedDate = DateTime.Now,
                };

                _context.UserCommentHistories.Add(userCommentHistory);
                _context.Entry(existingComment).State = EntityState.Modified;
                _context.SaveChanges();

            }
            else
            {
                throw new Exception("User has not had any comments for this post.");
            };
        }

        //G: rewrite --> change nothing
        public void AddPostReaction(int currentUserId, int postId, int reaction)
        {
            var getPost = _context.Set<Post>()
                .Where(p => p.PostId == postId)
                .FirstOrDefault();

            if (getPost != null)
            {
                UserReaction userReaction = new UserReaction();
                userReaction.UserId = currentUserId;
                userReaction.PostId = postId;
                userReaction.Reaction = reaction;

                _context.UserReactions.Add(userReaction);
                _context.SaveChanges();
            }
        }

        public ViewPostDetailsResponse ViewPostDetails(int postId)
        {
            var postDetails = _context.Set<Post>()
                .Where(p => p.PostId == postId)
                .Select(p => new ViewPostDetailsResponse
                {
                    PostId = p.PostId,
                    PostTitle = p.Title,
                    PostContent = p.Content,
                    CreatedUser = p.CreatedUser.Name,
                    CreatedDate = p.CreatedDate,

                    UserComment = p.UserComments.Select(x => new CommentOfPostResponse
                    {
                        Username = x.User.Name,
                        Comment = x.Content
                    }).ToList(),

                    UserReaction = p.UserReactions.Select(x => new ReactionOfPostResponse
                    {
                        Username = x.User.Name,
                        Reaction = x.Reaction
                    }).ToList()
                })
                .FirstOrDefault();

            if (postDetails == null)
            {
                throw new Exception($"Post ID {postId} not correct.");
            }

            return postDetails;
        }

        public Post UpdatePost(int currentUserId, int postId, string title, string content)
        {
            var post = _context.Set<Post>().Where(x => x.PostId == postId && x.CreatedUserId == currentUserId).FirstOrDefault();

            if (post != null)
            {
                if (!string.IsNullOrWhiteSpace(title))
                {
                    post.Title = title;
                }

                if (!string.IsNullOrWhiteSpace(content))
                {
                    post.Content = content;
                }

                post.UpdatedDate = DateTime.Now;

                _context.Entry(post).State = EntityState.Modified;
                _context.SaveChanges();
            }
            else
            {
                throw new Exception($"Post ID {postId} not correct.");
            }

            return post;
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

        public List<SearchForTagsOrTitleResponse> SearchForTagsOrTitle(string searchingText)
        {

            var searchResult = _context.Set<PostTag>()
                  .Where(p => p.Post.Title.Contains(searchingText) || p.Tag.TagContent.Contains(searchingText))
                  .Select(p => new SearchForTagsOrTitleResponse
                  {
                      PostId = p.Post.PostId,
                      Title = p.Post.Title,
                      Content = p.Post.Content,
                      //how to get all tags of post

                  })
                 .ToList();

            if (searchResult.Count == 0)
            {
                throw new Exception("No record found.");
            }

            return searchResult;
        }

        public List<CommentOfPostResponse> ViewCommentsInPost(int postId)
        {
            var commentsOfPost = _context.Set<UserComment>().AsNoTracking()
                .Where(p => p.PostId == postId)
                .Select(p => new CommentOfPostResponse
                {
                    Username = p.User.Name,
                    Comment = p.Content,
                })
                .ToList();

            return commentsOfPost;
        }

        public List<UserCommentHistory> ViewCommentHistories(int commentId)
        {
            var getCommentHistory = _context.Set<UserCommentHistory>().AsNoTracking()
                .Where(p => p.UserCommentId == commentId)
                .Distinct()
                .ToList();

            return getCommentHistory;
        }

        public List<GetPostListResponse> GetPostList()
        {
            var postList = _context.Set<Post>().AsNoTracking()
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
            var postList = _context.Set<Post>().AsNoTracking()
                .Where(p => p.CreatedUserId == userId)
                .ToList();

            return postList;
        }

        public List<Post> GetPostListExceptCurrentUser(int userId)
        {
            var postList = _context.Set<Post>().AsNoTracking()
               .Where(p => p.CreatedUserId != userId)
               .ToList();

            return postList;
        }

        public List<int?> GetCommentIdList()
        {
            var getList = _context.Set<UserCommentHistory>().AsNoTracking()
                .Select(p => p.UserCommentId)
                .Distinct()
                .ToList();
            return getList;
        }

        public List<CommentListResponse> GetCommentListOfCurrentuser()
        {
            var commentList = _context.Set<UserCommentHistory>().AsNoTracking()
                .Where(p => p.UserComment.UserId == (int)SystemVariables.currentUserId)
                .Select(p => new CommentListResponse
                {
                    UserCommentId = p.UserCommentId,
                    PostId = p.UserComment.PostId
                })
                .Distinct()
                .ToList();

            if (commentList.Count > 0)
            {
                return commentList;
            }
            else
            {
                throw new Exception($"User {SystemVariables.currentUserId} has no comment");
            }
        }
    }
}
