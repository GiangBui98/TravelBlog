using System;
using System.Reflection.Metadata;
using TravelBlogManagement.DataAccess;
using TravelBlogManagement.DataAccess.DtAccess;
using TravelBlogManagement.Services;

namespace TravelBlogManagement
{
    public class Program
    {

        static TravelBlogContext travelBlogContext = new TravelBlogContext();
        static IPostDataAccess postDataAccess = new PostDataAccess(travelBlogContext);
        static IUserDataAccess userDataAccess = new UserDataAccess(travelBlogContext);

        static IUserService userService = new UserService(userDataAccess);
        static IPostService postService = new PostService(postDataAccess);
        static void Main(string[] args)
        {
            Console.WriteLine("Please select function as below: ");
            Console.WriteLine("1. Login");
            Console.WriteLine("2. Register");

            bool loggedIn = false;

            while (!loggedIn)
            {
                Console.WriteLine("--------------");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":

                        loggedIn = Login();
                        bool checkLoggIn = false;

                        if (!loggedIn)
                        {
                            Console.WriteLine("Login unsuccessful. Please try again.");

                            while (!checkLoggIn)
                            {
                                checkLoggIn = Login();
                            }

                        }
                        Console.WriteLine("--------------");
                        break;

                    case "2":

                        while (!Register())
                        {
                            Console.WriteLine("Registration unsuccessful. Please try again.");
                        }

                        while (!loggedIn)
                        {
                            loggedIn = Login();
                        }

                        Console.WriteLine("--------------");
                        break;

                    default:
                        Console.WriteLine("Please input valid value.");
                        break;
                }
                break;
            }

