using DAL;

using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BUS
{
    public class DanhMuc_BUS
    {
        DanhMuc_DAL danhmuc_DAL = new DanhMuc_DAL();
        public DataTable loadData()
        {
            return danhmuc_DAL.load();
        }

        public void InsertDanhMuc(DanhMuc_DTO ob)
        {
            danhmuc_DAL.InsertDanhMuc(ob);
        }

        public void DeleteDanhMuc(string CategoryId)
        {
            danhmuc_DAL.DeleteDanhMuc(CategoryId);
        }
        public void UpdateDocGia(DanhMuc_DTO ob, string CategoryId)
        {
            danhmuc_DAL.UpdateDanhMuc(ob, CategoryId);
        }
        public Dictionary<string, string> getDanhMuc()
        {
            return danhmuc_DAL.getDanhMuc();
        }

        public DataTable findDanhMuc(string tuKhoa)
        {
            return danhmuc_DAL.FindDanhMuc(tuKhoa);
        }
    }
}
