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
using Microsoft.Reporting.WinForms;

namespace QuanLyBanHang
{
    public partial class FormThongKeThanhToan : Form
    {
        ThanhToan_BUS thanhToan_BUS = new ThanhToan_BUS();
        public FormThongKeThanhToan()
        {
            InitializeComponent();
        }

        private void FormThongKeThanhToan_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();
            loadPhuongThuc();
        }
        public void loadPhuongThuc()
        {
            cbbPhuongThuc.DataSource = thanhToan_BUS.GetDanhSachPhuongThuc();
            cbbPhuongThuc.DisplayMember = "MethodName_N01";
            cbbPhuongThuc.ValueMember = "MethodID_N01";
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                // 1. Kiểm tra đầu vào
                if (cbbPhuongThuc.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn danh mục cần thống kê!");
                    return;
                }

                string maDM = cbbPhuongThuc.SelectedValue.ToString();

                // 2. Lấy dữ liệu từ BUS (Gọi hàm DAL bạn vừa viết ở câu trước)
                // Kết quả trả về là DataTable gồm cả thông tin Danh mục và List Sản phẩm
                DataTable dt = thanhToan_BUS.inThongKe(maDM);

                // Kiểm tra nếu không có dữ liệu
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Không có thanh toán nào của phương thức này");
                    return;
                }

                // 3. Cấu hình ReportViewer
                // Trỏ đường dẫn đến file .rdlc bạn đã thiết kế
                // Đảm bảo file này nằm trong thư mục bin/Debug (xem lưu ý bên dưới)
                reportViewer1.LocalReport.ReportPath = "thongKeThanhToan.rdlc";

                // Xóa các nguồn dữ liệu cũ (nếu có)
                reportViewer1.LocalReport.DataSources.Clear();

                // --- QUAN TRỌNG NHẤT: ĐẶT TÊN DATASET ---
                // "DataSet1": Phải trùng khớp 100% với tên Dataset trong file thiết kế .rdlc
                // (Bạn xem hình ảnh bên dưới để biết lấy tên này ở đâu)
                ReportDataSource rds = new ReportDataSource("dsThongKeThanhToan", dt);

                // Thêm nguồn dữ liệu mới vào report
                reportViewer1.LocalReport.DataSources.Add(rds);

                // 4. (Tùy chọn) Truyền tham số (Parameters)
                // Ví dụ: Truyền tên người lập báo cáo vào ô [Người lập bảng]
                // ReportParameter[] para = new ReportParameter[1];
                // para[0] = new ReportParameter("NguoiLap", "Admin"); // "NguoiLap" là tên Parameter trong rdlc
                // reportViewer1.LocalReport.SetParameters(para);

                // 5. Hiển thị báo cáo
                reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hiển thị báo cáo: " + ex.Message);
            }
        }
    }
}
