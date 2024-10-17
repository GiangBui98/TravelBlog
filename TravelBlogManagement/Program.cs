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
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        Login();
                        loggedIn = true;
                        break;

                     case "2":
                        Console.WriteLine("Please input username: ");
                        var usernameRegister = Console.ReadLine();
                        Console.WriteLine("Please input password: ");
                        var passwordRegister = Console.ReadLine();

                        userService.Register(usernameRegister, passwordRegister);

                        Console.WriteLine("Register successfully. Please login");

                        Login();
                        loggedIn = true;
                        break;

                    default: 
                        Console.WriteLine("Please input valid value.");
                        break;

                }
            }

           while (true)
            {
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

                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        Console.WriteLine("Post title: ");
                        var postTitle = Console.ReadLine();
                        Console.WriteLine("Content: ");
                        var content = Console.ReadLine();
                        Console.WriteLine("Tag will be separated by ; . Please input tag content: ");
                        var tagContent = Console.ReadLine();

                        postService.CreatePost(postTitle, content, tagContent);

                        break;

                    case "2":
                        Console.WriteLine("List of post: ");
                        postService.GetPostList();
                        Console.WriteLine("Input postId: ");
                        var postId = int.Parse(Console.ReadLine());

                        postService.ViewPostDetails(postId);

                        break;

                    case "3":
                        Console.WriteLine("Select post to update: ");
                        var postIdToUpdate = int.Parse(Console.ReadLine());
                        Console.WriteLine("Post title: ");
                        var postTitleUpdated = Console.ReadLine();
                        Console.WriteLine("Content: ");
                        var contentUpdated = Console.ReadLine();

                        postService.UpdatePost(postIdToUpdate, postTitleUpdated, contentUpdated);

                        break;

                    case "4":
                        Console.WriteLine("Input tag or title to search: ");
                        var searchingText = Console.ReadLine();

                        postService.SearchForTagsOrTitle(searchingText);

                        break;

                    case "5":
                        Console.WriteLine("Posts are ordered by Published Date: ");
                        postService.OrderPostByPublishedDate();

                        break;

                    case "6":
                        Console.WriteLine("Select post to add comment: ");
                        var postIdToAddComment = int.Parse(Console.ReadLine());
                        Console.WriteLine("Comment: ");
                        var comment = Console.ReadLine();

                        postService.AddComment(postIdToAddComment, comment);

                        break;

                    case "7":
                        Console.WriteLine("Select post Id to view comment: ");
                        var postIdToViewComment = int.Parse(Console.ReadLine());

                        postService.ViewCommentsInPost(postIdToViewComment);
                        break;
                    case "8":
                        Console.WriteLine("Select commentId to view: ");
                        var commentIdToView = int.Parse(Console.ReadLine());

                        postService.ViewCommentHistories(commentIdToView);
                        break;
                    case "9":

                        Console.WriteLine("Select post to add reaction: ");
                        var postIdToAddReaction = int.Parse(Console.ReadLine());
                        Console.WriteLine("1. Like  ||  2. Favourite: ");
                        var reaction = int.Parse(Console.ReadLine());
                       
                        postService.AddPostReaction(postIdToAddReaction, reaction);

                        break;
                }

            }
            
        }

        private static void Login()
        {
            Console.WriteLine("Please input username: ");
            var username = Console.ReadLine();
            Console.WriteLine("Please input password: ");
            var password = Console.ReadLine();

            userService.Login(username, password);
        }
    }
}