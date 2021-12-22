using Image_Crud_Application.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Image_Crud_Application.Controllers
{
    public class HomeController : Controller
    {
        ICRUDDBEntities db = new ICRUDDBEntities();
        // GET: Home
        public ActionResult Index()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                var data = db.Employees.ToList();
                return View(data);
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Employee emp)
        {
            if (ModelState.IsValid == true)
            {
                string fileName = Path.GetFileNameWithoutExtension(emp.ImageFile.FileName);
                string extension = Path.GetExtension(emp.ImageFile.FileName);
                HttpPostedFileBase postedFile = emp.ImageFile;
                int lenth = postedFile.ContentLength;

                if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                {
                    if (lenth <= 1000000)
                    {
                        fileName = fileName + extension;
                        emp.Image_Path = "~/images" + fileName;
                        fileName = Path.Combine(Server.MapPath("~/images/"), fileName);
                        emp.ImageFile.SaveAs(fileName);
                        db.Employees.Add(emp);
                        int a = db.SaveChanges();
                        if (a > 0)
                        {
                            TempData["CreateMessage"] = "<script>alert('Data Inserted Succesfully')</script>";
                            ModelState.Clear();
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            TempData["CreateMessage"] = "<script>alert('Data not Inserted')</script>";
                        }
                    }
                    else
                    {
                        TempData["SizeMessage"] = "<script>alert('Image size should be less then 1mb')</script>";
                    }
                }
                else
                {
                    TempData["ExtensionMessage"] = "<script>alert('Format not Suported')</script>";
                }
            }
            return View();
        }

        public ActionResult Edit(int id)
        {
            var row = db.Employees.Where(e => e.Id == id).FirstOrDefault();
            Session["Image"] = row.Image_Path;
            return View(row);
        }

        [HttpPost]
        public ActionResult Edit(Employee emp)
        {
            if (ModelState.IsValid == true)
            {
                if (emp.ImageFile != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(emp.ImageFile.FileName);
                    string extension = Path.GetExtension(emp.ImageFile.FileName);
                    HttpPostedFileBase postedFile = emp.ImageFile;
                    int lenth = postedFile.ContentLength;

                    if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                    {
                        if (lenth <= 1000000)
                        {
                            fileName = fileName + extension;
                            emp.Image_Path = "~/images" + fileName;
                            fileName = Path.Combine(Server.MapPath("~/images/"), fileName);
                            emp.ImageFile.SaveAs(fileName);
                            db.Entry(emp).State = System.Data.Entity.EntityState.Modified;
                            int a = db.SaveChanges();
                            if (a > 0)
                            {
                                TempData["UpdateMessage"] = "<script>alert('Data Updated Succesfully')</script>";
                                ModelState.Clear();
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                TempData["UpdateMessage"] = "<script>alert('Data not Updated')</script>";
                            }
                        }
                        else
                        {
                            TempData["SizeMessage"] = "<script>alert('Image size should be less then 1mb')</script>";
                        }
                    }
                    else
                    {
                        TempData["ExtensionMessage"] = "<script>alert('Format not Suported')</script>";
                    }
                }
                else
                {
                    emp.Image_Path = Session["Image"].ToString();
                    db.Entry(emp).State = System.Data.Entity.EntityState.Modified;
                    int a = db.SaveChanges();
                    if (a > 0)
                    {
                        TempData["UpdateMessage"] = "<script>alert('Data Updated Succesfully')</script>";
                        ModelState.Clear();
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["UpdateMessage"] = "<script>alert('Data not Updated')</script>";
                    }
                }
            }
                
            return View();
        }

        public ActionResult Delete(int id)
        {
            var row = db.Employees.Where(e => e.Id == id).FirstOrDefault();
            db.Employees.Remove(row);
            db.SaveChanges();
            TempData["DeleteMessage"] = "<script>alert('Data Deleted Succesfully')</script>";
            return RedirectToAction("Index", "Home");
        }
    }
}