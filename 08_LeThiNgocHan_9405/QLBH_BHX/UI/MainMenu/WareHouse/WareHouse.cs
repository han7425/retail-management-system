using QLBH_BHX.DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLBH_BHX.MainMenu.Products_Management
{
    public partial class WareHouse : Form
    {
        Ketnoi kn = new Ketnoi();
        public WareHouse()
        {
            InitializeComponent();
            LoadTonKho();
        }
        private void LoadTonKho()
        {
            string query = "SELECT * FROM TONKHO";

            // Giả định kn.GetConnect() trả về kết nối đã mở.
            using (SqlConnection conn = kn.GetConnect())
            {
                try
                {
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgv_tk.DataSource = dt;
                    if (dgv_tk.Columns.Count > 0)
                    {
                        if (dgv_tk.Columns.Contains("MaSP"))
                            dgv_tk.Columns["MaSP"].HeaderText = "Mã SP";
                        if (dgv_tk.Columns.Contains("MaKho"))
                            dgv_tk.Columns["MaKho"].HeaderText = "Mã Kho";
                        if (dgv_tk.Columns.Contains("NgayCapNhat"))
                            dgv_tk.Columns["NgayCapNhat"].HeaderText = "Ngày Cập Nhật";
                        if (dgv_tk.Columns.Contains("ThangTK"))
                            dgv_tk.Columns["ThangTK"].HeaderText = "Tháng TK";
                        if (dgv_tk.Columns.Contains("NamTK"))
                            dgv_tk.Columns["NamTK"].HeaderText = "Năm TK";
                        if (dgv_tk.Columns.Contains("TonDauKy"))
                            dgv_tk.Columns["TonDauKy"].HeaderText = "Tồn Đầu Kỳ";
                        if (dgv_tk.Columns.Contains("TriGiaTDK"))
                            dgv_tk.Columns["TriGiaTDK"].HeaderText = "Trị Giá TĐK";
                        if (dgv_tk.Columns.Contains("NhapTK"))
                            dgv_tk.Columns["NhapTK"].HeaderText = "Nhập TK";
                        if (dgv_tk.Columns.Contains("TriGiaNTK"))
                            dgv_tk.Columns["TriGiaNTK"].HeaderText = "Trị Giá NTK";
                        if (dgv_tk.Columns.Contains("XuatTK"))
                            dgv_tk.Columns["XuatTK"].HeaderText = "Xuất TK";
                        if (dgv_tk.Columns.Contains("TriGiaXTK"))
                            dgv_tk.Columns["TriGiaXTK"].HeaderText = "Trị Giá XTK";
                        if (dgv_tk.Columns.Contains("TonCK"))
                            dgv_tk.Columns["TonCK"].HeaderText = "Tồn Cuối Kỳ";
                        if (dgv_tk.Columns.Contains("TriGiaTCK"))
                            dgv_tk.Columns["TriGiaTCK"].HeaderText = "Trị Giá TCK";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải dữ liệu Tồn Kho: " + ex.Message, "Lỗi SQL");
                }
            }
        }
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void WareHouse_Load(object sender, EventArgs e)
        {

        }

        private void dgv_tk_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgv_tk.Rows.Count - 1)
            {
                return;
            }

            try
            {
                int index = e.RowIndex;
                DataGridViewRow row = dgv_tk.Rows[index];

                txtMaSP.Text = row.Cells["MaSP"].Value?.ToString() ?? string.Empty;
                txtMaKho.Text = row.Cells["MaKho"].Value?.ToString() ?? string.Empty;
                dtpNgayCN.Text = row.Cells["NgayCapNhat"].Value?.ToString() ?? string.Empty;
                txtThangTK.Text = row.Cells["ThangTK"].Value?.ToString() ?? string.Empty;
                txtNamTK.Text = row.Cells["NamTK"].Value?.ToString() ?? string.Empty;
                txtTDK.Text = row.Cells["TonDauKy"].Value?.ToString() ?? string.Empty;
                txtTriGiaTDK.Text = row.Cells["TriGiaTDK"].Value?.ToString() ?? string.Empty;
                txtNhapTK.Text = row.Cells["NhapTK"].Value?.ToString() ?? string.Empty;
                txtTriGiaNTK.Text = row.Cells["TriGiaNTK"].Value?.ToString() ?? string.Empty;
                txtXuatTK.Text = row.Cells["XuatTK"].Value?.ToString() ?? string.Empty;
                txtTriGiaXTK.Text = row.Cells["TriGiaXTK"].Value?.ToString() ?? string.Empty;
                txtTCK.Text = row.Cells["TonCK"].Value?.ToString() ?? string.Empty;
                txtTriGiaTCK.Text = row.Cells["TriGiaTCK"].Value?.ToString() ?? string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu lên Textbox: " + ex.Message, "Lỗi");
            }
        }

        private void btn_search_Click_1(object sender, EventArgs e)
        {
            string name = txtMaKho.Text.Trim();
            string id = txtMaSP.Text.Trim();

            string query = "SELECT MaSP, MaKho, NgayCapNhat, ThangTK, NamTK, TonDauKy, TriGiaTDK, NhapTK, TriGiaNTK, TriGiaXTK,TonCK,TriGiaTCK " +
                           "FROM TONKHO WHERE 1=1";

            // Nếu tìm kiếm theo mã Kho
            if (!string.IsNullOrEmpty(name))
            {
                query += " AND MaKho LIKE @MaKho";
            }

            // Nếu tìm kiếm theo Mã Sản phẩm (MaSP)
            if (!string.IsNullOrEmpty(id))
            {
                query += " AND MaSP LIKE @MaSP";
            }

            using (SqlConnection conn = kn.GetConnect())
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                if (!string.IsNullOrEmpty(name))
                    cmd.Parameters.AddWithValue("@TenSP", "%" + name + "%");
                if (!string.IsNullOrEmpty(id))
                    cmd.Parameters.AddWithValue("@MaSP", "%" + id + "%");
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgv_tk.DataSource = dt;
            }
        }

        private void clear()
        {
            txtMaSP.Clear();
            txtMaKho.Clear();
            txtThangTK.Clear();
            txtNamTK.Clear();
            txtTDK.Clear();
            txtTriGiaTDK.Clear();
            txtNhapTK.Clear();
            txtTriGiaNTK.Clear();
            txtXuatTK.Clear();
            txtTriGiaXTK.Clear();
            txtTCK.Clear();
            txtTriGiaTCK.Clear();
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            clear();
        }
        
        private void btnThem_Click(object sender, EventArgs e)
        {
            TonKhoDAL tonKhoDAL = new TonKhoDAL();

            try
            {
                string maSP = txtMaSP.Text.Trim();
                string maKho = txtMaKho.Text.Trim();
                DateTime ngayCapNhat = dtpNgayCN.Value;
                string thangTK = txtThangTK.Text.Trim();
                string namTK = txtNamTK.Text.Trim();
                decimal tonDauKy = decimal.Parse(txtTDK.Text);
                decimal triGiaTDK = decimal.Parse(txtTriGiaTDK.Text);
                decimal nhapTK = decimal.Parse(txtNhapTK.Text);
                decimal triGiaNTK = decimal.Parse(txtTriGiaNTK.Text);
                decimal xuatTK = decimal.Parse(txtXuatTK.Text);
                decimal triGiaXTK = decimal.Parse(txtTriGiaXTK.Text);
                decimal tonCK = decimal.Parse(txtTCK.Text);
                decimal triGiaTCK = decimal.Parse(txtTriGiaTCK.Text);

                int affectedRows = tonKhoDAL.CapNhatTonKho(
                    maSP, maKho, ngayCapNhat, thangTK, namTK,
                    tonDauKy, triGiaTDK, nhapTK, triGiaNTK,
                    xuatTK, triGiaXTK, tonCK, triGiaTCK
                );

                if (affectedRows > 0)
                {
                    MessageBox.Show("Cập nhật thông tin Tồn Kho thành công!");
                    LoadTonKho(); 
                    clear(); 
                }
                else
                {
                    MessageBox.Show("Cập nhật Tồn Kho không thành công. (Có thể do bản ghi đã tồn tại và lỗi update, hoặc chưa tồn tại và cần lệnh INSERT riêng).");
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Lỗi định dạng dữ liệu. Vui lòng kiểm tra lại các trường Số lượng và Trị giá.", "Lỗi nhập liệu");
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
                if (dgv_tk.CurrentRow == null || dgv_tk.CurrentRow.Index < 0)
                {
                    MessageBox.Show("Vui lòng chọn bản ghi Tồn Kho cần xóa.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int index = dgv_tk.CurrentCell.RowIndex;
                string maSP = dgv_tk.Rows[index].Cells["MaSP"].Value?.ToString();
                string maKho = dgv_tk.Rows[index].Cells["MaKho"].Value?.ToString();

                if (string.IsNullOrEmpty(maSP) || string.IsNullOrEmpty(maKho))
                {
                    MessageBox.Show("Không tìm thấy Mã Sản phẩm hoặc Mã Kho để xóa.", "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                DialogResult result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa tồn kho của SP: {maSP} tại Kho: {maKho}?",
                    "Xác nhận xóa Tồn Kho",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {

                    TonKhoDAL dal = new TonKhoDAL();
                    int affectedRows = dal.XoaTonKho(maSP, maKho); 

                    if (affectedRows > 0)
                    {
                        MessageBox.Show($"Xóa tồn kho SP: {maSP} thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadTonKho(); 
                    }
                    else
                    {
                        MessageBox.Show("Xóa không thành công. Bản ghi không tồn tại hoặc lỗi CSDL.", "Lỗi xóa", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi hệ thống");
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            TonKhoDAL tonKhoDAL = new TonKhoDAL();

            try
            {
                string maSP = txtMaSP.Text.Trim();
                string maKho = txtMaKho.Text.Trim();

                DateTime ngayCapNhat = dtpNgayCN.Value;

                string thangTK = txtThangTK.Text.Trim();
                string namTK = txtNamTK.Text.Trim();

                decimal tonDauKy = decimal.Parse(txtTDK.Text);
                decimal triGiaTDK = decimal.Parse(txtTriGiaTDK.Text);

                decimal nhapTK = decimal.Parse(txtNhapTK.Text);
                decimal triGiaNTK = decimal.Parse(txtTriGiaNTK.Text);

                decimal xuatTK = decimal.Parse(txtXuatTK.Text);
                decimal triGiaXTK = decimal.Parse(txtTriGiaXTK.Text);

                decimal tonCK = decimal.Parse(txtTCK.Text);
                decimal triGiaTCK = decimal.Parse(txtTriGiaTCK.Text);

                int affectedRows = tonKhoDAL.CapNhatTonKho(
                    maSP, maKho, ngayCapNhat, thangTK, namTK,
                    tonDauKy, triGiaTDK, nhapTK, triGiaNTK,
                    xuatTK, triGiaXTK, tonCK, triGiaTCK
                );


                if (affectedRows > 0)
                {
                    MessageBox.Show("Cập nhật thông tin Tồn Kho thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadTonKho(); 
                }
                else
                {
                    MessageBox.Show("Cập nhật thất bại. Vui lòng kiểm tra lại Mã SP/Mã Kho hoặc kết nối!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (FormatException)
            {

                MessageBox.Show("Lỗi định dạng dữ liệu. Vui lòng kiểm tra lại các trường Số lượng và Trị giá.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Lỗi hệ thống");
            }
        }

        private void showData (int index)
        {
            if (index < 0 || index >= dgv_tk.Rows.Count)
            {
                return;
            }
            DataGridViewRow row = dgv_tk.Rows[index];

            txtMaSP.Text = row.Cells["MaSP"].Value?.ToString() ?? string.Empty;
            txtMaKho.Text = row.Cells["MaKho"].Value?.ToString() ?? string.Empty;
            object ngayCNValue = row.Cells["NgayCapNhat"].Value;
            if (ngayCNValue != null && ngayCNValue != DBNull.Value && DateTime.TryParse(ngayCNValue.ToString(), out DateTime ngayCN))
            {
                dtpNgayCN.Value = ngayCN;
            }
            else
            {
                MessageBox.Show("Ngày không hợp lệ!");
            }

            txtThangTK.Text = row.Cells["ThangTK"].Value?.ToString() ?? string.Empty;
            txtNamTK.Text = row.Cells["NamTK"].Value?.ToString() ?? string.Empty;
            txtTDK.Text = row.Cells["TonDauKy"].Value?.ToString() ?? string.Empty;
            txtTriGiaTDK.Text = row.Cells["TriGiaTDK"].Value?.ToString() ?? string.Empty;
            txtNhapTK.Text = row.Cells["NhapTK"].Value?.ToString() ?? string.Empty;
            txtTriGiaNTK.Text = row.Cells["TriGiaNTK"].Value?.ToString() ?? string.Empty;
            txtXuatTK.Text = row.Cells["XuatTK"].Value?.ToString() ?? string.Empty;
            txtTriGiaXTK.Text = row.Cells["TriGiaXTK"].Value?.ToString() ?? string.Empty;
            txtTCK.Text = row.Cells["TonCK"].Value?.ToString() ?? string.Empty;
            txtTriGiaTCK.Text = row.Cells["TriGiaTCK"].Value?.ToString() ?? string.Empty;
        }

        private void btnDau_Click(object sender, EventArgs e)
        {

            btnKe.Enabled = true;
            btnCuoi.Enabled = true;

            if (dgv_tk.Rows.Count > 0)
            {
                showData(0);
                dgv_tk.CurrentCell = dgv_tk.Rows[0].Cells[0];
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

            if (dgv_tk.Rows.Count > 0)
            {
                int index = dgv_tk.CurrentCell.RowIndex;
                if (index > 0)
                {
                    index--;
                    showData(index);
                    dgv_tk.CurrentCell = dgv_tk.Rows[index].Cells[0];
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

            if (dgv_tk.Rows.Count > 0)
            {
                int index = dgv_tk.CurrentCell.RowIndex; //lấy dòng hiện tại
                if (index < dgv_tk.Rows.Count - 1)
                {
                    index++;
                    showData(index);
                    dgv_tk.CurrentCell = dgv_tk.Rows[index].Cells[0];
                    if (index == dgv_tk.Rows.Count - (dgv_tk.AllowUserToAddRows ? 2 : 1))
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

            if (dgv_tk.Rows.Count > 0)
            {
                int lastRow = dgv_tk.Rows.Count - (dgv_tk.AllowUserToAddRows ? 2 : 1);
                showData(lastRow);
                dgv_tk.CurrentCell = dgv_tk.Rows[lastRow].Cells[0];
                btnKe.Enabled = false;
                btnCuoi.Enabled = false;
            }
        }

        
        private void btn_export_Click(object sender, EventArgs e)
        {
            ExcelExporter.ExportToExcel(dgv_tk, "Danh Sách Tồn kho Sản phẩm", "DS_TonKho");
        }

        private void bbtnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

