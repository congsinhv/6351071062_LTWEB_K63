using bookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace bookStore.Controllers
{
    public class HomeController : Controller
    {
        BookStoreContext data = new BookStoreContext();
        // GET: Home
        public ActionResult Index()
        {
            //var chudeList = data.CHUDEs.ToList();
            //ViewBag.ChudeData = chudeList;
            return View();
        }
    }
}