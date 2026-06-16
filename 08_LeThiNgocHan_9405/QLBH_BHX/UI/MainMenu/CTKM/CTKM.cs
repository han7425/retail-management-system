using QLBH_BHX.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace QLBH_BHX.UI.MainMenu.CTKM
{
    public partial class CTKM : Form
    {
        Ketnoi kn = new Ketnoi();
        public CTKM()
        {
            InitializeComponent();
            LoadCTKM();
        }
        private void LoadCTKM()
        {
            string query = "SELECT * FROM CTKM";

            using (SqlConnection conn = kn.GetConnect())
            {
                try
                {
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgv_ctkm.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải dữ liệu CTKM: " + ex.Message, "Lỗi SQL");
                }
            }
        }
        private void showCTKMData(int index)
        {
            // Giả định DataGridView là dgv_ctkm
            if (index < 0 || index >= dgv_ctkm.Rows.Count)
            {
                return;
            }

            DataGridViewRow row = dgv_ctkm.Rows[index];

            txtMa.Text = row.Cells["MaCTKM"].Value?.ToString() ?? string.Empty;
            txtTen.Text = row.Cells["TenCTKM"].Value?.ToString() ?? string.Empty;
            txtMoTa.Text = row.Cells["MoTaCTKM"].Value?.ToString() ?? string.Empty;

            object ngayBDValue = row.Cells["NgayBatDau"].Value;
            if (ngayBDValue != null && ngayBDValue != DBNull.Value && DateTime.TryParse(ngayBDValue.ToString(), out DateTime ngayBD))
            {
                dtpBD.Value = ngayBD;
            }

            object ngayKTValue = row.Cells["NgayKetThuc"].Value;
            if (ngayKTValue != null && ngayKTValue != DBNull.Value && DateTime.TryParse(ngayKTValue.ToString(), out DateTime ngayKT))
            {
                dtpKT.Value = ngayKT;
            }
        }
        private void CTKM_Load(object sender, EventArgs e)
        {

        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgv_ctkm.CurrentRow == null || dgv_ctkm.CurrentRow.Index < 0)
                {
                    MessageBox.Show("Vui lòng chọn CTKM cần xóa.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string maCTKM = dgv_ctkm.Rows[dgv_ctkm.CurrentCell.RowIndex].Cells["MaCTKM"].Value?.ToString();

                if (string.IsNullOrEmpty(maCTKM))
                {
                    MessageBox.Show("Không tìm thấy Mã CTKM để xóa.", "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DialogResult result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa CTKM có Mã: {maCTKM}?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    ctkmDAL dal = new ctkmDAL();
                    int affectedRows = dal.XoaCTKM(maCTKM);

                    if (affectedRows > 0)
                    {
                        MessageBox.Show("Xóa CTKM thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadCTKM();
                    }
                    else
                    {
                        MessageBox.Show("Xóa không thành công. CTKM có thể đang được áp dụng cho Sản phẩm.", "Lỗi xóa", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi hệ thống");
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                string maCTKM = txtMa.Text.Trim();
                string tenCTKM = txtTen.Text.Trim();
                string moTaCTKM = txtMoTa.Text.Trim();
                DateTime ngayBatDau = dtpBD.Value;
                DateTime ngayKetThuc = dtpKT.Value;

                if (string.IsNullOrEmpty(maCTKM))
                {
                    MessageBox.Show("Vui lòng nhập Mã Chương trình Khuyến mãi.", "Cảnh báo");
                    txtMa.Focus();
                    return;
                }

                ctkmDAL dal = new ctkmDAL();
                int affectedRows = dal.ThemCTKM(maCTKM, tenCTKM, moTaCTKM, ngayBatDau, ngayKetThuc);

                if (affectedRows > 0)
                {
                    MessageBox.Show("Thêm Chương trình Khuyến mãi thành công!", "Thông báo");
                    LoadCTKM();
                    clear();
                }
                else
                {
                    MessageBox.Show("Thêm thất bại. Mã CTKM có thể đã tồn tại.", "Lỗi");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi hệ thống");
            }
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            string maCTKM = txtMa.Text.Trim();
            string tenCTKM = txtTen.Text.Trim();

            string query = "SELECT * FROM CTKM WHERE 1=1";
            List<SqlParameter> parameters = new List<SqlParameter>();

            //  tìm kiếm theo Mã CTKM
            if (!string.IsNullOrEmpty(maCTKM))
            {
                query += " AND MaCTKM LIKE @MaCTKM";
                parameters.Add(new SqlParameter("@MaCTKM", "%" + maCTKM + "%"));
            }

            //  tìm kiếm theo Tên CTKM
            if (!string.IsNullOrEmpty(tenCTKM))
            {
                query += " AND TenCTKM LIKE @TenCTKM";
                parameters.Add(new SqlParameter("@TenCTKM", "%" + tenCTKM + "%"));
            }

            using (SqlConnection conn = kn.GetConnect())
            {
                SqlCommand cmd = new SqlCommand(query, conn);

                if (!string.IsNullOrEmpty(tenCTKM))
                    cmd.Parameters.AddWithValue("@TenKH", "%" + tenCTKM + "%");

                if (!string.IsNullOrEmpty(maCTKM))
                    cmd.Parameters.AddWithValue("@MaKH", "%" + maCTKM + "%");

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgv_ctkm.DataSource = dt;
            }
        }
        private void clear()
        {
            txtTen.Clear();
            txtMa.Clear();
            txtMoTa.Clear();
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                string maCTKM = txtMa.Text.Trim();
                string tenCTKM = txtTen.Text.Trim();
                string moTaCTKM = txtMoTa.Text.Trim();
                DateTime ngayBatDau = dtpBD.Value;
                DateTime ngayKetThuc = dtpKT.Value;

                if (string.IsNullOrEmpty(maCTKM))
                {
                    MessageBox.Show("Vui lòng chọn Mã CTKM cần sửa.", "Cảnh báo");
                    return;
                }

                ctkmDAL dal = new ctkmDAL();
                int affectedRows = dal.CapNhatCTKM(maCTKM, tenCTKM, moTaCTKM, ngayBatDau, ngayKetThuc);

                if (affectedRows > 0)
                {
                    MessageBox.Show("Cập nhật Chương trình Khuyến mãi thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadCTKM(); 
                }
                else
                {
                    MessageBox.Show("Cập nhật thất bại. Mã CTKM không tồn tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi hệ thống");
            }
        }

        private void dgv_ctkm_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btn_export_Click(object sender, EventArgs e)
        {
            ExcelExporter.ExportToExcel(dgv_ctkm, "Danh Sách CTKM", "DS_CTKM");
        }
    }
}

