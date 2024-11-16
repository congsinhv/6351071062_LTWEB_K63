using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using bookStore.Models;

using PagedList;
using PagedList.Mvc;

namespace bookStore.Controllers
{
    public class BookstoreController : Controller
    {
        // GET: Bookstore
        BookStoreContext data= new BookStoreContext();

        private List<SACH> laysachmoi(int count)
        {
            var firstBooks = data.SACHes
          .Take(count) // Lấy số lượng sách được chỉ định (count)
          .ToList();

            return firstBooks;
        }

        private List<SACH> laysachtheomacd(int MaCD)
        {
            var listBooks = data.SACHes.Where(b => b.MaCD == MaCD).ToList();
            return listBooks;
        }


        private List<SACH> laysachtheomanxb(int MaNXB)
        {
            var listBooks = data.SACHes.Where(b => b.MaNXB == MaNXB).ToList();
            return listBooks;
        }


        private SACH laychitietsach(int maSach)
        {
            var book = data.SACHes.FirstOrDefault(b => b.Masach == maSach);
            return book;
        }
        public ActionResult Index(int ? page)
        {
            int pageSize = 5;
            int pageNum = (page ?? 1);

            var firstBooks = laysachmoi(15);
            return View(firstBooks.ToPagedList(pageNum,pageSize));
        }

        public PartialViewResult LayChuDe()
        {
            var categories = data.CHUDEs.ToList();
            return PartialView(categories);
        }

        public PartialViewResult LayNXB()
        {
            var catogory = data.NHAXUATBANs.ToList();
            return PartialView(catogory);
        }

        public ActionResult Chude(int id)
        {
            var response = laysachtheomacd(id);
            return View(response);
        }

        public ActionResult NhaXuatBan(int id)
        {
            return View(laysachtheomanxb(id));
        }

        public ActionResult Details(int id)
        {
            return View(laychitietsach(id));
        }
    }
}