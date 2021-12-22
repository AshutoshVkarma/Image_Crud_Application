using Image_Crud_Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Image_Crud_Application.Controllers
{
    public class LoginController : Controller
    {
        ICRUDDBEntities db = new ICRUDDBEntities();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(User user)
        {
            var Log = db.Users.Where(e => e.Email == user.Email && e.Password == user.Password).FirstOrDefault();
            if (user != null)
            {
                Session["UserId"] = user.Id.ToString();
                Session["UserName"] = user.Email.ToString();
                TempData["LoginSuccess"]= "<script>alert('Login Succesfully')</script>";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = "<script>alert('Email or Password is incorrect')</script>";
                return View();
            }
            
        }


        public ActionResult Signup(User user)
        {
            if (ModelState.IsValid == true)
            {
                db.Users.Add(user);
                int a=db.SaveChanges();
                if (a > 0)
                {
                    ViewBag.InsertMessage = "<script>alert('Registered Succesfully')</script>";
                    ModelState.Clear();
                }
                else
                {
                    ViewBag.InsertMessage = "<script>alert('Registeration Failed')</script>";
                }
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }
    }
}