            while (true)
            {
                Console.WriteLine("--------------");
                Console.WriteLine("Please select function as below: ");
                Console.WriteLine("1. Create Post");
                Console.WriteLine("2. View Post Details");
                Console.WriteLine("3. Update Post");
                Console.WriteLine("4. Search For Tags Or Title");
                Console.WriteLine("5. Order Post By Published Date");
                Console.WriteLine("6. Add Comment");
                Console.WriteLine("7. View Comments In Post");
                Console.WriteLine("8. View Comment Histories");
                Console.WriteLine("9. Add Post Reaction");
                Console.WriteLine("--------------");

                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        CreatePost();

                        break;

                    case "2":
                        ViewPostDetails();

                        break;

                    case "3":
                        UpdatePost();

                        break;

                    case "4":
                        SearchForTagsOrTitle();

                        break;

                    case "5":
                        Console.WriteLine("Posts are ordered by Published Date: ");
                        postService.OrderPostByPublishedDate();

                        break;

                    case "6":
                        AddCommentForPost();

                        break;

                    case "7":
                        ViewCommentsInPost();

                        break;
                    case "8":
                        ViewCommentHistories();

                        break;
                    case "9":
                        AddPostReaction();

                        break;

                    default:
                        Console.WriteLine("Goodbye");
                        return;
                }

            }

        }

        private static bool Login()
        {
            Console.WriteLine("Enter username:");
            string username = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(username))
            {
                Console.WriteLine("Username cannot be empty.");
                username = Console.ReadLine();
            }

            Console.WriteLine("Enter password:");
            string password = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("Password cannot be empty.");
                password = Console.ReadLine();
            }

            int loginResult = userService.Login(username, password);

            if (loginResult == 0)
            {
                Console.WriteLine("Username does not exist.");
                return false;
            }
            else if (loginResult == 1)
            {
                Console.WriteLine("Incorrect password. Please try again.");
                return false;
            }
            else
            {
                Console.WriteLine("Login successful!");
                return true;
            }

        }

        private static bool Register()
        {
            Console.WriteLine("Enter username:");
            string username = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(username))
            {
                Console.WriteLine("Username cannot be empty.");
                username = Console.ReadLine();
            }

            Console.WriteLine("Enter password:");
            string password = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("Password cannot be empty.");
                password = Console.ReadLine();
            }

            bool isRegistered = userService.Register(username, password);

            if (isRegistered)
            {
                Console.WriteLine("Registration successful! Please login.");
                return true;
            }
            else
            {
                Console.WriteLine("Username already exists.");
                return false;
            }

        }

        private static void CreatePost()
        {
            Console.WriteLine("Enter Title:");
            string title = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(title))
            {
                Console.WriteLine("Title cannot be empty.");
                title = Console.ReadLine();
            }

            Console.WriteLine("Enter Content:");
            string content = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(content))
            {
                Console.WriteLine("Title cannot be empty.");
                content = Console.ReadLine();
            }

            Console.WriteLine("Each tag will be separated by ';'. Enter tag:");
            var tagContent = Console.ReadLine();

            string tags;
            while (true)
            {
                tags = Console.ReadLine();
                try
                {
                    var newPost = postService.CreatePost(title, content, tags);
                    Console.WriteLine("Post created successfully!");
                    Console.WriteLine($"Post Title: {newPost.Title}");
                    Console.WriteLine($"Post Content: {newPost.Content}");
                    for (int i = 0; i < tags.Length; i++)
                    {
                        Console.Write(tags[i] + "; ");
                    }

                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");

                }

            }
        }

        private static void ViewPostDetails()
        {
            Console.WriteLine("List of post: ");
            var listPost = postService.GetPostList();

            foreach (var item in listPost)
            {
                Console.WriteLine($"Id: {item.PostId}");
            }

            Console.WriteLine("Select postId to view details: ");
            int postId;

            while (true)
            {
                postId = int.Parse(Console.ReadLine());

                try
                {
                    postService.ViewPostDetails(postId);
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{ex.Message}");
                }
            }
        }

        private static void UpdatePost()
        {
            Console.WriteLine("List of post: ");
            var listPost = postService.GetPostListOfCurrentUser();

            foreach (var item in listPost)
            {
                Console.WriteLine($"Id: {item.PostId}");
            }


            int postIdToUpdate;

            while (true)
            {
                Console.WriteLine("Select post to update: ");
                postIdToUpdate = int.Parse(Console.ReadLine());
                bool found = false;
                foreach (var item in listPost)
                {
                    if (item.PostId == postIdToUpdate)
                    {
                        found = true;
                        break;
                    }
                }

                if (found)
                {
                    Console.WriteLine("Post title: ");
                    var postTitleUpdated = Console.ReadLine();

                    while (string.IsNullOrWhiteSpace(postTitleUpdated))
                    {
                        Console.WriteLine("Title cannot be empty.");
                        postTitleUpdated = Console.ReadLine();
                    }

                    Console.WriteLine("Content: ");
                    var contentUpdated = Console.ReadLine();

                    while (string.IsNullOrWhiteSpace(contentUpdated))
                    {
                        Console.WriteLine("Title cannot be empty.");
                        contentUpdated = Console.ReadLine();
                    }

                    while (true)
                    {
                        try
                        {
                            postService.UpdatePost(postIdToUpdate, postTitleUpdated, contentUpdated);
                            Console.WriteLine($"Post with updated info:");
                            Console.WriteLine($"Post Tite Updated: {postTitleUpdated}");
                            Console.WriteLine($"Post Content Updated: {contentUpdated}");
                            return;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"{ex.Message}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Invalid postId. Please try again.");
                }

            }

        }

        private static void SearchForTagsOrTitle()
        {
            Console.WriteLine("Input tag or title to search: ");
            var searchingText = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(searchingText))
            {
                Console.WriteLine("Input tag or title cannot be empty.");
                searchingText = Console.ReadLine();
            }

            postService.SearchForTagsOrTitle(searchingText);
        }

        //jump to exception, AddCommmnet() not work
        private static void AddCommentForPost()
        {
            Console.WriteLine("List of post: ");
            var listPost = postService.GetPostList();

            foreach (var item in listPost)
            {
                Console.WriteLine($"Id: {item.PostId}");
            }
           
            int postId;

            while (true)
            {
                Console.WriteLine("Select post to add comment: ");
                postId = int.Parse(Console.ReadLine());
                bool found = false;
                foreach (var item in listPost)
                {
                    if (item.PostId == postId)
                    {
                        found = true;
                        break;
                    }
                }

                if (found)
                {
                    Console.WriteLine("Comment Content: ");
                    var content = Console.ReadLine();

                    while (string.IsNullOrWhiteSpace(content))
                    {
                        Console.WriteLine("Content cannot be empty.");
                        content = Console.ReadLine();
                    }

                    while (true)
                    {
                        try
                        {
                            postService.AddComment(postId, content);
                            Console.WriteLine($"Comment: {content} has been added for post {postId}");

                            return;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"{ex.Message}");                            
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Invalid postId. Please try again.");
                }
            }
        }

        private static void ViewCommentsInPost()
        {
            Console.WriteLine("List of post: ");
            var listPost = postService.GetPostList();

            foreach (var item in listPost)
            {
                Console.WriteLine($"Id: {item.PostId}");
            }

            int postId;

            while (true)
            {
                Console.WriteLine("Select post Id to view comment: ");
                postId = int.Parse(Console.ReadLine());
                bool found = false;
                foreach (var item in listPost)
                {
                    if (item.PostId == postId)
                    {
                        found = true;
                        break;
                    }
                }

                if (found)
                {
                    var listResult = postService.ViewCommentsInPost(postId);
                    if(listResult.Count > 0)
                    {
                        foreach (var item in listResult)
                        {
                            Console.WriteLine($"User: {item.Username} - Comment: {item.Comment}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("This post has no comment.");
                    }
                    
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid postId. Please try again.");
                }
            }
        }

        private static void ViewCommentHistories()
        {
            Console.WriteLine("List of comment to view: ");
            var listcomment = postService.GetCommentList();

            foreach (var item in listcomment)
            {
                Console.WriteLine($"Id: {item.UserCommentId}");
            }
            int commentId;

            while (true)
            {
                Console.WriteLine("Select comment Id to view its history ");
                commentId = int.Parse(Console.ReadLine());
                bool found = false;
                foreach (var item in listcomment)
                {
                    if (item.UserCommentId == commentId)
                    {
                        found = true;
                        break;
                    }
                }

                if (found)
                {
                    var listResult = postService.ViewCommentHistories(commentId);
                    if (listResult.Count == 1)
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
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid comment Id. Please try again.");
                }
            }
        }

        private static void AddPostReaction()
        {
            Console.WriteLine("List of post: ");
            var listPost = postService.GetPostListExceptCurrentUser();

            foreach (var item in listPost)
            {
                Console.WriteLine($"Id: {item.PostId}");
            }
          
            int postId;

            while (true)
            {
                Console.WriteLine("Select post to add reaction: ");
                postId = int.Parse(Console.ReadLine());
                bool found = false;
                foreach (var item in listPost)
                {
                    if (item.PostId == postId)
                    {
                        found = true;
                        break;
                    }
                }

                if (found)
                {
                    int reaction = 0;

                    while (reaction > 2 || reaction < 1)
                    {
                        Console.WriteLine("1. Like  ||  2. Favourite: ");
                        reaction = int.Parse(Console.ReadLine());
                    }

                    switch (reaction)
                    {
                        case 1:
                            //should have try...catch here
                            postService.AddPostReaction(postId, reaction);
                            Console.WriteLine($"You have added reaction {reaction} to post {postId}");
                            break;

                         case 2:
                            postService.AddPostReaction(postId, reaction);
                            Console.WriteLine($"You have added reaction {reaction} to post {postId}");
                            break;

                         default:
                            Console.WriteLine("Invalid value. Please input again.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid postId. Please try again.");
                }
            }
        }
    }
}
