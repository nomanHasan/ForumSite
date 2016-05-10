using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Forum1._0.Models
{
    public class Status
    {
        public int Status_ID { get; set; }

        public string Content { get; set; } 

        public string Username { get; set; }

        public User User { get; set; }

        public int LikeCount { get; set; }

        public int DislikeCount { get; set; }

        public DateTime Status_Created { get; set; }

        public List<User> Liked_Users { get; set; }

        public List<User> Disliked_Users { get; set; }

    }
}