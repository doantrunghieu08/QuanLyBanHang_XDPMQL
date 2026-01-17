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
        public DataTable load()
        {
            return LoadData(@"SELECT ProductID_N01, ProductName_N01, Price_N01, StockQuantity_N01, CategoryID_N01, ImageURL_N01, description_N01
FROM     Products_N01");
        }

        public void InsertSanPham(SanPham_DTO ob)
        {
            SqlCommand cmd = new SqlCommand(@"INSERT INTO Products_N01
                  (ProductID_N01, ProductName_N01, Price_N01, StockQuantity_N01, CategoryID_N01, ImageURL_N01, description_N01)
VALUES (@ProductID_N01,@ProductName_N01,@Price_N01,@StockQuantity_N01,@CategoryID_N01,@ImageURL_N01,@description_N01)  
            ");
            cmd.Parameters.AddWithValue("ProductID_N01", ob.ProductId);
            cmd.Parameters.AddWithValue("@ProductName_N01", ob.ProductName);
            cmd.Parameters.AddWithValue("@Price_N01", ob.Price);
            cmd.Parameters.AddWithValue("@StockQuantity_N01", ob.StockQuantity);
            cmd.Parameters.AddWithValue("@CategoryID_N01", ob.CategoryID);
            cmd.Parameters.AddWithValue("@ImageURL_N01", ob.ImageUrl);
            cmd.Parameters.AddWithValue("@description_N01", ob.Description);
            ExcuteNonQuery(cmd);
        }

        public void DeleteSanPham(string ProductId)
        {
            SqlCommand cmd = new SqlCommand(@"
                 DELETE FROM Products_N01
WHERE  (ProductID_N01 = @Original_ProductID_N01) 
            ");
            cmd.Parameters.AddWithValue("@Original_ProductID_N01", ProductId);
            ExcuteNonQuery(cmd);
        }

        public void UpdateSanPham(SanPham_DTO ob, string ProductId)
        {
            SqlCommand cmd = new SqlCommand(@"UPDATE Products_N01
SET          ProductName_N01 = @ProductName_N01, Price_N01 = @Price_N01, StockQuantity_N01 = @StockQuantity_N01, CategoryID_N01 = @CategoryID_N01, ImageURL_N01 = @ImageURL_N01, 
                  description_N01 = @description_N01
WHERE  (ProductID_N01 = @Original_ProductID_N01)
            ");
            cmd.Parameters.AddWithValue("@ProductName_N01", ob.ProductName);
            cmd.Parameters.AddWithValue("@Price_N01", ob.Price);
            cmd.Parameters.AddWithValue("@StockQuantity_N01", ob.StockQuantity);
            cmd.Parameters.AddWithValue("@CategoryID_N01", ob.CategoryID);
            cmd.Parameters.AddWithValue("@ImageURL_N01", ob.ImageUrl);
            cmd.Parameters.AddWithValue("@description_N01", ob.Description);
            cmd.Parameters.AddWithValue("@Original_ProductID_N01", ProductId);
            ExcuteNonQuery(cmd);

        }
        public Dictionary<string, string> getSanPham()
        {
            Dictionary<string, string> mapSanPham = new Dictionary<string, string>();

            DataTable dt = LoadData(@"SELECT ProductID_N01, ProductName_N01, Price_N01, StockQuantity_N01, CategoryID_N01, ImageURL_N01, description_N01
FROM     Products_N01");

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string maSanPham = row["ProductID_N01"].ToString();
                    String tenSanPham = row["ProductName_N01"].ToString();

                    if (!mapSanPham.ContainsKey(maSanPham))
                    {
                        mapSanPham.Add(maSanPham, tenSanPham);
                    }
                }
            }
            return mapSanPham;

        }

        public DataTable FindSanPham(string tuKhoa)
        {
            SqlCommand cmd = new SqlCommand(@"
                SELECT ProductID_N01, ProductName_N01, Price_N01, StockQuantity_N01, CategoryID_N01, ImageURL_N01, description_N01
FROM     Products_N01
WHERE 
    (
        
        ProductID_N01 LIKE N'%' + @TuKhoa + '%'
        
        
        OR ProductName_N01 LIKE N'%' + @TuKhoa + '%'

      
        OR Price_N01 LIKE N'%' + @TuKhoa + '%'

        
        OR StockQuantity_N01 LIKE N'%' + @TuKhoa + '%'

        
        OR CategoryID_N01 LIKE N'%' + @TuKhoa + '%'

        
        OR ImageURL_N01 LIKE N'%' + @TuKhoa + '%'

        
        OR description_N01 LIKE N'%' + @TuKhoa + '%'
    )
            ");
            cmd.Parameters.AddWithValue("@TuKhoa", tuKhoa);
            return loadDataByParameter(cmd);
        }
    }
}
