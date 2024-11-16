﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bookStore.Models
{
    public class Giohang
    {
        BookStoreContext data = new BookStoreContext();

        public int iMasach { get; set; }

        public string sTensach { get; set; }

        public string sAnhbia { get; set; }
        public double dDongia {  get; set; }
         public int iSoluong {  get; set; }
        public Double dThanhtien
        {
            get { return iSoluong * dDongia; }
        }
        public Giohang(int Masach)
        {
            int iMasach = Masach;
            SACH sach=data.SACHes.Single(n=>n.Masach == iMasach);
            sTensach = sach.Tensach;
            sAnhbia = sach.Anhbia;
            dDongia=double.Parse(sach.Giaban.ToString());
            iSoluong = 1;
        }
    }
    
}