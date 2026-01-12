using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient; // Thư viện chính cho SQL Server

namespace DAL
{
    public class Connect
    {
        // 1. Chuỗi kết nối DB (Đúng theo yêu cầu của bạn)
        private string strCon = @"Data Source=TRUNGHIEU\SQLEXPRESS;Initial Catalog=QuanLyBanHang_N01;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";

        // 2. Hàm trả về đối tượng kết nối (Dùng cho các hàm xử lý riêng bên ngoài nếu cần)
        public SqlConnection GetConnection()
        {
            return new SqlConnection(strCon);
        }

        // 3. Hàm LoadData: Dùng cho câu lệnh SELECT -> Trả về DataTable
        // Tham số 'parameters' để mặc định là null để dùng được cho cả câu SQL không có tham số
        public DataTable LoadData(string sql, SqlParameter[] parameters = null)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        // Nếu có tham số truyền vào thì thêm vào cmd
                        if (parameters != null && parameters.Length > 0)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Ném lỗi ra để tầng UI hoặc BUS biết đường xử lý
                    throw new Exception("Lỗi kết nối hoặc truy vấn: " + ex.Message);
                }
            }
            return dt;
        }

        // 4. Hàm Execute: Dùng cho INSERT, UPDATE, DELETE -> Trả về số dòng bị ảnh hưởng
        public int Execute(string sql, SqlParameter[] parameters = null)
        {
            int rowsAffected = 0;
            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        if (parameters != null && parameters.Length > 0)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }
                        rowsAffected = cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi thực thi lệnh: " + ex.Message);
                }
            }
            return rowsAffected;
        }

        // 5. Hàm ExecuteTransaction: Dùng để thực hiện một chuỗi lệnh (Ví dụ: Xóa Chi tiết trước -> Xóa Đơn hàng sau)
        // Nếu 1 lệnh lỗi thì Rollback (hoàn tác) toàn bộ để tránh lỗi dữ liệu rác
        public void ExecuteTransaction(List<SqlCommand> commandList)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        foreach (SqlCommand cmd in commandList)
                        {
                            cmd.Connection = conn;
                            cmd.Transaction = transaction;
                            cmd.ExecuteNonQuery();
                        }
                        // Nếu chạy hết vòng lặp mà không lỗi thì Commit (Lưu)
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        // Nếu có lỗi bất kỳ đâu thì Rollback (Hoàn tác về trạng thái ban đầu)
                        transaction.Rollback();
                        throw new Exception("Lỗi giao dịch (Transaction): " + ex.Message);
                    }
                }
            }
        }
    }
}