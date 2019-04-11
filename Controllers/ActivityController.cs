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
    [Route("Activity")]
    public class ActivityController : Controller
    {
        private MyContext dbContext;
        public ActivityController(MyContext context)
        {
            dbContext = context;
        }
        private bool inSession
        {
            get { return HttpContext.Session.GetInt32("UserId") != null; }
        }
        public User loggedInUser
        {
            get
            {
                return dbContext.Users.FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("UserId"));
            }
        }       


        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            if (!inSession)
            {
                return RedirectToAction("Index","Login");
            }
            ViewBag.CurrentUser = loggedInUser;
            ViewBag.UserName = loggedInUser.Name;
            ViewBag.ErrorMessage = HttpContext.Session.GetString("ErrorMessage");
            var AllActivities = dbContext.Activities.Where( a => a.DateAndTime >= DateTime.Now).Include(a => a.Planner).Include(a => a.Responses).OrderByDescending(a => a.Date).ToList();
            return View(AllActivities);
        }

        [Route("Expired")]
        [HttpGet]
        public IActionResult ExpiredActivities()
        {
            if (!inSession)
            {
                return RedirectToAction("Index","Login");
            }
            ViewBag.CurrentUser = loggedInUser;
            var AllExpiredActivities = dbContext.Activities.Where( a => a.DateAndTime <= DateTime.Now).Include(a => a.Responses).OrderByDescending(a => a.Date).ToList();
            return View(AllExpiredActivities);
        }

        [Route("Show/{activityId}")]
        [HttpGet]
        public IActionResult ShowActivity(int activityId)
        {
            if (!inSession)
            {
                return RedirectToAction("Index","Login");
            }
            ViewBag.CurrentUser = loggedInUser;
            var UsersGoing = dbContext.Activities
                        .Include(a => a.Planner)
                        .Include(a => a.Responses)
                        .ThenInclude(r => r.User)
                        .FirstOrDefault(a => a.ActivityId == activityId);
            return View("ShowActivity", UsersGoing);
        }

        [Route("Add")]
        [HttpGet]
        public IActionResult ActivityForm()
        {
            if (!inSession)
            {
                return RedirectToAction("Index","Login");
            }
            ViewBag.CurrentUser = loggedInUser;
            var currentUser = loggedInUser;
            return View("ActivityForm");
        }
        [HttpPost("Create")]
        public IActionResult CreateActivity(Activity newActivity)
        {
            if(newActivity.DateAndTime < DateTime.Now)
            {
                ModelState.AddModelError("Date","The Date and/or Time cannot be in the past");
                ModelState.AddModelError("Time","The Date and/or Time cannot be in the past");

            }
            if(ModelState.IsValid)
            {
                newActivity.UserId = loggedInUser.UserId;
                dbContext.Add(newActivity);
                dbContext.SaveChanges();
                return RedirectToAction("Index","Activity");
            }
            return View("ActivityForm");
        }

        [Route("Delete/{activityId}")]
        [HttpGet]
        public IActionResult DeleteActivity(int activityId)
        {
            if (!inSession)
            {
                return RedirectToAction("Index","Login");
            }
            ViewBag.CurrentUser = loggedInUser;
            Activity RetrievedActivity = dbContext.Activities.SingleOrDefault(r => r.ActivityId == activityId && r.UserId == loggedInUser.UserId);

            if(RetrievedActivity == null)
                return RedirectToAction("Index", "Activity");            
            dbContext.Activities.Remove(RetrievedActivity);
            dbContext.SaveChanges();
            return RedirectToAction("Index","Activity");
        }
    }
}
