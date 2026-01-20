using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient; // Thư viện SQL Server
using DTO;

namespace DAL
{
    public class ThanhToan_DAL
    {
        Connect db = new Connect();

        // 1. Lấy danh sách Phương thức (ComboBox)
        public DataTable GetMethods()
        {
            return db.LoadData("SELECT MethodID_N01, MethodName_N01 FROM PaymentMethods_N01");
        }

        // 2. Lấy danh sách Trạng thái (ComboBox)
        public DataTable GetStatuses()
        {
            return db.LoadData("SELECT StatusID_N01, StatusName_N01 FROM PaymentStatus_N01");
        }

        // 3. Tìm kiếm thanh toán (Lấy danh sách)
        public List<ThanhToan_DTO> SearchPayments_AllFields(string keyword)
        {
            List<ThanhToan_DTO> list = new List<ThanhToan_DTO>();
            string searchKey = "%" + keyword + "%"; // Chuẩn bị từ khóa cho LIKE

            string sql = @"
        SELECT 
            p.PaymentID_N01,
            p.OrderID_N01, 
            p.Amount_N01, 
            p.PaymentDate_N01,
            p.MethodID_N01, 
            m.MethodName_N01,
            p.StatusID_N01, 
            ps.StatusName_N01
        FROM Payments_N01 p
        LEFT JOIN PaymentMethods_N01 m ON p.MethodID_N01 = m.MethodID_N01
        LEFT JOIN PaymentStatus_N01 ps ON p.StatusID_N01 = ps.StatusID_N01
        WHERE 
            -- 1. Tìm theo Mã thanh toán
            p.PaymentID_N01 LIKE @kw
            
            -- 2. Tìm theo Mã đơn hàng
            OR p.OrderID_N01 LIKE @kw
            
            -- 3. Tìm theo Tên phương thức (Ví dụ: 'Momo', 'Tiền mặt')
            OR m.MethodName_N01 LIKE @kw
            
            -- 4. Tìm theo Tên trạng thái (Ví dụ: 'Thành công')
            OR ps.StatusName_N01 LIKE @kw
            
            -- 5. Tìm theo Số tiền (Phải ép kiểu số sang chuỗi mới so sánh được)
            OR CAST(p.Amount_N01 AS NVARCHAR(50)) LIKE @kw
            
            -- 6. (Tùy chọn) Tìm theo ngày tháng dd/MM/yyyy
            -- OR CONVERT(VARCHAR(10), p.PaymentDate_N01, 103) LIKE @kw
    ";

            using (SqlConnection conn = (SqlConnection)db.GetConnection())
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        // Chỉ cần 1 tham số duy nhất
                        cmd.Parameters.Add(new SqlParameter("@kw", searchKey));

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ThanhToan_DTO item = new ThanhToan_DTO();
                                item.PaymentID = reader["PaymentID_N01"].ToString();
                                item.OrderID = reader["OrderID_N01"].ToString();

                                if (reader["Amount_N01"] != DBNull.Value)
                                    item.Amount = Convert.ToInt32(reader["Amount_N01"]);

                                if (reader["PaymentDate_N01"] != DBNull.Value)
                                    item.PaymentDate = Convert.ToDateTime(reader["PaymentDate_N01"]);

                                item.MethodID = reader["MethodID_N01"].ToString();
                                item.MethodName = reader["MethodName_N01"] != DBNull.Value ? reader["MethodName_N01"].ToString() : "";

                                item.StatusID = reader["StatusID_N01"].ToString();
                                item.StatusName = reader["StatusName_N01"] != DBNull.Value ? reader["StatusName_N01"].ToString() : "";

                                list.Add(item);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi SearchPayments_AllFields: " + ex.Message);
                }
            }
            return list;
        }


        public bool InsertPayment(ThanhToan_DTO p)
        {
            
            string sql = @"INSERT INTO Payments_N01 (PaymentID_N01, OrderID_N01, Amount_N01, MethodID_N01, StatusID_N01, PaymentDate_N01) 
                           VALUES (@PayID, @OrdID, @Amt, @Mid, @Sid, @Date)";

            SqlParameter[] para = {
                new SqlParameter("@PayID", p.PaymentID),
                new SqlParameter("@OrdID", p.OrderID),
                new SqlParameter("@Amt", p.Amount),
                new SqlParameter("@Mid", p.MethodID),
                new SqlParameter("@Sid", p.StatusID),
                new SqlParameter("@Date", p.PaymentDate)
            };

            return db.Execute(sql, para) > 0;
        }

        
        public bool UpdatePayment(ThanhToan_DTO p)
        {
            string sql = @"UPDATE Payments_N01 
                           SET OrderID_N01 = @OrdID,
                               Amount_N01 = @Amt,
                               MethodID_N01 = @Mid, 
                               StatusID_N01 = @Sid,
                               PaymentDate_N01 = @Date
                           WHERE PaymentID_N01 = @PayID";

            SqlParameter[] para = {
                new SqlParameter("@PayID", p.PaymentID), // Khóa chính để tìm dòng sửa
                new SqlParameter("@OrdID", p.OrderID),
                new SqlParameter("@Amt", p.Amount),
                new SqlParameter("@Mid", p.MethodID),
                new SqlParameter("@Sid", p.StatusID),
                new SqlParameter("@Date", p.PaymentDate)
            };

            return db.Execute(sql, para) > 0;
        }

        
        public bool DeletePayment(string paymentId)
        {
            string sql = "DELETE FROM Payments_N01 WHERE PaymentID_N01 = @PayID";

            SqlParameter[] para = {
                new SqlParameter("@PayID", paymentId)
            };

            return db.Execute(sql, para) > 0;
        }

        
        public bool CheckPaymentIDExists(string id)
        {
            string sql = "SELECT COUNT(*) FROM Payments_N01 WHERE PaymentID_N01 = @ID";
            SqlParameter[] para = { new SqlParameter("@ID", id) };
            DataTable dt = db.LoadData(sql, para);
            if (dt.Rows.Count > 0 && Convert.ToInt32(dt.Rows[0][0]) > 0)
                return true;
            return false;
        }

        public DataTable loadTrangThaiThanhToan()
        {
            return db.LoadData("SELECT StatusID_N01, StatusName_N01 FROM dbo.PaymentStatus_N01");

        }
        // Lấy danh sách tất cả thanh toán (kèm tên phương thức và trạng thái)
        public DataTable GetAllPayments()
        {
            string sql = @"
        SELECT 
            p.PaymentID_N01,
            p.OrderID_N01, 
            p.Amount_N01, 
            p.PaymentDate_N01,
            p.MethodID_N01, 
            m.MethodName_N01,    -- Lấy tên phương thức
            p.StatusID_N01, 
            ps.StatusName_N01    -- Lấy tên trạng thái
        FROM Payments_N01 p
        LEFT JOIN PaymentMethods_N01 m ON p.MethodID_N01 = m.MethodID_N01
        LEFT JOIN PaymentStatus_N01 ps ON p.StatusID_N01 = ps.StatusID_N01
        ORDER BY p.PaymentDate_N01 DESC"; // Sắp xếp ngày mới nhất lên đầu

            return db.LoadData(sql);
        }
        public DataTable LayChiTietThanhToan_TheoPhuongThuc(string maPhuongThuc)
        {
            string sql = @"
        SELECT 
            -- 1. Thông tin Phương thức (Dùng cho Header báo cáo)
            m.MethodID_N01,
            m.MethodName_N01,

            -- 2. Thông tin Chi tiết giao dịch (Dùng cho Table báo cáo)
            p.PaymentID_N01,
            p.OrderID_N01,
            p.Amount_N01,
            p.PaymentDate_N01,
            
            -- 3. Tên trạng thái (Thay vì hiện mã số khó hiểu)
            s.StatusName_N01

        FROM PaymentMethods_N01 m
        -- Join với bảng Payments để lấy các giao dịch
        JOIN Payments_N01 p ON m.MethodID_N01 = p.MethodID_N01
        -- Join với bảng PaymentStatus để lấy tên trạng thái (Thành công/Thất bại...)
        LEFT JOIN PaymentStatus_N01 s ON p.StatusID_N01 = s.StatusID_N01

        WHERE m.MethodID_N01 = @MethodID

        -- Sắp xếp: Giao dịch mới nhất lên đầu
        ORDER BY p.PaymentDate_N01 DESC";

            SqlParameter[] para = {
        new SqlParameter("@MethodID", maPhuongThuc)
    };

            return db.LoadData(sql, para);
        }
    }
}