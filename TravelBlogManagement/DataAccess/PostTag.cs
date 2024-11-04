namespace TravelBlogManagement.DataAccess
{
    public partial class PostTag
    {
        public int PostTagId { get; set; }

        public int? PostId { get; set; }

        public int? TagId { get; set; }

        public virtual Post? Post { get; set; }

        public virtual Tag? Tag { get; set; }
    }
}
