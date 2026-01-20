using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BUS;
using Microsoft.Reporting.WinForms;
using System.Windows.Forms;
using System.Data;
using System.IO;

namespace QuanLyBanHang
{
    public class ClassPDF
    {
        public void XuatPDFTuReport(DataTable dt,string reportPath, string dataSet, string duongDanLuu)
        {
            try
            {

                // Code nút In hóa đơn
                LocalReport report = new LocalReport();
                report.ReportPath = reportPath;

                // Lấy dữ liệu từ hàm DAL vừa viết trên
                //DataTable dt = donHang_BUS.inHoaDon(txtMaHoaDon.Text);

                report.DataSources.Clear();
                // "DataSet1" là tên bạn đặt trong file .rdlc (như ảnh bạn gửi)
                report.DataSources.Add(new ReportDataSource(dataSet, dt));

                // ... Đoạn sau là xuất PDF như cũ


                string deviceInfo = "";
                string[] streamIds;
                Warning[] warnings;
                string mimeType;
                string encoding;
                string extension;

                byte[] bytes = report.Render(
                    "PDF",
                    deviceInfo,
                    out mimeType,
                    out encoding,
                    out extension,
                    out streamIds,
                    out warnings);


                File.WriteAllBytes(duongDanLuu, bytes);

                MessageBox.Show("Xuất pdf thành công!", "Thông báo");


                System.Diagnostics.Process.Start(duongDanLuu);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xuất PDF: " + ex.Message);
            }
        }
    }
}
