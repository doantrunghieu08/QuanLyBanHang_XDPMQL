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

        private void tìmKiếmToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Form form = new Form();
            switch (e.ClickedItem.Name)
            {
                case "danhMụcItem":
                    FormTimKiemDanhMuc formTimKiemDanhMuc = new FormTimKiemDanhMuc();
                    form = formTimKiemDanhMuc;
                    break;
                case "sảnPhẩmItem":
                    FormTimKiemSanPham formTimKiemSanPham = new FormTimKiemSanPham();
                    form = formTimKiemSanPham;
                    break;
                case "đơnHàngItem":
                    FormTimKiemDonHang formTimKiemDonHang = new FormTimKiemDonHang();
                    form = formTimKiemDonHang;
                    break;
                case "ngườiDùngItem":
                    FormTimKiemNguoiDung formTimKiemNguoiDung = new FormTimKiemNguoiDung();
                    form = formTimKiemNguoiDung;
                    break;
                case "thanhToánItem":
                    FormTimKiemThanhToan formTimKiemThanhToan = new FormTimKiemThanhToan();
                    form = formTimKiemThanhToan;
                    break;
            }
            form.MdiParent = this;
            form.Show();
            form.BringToFront();

        }

        private void danhMụcToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
