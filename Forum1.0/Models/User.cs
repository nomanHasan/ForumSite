using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Forum1._0.Models
{
    public class User
    {
        [Required(ErrorMessage = "Please Enter a Username ")]
        public string Username { get; set; }
        [Required(ErrorMessage ="Your need to Enter the Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Your need to Enter the Name")]
        public string Name { get; set; }

        [Required(ErrorMessage ="This field cannot be blank")]
        [ EmailAddress(ErrorMessage = "Your need to Enter a Valid Email Address")]
        public string Email { get; set; }
        [Required(ErrorMessage ="This field cannot be blank")]
        [ Phone(ErrorMessage ="Enter a valid Phone Number")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Your need to Enter the Address")]
        public string Address { get; set; }

        public float Activity_Rating { get; set; }

        public float Like_Rating { get; set; }

        public float Dislike_Rating { get; set; }

    }
}