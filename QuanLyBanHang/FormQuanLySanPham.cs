using BUS;
using DTO;
using QuanLyThuVien;
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
            ob.ImageUrl = picAnhSP.Text;
            ob.Description = txtMoTa.Text;



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
                ob.ImageUrl = picAnhSP.Text;
                ob.Description = txtMoTa.Text;

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

        private void button7_Click(object sender, EventArgs e)
        {
            ClassExcel excel = new ClassExcel();
            excel.XuatExcel(dgvSanPham, "QuanLySanPham.xlsx");
        }
    }
}
