using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using belt_exam.Models;

namespace belt_exam.Models
{
    public class Activity
    {
        [Key]
        public int ActivityId {get; set;}
        [Required]
        public string Name {get; set;}
        [Required]
        public DateTime Date {get; set;}
        [Required]
        public DateTime Time {get; set;}
        [Required]
        public int Duration {get; set;}
        [Required]
        public string DurationType {get; set;}
        [Required]
        public string Description {get; set;}
        public DateTime Created_At {get; set;} = DateTime.Now;
        public DateTime Modified_At {get; set;} = DateTime.Now; 
        public int UserId {get; set;}
        public User Planner {get; set;}
        public List<Response> Responses { get; set; }
        
        [NotMapped]
        public DateTime DateAndTime 
        {
            get{
                return Combine(Date, Time);
            }
        }

        
        [NotMapped]
        public DateTime EndDate
        {
            get 
            {
                if(DurationType == "Hours")
                {
                    DateTime end = Date.AddHours(Duration);
                    return end;
                }
                else if(DurationType == "Days")
                {
                    DateTime end = Date.AddDays(Duration);
                    return end;
                }
                else
                {
                    DateTime end = Date.AddMinutes(Duration);
                    return end;
                }
            }
        }

        public DateTime Combine(DateTime date, DateTime time)
        {
            return (date.Date + time.TimeOfDay);
        }               
    }
    
}