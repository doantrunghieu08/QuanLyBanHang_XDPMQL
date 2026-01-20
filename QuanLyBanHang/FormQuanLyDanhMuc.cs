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
    public partial class FormQuanLyDanhMuc : Form
    {
        DanhMuc_BUS danhMuc_BUS = new DanhMuc_BUS();
        public FormQuanLyDanhMuc()
        {
            InitializeComponent();
        }

        private void FormQuanLyDanhMuc_Load(object sender, EventArgs e)
        {
            loadData();
        }
        public void loadData()
        {
            dgvDanhMuc.DataSource = danhMuc_BUS.loadData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DanhMuc_DTO ob = new DanhMuc_DTO();
            ob.CategoryId =int.Parse( txtMaDanhMuc.Text);
            ob.Name = txtTenDanhMuc.Text;
            ob.Description = rtbGhiChu.Text;
            

            danhMuc_BUS.InsertDanhMuc(ob);
            MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            loadData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc muốn sửa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                string maDanhMuc = txtMaDanhMuc.Text;
                DanhMuc_DTO ob = new DanhMuc_DTO();
                ob.Name = txtTenDanhMuc.Text;
                ob.Description = rtbGhiChu.Text;
                
                danhMuc_BUS.UpdateDanhMuc(ob, maDanhMuc);
                MessageBox.Show("Sửa thành công");
                loadData();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                string maDanhMuc = txtMaDanhMuc.Text;
                danhMuc_BUS.DeleteDanhMuc(maDanhMuc);
                MessageBox.Show("Xóa thành công");
                loadData();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void dgvDanhMuc_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > 0)
            {
                DataGridViewRow row = dgvDanhMuc.Rows[e.RowIndex];
                txtMaDanhMuc.Text = row.Cells["CategoryID_N01"].Value.ToString();
                txtTenDanhMuc.Text = row.Cells["CategoryName_N01"].Value.ToString();
                rtbGhiChu.Text = row.Cells["Description_N01"].Value.ToString();
                
            }
        }
    }
}
