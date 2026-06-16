using QLBH_BHX.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLBH_BHX.UI.MainMenu.Sales_Management
{
    public partial class ThemLSP : Form
    {
        public ThemLSP()
        {
            InitializeComponent();
        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            string maLoaiSP = txtMa.Text.Trim();
            string tenLoaiSP = txtTen.Text.Trim();

            if (string.IsNullOrEmpty(maLoaiSP))
            {
                MessageBox.Show("Vui lòng nhập Mã loại sản phẩm.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMa.Focus();
                return;
            }
            if (string.IsNullOrEmpty(tenLoaiSP))
            {
                MessageBox.Show("Vui lòng nhập Tên loại sản phẩm.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTen.Focus();
                return;
            }

            try
            {
                LoaiSPDAL dal = new LoaiSPDAL();
                int affectedRows = dal.ThemLoaiSanPham(maLoaiSP, tenLoaiSP);

                if (affectedRows > 0)
                {
                    MessageBox.Show("Thêm Loại Sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtMa.Clear();
                    txtTen.Clear();
                    txtMa.Focus();
                }
                else
                {
                    MessageBox.Show("Thêm không thành công. Mã loại sản phẩm có thể đã tồn tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống: " + ex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
