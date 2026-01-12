using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyBanHang
{
    public partial class FormMenu : Form
    {
        public FormMenu()
        {
            InitializeComponent();
        }

        private void tìmKiếmToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void quảnLýToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Form frm = new Form();
            switch (e.ClickedItem.Name)
            {
                case "DanhMucItem":
                    FormQuanLyDanhMuc formQuanLyDanhMuc = new FormQuanLyDanhMuc();
                    frm = formQuanLyDanhMuc;
                    break;
                case "SanPhamItem":
                    FormQuanLySanPham formQuanLySanPham = new FormQuanLySanPham();
                    frm = formQuanLySanPham;
                    break;
                case "DonHangItem":
                    FormQuanLyDonHang formQuanLyDonHang = new FormQuanLyDonHang();
                    frm = formQuanLyDonHang;
                    break;

                case "NguoiDungItem":
                    FormNguoiDung formNguoiDung = new FormNguoiDung();
                    frm = formNguoiDung;
                    break;
                case "ThanhToanItem":
                    FormQuanLyThanhToan formQuanLyThanhToan = new FormQuanLyThanhToan();
                    frm = formQuanLyThanhToan;
                    break;
            }
            frm.MdiParent = this;
            frm.WindowState = FormWindowState.Maximized;
            frm.Show();
            frm.BringToFront();
        }
    }
}
