using bookStore.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace bookStore.Controllers
{
    public class NguoidungController : Controller
    {
        // GET: Nguoidung
        BookStoreContext data = new BookStoreContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Dangky() 
        { 
            return View();
        }

        [HttpPost]
        public ActionResult Dangky(FormCollection collection, KHACHHANG kh)
        {
            var hoten = collection["HotenKH"];
            var tendn = collection["TenDN"];
            var matkhau=collection["Matkhau"];
            var matkhaunhaplai = collection["Matkhaunhaplai"];
            var diachi = collection["Diachi"];
            var email=collection["email"];
            var dienthoai = collection["Dienthoai"];
            //var ngaysinh = DateTime.ParseExact(collection["Ngaysinh"], "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

            DateTime parsedDate;
            var dateInput = collection["Ngaysinh"];
            if (DateTime.TryParseExact(dateInput, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
            {
                kh.Ngaysinh = parsedDate;
            }
            else
            {
                ViewData["Loi7"] = "Ngày sinh không hợp lệ.";
            }

            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Không để trống Họ và tên";
            }
            else if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi2"] = "Phải nhập tên đăng nhập";
            }
            else if(String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi3"] = "Phải nhập mật khẩu";
            }
            else if (String.IsNullOrEmpty(matkhaunhaplai))
            {
                ViewData["Loi4"] = "Phải nhập lại mật khẩu";
            }

            if(String.IsNullOrEmpty(email))
            {
                ViewData["Loi5"] = "Email không được bỏ trống";
            }

            if (String.IsNullOrEmpty(dienthoai))
            {
                ViewData["Loi6"] = "Phải nhập số điện thoại";
            }
            else
            {
                kh.HoTen=hoten;
                kh.Taikhoan=tendn;
                kh.Matkhau=matkhau;
                kh.Email=email;
                kh.DiachiKH=diachi;
                kh.DienthoaiKH=dienthoai;
                

                // Thêm đối tượng KHACHHANG vào DbSet KHACHHANGs
                data.KHACHHANGs.Add(kh);

                // Gọi SaveChanges() để lưu đối tượng vào cơ sở dữ liệu
                data.SaveChanges();

                // Sử dụng TempData để truyền thông báo đăng ký thành công
                TempData["ThongBao"] = "Đăng ký thành công! Bạn có thể đăng nhập ngay.";

                // Chuyển hướng đến trang đăng nhập
                return RedirectToAction("Dangnhap", "Nguoidung");

            }
            return View();
        }

  
        public ActionResult Dangnhap(FormCollection collection)
        {
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Không để trống Họ và tên";
            }
            else if(String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Không để trống mật khẩu";
            }    
            else
            {
                KHACHHANG kh = data.KHACHHANGs.SingleOrDefault(n=>n.Taikhoan == tendn);
                if (kh != null)
                {
                    ViewBag.Thongbao = "Đăng nhập thành công";
                    Session["Taikhoan"] = kh;
                    return RedirectToAction("Index", "BookSTore");
                }
                else
                {
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }    
            return View();
        }
    }
}