using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DanhMuc_DAL : Connect
    {
        // 1. Load danh sách (Sử dụng hàm LoadData của Connect)
        public DataTable load()
        {
            return LoadData(@"SELECT CategoryID_N01, CategoryName_N01, Description_N01 FROM Categories_N01");
        }

        // 2. Thêm mới (Sử dụng hàm Execute với mảng tham số)
        public void InsertDanhMuc(DanhMuc_DTO ob)
        {
            string sql = @"INSERT INTO Categories_N01 (CategoryID_N01, CategoryName_N01, Description_N01)
                           VALUES (@CategoryID_N01, @CategoryName_N01, @Description_N01)";

            // Tạo mảng tham số truyền vào hàm Execute
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@CategoryID_N01", ob.CategoryId),
                new SqlParameter("@CategoryName_N01", ob.Name),
                new SqlParameter("@Description_N01", ob.Description)
            };

            Execute(sql, parameters);
        }

        // 3. Xóa (Sử dụng hàm Execute)
        public void DeleteDanhMuc(string CategoryId)
        {
            string sql = @"DELETE FROM Categories_N01 WHERE CategoryID_N01 = @Original_CategoryID_N01";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Original_CategoryID_N01", CategoryId)
            };

            Execute(sql, parameters);
        }

        // 4. Sửa (Sử dụng hàm Execute)
        public void UpdateDanhMuc(DanhMuc_DTO ob, string CategoryId)
        {
            string sql = @"UPDATE Categories_N01
                           SET CategoryName_N01 = @CategoryName_N01, Description_N01 = @Description_N01
                           WHERE CategoryID_N01 = @Original_CategoryID_N01";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@CategoryName_N01", ob.Name),
                new SqlParameter("@Description_N01", ob.Description),
                new SqlParameter("@Original_CategoryID_N01", CategoryId)
            };

            Execute(sql, parameters);
        }

        // 5. Lấy Dictionary danh mục (Giữ nguyên logic, gọi LoadData)
        public Dictionary<string, string> getDanhMuc()
        {
            Dictionary<string, string> mapDanhMuc = new Dictionary<string, string>();
            DataTable dt = LoadData(@"SELECT CategoryID_N01, CategoryName_N01, Description_N01 FROM Categories_N01");

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string maDanhMuc = row["CategoryID_N01"].ToString();
                    string tenDanhMuc = row["CategoryName_N01"].ToString();

                    if (!mapDanhMuc.ContainsKey(maDanhMuc))
                    {
                        mapDanhMuc.Add(maDanhMuc, tenDanhMuc);
                    }
                }
            }
            return mapDanhMuc;
        }

        // 6. Tìm kiếm (Tối ưu dùng LoadData(string, SqlParameter[]) thay vì tạo cmd thủ công)
        public DataTable FindDanhMuc(string tuKhoa)
        {
            string sql = @"SELECT CategoryID_N01, CategoryName_N01, Description_N01
                           FROM Categories_N01
                           WHERE (CategoryID_N01 LIKE N'%' + @TuKhoa + '%'
                               OR CategoryName_N01 LIKE N'%' + @TuKhoa + '%'
                               OR Description_N01 LIKE N'%' + @TuKhoa + '%')";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@TuKhoa", tuKhoa)
            };

            return LoadData(sql, parameters);
        }
    }
}