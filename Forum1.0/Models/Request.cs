using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Forum1._0.Models
{
    public class Request
    {
        public int GroupID { get; set; }

        public Group Group { get; set; }

        public string Username { get; set; }

        public User User { get; set; }
    }
}