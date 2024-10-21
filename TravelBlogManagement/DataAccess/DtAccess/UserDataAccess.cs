using System.Reflection.Metadata.Ecma335;
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
            if (checkUserExist == null)
            {
                var newUser = new User();
                newUser.Name = username;
                newUser.Password = HashPassword.GetMd5Hash(password);

                _context.Set<User>().Add(newUser);
                _context.SaveChanges();

                return true;
            }
            else
            {
                return false;
                                
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

        public int Login(string username, string password)
        {
            var checkUserNameExist = _context.Set<User>().FirstOrDefault(x => x.Name.ToLower() == username.ToLower());

            if(checkUserNameExist == null)
            {
                return 0;
            }
            if(checkUserNameExist.Password != HashPassword.GetMd5Hash(password))
            {
                return 1;
            }
            
            SystemVariables.currentUserId = checkUserNameExist.UserId;
            return 2;
         
        }
    }
}
