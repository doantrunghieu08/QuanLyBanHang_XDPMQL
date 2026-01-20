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
using QuanLyThuVien;

namespace QuanLyBanHang
{
    public partial class FormTimKiemThanhToan : Form
    {
        ThanhToan_BUS thanhToan_BUS = new ThanhToan_BUS();
        public FormTimKiemThanhToan()
        {
            InitializeComponent();
        }

        private void txtTuKhoa_TextChanged(object sender, EventArgs e)
        {
            dgvTKThanhToan.DataSource = thanhToan_BUS.TimKiemThanhToan(txtTuKhoa.Text);
        }

        public void loadData()
        {
            dgvTKThanhToan.DataSource = thanhToan_BUS.loadData();
        }

        private void FormTimKiemThanhToan_Load(object sender, EventArgs e)
        {
            loadData();
        }

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            ClassExcel classExcel = new ClassExcel();
            classExcel.XuatExcel(dgvTKThanhToan, "DanhSachThanhToan");
        }

        private void btnXuatPdf_Click(object sender, EventArgs e)
        {
            ClassPDF classPDF = new ClassPDF();
            // Lấy mã hóa đơn đang chọn
            

            // Hộp thoại chọn nơi lưu
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF File|*.pdf";
            saveFileDialog.Title = "Xuất pdf";
            saveFileDialog.FileName = "DanhSachThanhToan.pdf";
            // Lấy dữ liệu đã lọc (Filter) ra DataTable mới
            DataTable dtDaLoc = ((DataTable)dgvTKThanhToan.DataSource).DefaultView.ToTable();
            classPDF.XuatPDFTuReport(dtDaLoc, "rptThanhToan.rdlc", "dsThanhToan", saveFileDialog.FileName);
        }
    }
}
