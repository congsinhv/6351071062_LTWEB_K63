using bookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace bookStore.Controllers
{
    public class GiohangController : Controller
    {
        // GET: Giohang
        BookStoreContext data = new BookStoreContext();
        public ActionResult Index()
        {
            return View();
        }

        public List<Giohang> layGiohang()
        {
            List<Giohang> lisstGiohang = Session["Giohang"] as List<Giohang>;
            if (lisstGiohang == null)
            {
                lisstGiohang=new List<Giohang>();
                Session["Giohang"]=lisstGiohang;
            }
            return lisstGiohang;
        }

        public ActionResult ThemGiohang(int iMasach, string strURL)
        {
            List<Giohang> listGiohang =layGiohang();
            Giohang sanpham = listGiohang.Find(n=>n.iMasach==iMasach);
            if (sanpham == null)
            {
                sanpham = new Giohang(iMasach);
                listGiohang.Add(sanpham);
                    return Redirect(strURL);
            }
            else
            {
                sanpham.iSoluong++;
                return Redirect(strURL);
            }
        }

        public int TongSoLuong()
        {
            int iTongSoluong = 0;
            List<Giohang> listGiohang = Session["Giohang"] as List<Giohang>;
            if(listGiohang != null)
            {
                iTongSoluong=listGiohang.Sum(n=>n.iSoluong);
            }
            return iTongSoluong;
        }
        private double TongTien()
        {
            double iTongTien = 0;
            List<Giohang> listGiohang = Session["Giohang"] as List<Giohang>;
            if(listGiohang != null)
            {
                iTongTien=listGiohang.Sum(n=>n.dThanhtien);
            }
            return iTongTien;
        }
        public ActionResult Giohang()
        {
            List<Giohang> lisstGiohang = layGiohang();
            if (lisstGiohang.Count == 0)
            {
                //return RedirectToAction("Index","BookStore");
                return View(lisstGiohang);
            }
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(lisstGiohang);

        }

        public ActionResult XoaGiohang(int iMaSP)
        {
            List<Giohang> listGiohang=layGiohang();
            Giohang sanpham = listGiohang.SingleOrDefault(n => n.iMasach == iMaSP);
            if (sanpham != null)
            {
                listGiohang.RemoveAll(n=>n.iMasach==iMaSP);
                return RedirectToAction("GioHang");
            }
            if(listGiohang.Count == 0)
            {
                return RedirectToAction("Index", "BookStore");
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult CapnhatGiohang(int iMaSP)
        {
            List<Giohang> listGiohang = layGiohang();
            Giohang sanpham = listGiohang.SingleOrDefault(n => n.iMasach == iMaSP);
            if (sanpham != null)
            {
                int soluong = 0;
                if (int.TryParse(Request.Form["txtSoluong"], out soluong))
                {
                    sanpham.iSoluong = soluong;
                }
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult XoaTatcaGiohang()
        {
            List<Giohang> listGiohang=layGiohang();
            listGiohang.Clear();
            return RedirectToAction("Index", "Bookstore");
        }

        [HttpGet]
        public ActionResult Dathang()
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("Dangnhap", "Nguoidung");
            }
            if (Session["Giohang"] == null)
            {
                return RedirectToAction("Index", "BookStore");
            }

            List<Giohang> listGioHang = layGiohang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();

            return View(listGioHang);
        }

        public ActionResult XacNhanDonHang(FormCollection collection)
        {
            // Create a new order
            DONDATHANG ddh = new DONDATHANG();

            // Get customer information from session
            KHACHHANG kh = (KHACHHANG)Session["Taikhoan"];
            List<Giohang> gh = layGiohang();

            // Set order details
            ddh.MaKH = kh.MaKH;
            ddh.Ngaydat = DateTime.Now;
            var ngaygiao = String.Format("{0:dd/MM/yyyy}", collection["Ngaygiao"]);
            ddh.Ngaygiao = DateTime.Parse(ngaygiao);
            ddh.Tinhtranggiaohang = false;
            ddh.Dathanhtoan = false;


            // Insert the order into the database
            data.DONDATHANGs.Add(ddh);
            data.SaveChanges();

            // Add order details for each item in the cart
            foreach (var item in gh)
            {
                CHITIETDONTHANG ctdh = new CHITIETDONTHANG();
                ctdh.MaDonHang = ddh.MaDonHang;
                ctdh.Masach = item.iMasach;
                ctdh.Soluong = item.iSoluong;
                ctdh.Dongia = (decimal)item.dDongia;

                data.CHITIETDONTHANGs.Add(ctdh);

            }

            // Submit changes to the database
            data.SaveChanges();

            // Clear the cart session
            Session["Giohang"] = null;

            return View();
        }
    }

}