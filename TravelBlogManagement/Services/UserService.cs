using TravelBlogManagement.DataAccess.DtAccess;

namespace TravelBlogManagement.Services
{
    public class UserService : IUserService
    {
        private readonly IUserDataAccess _userDataAccess;

        public UserService(IUserDataAccess userDataAccess)
        {
            _userDataAccess = userDataAccess;
        }

        public bool Login(string username, string password)
        {
            return _userDataAccess.Login(username, password);
        }

        public bool Register(string username, string password)
        {
            return _userDataAccess.Register(username, password);
        }        
    }
}
