using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBlogManagement.Services
{
    public interface IUserService
    {
        public void Register(string name, string password);

       // public bool Login(string name, string password);

        public void Login(string username, string password);

    }
}
