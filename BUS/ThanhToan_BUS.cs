using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Data;

namespace BUS
{
    public class ThanhToan_BUS
    {
        private readonly ThanhToan_DAL dal;

        public ThanhToan_BUS()
        {
            dal = new ThanhToan_DAL();
        }

        #region 1. Lấy dữ liệu (Read)

        /// <summary>
        /// Tìm kiếm thanh toán theo nhiều điều kiện.
        /// </summary>
        public List<ThanhToan_DTO> TimKiemThanhToan(string orderIdKeyword, string methodId, string statusId)
        {
            try
            {
                // Gọi xuống DAL
                return dal.SearchPayments(orderIdKeyword, methodId, statusId);
            }
            catch (Exception)
            {
                // Nếu lỗi trả về list rỗng để không crash GUI
                return new List<ThanhToan_DTO>();
            }
        }

        /// <summary>
        /// Lấy danh sách phương thức thanh toán (đổ ComboBox).
        /// </summary>
        public DataTable GetDanhSachPhuongThuc()
        {
            try
            {
                return dal.GetMethods();
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Lấy danh sách trạng thái thanh toán (đổ ComboBox).
        /// </summary>
        public DataTable GetDanhSachTrangThai()
        {
            try
            {
                return dal.GetStatuses();
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        #region 2. Thêm - Sửa - Xóa (CUD)

        /// <summary>
        /// Thêm giao dịch thanh toán mới.
        /// </summary>
        public bool ThemThanhToan(ThanhToan_DTO p)
        {
            // 1. Kiểm tra dữ liệu (Validation)
            if (string.IsNullOrEmpty(p.PaymentID))
            {
                return false; // ID không được rỗng
            }

            if (p.Amount <= 0)
            {
                return false; // Số tiền phải lớn hơn 0
            }

            if (string.IsNullOrEmpty(p.OrderID))
            {
                return false; // Phải gắn với đơn hàng nào đó
            }

            // 2. Kiểm tra trùng ID (Logic nghiệp vụ quan trọng)
            if (dal.CheckPaymentIDExists(p.PaymentID))
            {
                return false; // Đã tồn tại ID này rồi
            }

            // 3. Gọi xuống DAL
            try
            {
                return dal.InsertPayment(p);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Cập nhật thông tin thanh toán.
        /// </summary>
        public bool CapNhatThanhToan(ThanhToan_DTO p)
        {
            // 1. Kiểm tra dữ liệu
            if (string.IsNullOrEmpty(p.PaymentID))
            {
                return false;
            }

            if (p.Amount <= 0)
            {
                return false;
            }

            // 2. Gọi xuống DAL
            try
            {
                return dal.UpdatePayment(p);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Xóa giao dịch thanh toán.
        /// </summary>
        public bool XoaThanhToan(string paymentId)
        {
            if (string.IsNullOrEmpty(paymentId))
            {
                return false;
            }

            try
            {
                return dal.DeletePayment(paymentId);
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion
        public DataTable loadTTThanhToan()
        {
            return dal.loadTrangThaiThanhToan();
        }

        public DataTable loadData()
        {
            return dal.GetAllPayments();
        }
    }
}