using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClosedXML.Excel;

namespace QuanLyThuVien 
{
    internal class ClassExcel
    {
        public void XuatExcel(DataGridView dgv ,string txt)
        {
            string filePath;
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel Files (*.xlsx)|*.xlsx ";
            sfd.Title = "Nơi lưu file excel";
            sfd.FileName = txt;
            if(sfd.ShowDialog() == DialogResult.OK)
            {
                filePath = sfd.FileName;
                GhiDuLieuVaoExcel(dgv, filePath);
                MessageBox.Show("Lưu thành công");
            }
        }
        public void GhiDuLieuVaoExcel(DataGridView dgv, string filePath)
        {
            if (dgv.Rows.Count > 0)
            {
                //Tạo 1 file excel mới
                var wb = new XLWorkbook();

                //Tạo 1 trang tính mới
                var ws = wb.Worksheets.Add("Sheet1");

                //Header
                for (int c = 0; c < dgv.Columns.Count; c++)
                {
                    ws.Cell(1, c + 1).Value = dgv.Columns[c].HeaderText;
                }

                //Data 
                for (int r = 0; r < dgv.Rows.Count; r++)
                {
                    for (int c = 0; c < dgv.Columns.Count; c++)
                    {
                        ws.Cell(r + 2, c + 1).Value = dgv.Rows[r].Cells[c].Value?.ToString();
                    }
                }

                //Thêm border cho toàn bảng
                var range = ws.Range(1, 1, dgv.Rows.Count + 1, dgv.Columns.Count);
                range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                //Tự động co giãn cột
                ws.Columns().AdjustToContents();
                wb.SaveAs(filePath);
            }
            else {
                MessageBox.Show("Không có dữ liệu để xuất");
            }
        }
    }
}
