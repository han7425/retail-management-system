using OfficeOpenXml;
using OfficeOpenXml.Style;
using QLBH_BHX.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLBH_BHX.MainMenu.Sales_Management
{
    public partial class Customers : Form
    {
        Ketnoi kn = new Ketnoi();
        public Customers()
        {
            InitializeComponent();
            LoadKhachHang();
        }
        private void LoadKhachHang()
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM KHACHHANG", kn.GetConnect());
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgv_cus.DataSource = dt;
            if (dgv_cus.Columns.Count > 0)
            {
                if (dgv_cus.Columns.Contains("MaKH"))
                    dgv_cus.Columns["MaKH"].HeaderText = "Mã KH";
                if (dgv_cus.Columns.Contains("TenKH"))
                    dgv_cus.Columns["TenKH"].HeaderText = "Tên Khách Hàng";
                if (dgv_cus.Columns.Contains("GioiTinhKH"))
                    dgv_cus.Columns["GioiTinhKH"].HeaderText = "Giới Tính";
                if (dgv_cus.Columns.Contains("DiaChiKH"))
                    dgv_cus.Columns["DiaChiKH"].HeaderText = "Địa Chỉ";
                if (dgv_cus.Columns.Contains("SDTKH"))
                    dgv_cus.Columns["SDTKH"].HeaderText = "SĐT";
                if (dgv_cus.Columns.Contains("NgaySinh"))
                    dgv_cus.Columns["NgaySinh"].HeaderText = "Ngày Sinh";
                if (dgv_cus.Columns.Contains("MSTKH"))
                    dgv_cus.Columns["MSTKH"].HeaderText = "Mã Số Thuế";
                if (dgv_cus.Columns.Contains("HangThanhVien"))
                    dgv_cus.Columns["HangThanhVien"].HeaderText = "Hạng TV";
                if (dgv_cus.Columns.Contains("DiemTichLuy"))
                    dgv_cus.Columns["DiemTichLuy"].HeaderText = "Điểm TL";
            }
        }    
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Customers_Load(object sender, EventArgs e)
        {
            LoadKhachHang();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {

            string name = txtTen.Text.Trim();
            string id = txtMaKH.Text.Trim();

            string query = "SELECT MaKH, TenKH, SDTKH, GioiTinhKH, SDTKH, NgaySinh, DiaChiKH, HangThanhVien, DiemTichLuy FROM KHACHHANG WHERE 1=1";

            // Nếu người dùng nhập tên
            if (!string.IsNullOrEmpty(name))
            {
                query += " AND TenKH LIKE @TenKH";
            }

            // Nếu người dùng nhập mã khách hàng
            if (!string.IsNullOrEmpty(id))
            {
                query += " AND MaKH LIKE @MaKH";
            }

            using (SqlConnection conn = kn.GetConnect())
            {
                SqlCommand cmd = new SqlCommand(query, conn);

                if (!string.IsNullOrEmpty(name))
                    cmd.Parameters.AddWithValue("@TenKH", "%" + name + "%");

                if (!string.IsNullOrEmpty(id))
                    cmd.Parameters.AddWithValue("@MaKH", "%" + id + "%");

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgv_cus.DataSource = dt; 
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            clear();
          
        }
        private void clear()
        {
            txtTen.Clear();
            txtMaKH.Clear();
            txtMST.Clear();
            txtNgaysinh.Clear();
            txtSdt.Clear();
            txtHangtv.Clear();
            txtDiachi.Clear();
            txtDiemtl.Clear();
            txtGioi.Clear();
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "INSERT INTO KHACHHANG VALUES ('" + txtMaKH.Text + "', N'" + txtTen.Text + "', N'" + txtGioi.Text + "', '" + txtSdt.Text + "', N'" +txtNgaysinh.Text+ "',N'" +txtDiachi.Text+ "', N'" +txtDiemtl.Text+ "', N'" +txtHangtv.Text+ "', N'" +txtMST.Text+"' )";
                if (kn.ExcuteNonQuery(query) > 0)
                {
                    MessageBox.Show("Thêm thành công");
                    LoadKhachHang();
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
                if (dgv_cus.CurrentRow == null || dgv_cus.CurrentRow.Index < 0)
                {
                    MessageBox.Show("Vui lòng chọn khách hàng cần xóa.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int index = dgv_cus.CurrentCell.RowIndex;
                string MaKH = dgv_cus.Rows[index].Cells["MaKH"].Value?.ToString();

                if (string.IsNullOrEmpty(MaKH))
                {
                    MessageBox.Show("Không tìm thấy Mã Khách hàng để xóa.", "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DialogResult result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa khách hàng có Mã: {MaKH}?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    
                    KhachHangDAL dal = new KhachHangDAL();
                    int affectedRows = dal.XoaKhachHang(MaKH);

                    if (affectedRows > 0)
                    {
                        MessageBox.Show("Xóa thành công khách hàng có Mã: " + MaKH, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadKhachHang(); 
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

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            string maKH = txtMaKH.Text; 
            string tenKH = txtTen.Text;
            string mstKH = txtMST.Text;
            string ngaySinh = txtNgaysinh.Text;
            string sdtKH = txtSdt.Text;
            string diemTL = txtDiemtl.Text;
            string gioiTinh = txtGioi.Text;
            string hangTV = txtHangtv.Text;
            string diaChi = txtDiachi.Text;

            KhachHangDAL dal = new KhachHangDAL();
            int affectedRows = dal.CapNhatKhachHang(
                maKH, tenKH, diaChi, sdtKH, gioiTinh, hangTV, mstKH, ngaySinh, diemTL
            );

            
            if (affectedRows > 0)
            {
                MessageBox.Show("Cập nhật thông tin khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                
                LoadKhachHang(); 
            }
            else
            {
                MessageBox.Show("Cập nhật thất bại. Vui lòng kiểm tra lại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgv_cus_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgv_cus.Rows.Count - 1) 
            {
                return;
            }

            try
            {
               
                int index = e.RowIndex;
                txtMaKH.Text = dgv_cus.Rows[index].Cells["MaKH"].Value?.ToString() ?? string.Empty;
                txtTen.Text = dgv_cus.Rows[index].Cells["TenKH"].Value?.ToString() ?? string.Empty;
                txtMST.Text = dgv_cus.Rows[index].Cells["MSTKH"].Value?.ToString() ?? string.Empty;
                txtNgaysinh.Text = dgv_cus.Rows[index].Cells["NgaySinh"].Value?.ToString() ?? string.Empty;
                txtSdt.Text = dgv_cus.Rows[index].Cells["SDTKH"].Value?.ToString() ?? string.Empty;
                txtDiemtl.Text = dgv_cus.Rows[index].Cells["DiemTichLuy"].Value?.ToString() ?? string.Empty;
                txtGioi.Text = dgv_cus.Rows[index].Cells["GioiTinhKH"].Value?.ToString() ?? string.Empty;
                txtHangtv.Text = dgv_cus.Rows[index].Cells["HangThanhVien"].Value?.ToString() ?? string.Empty;
                txtDiachi.Text = dgv_cus.Rows[index].Cells["DiaChiKH"].Value?.ToString() ?? string.Empty;
            }
            catch (Exception ex)
            {
               
                MessageBox.Show("Lỗi khi tải dữ liệu lên Textbox: " + ex.Message, "Lỗi");
            }
        }
        private void showData(int index)
        {
            if (index < 0 || index >= dgv_cus.Rows.Count)
            {
                return;
            }
            DataGridViewRow row = dgv_cus.Rows[index];
            txtMaKH.Text = row.Cells["MaKH"].Value != null ? row.Cells["MaKH"].Value.ToString() : string.Empty;
            txtTen.Text = row.Cells["TenKH"].Value != null ? row.Cells["TenKH"].Value.ToString() : string.Empty;
            txtMST.Text = row.Cells["MSTKH"].Value != null ? row.Cells["MSTKH"].Value.ToString() : string.Empty;
            txtNgaysinh.Text = row.Cells["NgaySinh"].Value != null ? row.Cells["NgaySinh"].Value.ToString() : string.Empty;
            txtSdt.Text = row.Cells["SDTKH"].Value != null ? row.Cells["SDTKH"].Value.ToString() : string.Empty;
            txtDiemtl.Text = row.Cells["DiemTichLuy"].Value != null ? row.Cells["DiemTichLuy"].Value.ToString() : string.Empty;
            txtGioi.Text = row.Cells["GioiTinhKH"].Value != null ? row.Cells["GioiTinhKH"].Value.ToString() : string.Empty;
            txtHangtv.Text = row.Cells["HangThanhVien"].Value != null ? row.Cells["HangThanhVien"].Value.ToString() : string.Empty;
            txtDiachi.Text = row.Cells["DiaChiKH"].Value != null ? row.Cells["DiaChiKH"].Value.ToString() : string.Empty;
        }
        private void btnDau_Click(object sender, EventArgs e)
        {
            btnKe.Enabled = true;
            btnCuoi.Enabled = true;

            if (dgv_cus.Rows.Count > 0)
            {
                showData(0);
                dgv_cus.CurrentCell = dgv_cus.Rows[0].Cells[0];
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

            if (dgv_cus.Rows.Count > 0)
            {
                int index = dgv_cus.CurrentCell.RowIndex; 
                if (index > 0) 
                {
                    index--;
                    showData(index);
                    dgv_cus.CurrentCell = dgv_cus.Rows[index].Cells[0];
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

            if (dgv_cus.Rows.Count > 0)
            {
                int index = dgv_cus.CurrentCell.RowIndex; //lấy dòng hiện tại
                if (index < dgv_cus.Rows.Count - 1)
                {
                    index++;
                    showData(index);
                    dgv_cus.CurrentCell = dgv_cus.Rows[index].Cells[0];
                    if (index == dgv_cus.Rows.Count - (dgv_cus.AllowUserToAddRows ? 2 : 1))
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

            if (dgv_cus.Rows.Count > 0)
            {
                int lastRow = dgv_cus.Rows.Count - (dgv_cus.AllowUserToAddRows ? 2 : 1);
                showData(lastRow);
                dgv_cus.CurrentCell = dgv_cus.Rows[lastRow].Cells[0];
                btnKe.Enabled = false;
                btnCuoi.Enabled = false;
            }
        }

        private void dgv_cus_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btn_export_Click(object sender, EventArgs e)
        {
            ExcelExporter.ExportToExcel(dgv_cus, "Danh Sách Khách Hàng", "DS_KhachHang");
        }
    }
 }

