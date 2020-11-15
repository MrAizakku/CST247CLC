using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CST247CLC.Models
{
    public class User
    {
        public Guid UserID { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string LastName { get; set; }
        [Required]
        public string Sex { get; set; }
        [Required]
        [Range(1,99)]
        public int Age{ get; set; }
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string State { get; set; }
        [Required] 
        [EmailAddress]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Please use a correct email address.")]
        public string Email { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Username should be at least 4 characters.")]
        public string Username { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Password should be at least 4 characters.")]
        public string Password { get; set; }
    }
}