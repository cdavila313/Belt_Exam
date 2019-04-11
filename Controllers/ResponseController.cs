using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using belt_exam.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace belt_exam.Controllers
{
    [Route("Response")]
    public class ResponseController : Controller
    {
        private MyContext dbContext;
        public ResponseController(MyContext context)
        {
            dbContext = context;
        }
        public User loggedInUser
        {
            get
            {
                return dbContext.Users.FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("UserId"));
            }
        }

        public bool checkIfConflicting(int userId,int activityId)
        {
            var UserActivities = dbContext.Responses.Include(a => a.Activity).Where( r => r.UserId == loggedInUser.UserId).ToList();
            Activity RetrievedActivity = dbContext.Activities.SingleOrDefault(r => r.ActivityId == activityId);

            foreach(var activity in UserActivities)
            {
                if(activity.Activity.Date >= RetrievedActivity.Date && activity.Activity.Date <= RetrievedActivity.EndDate)
                {
                    return false;
                }   
                if(activity.Activity.EndDate >= RetrievedActivity.Date && activity.Activity.EndDate <= RetrievedActivity.Date)
                {
                    return false;
                }   
            }
            return true;
        }

        [HttpGet("going/{activityId}")]
        public IActionResult Going(int activityId)
        {
            if(checkIfConflicting(loggedInUser.UserId,activityId))
            {
                Response rsvp = new Response()
                {
                    UserId = loggedInUser.UserId,
                    ActivityId = activityId
                };
                dbContext.Responses.Add(rsvp);
                dbContext.SaveChanges();
                HttpContext.Session.SetString("ErrorMessage","");

                return RedirectToAction("Index","Activity");
            }
            else
            {
                HttpContext.Session.SetString("ErrorMessage","You have conficting time trying to add this event");
                return RedirectToAction("Index","Activity");
            }
        }

        [HttpGet("notgoing/{activityId}")]
        public IActionResult NotGoing(int activityId)
        {
            Response RetrievedResponse = dbContext.Responses.SingleOrDefault(r => r.ActivityId == activityId && r.UserId == loggedInUser.UserId);

            if(RetrievedResponse == null)
                return RedirectToAction("Index", "Activity");            
            dbContext.Responses.Remove(RetrievedResponse);
            dbContext.SaveChanges();
            HttpContext.Session.SetString("ErrorMessage","");
            return RedirectToAction("Index","Activity");

        }

    }
}
