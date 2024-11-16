using bookStore.Models;
using PagedList;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Runtime.Remoting.Contexts;

namespace bookStore.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        BookStoreContext data = new BookStoreContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(FormCollection collection)
        {
            var tendn = Request.QueryString["TenDN"];
            var matkhau = Request.QueryString["Matkhau"];

            if (string.IsNullOrEmpty(tendn)) {
                ViewData["loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (string.IsNullOrEmpty(matkhau))
            {
                ViewData["loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                ADMIN ad = data.ADMINs.SingleOrDefault(n => n.Hoten == tendn && n.PassAdmin == matkhau && n.UserAdmin == "admin");
                if (ad != null) {
                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    ViewBag.Thongbao = "Tên đang nhập hoặc mật khẩu không đúng";
                }
            }
            return View();
        }

        public ActionResult Sach(int ? page) {
            int PageNumber = page ?? 1;
            int PageSize = 7;

            //return View(data.SACHes.ToList());
            return View(data.SACHes.ToList().OrderBy(n => n.Masach).ToPagedList(PageNumber, PageSize));
        }
        [HttpGet]
        public ActionResult ThemmoiSach()
        {
           
            ViewBag.MaCD=new SelectList(data.CHUDEs.ToList().OrderBy(n=>n.TenChuDe),"MaCD","TenChude");
            ViewBag.MaNXB = new SelectList(data.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            
            return View();

        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemmoiSach(SACH sach, HttpPostedFileBase fileUpload)
        {
            ViewBag.MaCD = new SelectList(data.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChude");
            ViewBag.MaNXB = new SelectList(data.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");

            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/images/Hinhsanpham"), fileName);

                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                    }

                    sach.Anhbia = fileName;
                    data.SACHes.Add(sach);
                    data.SaveChanges();
                    return RedirectToAction("Sach");
                }
                else
                {
                    ViewBag.Thongbao = "Dữ liệu không hợp lệ";
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                ViewBag.Thongbao = "Đã xảy ra lỗi: " + ex.Message;
            }

            return View();
        }

        public ActionResult Chitietsach(int id)
        {
            SACH sach = data.SACHes.SingleOrDefault(n => n.Masach == id);
            ViewBag.Masach = sach.Masach;
            if(sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }

        [HttpGet]
        public ActionResult Xoasach(int id)
        {
            SACH sach = data.SACHes.SingleOrDefault(n => n.Masach == id);
            ViewBag.Masach = sach.Masach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }

        public ActionResult XacNhanXoa(int id)
        {
            SACH sach = data.SACHes.SingleOrDefault(n => n.Masach == id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return RedirectToAction("Sach");
            }

            try
            {
                data.SACHes.Remove(sach);
                data.SaveChanges();
                return RedirectToAction("Sach");
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu cần
                ViewBag.Thongbao = "Đã xảy ra lỗi khi xóa: " + ex.Message;
                return RedirectToAction("Sach");
            }
        }

        // GET: Edit Book
        [HttpGet]
        public ActionResult Suasach(int id)
        {
            // Load the list of categories and publishers for the dropdown lists
            ViewBag.MaCD = new SelectList(data.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(data.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");

            // Retrieve the book by ID
            SACH sach = data.SACHes.SingleOrDefault(n => n.Masach == id);
            if (sach == null)
            {
                // If the book doesn't exist, return a 404 error
                Response.StatusCode = 404;
                return null;
            }

            // Return the view with the current book data
            return View(sach);
        }

        // POST: Edit Book (Save Changes)
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaSach(SACH sach, HttpPostedFileBase fileUpload)
        {
            // Re-populate dropdowns
            ViewBag.MaCD = new SelectList(data.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(data.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");

            // Validate the form data
            if (ModelState.IsValid)
            {
                try
                {
                    // Handle the image upload
                    if (fileUpload != null)
                    {
                        // Check if the uploaded file is valid
                        var fileName = Path.GetFileName(fileUpload.FileName);
                        var path = Path.Combine(Server.MapPath("~/Content/images/Hinhsanpham"), fileName);

                        // If the file already exists, you can either overwrite or show an error
                        if (System.IO.File.Exists(path))
                        {
                            ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                        }
                        else
                        {
                            // Save the file to the server
                            fileUpload.SaveAs(path);
                            sach.Anhbia = fileName; // Update the book's image field
                        }
                    }
                    else
                    {
                        // If no image is uploaded, retain the old image
                        sach.Anhbia = sach.Anhbia; // Maintain the current image name if not uploading a new one
                    }

                    // Update the book details
                    UpdateModel(sach);  // This will update the properties of the book object
                    data.SaveChanges(); // Save changes to the database

                    // Redirect to the book list
                    return RedirectToAction("Sach");
                }
                catch (Exception ex)
                {
                    // If an error occurs, show an error message
                    ViewBag.Thongbao = "Đã xảy ra lỗi: " + ex.Message;
                }
            }
            else
            {
                // If validation fails, show an error message
                ViewBag.Thongbao = "Dữ liệu không hợp lệ";
            }

            // Return the view with the model data if the validation fails
            return View(sach);
        }

        public ActionResult ThongKeSach()
        {
            // Get the list of books from the database (adjust based on your actual model and context)
            var books = data.SACHes.ToList();

            // Group books by 'TenChuDe' (category) and count the number of books per category
            var groupedData = books
                .GroupBy(b => b.CHUDE.TenChuDe)
                .Select(g => new {
                    TenChuDe = g.Key,
                    SoLuongTon = g.Sum(b => b.Soluongton)
                }).ToList();

            // Prepare data points for the chart
            List<DataPoint> dataPoints = new List<DataPoint>();
            foreach (var item in groupedData)
            {
                dataPoints.Add(new DataPoint(item.TenChuDe, (double)item.SoLuongTon));
            }

            // Serialize the data points and pass to the view
            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

            return View();
        }
     


    }

}