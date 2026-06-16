using QLBH_BHX.DAL;
using QLBH_BHX.UI.MainMenu.Sales_Management;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLBH_BHX.MainMenu.Sales_Management
{
    public partial class Products : Form
    {
        Ketnoi kn = new Ketnoi();
        public Products()
        {
            InitializeComponent();
            LoadSP();
        }
        private void LoadSP()
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM SANPHAM", kn.GetConnect());
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgv_sp.DataSource = dt;
            if (dgv_sp.Columns.Count > 0)
            {
                if (dgv_sp.Columns.Contains("MaSP"))
                    dgv_sp.Columns["MaSP"].HeaderText = "Mã SP";
                if (dgv_sp.Columns.Contains("MaKho"))
                    dgv_sp.Columns["MaKho"].HeaderText = "Mã Kho";
                if (dgv_sp.Columns.Contains("MaLoaiSP"))
                    dgv_sp.Columns["MaLoaiSP"].HeaderText = "Loại SP";
                if (dgv_sp.Columns.Contains("MaDVT"))
                    dgv_sp.Columns["MaDVT"].HeaderText = "Đơn Vị Tính";
                if (dgv_sp.Columns.Contains("MaCTKM"))
                    dgv_sp.Columns["MaCTKM"].HeaderText = "Mã KM";
                if (dgv_sp.Columns.Contains("TenSP"))
                    dgv_sp.Columns["TenSP"].HeaderText = "Tên Sản Phẩm";
                if (dgv_sp.Columns.Contains("GiaBan"))
                    dgv_sp.Columns["GiaBan"].HeaderText = "Giá Bán";
                if (dgv_sp.Columns.Contains("Barcode"))
                    dgv_sp.Columns["Barcode"].HeaderText = "Mã Vạch";
                if (dgv_sp.Columns.Contains("NSXSP"))
                    dgv_sp.Columns["NSXSP"].HeaderText = "Ngày SX";
                if (dgv_sp.Columns.Contains("HSDSP"))
                    dgv_sp.Columns["HSDSP"].HeaderText = "Hạn Sử Dụng";
                if (dgv_sp.Columns.Contains("XuatXu"))
                    dgv_sp.Columns["XuatXu"].HeaderText = "Xuất Xứ";
                if (dgv_sp.Columns.Contains("NhaCungCap"))
                    dgv_sp.Columns["NhaCungCap"].HeaderText = "Nhà Cung Cấp";
                if (dgv_sp.Columns.Contains("VAT"))
                    dgv_sp.Columns["VAT"].HeaderText = "VAT";
                if (dgv_sp.Columns.Contains("TrongLuong"))
                    dgv_sp.Columns["TrongLuong"].HeaderText = "Trọng Lượng";
                if (dgv_sp.Columns.Contains("SoLuongToiThieu"))
                    dgv_sp.Columns["SoLuongToiThieu"].HeaderText = "SL Tối Thiểu";
                if (dgv_sp.Columns.Contains("MoTaSP"))
                    dgv_sp.Columns["MoTaSP"].HeaderText = "Mô Tả";
                if (dgv_sp.Columns.Contains("HinhAnhSP"))
                {
                    dgv_sp.Columns["HinhAnhSP"].HeaderText = "Ảnh";

                }
            }
        }
        private void label33_Click(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void Products_Load(object sender, EventArgs e)
        {
            LoadSP();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            string name = txtTen.Text.Trim();
            string id = txtMaSP.Text.Trim();

            string query = "SELECT MaSP, TenSP, GiaBan, NSXSP, HSDSP, XuatXu, NhaCungCap, VAT, TrongLuong, SoLuongToiThieu " +
                           "FROM SANPHAM WHERE 1=1";

            // Nếu tìm kiếm theo Tên Sản phẩm (TenSP)
            if (!string.IsNullOrEmpty(name))
            {
                query += " AND TenSP LIKE @TenSP";
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
                dgv_sp.DataSource = dt;
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            clear();
        }
        private void clear()
        {
            txtMaSP.Clear();
            txtTen.Clear();
            txtGiaban.Clear();
            txtBarcode.Clear();
            txtNsx.Clear();
            txtHsd.Clear();
            txtXuatxu.Clear();
            txtNcc.Clear();
            txtVat.Clear();
            txtTrongluong.Clear();
            txtSLtt.Clear();
            txtMota.Clear();
            txtMakho.Clear();
            txtMaloaisp.Clear();
            txtMadvt.Clear();
            txtMactkm.Clear();
            pictureBoxHa.Image = null;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            SanPhamDAL sanPhamDAL = new SanPhamDAL();

            try
            {
                string maSP = txtMaSP.Text.Trim();
                string maKho = txtMakho.Text.Trim();
                string maLoaiSP = txtMaloaisp.Text.Trim();
                string maDVT = txtMadvt.Text.Trim();
                string maCTKM = txtMactkm.Text.Trim();
                decimal giaBan = decimal.Parse(txtGiaban.Text);
                string barcode = txtBarcode.Text.Trim();
                string tenSP = txtTen.Text.Trim();
                DateTime nsx = DateTime.Parse(txtNsx.Text);
                DateTime hsd = DateTime.Parse(txtHsd.Text);
                string xuatXu = txtXuatxu.Text.Trim();
                string ncc = txtNcc.Text.Trim();
                decimal vat = decimal.Parse(txtVat.Text);
                decimal trongLuong = decimal.Parse(txtTrongluong.Text);
                int slToiThieu = int.Parse(txtSLtt.Text);
                string moTaSP = txtMota.Text.Trim();
                int affectedRows = sanPhamDAL.CapNhatSanPham(
                    maSP, maKho, maLoaiSP, maDVT, maCTKM, giaBan, barcode, tenSP, nsx, hsd,
                    xuatXu, ncc, vat, trongLuong, slToiThieu, moTaSP
                );

                if (affectedRows > 0)
                {
                    MessageBox.Show("Thêm thành công");
                    LoadSP();
                    clear();
                }
                else
                {
                    MessageBox.Show("Thêm không thành công");
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Lỗi định dạng dữ liệu. Vui lòng kiểm tra lại Giá Bán, NSX, HSD, VAT, Trọng Lượng, SL Tối Thiểu.", "Lỗi nhập liệu");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            SanPhamDAL sanPhamDAL = new SanPhamDAL();

            try
            {

                string maSP = txtMaSP.Text.Trim();
                string maKho = txtMakho.Text.Trim();
                string maLoaiSP = txtMaloaisp.Text.Trim();
                string maDVT = txtMadvt.Text.Trim();
                string maCTKM = txtMactkm.Text.Trim();
                string barcode = txtBarcode.Text.Trim();
                string tenSP = txtTen.Text.Trim();
                string xuatXu = txtXuatxu.Text.Trim();
                string ncc = txtNcc.Text.Trim();
                string moTaSP = txtMota.Text.Trim();
                decimal giaBan = decimal.Parse(txtGiaban.Text);
                DateTime nsx = DateTime.Parse(txtNsx.Text);
                DateTime hsd = DateTime.Parse(txtHsd.Text);
                decimal vat = decimal.Parse(txtVat.Text);
                decimal trongLuong = decimal.Parse(txtTrongluong.Text);
                int slToiThieu = int.Parse(txtSLtt.Text);

                int affectedRows = sanPhamDAL.CapNhatSanPham(
                    maSP, maKho, maLoaiSP, maDVT, maCTKM,
                    giaBan, barcode, tenSP, nsx, hsd,
                    xuatXu, ncc, vat, trongLuong, slToiThieu,
                    moTaSP
                );

                if (affectedRows > 0)
                {
                    MessageBox.Show("Cập nhật thông tin sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadSP();
                }
                else
                {
                    MessageBox.Show("Cập nhật thất bại. Vui lòng kiểm tra lại Mã SP hoặc kết nối!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Lỗi định dạng dữ liệu! Vui lòng kiểm tra lại các trường số (Giá Bán, VAT, T.Lượng, SL) và Ngày tháng (NSX, HSD).", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Lỗi hệ thống");
            }
        }

        private void bbtnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void showData(int index)
        {
            if (index < 0 || index >= dgv_sp.Rows.Count)
            {
                return;
            }
            DataGridViewRow row = dgv_sp.Rows[index];

            txtMaSP.Text = row.Cells["MaSP"].Value != null ? row.Cells["MaSP"].Value.ToString() : string.Empty;
            txtMakho.Text = row.Cells["MaKho"].Value != null ? row.Cells["MaKho"].Value.ToString() : string.Empty;
            txtMaloaisp.Text = row.Cells["MaLoaiSP"].Value != null ? row.Cells["MaLoaiSP"].Value.ToString() : string.Empty;
            txtMadvt.Text = row.Cells["MaDVT"].Value != null ? row.Cells["MaDVT"].Value.ToString() : string.Empty;
            txtMactkm.Text = row.Cells["MaCTKM"].Value != null ? row.Cells["MaCTKM"].Value.ToString() : string.Empty;
            txtGiaban.Text = row.Cells["GiaBan"].Value != null ? row.Cells["GiaBan"].Value.ToString() : string.Empty;
            txtBarcode.Text = row.Cells["Barcode"].Value != null ? row.Cells["Barcode"].Value.ToString() : string.Empty;
            txtTen.Text = row.Cells["TenSP"].Value != null ? row.Cells["TenSP"].Value.ToString() : string.Empty;
            txtNsx.Text = row.Cells["NSXSP"].Value != null ? row.Cells["NSXSP"].Value.ToString() : string.Empty;
            txtHsd.Text = row.Cells["HSDSP"].Value != null ? row.Cells["HSDSP"].Value.ToString() : string.Empty;
            txtXuatxu.Text = row.Cells["XuatXu"].Value != null ? row.Cells["XuatXu"].Value.ToString() : string.Empty;
            txtNcc.Text = row.Cells["NhaCungCap"].Value != null ? row.Cells["NhaCungCap"].Value.ToString() : string.Empty;
            txtVat.Text = row.Cells["VAT"].Value != null ? row.Cells["VAT"].Value.ToString() : string.Empty;
            txtTrongluong.Text = row.Cells["TrongLuong"].Value != null ? row.Cells["TrongLuong"].Value.ToString() : string.Empty;
            txtSLtt.Text = row.Cells["SoLuongToiThieu"].Value != null ? row.Cells["SoLuongToiThieu"].Value.ToString() : string.Empty;
            txtMota.Text = row.Cells["MoTaSP"].Value != null ? row.Cells["MoTaSP"].Value.ToString() : string.Empty;
        }

        private void dgv_sp_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgv_sp.Rows.Count - 1)
            {
                return;
            }

            try
            {
                int index = e.RowIndex;
                DataGridViewRow row = dgv_sp.Rows[index];

                txtMaSP.Text = row.Cells["MaSP"].Value?.ToString() ?? string.Empty;
                txtMakho.Text = row.Cells["MaKho"].Value?.ToString() ?? string.Empty;
                txtMaloaisp.Text = row.Cells["MaLoaiSP"].Value?.ToString() ?? string.Empty;
                txtMadvt.Text = row.Cells["MaDVT"].Value?.ToString() ?? string.Empty;
                txtMactkm.Text = row.Cells["MaCTKM"].Value?.ToString() ?? string.Empty;
                txtGiaban.Text = row.Cells["GiaBan"].Value?.ToString() ?? string.Empty;
                txtBarcode.Text = row.Cells["Barcode"].Value?.ToString() ?? string.Empty;
                txtTen.Text = row.Cells["TenSP"].Value?.ToString() ?? string.Empty;
                txtNsx.Text = row.Cells["NSXSP"].Value?.ToString() ?? string.Empty;
                txtHsd.Text = row.Cells["HSDSP"].Value?.ToString() ?? string.Empty;
                txtXuatxu.Text = row.Cells["XuatXu"].Value?.ToString() ?? string.Empty;
                txtNcc.Text = row.Cells["NhaCungCap"].Value?.ToString() ?? string.Empty;
                txtVat.Text = row.Cells["VAT"].Value?.ToString() ?? string.Empty;
                txtTrongluong.Text = row.Cells["TrongLuong"].Value?.ToString() ?? string.Empty;
                txtSLtt.Text = row.Cells["SoLuongToiThieu"].Value?.ToString() ?? string.Empty;
                txtMota.Text = row.Cells["MoTaSP"].Value?.ToString() ?? string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu lên Textbox: " + ex.Message, "Lỗi");
            }
        }

        private void btnDau_Click(object sender, EventArgs e)
        {
            btnKe.Enabled = true;
            btnCuoi.Enabled = true;

            if (dgv_sp.Rows.Count > 0)
            {
                showData(0);
                dgv_sp.CurrentCell = dgv_sp.Rows[0].Cells[0];
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

            if (dgv_sp.Rows.Count > 0)
            {
                int index = dgv_sp.CurrentCell.RowIndex;
                if (index > 0)
                {
                    index--;
                    showData(index);
                    dgv_sp.CurrentCell = dgv_sp.Rows[index].Cells[0];
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

            if (dgv_sp.Rows.Count > 0)
            {
                int index = dgv_sp.CurrentCell.RowIndex; //lấy dòng hiện tại
                if (index < dgv_sp.Rows.Count - 1)
                {
                    index++;
                    showData(index);
                    dgv_sp.CurrentCell = dgv_sp.Rows[index].Cells[0];
                    if (index == dgv_sp.Rows.Count - (dgv_sp.AllowUserToAddRows ? 2 : 1))
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

            if (dgv_sp.Rows.Count > 0)
            {
                int lastRow = dgv_sp.Rows.Count - (dgv_sp.AllowUserToAddRows ? 2 : 1);
                showData(lastRow);
                dgv_sp.CurrentCell = dgv_sp.Rows[lastRow].Cells[0];
                btnKe.Enabled = false;
                btnCuoi.Enabled = false;
            }
        }

        private void pictureBoxHa_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();

            // Thiết lập bộ lọc file (chỉ cho phép chọn file ảnh)
            openFile.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";

            // Thiết lập tiêu đề hộp thoại
            openFile.Title = "Chọn Hình Ảnh Sản Phẩm";

            // Mở hộp thoại và kiểm tra nếu người dùng chọn file
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Gán đường dẫn file ảnh vừa chọn vào thuộc tính ImageLocation của PictureBox
                    // Giả định PictureBox của bạn có tên là 'pictureBoxHinhAnh'
                    pictureBoxHa.ImageLocation = openFile.FileName;

                    // Hoặc: Dùng Image.FromFile để tải ảnh vào control
                    // pictureBoxHinhAnh.Image = Image.FromFile(openFile.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không thể tải file ảnh: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgv_sp.CurrentRow == null || dgv_sp.CurrentRow.Index < 0)
                {
                    MessageBox.Show("Vui lòng chọn Sản phẩm cần xóa.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                int index = dgv_sp.CurrentCell.RowIndex;
                string maSP = dgv_sp.Rows[index].Cells["MaSP"].Value?.ToString();

                if (string.IsNullOrEmpty(maSP))
                {
                    MessageBox.Show("Không tìm thấy Mã Sản phẩm để xóa.", "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                DialogResult result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa Sản phẩm có Mã: {maSP}?",
                    "Xác nhận xóa Sản phẩm",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    SanPhamDAL dal = new SanPhamDAL();
                    int affectedRows = dal.XoaSanPham(maSP);
                    if (affectedRows > 0)
                    {
                        MessageBox.Show($"Xóa Sản phẩm: {maSP} thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadSP(); 
                    }
                    else
                    {
                        MessageBox.Show("Xóa không thành công. Sản phẩm có thể có ràng buộc khóa ngoại (ví dụ: đang nằm trong hóa đơn) hoặc lỗi CSDL.", "Lỗi xóa", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi hệ thống");
            }
        }

        private void btnThemLSP_Click(object sender, EventArgs e)
        {
            string maLoaiSP = txtMaloaisp.Text.Trim();

            if (string.IsNullOrEmpty(maLoaiSP))
            {
                MessageBox.Show("Vui lòng nhập Mã Loại Sản phẩm.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaloaisp.Focus();
                return;
            }

            LoaiSPDAL dal = new LoaiSPDAL();

            // 2. Kiểm tra sự tồn tại của Mã Loại SP
            bool exists = dal.KiemTraTonTai(maLoaiSP);

            if (!exists)
            {
                // 3. Nếu KHÔNG tồn tại: Hỏi người dùng có muốn thêm mới
                DialogResult result = MessageBox.Show(
                    $"Mã Loại Sản phẩm '{maLoaiSP}' chưa tồn tại. Bạn có muốn thêm mới không?",
                    "Xác nhận Thêm mới",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    ThemLSP themLSP = new ThemLSP();
                    themLSP.Show();
                }
                else
                {
                    txtMaloaisp.Focus();
                    txtMaloaisp.SelectAll();
                }
            }
            else
            {
                // Mã đã tồn tại: Thông báo và cho phép tiếp tục nhập
                MessageBox.Show("Mã Loại Sản phẩm này đã tồn tại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Có thể cho focus vào ô tiếp theo nếu muốn
            }
        }

        private void btn_export_Click(object sender, EventArgs e)
        {
            ExcelExporter.ExportToExcel(dgv_sp, "Danh Sách Sản Phẩm", "DS_SanPham");
        }

        private void btnThemDVT_Click(object sender, EventArgs e)
        {
            string maDVT = txtMadvt.Text.Trim();

            if (string.IsNullOrEmpty(maDVT))
            {
                MessageBox.Show("Vui lòng nhập Mã đơn vị tính của Sản phẩm.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaloaisp.Focus();
                return;
            }

            DvtDAL dal = new DvtDAL();

            // 2. Kiểm tra sự tồn tại của Mã Loại SP
            bool exists = dal.KiemTraTonTai(maDVT);

            if (!exists)
            {
                // 3. Nếu KHÔNG tồn tại: Hỏi người dùng có muốn thêm mới
                DialogResult result = MessageBox.Show(
                    $"Mã Đơn vị tính '{maDVT}' chưa tồn tại. Bạn có muốn thêm mới không?",
                    "Xác nhận Thêm mới",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    ThemDVT themDVT = new ThemDVT();
                    themDVT.Show();
                }
                else
                {
                    txtMadvt.Focus();
                    txtMadvt.SelectAll();
                }
            }
            else
            {
                // Mã đã tồn tại: Thông báo và cho phép tiếp tục nhập
                MessageBox.Show("Mã Đơn vị tính này đã tồn tại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Có thể cho focus vào ô tiếp theo nếu muốn
            }
        }
    }
}
