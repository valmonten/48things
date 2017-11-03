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
                newuser.fname = registration.fname;
                newuser.lname = registration.lname;
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
            ViewBag.isdata = false;
            //check logged in id
            int? loggedid = HttpContext.Session.GetInt32("logid");
            Users whohere = _context.users.SingleOrDefault(users => users.usersid == (int)loggedid);
            if(loggedid > 0)
            {
                ViewBag.user = whohere;
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
        
    }
}
