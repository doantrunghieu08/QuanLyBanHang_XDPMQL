
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
    public class SanPham_DAL : Connect
    {
        // 1. Load danh sách sản phẩm
        public DataTable load()
        {
            return LoadData(@"SELECT ProductID_N01, ProductName_N01, Price_N01, StockQuantity_N01, CategoryID_N01, ImageURL_N01, description_N01
                              FROM Products_N01");
        }

        // 2. Thêm mới sản phẩm
        public void InsertSanPham(SanPham_DTO ob)
        {
            string sql = @"INSERT INTO Products_N01
                              (ProductID_N01, ProductName_N01, Price_N01, StockQuantity_N01, CategoryID_N01, ImageURL_N01, description_N01)
                           VALUES (@ProductID_N01, @ProductName_N01, @Price_N01, @StockQuantity_N01, @CategoryID_N01, @ImageURL_N01, @description_N01)";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ProductID_N01", ob.ProductId),
                new SqlParameter("@ProductName_N01", ob.ProductName),
                new SqlParameter("@Price_N01", ob.Price),
                new SqlParameter("@StockQuantity_N01", ob.StockQuantity),
                new SqlParameter("@CategoryID_N01", ob.CategoryID),
                new SqlParameter("@ImageURL_N01", ob.ImageUrl),
                new SqlParameter("@description_N01", ob.Description)
            };

            Execute(sql, parameters);
        }

        // 3. Xóa sản phẩm
        public void DeleteSanPham(string ProductId)
        {
            string sql = @"DELETE FROM Products_N01 WHERE ProductID_N01 = @Original_ProductID_N01";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Original_ProductID_N01", ProductId)
            };

            Execute(sql, parameters);
        }

        // 4. Cập nhật sản phẩm
        public void UpdateSanPham(SanPham_DTO ob, string ProductId)
        {
            string sql = @"UPDATE Products_N01
                           SET ProductName_N01 = @ProductName_N01, 
                               Price_N01 = @Price_N01, 
                               StockQuantity_N01 = @StockQuantity_N01, 
                               CategoryID_N01 = @CategoryID_N01, 
                               ImageURL_N01 = @ImageURL_N01, 
                               description_N01 = @description_N01
                           WHERE ProductID_N01 = @Original_ProductID_N01";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ProductName_N01", ob.ProductName),
                new SqlParameter("@Price_N01", ob.Price),
                new SqlParameter("@StockQuantity_N01", ob.StockQuantity),
                new SqlParameter("@CategoryID_N01", ob.CategoryID),
                new SqlParameter("@ImageURL_N01", ob.ImageUrl),
                new SqlParameter("@description_N01", ob.Description),
                new SqlParameter("@Original_ProductID_N01", ProductId)
            };

            Execute(sql, parameters);
        }

        // 5. Lấy Dictionary (Để đổ vào ComboBox nếu cần)
        public Dictionary<string, string> getSanPham()
        {
            Dictionary<string, string> mapSanPham = new Dictionary<string, string>();

            DataTable dt = LoadData(@"SELECT ProductID_N01, ProductName_N01 FROM Products_N01");

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string maSanPham = row["ProductID_N01"].ToString();
                    string tenSanPham = row["ProductName_N01"].ToString();

                    if (!mapSanPham.ContainsKey(maSanPham))
                    {
                        mapSanPham.Add(maSanPham, tenSanPham);
                    }
                }
            }
            return mapSanPham;
        }

        // 6. Tìm kiếm sản phẩm
        public DataTable FindSanPham(string tuKhoa)
        {
            string sql = @"SELECT ProductID_N01, ProductName_N01, Price_N01, StockQuantity_N01, CategoryID_N01, ImageURL_N01, description_N01
                           FROM Products_N01
                           WHERE (ProductID_N01 LIKE N'%' + @TuKhoa + '%'
                               OR ProductName_N01 LIKE N'%' + @TuKhoa + '%'
                               OR Price_N01 LIKE N'%' + @TuKhoa + '%'
                               OR StockQuantity_N01 LIKE N'%' + @TuKhoa + '%'
                               OR CategoryID_N01 LIKE N'%' + @TuKhoa + '%'
                               OR ImageURL_N01 LIKE N'%' + @TuKhoa + '%'
                               OR description_N01 LIKE N'%' + @TuKhoa + '%')";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@TuKhoa", tuKhoa)
            };

            return LoadData(sql, parameters);
        }
    }
}