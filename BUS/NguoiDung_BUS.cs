using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Data;

namespace BUS
{
    public class NguoiDung_BUS
    {
        private readonly NguoiDung_DAL dal;

        public NguoiDung_BUS()
        {
            dal = new NguoiDung_DAL();
        }

        #region 1. Lấy dữ liệu (Read)

        /// <summary>
        /// Lấy danh sách tất cả người dùng.
        /// </summary>
        public DataTable GetDanhSachNguoiDung()
        {
            try
            {
                return dal.GetAllUsers();
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Lấy danh sách quyền hạn (Roles) để đổ vào ComboBox.
        /// </summary>
        public DataTable GetDanhSachQuyen()
        {
            try
            {
                return dal.GetRoles();
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        #region 2. Thêm - Sửa - Xóa (CUD)

        /// <summary>
        /// Thêm người dùng mới.
        /// </summary>
        public bool ThemNguoiDung(NguoiDung_DTO user)
        {
            // 1. Kiểm tra dữ liệu hợp lệ (Validation)
            if (string.IsNullOrEmpty(user.Username))
            {
                return false; // Tên đăng nhập không được để trống
            }

            if (string.IsNullOrEmpty(user.Password))
            {
                return false; // Mật khẩu không được để trống
            }

            if (string.IsNullOrEmpty(user.Email)) // Kiểm tra thêm nếu cần
            {
                return false;
            }

            // 2. Gọi xuống DAL
            try
            {
                dal.InsertUser(user);
                return true;
            }
            catch (Exception)
            {
                // Có thể trùng ID hoặc trùng Username (nếu DB có rằng buộc Unique)
                return false;
            }
        }

        /// <summary>
        /// Cập nhật thông tin người dùng.
        /// </summary>
        public bool CapNhatNguoiDung(NguoiDung_DTO user)
        {
            // 1. Kiểm tra dữ liệu
            if (user.UserId <= 0) // ID phải hợp lệ (nếu ID là số dương)
            {
                return false;
            }

            if (string.IsNullOrEmpty(user.Username))
            {
                return false;
            }

            // 2. Gọi xuống DAL
            try
            {
                dal.UpdateUser(user);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Xóa người dùng.
        /// </summary>
        /// <param name="userId">ID người dùng (int theo DTO)</param>
        public bool XoaNguoiDung(int userId)
        {
            if (userId <= 0)
            {
                return false;
            }

            try
            {
                // Lưu ý: DAL của bạn phương thức DeleteUser nhận tham số là string
                // nên ta cần chuyển đổi userId.ToString()
                dal.DeleteUser(userId.ToString());
                return true;
            }
            catch (Exception)
            {
                // Lỗi thường gặp: Người dùng này đang có dữ liệu liên quan (khóa ngoại) ở bảng khác
                return false;
            }
        }

        #endregion

        #region 3. Tiện ích (Helper)

        /// <summary>
        /// Chuyển đổi DataTable sang List<NguoiDung_DTO> (Nếu muốn dùng List Object trên GUI).
        /// </summary>
        public List<NguoiDung_DTO> ConvertToList(DataTable dt)
        {
            List<NguoiDung_DTO> list = new List<NguoiDung_DTO>();
            if (dt == null) return list;

            foreach (DataRow row in dt.Rows)
            {
                NguoiDung_DTO user = new NguoiDung_DTO();

                // Ánh xạ dữ liệu và xử lý NULL an toàn
                if (row["UserID_N01"] != DBNull.Value)
                    user.UserId = Convert.ToInt32(row["UserID_N01"]);

                user.Username = row["Username_N01"] != DBNull.Value ? row["Username_N01"].ToString() : "";
                user.Email = row["Email_N01"] != DBNull.Value ? row["Email_N01"].ToString() : "";
                user.Password = row["Password_N01"] != DBNull.Value ? row["Password_N01"].ToString() : "";
                user.Phone = row["Phone_N01"] != DBNull.Value ? row["Phone_N01"].ToString() : "";
                user.Address = row["Address_N01"] != DBNull.Value ? row["Address_N01"].ToString() : "";
                user.Status = row["Status_N01"] != DBNull.Value ? row["Status_N01"].ToString() : "";

                // Role Name (từ bảng Join)
                if (dt.Columns.Contains("RoleName_N01") && row["RoleName_N01"] != DBNull.Value)
                {
                    user.RoleName = row["RoleName_N01"].ToString();
                }

                list.Add(user);
            }
            return list;
        }

        #endregion

        public DataTable loadRole()
        {
            return dal.loadRole();
        }

        public DataTable loadStatus()
        {
            return dal.loadSatus();
        }

        public DataTable TimKiemNguoiDung(string keyword)
        {
            // Nếu từ khóa rỗng, trả về danh sách đầy đủ
            if (string.IsNullOrEmpty(keyword))
            {
                return dal.GetAllUsers();
            }

            return dal.SearchUsers(keyword);
        }
    }
}