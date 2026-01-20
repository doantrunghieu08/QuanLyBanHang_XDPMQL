
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

        public DataTable LayDuLieuBaoCao_TheoDanhMuc(string maDanhMuc)
        {
            // Cần lấy đủ các cột trùng tên với các textbox trong Report
            string sql = @"
        SELECT 
            -- Phần thông tin danh mục (Header của báo cáo)
            c.CategoryID_N01, 
            c.CategoryName_N01,
            
            -- Phần danh sách sản phẩm (Table chi tiết)
            p.ProductID_N01, 
            p.ProductName_N01, 
            p.Price_N01, 
            p.StockQuantity_N01,
            
            -- Tính tổng số lượng bán (nếu null thì là 0)
            ISNULL(SUM(d.Quantity_N01), 0) AS SoLuongDaBan,
            
            -- Tính tổng tiền thu được (Số lượng * Đơn giá lúc bán)
            ISNULL(SUM(d.Quantity_N01 * d.UnitPrice_N01), 0) AS SoTienThuDuoc

        FROM Categories_N01 c
        -- Join để lấy sản phẩm thuộc danh mục
        INNER JOIN Products_N01 p ON c.CategoryID_N01 = p.CategoryID_N01
        -- Left Join để lấy dữ liệu bán hàng (dùng LEFT để SP chưa bán vẫn hiện ra)
        LEFT JOIN OrderItems_N01 d ON p.ProductID_N01 = d.ProductID_N01
        
        WHERE c.CategoryID_N01 = @CateID

        -- Group By toàn bộ các cột không tính toán
        GROUP BY 
            c.CategoryID_N01, 
            c.CategoryName_N01,
            p.ProductID_N01, 
            p.ProductName_N01, 
            p.Price_N01, 
            p.StockQuantity_N01";

            SqlParameter[] para = {
        new SqlParameter("@CateID", maDanhMuc)
    };

            return LoadData(sql,para);
        }
    }
}