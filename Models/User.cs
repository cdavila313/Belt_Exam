using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace belt_exam.Models
{
    public class User
    {
        [Key]
        public int UserId {get; set;}
        [Required]
        [MinLength(2)]
        public string Name {get; set;}
        [Required]
        [EmailAddress]
        public string Email {get; set;}
        [DataType(DataType.Password)]
        [Required]
        [RegularExpression(@"^((?=.*?[A-Za-z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$)", ErrorMessage = "Must be atleast 8 characters and contain at least 1 number, 1 letter and a special character.")]
        public string Password {get; set;}
        [NotMapped]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string PasswordConfirmation {get; set;}
        public DateTime Created_At {get; set;} = DateTime.Now;
        public DateTime Modified_At {get; set;} = DateTime.Now;
        public List<Response> ResponsesIssued { get; set; }
        public List<Activity> ActivitiesPlanned {get; set;}
    }
    
    public class LoginUser
    {
        [Required]
        [EmailAddress]
        public string Email {get; set;}
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}