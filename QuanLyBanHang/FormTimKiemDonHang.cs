using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BUS;
using Microsoft.Reporting.WinForms;
using QuanLyThuVien;

namespace QuanLyBanHang
{
    public partial class FormTimKiemDonHang : Form
    {
        DonHang_BUS donHang_BUS = new DonHang_BUS();
        public FormTimKiemDonHang()
        {
            InitializeComponent();
        }

        private void txtTuKhoa_TextChanged(object sender, EventArgs e)
        {
           
        }
        public void loadData()
        {
            dgvHoaDon.DataSource = donHang_BUS.GetDanhSachDonHang();
        }

        private void FormTimKiemDonHang_Load(object sender, EventArgs e)
        {
            loadData();
        }

        private void btnXuatPdf_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            dgvHoaDon.DataSource = donHang_BUS.TimKiemDonHang(txtTuKhoa.Text);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dgvHoaDon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvHoaDon.Rows[e.RowIndex];
                txtMaHoaDon.Text = row.Cells["OrderID_N01"].Value.ToString();
                
            }
        }

        private void txtMaHoaDon_Click(object sender, EventArgs e)
        {

        }

        private void txtMaHoaDon_TextChanged(object sender, EventArgs e)
        {
            dgvChiTiet.DataSource = donHang_BUS.GetChiTietDonHang(txtMaHoaDon.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClassExcel excel = new ClassExcel();
            excel.XuatExcel(dgvHoaDon, "HoaDon.xlsx");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ClassPDF classPDF = new ClassPDF();
            // Lấy mã hóa đơn đang chọn
            string maHD = txtMaHoaDon.Text;

            if (string.IsNullOrEmpty(maHD)) return;

            // Hộp thoại chọn nơi lưu
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF File|*.pdf";
            saveFileDialog.Title = "Xuất pdf";
            saveFileDialog.FileName = "HoaDon_" + maHD + ".pdf";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                DataTable dt = donHang_BUS.inHoaDon(txtMaHoaDon.Text);
                classPDF.XuatPDFTuReport(dt, "rptHoaDon.rdlc", "DataSet1", saveFileDialog.FileName);
            }
        }
        //public void XuatPDFTuReport(string maHoaDon, string duongDanLuu)
        //{
        //    try
        //    {

        //        // Code nút In hóa đơn
        //        LocalReport report = new LocalReport();
        //        report.ReportPath = "rptHoaDon.rdlc";

        //        // Lấy dữ liệu từ hàm DAL vừa viết trên
        //        DataTable dt = donHang_BUS.inHoaDon(txtMaHoaDon.Text);

        //        report.DataSources.Clear();
        //        // "DataSet1" là tên bạn đặt trong file .rdlc (như ảnh bạn gửi)
        //        report.DataSources.Add(new ReportDataSource("DataSet1", dt));

        //        // ... Đoạn sau là xuất PDF như cũ


        //        string deviceInfo = "";
        //        string[] streamIds;
        //        Warning[] warnings;
        //        string mimeType;
        //        string encoding;
        //        string extension;

        //        byte[] bytes = report.Render(
        //            "PDF",         
        //            deviceInfo,
        //            out mimeType,
        //            out encoding,
        //            out extension,
        //            out streamIds,
        //            out warnings);

               
        //        File.WriteAllBytes(duongDanLuu, bytes);

        //        MessageBox.Show("Xuất hóa đơn thành công!", "Thông báo");

                
        //        System.Diagnostics.Process.Start(duongDanLuu);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Lỗi xuất PDF: " + ex.Message);
        //    }
        
    }
}
