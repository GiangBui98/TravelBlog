using TravelBlogManagement.Services;

namespace TravelBlogManagement.DataAccess.DtAccess
{
    public class UserDataAccess : IUserDataAccess
    {
        private readonly TravelBlogContext _context;

        public UserDataAccess(TravelBlogContext context)
        {
            _context = context;
        }

        public void Register(string username, string password)
        {
            var checkUserExist = _context.Set<User>().FirstOrDefault(x => x.Name.ToLower() == username.ToLower());
            if (checkUserExist == null)
            {
                var newUser = new User();
                newUser.Name = username;
                newUser.Password = HashPassword.GetMd5Hash(password);

                _context.Set<User>().Add(newUser);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception($"User name {username} existed. Please enter a new name.");
            }
        }

        public User GetUserByUserName(string username)
        {
            var user = _context.Set<User>().FirstOrDefault(x => x.Name == username);

            if (user == null) 
            {
                throw new Exception($"User name {username} not existed. Please enter valid name.");
            }

            return user;
        }

        public void Login(string username, string password)
        {
            var checkUserNameExist = _context.Set<User>().FirstOrDefault(x => x.Name.ToLower() == username.ToLower());
            if (checkUserNameExist != null)
            {
                if (checkUserNameExist.Password == HashPassword.GetMd5Hash(password))
                {
                    Console.WriteLine("Login successfully");
                    SystemVariables.currentUserId = checkUserNameExist.UserId;
                }
                else
                {
                    Console.WriteLine("Incorrect credential.");
                }
            }
            else
            {
                throw new Exception($"User name {username} not existed. Please enter valid name.");
            }
        }
    }
}
