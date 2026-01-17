using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq; // Dùng để xử lý dữ liệu tiện hơn nếu cần

namespace BUS
{
    public class DonHang_BUS
    {
        private readonly DonHang_DAL dal;

        public DonHang_BUS()
        {
            dal = new DonHang_DAL();
        }

        #region 1. Lấy dữ liệu (Read)

        /// <summary>
        /// Lấy toàn bộ danh sách đơn hàng.
        /// </summary>
        public DataTable GetDanhSachDonHang()
        {
            try
            {
                return dal.GetAllOrders();
            }
            catch (Exception ex)
            {
                // Ghi log lỗi nếu cần thiết
                throw ex; // Ném lỗi ra để Form hiển thị MessageBox
            }
        }

        /// <summary>
        /// Lấy danh sách trạng thái để đổ vào ComboBox.
        /// </summary>
        public DataTable GetDanhSachTrangThai()
        {
            return dal.GetOrderStatuses();
        }

        /// <summary>
        /// Lấy chi tiết sản phẩm của một đơn hàng.
        /// </summary>
        public DataTable GetChiTietDonHang(string orderId)
        {
            if (string.IsNullOrEmpty(orderId))
            {
                return new DataTable(); // Trả về bảng rỗng nếu ID không hợp lệ
            }
            return dal.GetOrderDetails(orderId);
        }

        /// <summary>
        /// Tìm kiếm đơn hàng theo từ khóa và khoảng thời gian.
        /// </summary>
        public DataTable TimKiemDonHang(string keyword, DateTime fromDate, DateTime toDate)
        {
            // LOGIC BUS: Kiểm tra ngày bắt đầu không được lớn hơn ngày kết thúc
            if (fromDate > toDate)
            {
                // Cách 1: Tự động đảo ngược ngày
                // DateTime temp = fromDate; fromDate = toDate; toDate = temp;

                // Cách 2: Trả về null để báo lỗi
                return null;
            }

            return dal.SearchOrders(keyword, fromDate, toDate);
        }

        /// <summary>
        /// Lấy tổng doanh thu.
        /// </summary>
        public int LayTongDoanhThu()
        {
            return dal.GetTotalRevenue();
        }

        #endregion

        #region 2. Cập nhật dữ liệu (Update/Delete)

        /// <summary>
        /// Cập nhật trạng thái đơn hàng.
        /// </summary>
        /// <returns>True nếu thành công, False nếu thất bại</returns>
        public bool CapNhatTrangThai(string orderId, string statusId)
        {
            // LOGIC BUS: Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrEmpty(orderId) || string.IsNullOrEmpty(statusId))
            {
                return false;
            }

            try
            {
                dal.UpdateOrderStatus(orderId, statusId);
                return true;
            }
            catch (Exception)
            {
                // Có thể log lỗi tại đây
                return false;
            }
        }

        /// <summary>
        /// Xóa đơn hàng (và xóa luôn chi tiết nhờ Transaction trong DAL).
        /// </summary>
        /// <returns>True nếu thành công, False nếu thất bại</returns>
        public bool XoaDonHang(string orderId)
        {
            if (string.IsNullOrEmpty(orderId))
            {
                return false;
            }

            try
            {
                dal.DeleteOrder(orderId);
                return true;
            }
            catch (Exception)
            {
                // Lỗi này thường do Transaction bên DAL ném ra
                return false;
            }
        }

        #endregion

        #region 3. Helper (Chuyển đổi Dữ liệu)

        /// <summary>
        /// Hàm hỗ trợ chuyển đổi từ DataTable sang List<OrderDTO>.
        /// Dùng khi bạn muốn làm việc với Object thay vì DataTable trên giao diện.
        /// </summary>
        public List<OrderDTO> ConvertDataTableToList(DataTable dt)
        {
            List<OrderDTO> list = new List<OrderDTO>();
            if (dt == null) return list;

            foreach (DataRow row in dt.Rows)
            {
                OrderDTO item = new OrderDTO();

                // Ánh xạ dữ liệu an toàn (tránh lỗi null)

                // 1. OrderID (DB là int hoặc string -> DTO là int)
                if (row["OrderID_N01"] != DBNull.Value)
                    item.OrderId = Convert.ToInt32(row["OrderID_N01"]);

                // 2. OrderDate
                if (row["OrderDate_N01"] != DBNull.Value)
                    item.OrderDate = Convert.ToDateTime(row["OrderDate_N01"]);

                // 3. TotalAmount
                if (row["TotalAmount_N01"] != DBNull.Value)
                    item.TotalAmount = Convert.ToDecimal(row["TotalAmount_N01"]);

                // 4. Các trường String
                item.Address = row["Address_N01"] != DBNull.Value ? row["Address_N01"].ToString() : "";
                item.Phone = row["Phone_N01"] != DBNull.Value ? row["Phone_N01"].ToString() : "";

                // 5. Các trường từ bảng Join (User, Status) - Kiểm tra cột có tồn tại không trước
                if (dt.Columns.Contains("Username_N01") && row["Username_N01"] != DBNull.Value)
                    item.CustomerName = row["Username_N01"].ToString();

                if (dt.Columns.Contains("StatusName_N01") && row["StatusName_N01"] != DBNull.Value)
                    item.StatusName = row["StatusName_N01"].ToString();

                list.Add(item);
            }
            return list;
        }

        #endregion
    }
}