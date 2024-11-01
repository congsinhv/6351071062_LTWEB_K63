using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        Bookstore dbContext = new Bookstore();
        // GET: BookStore
        public ActionResult Index()
        {
            var listItem = dbContext.SACHes.Take(5).ToList();
            return View(listItem);
        }
    }
}