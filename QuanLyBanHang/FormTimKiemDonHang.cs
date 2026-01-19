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
            dgvTKDonHang.DataSource = donHang_BUS.TimKiemDonHang(txtTuKhoa.Text);
        }
        public void loadData()
        {
            dgvTKDonHang.DataSource = donHang_BUS.GetDanhSachDonHang();
        }

        private void FormTimKiemDonHang_Load(object sender, EventArgs e)
        {
            loadData();
        }

        private void btnXuatPdf_Click(object sender, EventArgs e)
        {

        }
    }
}
