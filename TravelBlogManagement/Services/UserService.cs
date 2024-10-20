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

        public void Login(string username, string password)
        {
            try 
            { 
                _userDataAccess.Login(username, password); 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Register(string username, string password)
        {
            if(_userDataAccess.Register(username, password)) {
                Console.WriteLine("Register successfully.");
            }
            else
            {
                Console.WriteLine("Please try again");
            }
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
