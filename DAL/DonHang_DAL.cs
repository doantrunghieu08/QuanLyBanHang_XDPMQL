using DTO; // Đảm bảo DTO của bạn đã cập nhật các thuộc tính string cho ID
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient; // Thư viện SQL Server
using System.Text;

namespace DAL
{
    public class DonHang_DAL
    {
       
        Connect db = new Connect();

   
        public DataTable GetAllOrders()
        {
            string sql = @"
                SELECT 
                    o.OrderID_N01, 
                    o.OrderDate_N01, 
                    o.TotalAmount_N01, 
                    o.Address_N01,
                    o.Phone_N01,
                    
                    u.Username_N01,  -- Lấy tên người đặt từ bảng User
                    s.StatusName_N01 -- Lấy tên trạng thái từ bảng OrderStatus
                FROM Orders_N01 o
                JOIN User_N01 u ON o.UserID_N01 = u.UserID_N01
                JOIN OrderStatus_N01 s ON o.StatusID_N01 = s.StatusID_N01
                ORDER BY o.OrderDate_N01 DESC";

            return db.LoadData(sql);
        }

 
        public DataTable GetOrderDetails(string orderId)
        {
          
            string sql = @"
                SELECT 
                    p.ProductName_N01, 
                    d.Quantity_N01, 
                    d.UnitPrice_N01,
                    d.Unit_N01, -- Đơn vị tính (nếu cần hiển thị)
                    (d.Quantity_N01 * d.UnitPrice_N01) AS ThanhTien
                FROM OrderItems_N01 d
                JOIN Products_N01 p ON d.ProductID_N01 = p.ProductID_N01
                WHERE d.OrderID_N01 = @OrderID";

            SqlParameter[] para = {
                new SqlParameter("@OrderID", orderId) 
            };

            return db.LoadData(sql, para);
        }

        
        public DataTable GetOrderStatuses()
        {
            return db.LoadData("SELECT StatusID_N01, StatusName_N01 FROM OrderStatus_N01");
        }

        
        public void UpdateOrderStatus(string orderId, string statusId)
        {
            string sql = "UPDATE Orders_N01 SET StatusID_N01 = @StatusID WHERE OrderID_N01 = @OrderID";

            SqlParameter[] para = {
                new SqlParameter("@StatusID", statusId),
                new SqlParameter("@OrderID", orderId)
            };

            db.Execute(sql, para);
        }

 
        public void DeleteOrder(string orderId)
        {
            List<SqlCommand> cmdList = new List<SqlCommand>();

           
            SqlCommand cmdItems = new SqlCommand("DELETE FROM OrderItems_N01 WHERE OrderID_N01 = @OrderID");
            cmdItems.Parameters.AddWithValue("@OrderID", orderId);
            cmdList.Add(cmdItems);

     
            SqlCommand cmdOrder = new SqlCommand("DELETE FROM Orders_N01 WHERE OrderID_N01 = @OrderID");
            cmdOrder.Parameters.AddWithValue("@OrderID", orderId);
            cmdList.Add(cmdOrder);

           
            db.ExecuteTransaction(cmdList);
        }

  
        public DataTable SearchOrders(string keyword, DateTime fromDate, DateTime toDate)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
                SELECT 
                    o.OrderID_N01, o.OrderDate_N01, o.TotalAmount_N01, o.Address_N01, o.Phone_N01,
                    u.Username_N01, s.StatusName_N01
                FROM Orders_N01 o
                JOIN User_N01 u ON o.UserID_N01 = u.UserID_N01
                JOIN OrderStatus_N01 s ON o.StatusID_N01 = s.StatusID_N01
                WHERE o.OrderDate_N01 BETWEEN @FromDate AND @ToDate");

            List<SqlParameter> para = new List<SqlParameter>();

          
            para.Add(new SqlParameter("@FromDate", fromDate.Date));
            para.Add(new SqlParameter("@ToDate", toDate.Date.AddDays(1).AddTicks(-1)));

            if (!string.IsNullOrEmpty(keyword))
            {
             
                sql.Append(" AND (u.Username_N01 LIKE @Kw OR o.OrderID_N01 LIKE @Kw)");
                para.Add(new SqlParameter("@Kw", "%" + keyword + "%"));
            }

            return db.LoadData(sql.ToString(), para.ToArray());
        }

   
        public int GetTotalRevenue()
        {
            
            string sql = "SELECT SUM(TotalAmount_N01) FROM Orders_N01";
            DataTable dt = db.LoadData(sql);

            if (dt.Rows.Count > 0 && dt.Rows[0][0] != DBNull.Value)
            {
                return Convert.ToInt32(dt.Rows[0][0]);
            }
            return 0;
        }
    }
}