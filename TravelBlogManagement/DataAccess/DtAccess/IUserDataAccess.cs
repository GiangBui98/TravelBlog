namespace TravelBlogManagement.DataAccess.DtAccess
{
    public interface IUserDataAccess
    {
        public bool Login(string username, string password);

        public bool Register(string username, string password);

        public User GetUserByUserName(string username);
    }
}
