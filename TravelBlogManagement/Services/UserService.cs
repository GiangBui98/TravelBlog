using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public int Login(string username, string password)
        {
            return _userDataAccess.Login(username, password);
        }

        public bool Register(string username, string password)
        {
            return _userDataAccess.Register(username, password);
        }

       /* public bool Login(string username, string password) 
        {
            var user = _userDataAccess.GetUserByUserName(username);
            if (user.Password != password)
            {
               // throw new Exception($"Incorrect password. Please enter correct password.");
                return false;
            }
             
            return true;
        }*/

        
    }
}
