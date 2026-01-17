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
        public DataTable load()
        {
            return LoadData(@"SELECT CategoryID_N01, CategoryName_N01, Description_N01
FROM     Categories_N01");
        }

        public void InsertDanhMuc(DanhMuc_DTO ob)
        {
            SqlCommand cmd = new SqlCommand(@"INSERT INTO Categories_N01
                  (CategoryID_N01, CategoryName_N01, Description_N01)
VALUES (@CategoryID_N01,@CategoryName_N01,@Description_N01)   
            ");
            cmd.Parameters.AddWithValue("CategoryID_N01", ob.CategoryId);
            cmd.Parameters.AddWithValue("@CategoryName_N01", ob.Name);
            cmd.Parameters.AddWithValue("@Description_N01", ob.Description);
            ExcuteNonQuery(cmd);
        }

        public void DeleteDanhMuc(string CategoryId)
        {
            SqlCommand cmd = new SqlCommand(@"
                 DELETE FROM Categories_N01
WHERE  (CategoryID_N01 = @Original_CategoryID_N01)
            ");
            cmd.Parameters.AddWithValue("@Original_CategoryID_N01", CategoryId);
            ExcuteNonQuery(cmd);
        }

        public void UpdateDanhMuc(DanhMuc_DTO ob, string CategoryId)
        {
            SqlCommand cmd = new SqlCommand(@"UPDATE Categories_N01
SET         CategoryName_N01 = @CategoryName_N01, Description_N01 = @Description_N01
WHERE  (CategoryID_N01 = @Original_CategoryID_N01)
            ");
            cmd.Parameters.AddWithValue("@CategoryName_N01", ob.Name);
            cmd.Parameters.AddWithValue("@Description_N01", ob.Description);
            cmd.Parameters.AddWithValue("@Original_CategoryID_N01", CategoryId);
            ExcuteNonQuery(cmd);

        }
        public Dictionary<string, string> getDanhMuc()
        {
            Dictionary<string, string> mapDanhMuc = new Dictionary<string, string>();

            DataTable dt = LoadData(@"SELECT CategoryID_N01, CategoryName_N01, Description_N01
FROM     Categories_N01");

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string maDanhMuc = row["CategoryID_N01"].ToString();
                    String tenDanhMuc = row["CategoryName_N01"].ToString();

                    if (!mapDanhMuc.ContainsKey(maDanhMuc))
                    {
                        mapDanhMuc.Add(maDanhMuc, tenDanhMuc);
                    }
                }
            }
            return mapDanhMuc;

        }

        public DataTable FindDanhMuc(string tuKhoa)
        {
            SqlCommand cmd = new SqlCommand(@"
                SELECT CategoryID_N01, CategoryName_N01, Description_N01
FROM     Categories_N01
WHERE 
    (
        
        CategoryID_N01 LIKE N'%' + @TuKhoa + '%'
        
        
        OR CategoryName_N01 LIKE N'%' + @TuKhoa + '%'

      
        OR Description_N01 LIKE N'%' + @TuKhoa + '%'
    )
            ");
            cmd.Parameters.AddWithValue("@TuKhoa", tuKhoa);
            return loadDataByParameter(cmd);
        }
    }
}
