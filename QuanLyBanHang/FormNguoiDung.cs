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
    public partial class FormNguoiDung : Form
    {
        NguoiDung_BUS nguoiDung_BUS = new NguoiDung_BUS();
        public FormNguoiDung()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void FormNguoiDung_Load(object sender, EventArgs e)
        {
            loadData();
        }
        public void loadData()
        {
            dgvUser.DataSource = nguoiDung_BUS.GetDanhSachNguoiDung();
        }
    }
}
