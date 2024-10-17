using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBlogManagement.Services.MessageResponse
{
    public class PostTagMessage
    {
        public void TagContentContainWhiteSpace()
        {
            Console.WriteLine("Tag Content cannot contains space.");
        }

        public void TagContentViolateLength()
        {
            Console.WriteLine("Each Tag Content only accepts from 3 to 10 character.");
        }

        public void PostIdNotExisted()
        {
            Console.WriteLine("Not existing this post id.");
        }

        public void NoCommentForPost()
        {
            Console.WriteLine("You have no comment for this post to update");
        }
    }
}
