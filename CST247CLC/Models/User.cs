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
        [RegularExpression(@"^[A-Z]+[a-zA-Z]*$")]
        [StringLength(50, MinimumLength = 3)]
        public string FirstName { get; set; }
        [Required]
        [RegularExpression(@"^[A-Z]+[a-zA-Z]*$")]
        [StringLength(50, MinimumLength = 3)]
        public string LastName { get; set; }
        [Required]
        [RegularExpression(@"^[A-Z]+[a-zA-Z]*$")]
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
        [RegularExpression(@"^[A-Z]+[a-zA-Z]*$")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Username should be at least 4 characters.")]
        public string Username { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$", ErrorMessage = @"Password must contain a number, one lowercase, one uppercase, one symbol, and be 8 - 15 characters.")]
        [StringLength(15, MinimumLength = 8)]
        public string Password { get; set; }
    }
}