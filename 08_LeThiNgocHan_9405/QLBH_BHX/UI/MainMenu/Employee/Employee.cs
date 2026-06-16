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

namespace QLBH_BHX.UI.MainMenu.Employee
{
    public partial class Employee : Form
    {
        Ketnoi kn = new Ketnoi();
        public Employee()
        {
            InitializeComponent();
            loadNhanVien();
        }
        private void loadNhanVien()
        {
            string query = "SELECT * FROM NHANVIEN";
            using (SqlConnection conn = kn.GetConnect())
            {
                try
                {
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgv_nv.DataSource = dt;
                }
                catch (Exception ex)
                {

                    throw new Exception("Lỗi khi tải dữ liệu Nhân viên: " + ex.Message);
                }
            }
        }

        private void Employee_Load(object sender, EventArgs e)
        {
            
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            string maNV = txtMaNV.Text.Trim();
            string tenNV = txtTenNV.Text.Trim();

            string query = "SELECT * FROM NHANVIEN WHERE 1=1";
            // KHÔNG cần sử dụng List<SqlParameter> nếu bạn dùng SqlCommand trực tiếp sau đó

            // 1. Thêm điều kiện tìm kiếm theo Mã NV
            if (!string.IsNullOrEmpty(maNV))
            {
                query += " AND MaNV LIKE @MaNV";
            }

            // 2. Thêm điều kiện tìm kiếm theo Tên NV
            if (!string.IsNullOrEmpty(tenNV))
            {
                query += " AND TenNV LIKE @TenNV";
            }

            // 3. Thực thi truy vấn bằng tham số hóa an toàn
            using (SqlConnection conn = kn.GetConnect())
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (!string.IsNullOrEmpty(maNV))
                            cmd.Parameters.AddWithValue("@MaNV", "%" + maNV + "%");

                        if (!string.IsNullOrEmpty(tenNV))
                            cmd.Parameters.AddWithValue("@TenNV", "%" + tenNV + "%");

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dgv_nv.DataSource = dt;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi tìm kiếm: " + ex.Message, "Lỗi hệ thống");
                }
            }
        }
        private void clear()
        {
            txtMaNV.Clear();
            txtMaBP.Clear();
            txtMaCV.Clear();
            txtMaTK.Clear();
            txtTenNV.Clear();
            txtEmail.Clear();
            txtSDT.Clear();
            cbGioiTinh.Items.Clear();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "INSERT INTO NHANVIEN VALUES ('" + txtMaNV.Text + "', N'" + txtMaBP.Text + "', N'" + txtMaCV.Text + "', '" + txtMaTK.Text + "', N'" + txtTenNV.Text + "',N'" + txtEmail.Text + "', N'" + txtSDT.Text + "', N'" + cbGioiTinh.Text + "', )";
                if (kn.ExcuteNonQuery(query) > 0)
                {
                    MessageBox.Show("Thêm thành công");
                    loadNhanVien();
                    clear();
                }
                else
                {
                    MessageBox.Show("Thêm không thành công");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgv_nv.CurrentRow == null || dgv_nv.CurrentRow.Index < 0)
                {
                    MessageBox.Show("Vui lòng chọn nhân viên cần xóa.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int index = dgv_nv.CurrentCell.RowIndex;
                string MaNV = dgv_nv.Rows[index].Cells["MaNV"].Value?.ToString();

                if (string.IsNullOrEmpty(MaNV))
                {
                    MessageBox.Show("Không tìm thấy Mã Nhân viên để xóa.", "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DialogResult result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa nhân viên có Mã: {MaNV}?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {

                    NhanVienDAL dal = new NhanVienDAL();
                    int affectedRows = dal.XoaNhanVien(MaNV);

                    if (affectedRows > 0)
                    {
                        MessageBox.Show("Xóa thành công khách hàng có Mã: " + MaNV, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        loadNhanVien();
                    }
                    else
                    {
                        MessageBox.Show("Xóa không thành công. Khách hàng có thể không tồn tại hoặc có ràng buộc khóa ngoại.", "Lỗi xóa", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi hệ thống");
            }
        }

        private void bbtnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            string maNV = txtMaNV.Text;
            string tenNV = txtTenNV.Text;
            string maBP = txtMaBP.Text;
            string maCV = txtMaCV.Text;
            string maTK = txtMaTK.Text;
            string sdtNV = txtSDT.Text;
            string gioiTinhNV = cbGioiTinh.Text;
            string emailNV = txtEmail.Text;

            NhanVienDAL dal = new NhanVienDAL();
            int affectedRows = dal.CapNhatNhanVien(
                maNV, tenNV, maBP, maCV, maTK, gioiTinhNV, sdtNV, emailNV);


            if (affectedRows > 0)
            {
                MessageBox.Show("Cập nhật thông tin nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                loadNhanVien();
            }
            else
            {
                MessageBox.Show("Cập nhật thất bại. Vui lòng kiểm tra lại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void showData(int index)
        {
            if (index < 0 || index >= dgv_nv.Rows.Count)
            {
                return;
            }
            DataGridViewRow row = dgv_nv.Rows[index];
            txtMaNV.Text = row.Cells["MaNV"].Value != null ? row.Cells["MaNV"].Value.ToString() : string.Empty;
            txtTenNV.Text = row.Cells["TenNV"].Value != null ? row.Cells["TenNV"].Value.ToString() : string.Empty;
            txtMaBP.Text = row.Cells["MaBP"].Value != null ? row.Cells["MaBP"].Value.ToString() : string.Empty;
            txtMaCV.Text = row.Cells["MaCV"].Value != null ? row.Cells["MaCV"].Value.ToString() : string.Empty;
            txtMaTK.Text = row.Cells["MaTK"].Value != null ? row.Cells["MaTK"].Value.ToString() : string.Empty;
            cbGioiTinh.Text = row.Cells["GioiTinhNV"].Value != null ? row.Cells["GioiTinhNV"].Value.ToString() : string.Empty;
            txtSDT.Text = row.Cells["SDTNV"].Value != null ? row.Cells["SDTNV"].Value.ToString() : string.Empty;
            txtEmail.Text = row.Cells["EmailNV"].Value != null ? row.Cells["EmailNV"].Value.ToString() : string.Empty;
        }
        private void btnDau_Click(object sender, EventArgs e)
        {
            btnKe.Enabled = true;
            btnCuoi.Enabled = true;

            if (dgv_nv.Rows.Count > 0)
            {
                showData(0);
                dgv_nv.CurrentCell = dgv_nv.Rows[0].Cells[0];
                btnDau.Enabled = false;
                btnTruoc.Enabled = false;
            }
            else
            {
                MessageBox.Show("Không có dữ liệu để hiển thị");
            }
        }

        private void btnTruoc_Click(object sender, EventArgs e)
        {
            btnKe.Enabled = true;
            btnCuoi.Enabled = true;

            if (dgv_nv.Rows.Count > 0)
            {
                int index = dgv_nv.CurrentCell.RowIndex;
                if (index > 0)
                {
                    index--;
                    showData(index);
                    dgv_nv.CurrentCell = dgv_nv.Rows[index].Cells[0];
                    if (index == 0)
                    {
                        btnDau.Enabled = false;
                        btnTruoc.Enabled = false;
                    }
                }
            }
        }

        private void btnKe_Click(object sender, EventArgs e)
        {
            btnDau.Enabled = true;
            btnTruoc.Enabled = true;

            if (dgv_nv.Rows.Count > 0)
            {
                int index = dgv_nv.CurrentCell.RowIndex; //lấy dòng hiện tại
                if (index < dgv_nv.Rows.Count - 1)
                {
                    index++;
                    showData(index);
                    dgv_nv.CurrentCell = dgv_nv.Rows[index].Cells[0];
                    if (index == dgv_nv.Rows.Count - (dgv_nv.AllowUserToAddRows ? 2 : 1))
                    {
                        btnKe.Enabled = false;
                        btnCuoi.Enabled = false;
                    }
                }
            }
        }

        private void btnCuoi_Click(object sender, EventArgs e)
        {
            btnDau.Enabled = true;
            btnTruoc.Enabled = true;

            if (dgv_nv.Rows.Count > 0)
            {
                int lastRow = dgv_nv.Rows.Count - (dgv_nv.AllowUserToAddRows ? 2 : 1);
                showData(lastRow);
                dgv_nv.CurrentCell = dgv_nv.Rows[lastRow].Cells[0];
                btnKe.Enabled = false;
                btnCuoi.Enabled = false;
            }
        }

        private void btn_export_Click(object sender, EventArgs e)
        {
            ExcelExporter.ExportToExcel(dgv_nv, "Danh Sách Nhân viên", "DS_NhanVien");
        }

        private void dgv_nv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
