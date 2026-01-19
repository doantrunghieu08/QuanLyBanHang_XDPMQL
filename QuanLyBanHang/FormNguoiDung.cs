using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BUS;
using DTO;
using QuanLyThuVien;

namespace QuanLyBanHang
{
    public partial class FormNguoiDung : Form
    {
        NguoiDung_BUS nguoiDung_BUS = new NguoiDung_BUS();
        public FormNguoiDung()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Kiểm tra sơ bộ trên giao diện (Optional nhưng nên có)
                if (string.IsNullOrEmpty(txtMaNguoiDung.Text) || string.IsNullOrEmpty(txtUsername.Text))
                {
                    MessageBox.Show("Vui lòng nhập Mã ID và Tên đăng nhập!");
                    return;
                }

                // 2. Khởi tạo đối tượng DTO và gán dữ liệu từ các ô nhập
                NguoiDung_DTO user = new NguoiDung_DTO();

                // --- Xử lý ID ---
                // Vì trong DTO bạn khai báo UserId là int, nên phải ép kiểu từ Textbox
                // Nếu nhập chữ vào ô ID sẽ gây lỗi -> Catch ở dưới sẽ bắt
                user.UserId = int.Parse(txtMaNguoiDung.Text.Trim());

                // --- Xử lý các thông tin cơ bản ---
                user.Username = txtUsername.Text.Trim();
                user.Password = txtPwd.Text.Trim();
                user.Email = txtEmail.Text.Trim();
                user.Phone = txtSDT.Text.Trim();
                user.Address = txtDiaChi.Text.Trim();

                // --- Xử lý ComboBox Quyền (Role) ---
                // Giả sử bạn đã load dữ liệu vào cboRole (DisplayMember=RoleName, ValueMember=RoleID)
                if (cbbRole.SelectedValue != null)
                {
                    user.RoleId = int.Parse(cbbRole.SelectedValue.ToString());
                }
                else
                {
                    // Nếu chưa chọn quyền, bạn có thể gán mặc định hoặc báo lỗi
                    MessageBox.Show("Vui lòng chọn Quyền hạn!");
                    return;
                }

                // --- Xử lý Trạng thái (Status) ---
                // Có thể lấy từ ComboBox hoặc mặc định
                user.Status = string.IsNullOrEmpty(cbbStatus.Text) ? "Active" : cbbStatus.Text;

                // 3. Gọi xuống lớp BUS để xử lý
                // (Biến nguoiDung_BUS đã được new ở đầu Form)
                bool ketQua = nguoiDung_BUS.ThemNguoiDung(user);

                // 4. Kiểm tra kết quả và thông báo
                if (ketQua)
                {
                    MessageBox.Show("Thêm người dùng thành công!");
                    loadData(); // Gọi hàm load lại DataGridView để thấy dòng mới

                    // (Tùy chọn) Xóa trắng các ô nhập sau khi thêm
                    txtMaNguoiDung.Text = "";
                    txtUsername.Text = "";
                    // ...
                }
                else
                {
                    MessageBox.Show("Thêm thất bại! Vui lòng kiểm tra lại:\n- ID có thể đã tồn tại.\n- Tên đăng nhập/Email/Mật khẩu không được để trống.");
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Lỗi: Mã người dùng (ID) phải là số nguyên!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống: " + ex.Message);
            }
        }

        private void FormNguoiDung_Load(object sender, EventArgs e)
        {
            loadData();
            loadRole();
            loadStatus();

        }
        public void loadData()
        {
            dgvUser.DataSource = nguoiDung_BUS.GetDanhSachNguoiDung();
        }
        public void loadRole()
        {
            cbbRole.DataSource = nguoiDung_BUS.loadRole();
            cbbRole.DisplayMember = "RoleName_N01";
            cbbRole.ValueMember = "RoleID_N01";
        }

        public void loadStatus()
        {
            cbbStatus.DataSource = nguoiDung_BUS.loadStatus();
            cbbStatus.DisplayMember = "Status_N01";
            cbbStatus.ValueMember = "Status_N01";
        }

        private void dgvUser_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvUser.Rows[e.RowIndex];
                txtMaNguoiDung.Text = row.Cells["UserID_N01"].Value.ToString();
                txtUsername.Text = row.Cells["username_N01"].Value.ToString();
                txtPwd.Text = row.Cells["Password_N01"].Value.ToString();
                txtSDT.Text = row.Cells["Phone_N01"].Value.ToString();
                txtDiaChi.Text = row.Cells["Address_N01"].Value.ToString();
                txtEmail.Text = row.Cells["Email_N01"].Value.ToString();

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtMaNguoiDung.Text))
                {
                    MessageBox.Show("Vui lòng chọn người dùng cần sửa từ danh sách hoặc nhập ID!");
                    return;
                }

                DialogResult confirm = MessageBox.Show("Bạn có chắc chắn muốn cập nhật thông tin cho ID: " + txtMaNguoiDung.Text + " ?",
                                                     "Xác nhận sửa",
                                                     MessageBoxButtons.YesNo,
                                                     MessageBoxIcon.Question);
                if (confirm == DialogResult.No) return;

                NguoiDung_DTO user = new NguoiDung_DTO();

                user.UserId = int.Parse(txtMaNguoiDung.Text.Trim());
                user.Username = txtUsername.Text.Trim();
                user.Email = txtEmail.Text.Trim();
                user.Phone = txtSDT.Text.Trim();
                user.Address = txtDiaChi.Text.Trim();

                if (cbbRole.SelectedValue != null)
                {
                    user.RoleId = int.Parse(cbbRole.SelectedValue.ToString());
                }

                user.Status = string.IsNullOrEmpty(cbbStatus.Text) ? "Active" : cbbStatus.Text;

                bool ketQua = nguoiDung_BUS.CapNhatNguoiDung(user);

                if (ketQua)
                {
                    MessageBox.Show("Cập nhật thành công!");
                    loadData();
                }
                else
                {
                    MessageBox.Show("Cập nhật thất bại! Có thể ID không tồn tại hoặc lỗi kết nối.");
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Lỗi: ID người dùng phải là số nguyên!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống: " + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            DialogResult confirm = MessageBox.Show("Bạn có chắc chắn muốn xóa thông tin cho ID: " + txtMaNguoiDung.Text + " ?",
                                                 "Xác nhận sửa",
                                                 MessageBoxButtons.YesNo,
                                                 MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                nguoiDung_BUS.XoaNguoiDung(Convert.ToInt32(txtMaNguoiDung.Text));
                loadData();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ClassExcel excel = new ClassExcel();
            excel.XuatExcel(dgvUser, "NguoiDung.xlsx");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DialogResult confirm = MessageBox.Show("Bạn có chắc chắn muốn xóa thông tin cho ID: " + txtMaNguoiDung.Text + " ?",
                                                 "Xác nhận sửa",
                                                 MessageBoxButtons.YesNo,
                                                 MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
               this.Close();
            }
        }
    }
}
