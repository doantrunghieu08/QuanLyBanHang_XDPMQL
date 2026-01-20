using BUS;
using DocumentFormat.OpenXml.Wordprocessing;
using DTO;
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
    public partial class FormQuanLySanPham : Form
    {
        SanPham_BUS sanPham_BUS = new SanPham_BUS();
        public FormQuanLySanPham()
        {
            InitializeComponent();
        }

        private void picAnhSP_Click(object sender, EventArgs e)
        {

        }

        private void FormQuanLySanPham_Load(object sender, EventArgs e)
        {
            loadData();
        }
        public void loadData()
        {
            dgvSanPham.DataSource = sanPham_BUS.loadData();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            SanPham_DTO ob = new SanPham_DTO();
            ob.ProductId = int.Parse(txtMaSanPham.Text);
            ob.ProductName = txtTenSP.Text;
            ob.CategoryID = (int)cboDanhMuc.SelectedValue;
            ob.Price = (int)nudGiaBan.Value;
            ob.StockQuantity = (int)nudTonKho.Value;
            ob.Description = txtMoTa.Text;
            ob.ImageUrl = picAnhSP.Text;

            sanPham_BUS.InsertSanPham(ob);
            MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            loadData();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc muốn sửa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                string maSanPham = txtMaSanPham.Text;
                SanPham_DTO ob = new SanPham_DTO();
                ob.ProductName = txtTenSP.Text;
                ob.CategoryID = (int)cboDanhMuc.SelectedValue;
                ob.Price = (int)nudGiaBan.Value;
                ob.StockQuantity = (int)nudTonKho.Value;
                ob.Description = txtMoTa.Text;
                ob.ImageUrl = picAnhSP.Text;
                sanPham_BUS.UpdateSanPham(ob, maSanPham);
                MessageBox.Show("Sửa thành công");
                loadData();
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                string maSanPham = txtMaSanPham.Text;
                sanPham_BUS.DeleteSanPham(maSanPham);
                MessageBox.Show("Xóa thành công");
                loadData();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void dgvSanPham_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > 0)
            {
                DataGridViewRow row = dgvSanPham.Rows[e.RowIndex];
                txtMaSanPham.Text = row.Cells["ProductID_N01"].Value.ToString();
                txtTenSP.Text = row.Cells["ProductName_N01"].Value.ToString();
                cboDanhMuc.Text = row.Cells["CategoryID_N01"].Value.ToString();
                nudGiaBan.Text = row.Cells["Price_N01"].Value.ToString();
                nudTonKho.Text = row.Cells["StockQuantity_N01"].Value.ToString();
                txtMoTa.Text = row.Cells["description_N01"].Value.ToString();
                picAnhSP.Text = row.Cells["ImageURL_N01"].Value.ToString();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            dgvSanPham.DataSource = sanPham_BUS.findSanPham(txttimkiem.Text);
        }
    }
    
}
