using DAL;
using DTO;
using System.Collections.Generic;
using System.Data;

namespace BUS
{
    public class SanPham_BUS
    {
        SanPham_DAL sanPham_DAL = new SanPham_DAL();
        public DataTable loadData()
        {
            return sanPham_DAL.load();
        }

        public void InsertSanPham(SanPham_DTO ob)
        {
            sanPham_DAL.InsertSanPham(ob);
        }

        public void DeleteSanPham(string ProductId)
        {
            sanPham_DAL.DeleteSanPham(ProductId);
        }
        public void UpdateSanPham(SanPham_DTO ob, string ProductId)
        {
            sanPham_DAL.UpdateSanPham(ob, ProductId);
        }
        public Dictionary<string, string> getSanPham()
        {
            return sanPham_DAL.getSanPham();
        }

        public DataTable findSanPham(string tuKhoa)
        {
            return sanPham_DAL.FindSanPham(tuKhoa);
        }
        public DataTable thongKeSanPham(string maDanhMuc)
        {
            return sanPham_DAL.LayDuLieuBaoCao_TheoDanhMuc(maDanhMuc);
        }
    }
}
