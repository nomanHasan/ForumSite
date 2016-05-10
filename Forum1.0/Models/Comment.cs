using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Forum1._0.Models
{
    public class Comment
    {
        public int Comment_ID { get; set; }

        public string Comment_Content { get; set; }

        public int Thread_ID { get; set; }

        public Thread Thread { get; set; }

        public string Username { get; set; }

        public User User { get; set; }

        public int Reply_Comment_ID { get; set; }

        public Comment Reply_Comment { get; set; }

        public DateTime Comment_Created { get; set; }

        public int Like_Count { get; set; }

        public List<User> Liked_Users { get; set; }

        public int Dislike_Count { get; set; }

        public List<User> Disliked_Users { get; set; }
    }
}