using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using logreg.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace logreg.Controllers
{
    
    public class HomeController : Controller
    {
        private logregContext _context;
        public HomeController(logregContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("/")]
        public IActionResult Index()
        {
            ViewBag.isregerror = false;
            ViewBag.isregerror2 = false;
            ViewBag.islogerror = false;
            return View();
        }
        [HttpPost]
        [Route("/login/")]
        public IActionResult Login(string email, string password)
        {
            ViewBag.isregerror = false;
            ViewBag.isregerror2 = false;
            Users logger = _context.users.SingleOrDefault(users => users.email == email && users.password == password);
            if(logger!=null)
            {
                ViewBag.islogerror = false;
                // Set login session
                HttpContext.Session.SetInt32("logid", logger.usersid);
                return RedirectToAction("Dashboard");
            }else{
                // Show error
                ViewBag.islogerror = true;
                ViewBag.errors = "That password email combo does not exist";
            }
            return View("Index");
        }
        [HttpPost]
        [Route("/register/")]
        public IActionResult Register(RegisterViewModel registration)
        {
            ViewBag.islogerror= false;
            TryValidateModel(registration);
            if(ModelState.IsValid){
                //check if user email already in db
                Users check = _context.users.SingleOrDefault(users => users.email == registration.email);
                if(check != null)
                {
                    // Throw error
                    ViewBag.isregerror2 = true;
                    ViewBag.isregerror = false;
                    ViewBag.errors = "Email already exists";
                }else{

                //add to database
                Users newuser = new Users();
                newuser.name = registration.name;
                newuser.alias = registration.alias;
                newuser.email = registration.email;
                newuser.password = registration.password;
                _context.Add(newuser);
                _context.SaveChanges();

                ViewBag.isregerror2 = true;
                ViewBag.isregerror1 = false;
                ViewBag.errors = "SUCCESS!! Log in now!";
                }
            }else{
                //Not valid so throw errors
                ViewBag.isregerror = true;
                ViewBag.isregerror2 =false;
                ViewBag.errors = ModelState.Values;
            }
            return View("Index");
        }
        [HttpGet]
        [Route("/dashboard")]
        public IActionResult Dashboard()
        {
            ViewBag.ispost = false;
            ViewBag.isdata = false;
            //check logged in id
            int? loggedid = HttpContext.Session.GetInt32("logid");
            Users whohere = _context.users.SingleOrDefault(users => users.usersid == (int)loggedid);
            if(loggedid > 0)
            {
                List<Users> userage = _context.users.Include(users => users.Posts).ThenInclude(users => users.Likes).ToList();
                foreach(var usering in userage)
                {
                    foreach(var post in usering.Posts)
                    {
                    ViewBag.ispost = true;

                    }
                }

                System.Console.WriteLine();
                ViewBag.user = whohere;
                ViewBag.userage = userage;

                return View();
            }
            else{
                return RedirectToAction("Index");
            }
            //show trainsactions
        }
        [HttpGet]
        [Route("/logout/")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
        [HttpPost]
        [Route("/posting/")]
        public IActionResult Posting(string message)
        {
            int? loggedid = HttpContext.Session.GetInt32("logid");
            Posts thepost = new Posts();
            thepost.message = message;
            thepost.usersid = (int)loggedid;
            thepost.created_at = DateTime.Now;
            thepost.updated_at = DateTime.Now;
            _context.Add(thepost);
            _context.SaveChanges();

            return RedirectToAction("Dashboard");
        }
        [HttpGet]
        [Route("/liking/{pid}")]
        public IActionResult Liking(int pid)
        {
            int? loggedid = HttpContext.Session.GetInt32("logid");
            List<Likes> nosecondlike = _context.likes.Where(likes => likes.usersid == (int)loggedid && likes.postsid== pid).ToList();
            if(nosecondlike.Count>0){
                return RedirectToAction("Dashboard");
            }
            Likes newlike = new Likes();
            newlike.likingit = 1;
            newlike.postsid = pid;
            newlike.usersid = (int)loggedid;
            newlike.created_at = DateTime.Now;
            newlike.updated_at = DateTime.Now;
            _context.Add(newlike);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        [HttpGet]
        [Route("/dashboard/{pid}")]
        public IActionResult Postlikes(int pid)
        {
            ViewBag.islikers = false;
            Posts postdetails = _context.posts.SingleOrDefault(posts => posts.postsid == pid);
            List<Likes> wholike = _context.likes.Include(likes => likes.Users).Where(likes => likes.postsid == pid).ToList();
            Users poster = _context.users.SingleOrDefault(users => users.usersid==postdetails.usersid);
            if(wholike.Count > 0){
                ViewBag.islikers = true;
            }
            ViewBag.postedby = poster;
            ViewBag.postmessage = postdetails.message;
            ViewBag.likers = wholike;
            return View();
        }

        [HttpGet]
        [Route("/users/{uid}")]
        public IActionResult Userinfo(int uid)
        {
            Users personinfo = _context.users.Include(users => users.Posts).Include(users => users.Likes).SingleOrDefault(users => users.usersid == uid);
            ViewBag.postcount =0;
            ViewBag.likecount =0;
            if(personinfo.Posts.Count>0)
            {
                foreach(var post in personinfo.Posts)
                {
                    ViewBag.postcount++;
                }
            }
            if(personinfo.Likes.Count>0)
            {
                foreach(var post in personinfo.Likes)
                {
                    ViewBag.likecount++;
                }
            }
            ViewBag.person = personinfo;
            return View();
        }
        
    }
}
