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
    public partial class FormTimKiemNguoiDung : Form
    {
        NguoiDung_BUS nguoiDung_BUS = new NguoiDung_BUS();
        public FormTimKiemNguoiDung()
        {
            InitializeComponent();
        }

        private void FormTimKiemNguoiDung_Load(object sender, EventArgs e)
        {
            loadData();
        }
        public void loadData()
        {
            dgvTKNguoiDung.DataSource = nguoiDung_BUS.GetDanhSachNguoiDung();
        }

        private void txtTuKhoa_TextChanged(object sender, EventArgs e)
        {
            dgvTKNguoiDung.DataSource = nguoiDung_BUS.TimKiemNguoiDung(txtTuKhoa.Text);
        }

        private void btnXuatPdf_Click(object sender, EventArgs e)
        {
           
            // Lấy mã hóa đơn đang chọn
            string tuKHoa = txtTuKhoa.Text;

          

            // Hộp thoại chọn nơi lưu
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF File|*.pdf";
            saveFileDialog.Title = "Xuất pdf";
            saveFileDialog.FileName = "DanhSachNguoiDung.pdf";

            DataTable dt = nguoiDung_BUS.GetDanhSachNguoiDung();

            ClassPDF classPDF = new ClassPDF();
            classPDF.XuatPDFTuReport(dt, "rptNguoiDung.rdlc", "dsNguoiDung", saveFileDialog.FileName);
        }

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            ClassExcel classExcel = new ClassExcel();
            classExcel.XuatExcel(dgvTKNguoiDung, "DanhSachNguoiDung.xlsx");
        }
    }
}
