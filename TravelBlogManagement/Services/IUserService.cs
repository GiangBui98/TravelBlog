namespace TravelBlogManagement.Services
{
    public interface IUserService
    {
        public bool Register(string name, string password);
      
        public bool Login(string username, string password);

    }
}
