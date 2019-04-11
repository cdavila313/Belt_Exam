using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace belt_exam.Models
{
    public class Response
    {  
        [Key]
        public int ResponseId {get; set;}
        public int ActivityId {get; set;}
        public int UserId {get; set;}
        public User User {get; set;}
        public Activity Activity {get; set;}
    }
}