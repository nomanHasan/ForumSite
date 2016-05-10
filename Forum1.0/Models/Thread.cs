using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Forum1._0.Models
{
    public class Thread
    {
        public int Thread_ID { get; set; }

        public string Thread_Title { get; set; }

        public string Thread_Desc { get; set; }

        public int Group_ID { get; set; }

        public Group Group { get; set; }

        public string Username { get; set; }

        public User User { get; set; }

        public int Lock_Status { get; set; }

        public DateTime Thread_Created { get; set; }

        public List<User> Liked_Users { get; set; }

        public List<User> Disliked_Users { get; set; }
    }
}