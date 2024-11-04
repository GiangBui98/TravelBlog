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

        public bool Register(string username, string password)
        {
            var checkUserExist = _context.Set<User>().FirstOrDefault(x => x.Name.ToLower() == username.ToLower());
            if (checkUserExist != null)
            {
                return false;
            }

            var newUser = new User
            {
                Name = username,
                Password = HashPassword.GetMd5Hash(password)
            };

            _context.Set<User>().Add(newUser);
            _context.SaveChanges();

            return true;
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

        public bool Login(string username, string password)
        {
            var existingUser = _context.Set<User>().FirstOrDefault(x => x.Name.ToLower() == username.ToLower());

            if (existingUser == null)
            {
                throw new Exception($"{username} is not registered.");
            }

            if (existingUser.Password != HashPassword.GetMd5Hash(password))
            {
                throw new Exception($"{username} - Password is not correct.");
            }

            SystemVariables.currentUserId = existingUser.UserId;
            return true;
        }
    }
}
