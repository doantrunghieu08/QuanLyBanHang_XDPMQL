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
    }
}
