using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
    public partial class FormQuanLyThanhToan : Form
    {
        ThanhToan_BUS thanhToan_BUS = new ThanhToan_BUS();
        public FormQuanLyThanhToan()
        {
            InitializeComponent();
            dTPNgayThanhToan.Format = DateTimePickerFormat.Custom;
            dTPNgayThanhToan.CustomFormat = "dd/MM/yyyy";

        }

        private void FormQuanLyThanhToan_Load(object sender, EventArgs e)
        {
            loadTT();
            loadData();
            loadPTTT();
            dgvQLThanhToan.Columns["MethodID_N01"].Visible = false;
            dgvQLThanhToan.Columns["StatusID_N01"].Visible = false;
        }
        public void loadTT()
        {
            cbbTTTT.DataSource = thanhToan_BUS.loadTTThanhToan();
            cbbTTTT.DisplayMember = "StatusName_N01";
            cbbTTTT.ValueMember = "StatusID_N01";
        }

        public void loadData()
        {
            dgvQLThanhToan.DataSource = thanhToan_BUS.loadData();
        }
        public void loadPTTT()
        {
            cboPTTT.DataSource = thanhToan_BUS.GetDanhSachPhuongThuc();
            cboPTTT.DisplayMember = "MethodName_N01";
            cboPTTT.ValueMember = "MethodID_N01";
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Kiểm tra sơ bộ các trường bắt buộc trên giao diện
                if (string.IsNullOrEmpty(txtMaThanhToan.Text) || string.IsNullOrEmpty(txtTongTien.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ Mã thanh toán và Số tiền!");
                    return;
                }

                // 2. Tạo đối tượng DTO và gán dữ liệu
                ThanhToan_DTO tt = new ThanhToan_DTO();

                tt.PaymentID = txtMaThanhToan.Text.Trim();
                tt.OrderID = txtMaDonHang.Text.Trim();

                // Lưu ý: DTO Amount là int, cần ép kiểu. Nếu nhập chữ sẽ nhảy xuống catch FormatException
                tt.Amount = int.Parse(txtTongTien.Text.Trim());

                tt.PaymentDate = dTPNgayThanhToan.Value;

                // Xử lý ComboBox Phương thức (Method)
                if (cboPTTT.SelectedValue != null)
                {
                    tt.MethodID = cboPTTT.SelectedValue.ToString();
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn Phương thức thanh toán!");
                    return;
                }

                // Xử lý ComboBox Trạng thái (Status)
                if (cbbTTTT.SelectedValue != null)
                {
                    tt.StatusID = cbbTTTT.SelectedValue.ToString();
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn Trạng thái thanh toán!");
                    return;
                }

                // 3. Gọi xuống BUS để xử lý
                // (Biến thanhToan_BUS phải được khai báo ở đầu Form: ThanhToan_BUS bus = new ThanhToan_BUS();)
                bool ketQua = thanhToan_BUS.ThemThanhToan(tt);

                // 4. Kiểm tra kết quả
                if (ketQua)
                {
                    MessageBox.Show("Thêm thanh toán thành công!");
                    loadData(); // Load lại DataGridView để thấy dòng mới thêm

                    // (Tùy chọn) Xóa trắng các ô sau khi thêm
                    txtMaThanhToan.Text = "";
                    txtTongTien.Text = "";
                    txtMaDonHang.Text = "";
                }
                else
                {
                    MessageBox.Show("Thêm thất bại!\n- Kiểm tra xem Mã thanh toán (PaymentID) đã tồn tại chưa.\n- Số tiền phải lớn hơn 0.\n- Mã đơn hàng phải có thực.");
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Lỗi định dạng: Số tiền phải là số nguyên!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống: " + ex.Message);
            }
        }

        private void dgvQLThanhToan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvQLThanhToan.Rows[e.RowIndex];
                txtMaDonHang.Text = row.Cells["OrderID_N01"].Value.ToString();
                txtMaThanhToan.Text = row.Cells["PaymentID_N01"].Value.ToString();
                dTPNgayThanhToan.Value = Convert.ToDateTime(row.Cells["PaymentDate_N01"].Value);
                txtTongTien.Text = row.Cells["Amount_N01"].Value.ToString();

            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Kiểm tra xem người dùng đã chọn hoặc nhập Mã thanh toán chưa
                if (string.IsNullOrEmpty(txtMaThanhToan.Text))
                {
                    MessageBox.Show("Vui lòng chọn giao dịch cần sửa từ danh sách hoặc nhập Mã thanh toán!");
                    return;
                }

                // 2. Hỏi xác nhận (Best Practice khi Sửa/Xóa dữ liệu)
                DialogResult confirm = MessageBox.Show(
                    "Bạn có chắc chắn muốn cập nhật thông tin cho giao dịch: " + txtMaThanhToan.Text + "?",
                    "Xác nhận cập nhật",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirm == DialogResult.No) return;

                // 3. Tạo đối tượng DTO và gom dữ liệu mới từ giao diện
                ThanhToan_DTO tt = new ThanhToan_DTO();

                // --- KHÓA CHÍNH (Không được đổi nếu không có logic xử lý riêng) ---
                tt.PaymentID = txtMaThanhToan.Text.Trim();

                // --- Các thông tin cần cập nhật ---
                tt.OrderID = txtMaDonHang.Text.Trim();
                tt.PaymentDate = dTPNgayThanhToan.Value;

                // Ép kiểu số tiền (Nếu nhập chữ sẽ nhảy xuống catch FormatException)
                tt.Amount = int.Parse(txtTongTien.Text.Trim());

                // Lấy phương thức thanh toán
                if (cboPTTT.SelectedValue != null)
                {
                    tt.MethodID = cboPTTT.SelectedValue.ToString();
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn Phương thức thanh toán!");
                    return;
                }

                // Lấy trạng thái
                if (cbbTTTT.SelectedValue != null)
                {
                    tt.StatusID = cbbTTTT.SelectedValue.ToString();
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn Trạng thái!");
                    return;
                }

                // 4. Gọi xuống BUS để thực hiện Update
                // (Biến thanhToan_BUS đã được khai báo ở đầu Form)
                bool ketQua = thanhToan_BUS.CapNhatThanhToan(tt);

                // 5. Kiểm tra kết quả
                if (ketQua)
                {
                    MessageBox.Show("Cập nhật thành công!");
                    loadData(); // Tải lại DataGridView để thấy dữ liệu mới

                    // (Tùy chọn) Có thể giữ nguyên dữ liệu trên ô nhập để user xem lại, hoặc xóa trắng tùy bạn
                }
                else
                {
                    MessageBox.Show("Cập nhật thất bại!\n- Có thể Mã thanh toán không tồn tại.\n- Hoặc số tiền không hợp lệ (phải > 0).");
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Lỗi định dạng: Số tiền phải là số nguyên!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống: " + ex.Message);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra xem người dùng đã chọn hoặc nhập Mã thanh toán chưa
            if (string.IsNullOrEmpty(txtMaThanhToan.Text))
            {
                MessageBox.Show("Vui lòng chọn giao dịch cần xóa từ danh sách hoặc nhập Mã thanh toán!");
                return;
            }

            // 2. Hỏi xác nhận (Best Practice khi Sửa/Xóa dữ liệu)
            DialogResult confirm = MessageBox.Show(
                "Bạn có chắc chắn muốn cập nhật thông tin cho giao dịch: " + txtMaThanhToan.Text + "?",
                "Xác nhận cập nhật",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm == DialogResult.No) return;
            thanhToan_BUS.XoaThanhToan(txtMaThanhToan.Text);
            
            loadData();
        }

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            ClassExcel excel = new ClassExcel();
            excel.XuatExcel(dgvQLThanhToan, "ThanhToan.xlsx");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DialogResult confirm = MessageBox.Show("Bạn có chắc muốn thoát không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.No) return;
            this.Close();
        }
    }
}
