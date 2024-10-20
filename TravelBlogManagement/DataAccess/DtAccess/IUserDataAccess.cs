using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBlogManagement.DataAccess.DtAccess
{
    public interface IUserDataAccess
    {

        public int Login(string username, string password);
        public bool Register(string username, string password);

        public User GetUserByUserName(string username);
    }
}
