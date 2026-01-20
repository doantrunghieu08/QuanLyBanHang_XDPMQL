using BUS;
using Microsoft.Reporting.WinForms;
using QuanLyThuVien;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyBanHang
{
    public partial class FormTimKiemDanhMuc : Form
    {
        DanhMuc_BUS danhMuc_BUS = new DanhMuc_BUS();
        public FormTimKiemDanhMuc()
        {
            InitializeComponent();
        }

        private void txtTuKhoa_TextChanged(object sender, EventArgs e)
        {
            dgvTKDanhMuc.DataSource = danhMuc_BUS.findDanhMuc(txtTuKhoa.Text);
        }

        private void btnXuatPdf_Click(object sender, EventArgs e)
        {
            DataTable dt = danhMuc_BUS.findDanhMuc(txtTuKhoa.Text);
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu nhân viên!");
                return;
            }

            // 2. KHỞI TẠO REPORT ẢO (Không dùng Form)
            LocalReport localReport = new LocalReport();

            // Đường dẫn file thiết kế (Bạn phải tạo file này trước nhé)
            localReport.ReportPath = "rptDanhMuc.rdlc";

            // 3. ĐỔ DỮ LIỆU
            localReport.DataSources.Clear();
            // "dsNhanVien": Tên Dataset bạn đặt trong file NhanVienReport.rdlc
            localReport.DataSources.Add(new ReportDataSource("DataSet1", dt));

            // 4. XUẤT RA PDF (Render)
            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;
            Warning[] warnings;
            string[] streams;

            byte[] renderedBytes = localReport.Render(
                reportType, null, out mimeType, out encoding,
                out fileNameExtension, out streams, out warnings
            );

            string filePath = @"D:\DanhSachDanhMuc.pdf"; // Đường dẫn lưu file


            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                fs.Write(renderedBytes, 0, renderedBytes.Length);
            }

            MessageBox.Show("Đã xuất xong tại: " + filePath);
            Process.Start(filePath); // Tự động mở file lên xem>
        }

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            ClassExcel excel = new ClassExcel();
            excel.XuatExcel(dgvTKDanhMuc, "DanhMuc.xlsx");
        }
    }
    
}
