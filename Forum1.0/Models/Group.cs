using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Forum1._0.Models
{
    public class Group
    {
        public int Group_ID { get; set; }

        [Display(Name ="Group Name")]
        public string Group_Name { get; set; }

        public DateTime Group_Created { get; set; }

        [Display(Name = "Group Category")]
        public string Group_Category { get; set; }

        public string Creator_ID { get; set; }

        public User Creator { get; set; }
    }
}