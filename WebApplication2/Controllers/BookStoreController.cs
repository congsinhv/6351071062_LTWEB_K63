using System;
using System.Collections.Generic;
using WebApplication2.Models;
using System.Web.Mvc;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;

namespace WebApplication2.Controllers
{

    public class BookStoreController : Controller
    {
        Bookstore dbContext = new Bookstore();
        // GET: BookStore
        public ActionResult Index()
        {
            var listItem = dbContext.SACHes.Take(5).ToList();
            return View(listItem);
        }

        public ActionResult ChuDe(int id)
        {
            var listBooks = dbContext.SACHes.Where(b => b.MaCD == id).ToList();
            return View(listBooks);
        }

        public ActionResult NhaXuatBan(int id)
        {
            var listBooks = dbContext.SACHes.Where(b => b.MaNXB == id).ToList();
            return View(listBooks);
        }

        public PartialViewResult LayChuDe()
        {
            var categories = dbContext.CHUDEs.ToList();
            return PartialView(categories);
        }

        public PartialViewResult LayNXB()
        {
            var catogory = dbContext.NHAXUATBANs.ToList();
            return PartialView(catogory);
        }

        public ActionResult Details(int id)
        {
            var bookDetails = dbContext.SACHes.FirstOrDefault(b => b.Masach == id);
            return View(bookDetails);
        }
    }
}