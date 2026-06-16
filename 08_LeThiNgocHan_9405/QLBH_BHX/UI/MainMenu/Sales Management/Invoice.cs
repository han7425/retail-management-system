using CrystalDecisions.CrystalReports.Engine;
using QLBH_BHX.DAL;
using QLBH_BHX.UI.MainMenu.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static QLBH_BHX.DAL.HoaDonDAL;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QLBH_BHX.MainMenu.Sales_Management
{
    public partial class Invoice : Form
    {
        public Invoice()
        {
            InitializeComponent();
        }

        private void Invoice_Load(object sender, EventArgs e)
        {

            SetChiTietHoaDonEnabled(false);
        }
        private void SetChiTietHoaDonEnabled(bool isEnabled)
        {
            grbCTHD.Enabled = isEnabled;
            dgv_CTHD.Enabled = isEnabled;
            btAdd.Enabled = isEnabled;
            btnThemHD.Enabled = isEnabled;
            btDelete.Enabled = isEnabled;
        }
        private void lblPrice_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            string maSP = txtMaSP.Text.Trim();
            if (string.IsNullOrEmpty(maSP))
            {
                MessageBox.Show("Vui lòng nhập Mã Sản phẩm để tra cứu.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaSP.Focus();
                return;
            }
            HoaDonDAL dal = new HoaDonDAL();
            DataTable dt = dal.TraCuuSanPhamTheoMa(maSP);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];

                txtDonGia.Text = row["GiaBan"].ToString();
                txtVAT.Text = row["VAT"].ToString();
                txtSoLuong.Focus();
            }
            else
            {
                DialogResult result = MessageBox.Show(
                    "Mã sản phẩm này chưa tồn tại, bạn có muốn thêm mới?",
                    "Xác nhận Thêm mới Sản phẩm",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    Products prd = new Products();
                    prd.Show();
                }
                else
                {
                    txtMaSP.Focus();
                    txtMaSP.SelectAll();
                }
            }
        }


        private void btnNew_Click(object sender, EventArgs e)
        {
            txtSoLuong.Clear();
            txtMaSP.Clear();

        }

        private void btnCreateNew_Click(object sender, EventArgs e)
        {
            if (IsThongTinChungValid())
            {

                SetChiTietHoaDonEnabled(true);

                txtTriGiaHD.Text = "0.00";

                txtMaSP.Focus();
            }
            else
            {

                MessageBox.Show("Vui lòng nhập đầy đủ thông tin chung trước khi tạo chi tiết hóa đơn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                SetChiTietHoaDonEnabled(false);
            }
        }

        private void btnCreateCus_Click(object sender, EventArgs e)
        {
            string maKH = txtMaKH.Text.Trim();
            if (string.IsNullOrEmpty(maKH))
            {
                MessageBox.Show("Vui lòng nhập Mã Khách hàng để tra cứu.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaSP.Focus();
                return;
            }
            HoaDonDAL dal = new HoaDonDAL();
            DataTable dt = dal.TraCuuKhachHangTheoMa(maKH);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                txtTenKH.Text = row["TenKH"].ToString();
                cbPTTT.Focus();
            }
            else
            {
                DialogResult result = MessageBox.Show(
                    "Mã khách hàng này chưa tồn tại, bạn có muốn thêm mới?",
                    "Xác nhận Thêm mới Khách hàng",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    Customers ctm = new Customers();
                    ctm.Show();
                }
                else
                {
                    txtMaKH.Focus();
                    txtMaKH.SelectAll();
                }
            }
        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaSP.Text) || string.IsNullOrEmpty(txtSoLuong.Text) || string.IsNullOrEmpty(txtDonGia.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Mã SP, Số lượng và đảm bảo Đơn giá đã được tra cứu.", "Cảnh báo");
                return;
            }

            // 2. Chuyển đổi và Tính toán (Phải kiểm tra định dạng)
            if (!int.TryParse(txtSoLuong.Text, out int soLuong) || soLuong <= 0)
            {
                MessageBox.Show("Số lượng không hợp lệ.", "Lỗi");
                txtSoLuong.Focus();
                return;
            }
            if (!decimal.TryParse(txtDonGia.Text, out decimal donGia))
            {
                MessageBox.Show("Đơn giá không hợp lệ.", "Lỗi");
                txtDonGia.Focus();
                return;
            }
            if (!decimal.TryParse(txtVAT.Text, out decimal vatRate))
            {
                vatRate = 0;
            }
            HoaDonDAL dal = new HoaDonDAL();
            string tenSP = dal.TraCuuTenSP(txtMaSP.Text);

            decimal thanhTienChuaVAT = soLuong * donGia;
            decimal tongThanhTien = thanhTienChuaVAT * (1 + vatRate);

            dgv_CTHD.Rows.Add(
                txtMaSP.Text,
                tenSP,
                soLuong,
                donGia,
                vatRate,
                tongThanhTien
            );


            CapNhatTongTienHoaDon();
            btnNew_Click(sender, e);
        }
        private void CapNhatTongTienHoaDon()
        {
            decimal tongTriGia = 0;
            const int THANH_TIEN_COLUMN_INDEX = 5;
            foreach (DataGridViewRow row in dgv_CTHD.Rows)
            {

                if (row.IsNewRow) continue;

                object thanhTienValue = row.Cells[THANH_TIEN_COLUMN_INDEX].Value;

                if (thanhTienValue != null)
                {

                    if (decimal.TryParse(thanhTienValue.ToString(), out decimal thanhTien))
                    {
                        tongTriGia += thanhTien;
                    }
                }
            }
            txtTriGiaHD.Text = tongTriGia.ToString("N0");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }

        private void btDelete_Click(object sender, EventArgs e)
        {
            btnNew_Click(sender, e);
            btnNewHD_Click(sender, e);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtNgayLap_TextChanged(object sender, EventArgs e)
        {

        }
        private bool IsThongTinChungValid()
        {
            if (string.IsNullOrEmpty(txtSoHD.Text.Trim()))
            {
                MessageBox.Show("Vui lòng nhập Số Hóa đơn.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoHD.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(txtMaKH.Text.Trim()))
            {
                MessageBox.Show("Vui lòng nhập Mã Khách hàng.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaKH.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(txtMaNV.Text.Trim()))
            {
                MessageBox.Show("Vui lòng nhập Mã Nhân viên.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaNV.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(txtTenNV.Text.Trim()))
            {
                MessageBox.Show("Vui lòng nhập Tên Nhân viên.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenNV.Focus();
                return false;
            }

            if (cbPTTT.SelectedIndex == -1 || string.IsNullOrEmpty(cbPTTT.Text))
            {
                MessageBox.Show("Vui lòng chọn Phương thức Thanh toán.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbPTTT.Focus();
                return false;
            }

            if (cbTTHD.SelectedIndex == -1 || string.IsNullOrEmpty(cbTTHD.Text))
            {
                MessageBox.Show("Vui lòng chọn Trạng thái Hóa đơn.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbTTHD.Focus();
                return false;
            }
            return true;
        }

        private void btnNewHD_Click(object sender, EventArgs e)
        {
            txtMaKH.Clear();
            txtSoHD.Clear();
            txtTriGiaHD.Clear();
            cbPTTT.SelectedIndex = -1;
            cbTTHD.SelectedIndex = -1;
        }
        private HoaDonDAL _hoaDonDAL = new HoaDonDAL();
        const int COL_MaSP = 0;
        const int COL_TenSP = 1;
        const int COL_SoLuong = 2;
        const int COL_DonGia = 3;
        const int COL_VAT = 4;
        private void btnThemHD_Click(object sender, EventArgs e)
        {
            if (!IsThongTinChungValid())
            {
                return;
            }
            string soHD = txtSoHD.Text.Trim();
            string maKH = txtMaKH.Text.Trim();
            string maNV = "NV00000005";

            DateTime ngayLap = dtpNgayLap.Value;
            string phuongThucTT = cbPTTT.Text;
            string trangThaiHD = cbTTHD.Text;

            string rawTriGia = txtTriGiaHD.Text.Replace(".", "").Replace(",", "");
            if (!decimal.TryParse(rawTriGia, out decimal triGiaHD))
            {
                MessageBox.Show("Lỗi: Trị giá Hóa đơn không hợp lệ.", "Lỗi");
                return;
            }

            decimal giamGia = 0;

            var danhSachCTHD = new List<ChiTietHoaDon>();
            foreach (DataGridViewRow row in dgv_CTHD.Rows)
            {
                if (row.IsNewRow) continue;

                try
                {
                    var ct = new ChiTietHoaDon();
                    ct.MaSP = row.Cells[COL_MaSP].Value.ToString();

                    if (short.TryParse(row.Cells[COL_SoLuong].Value.ToString(), out short soLuong))
                        ct.SoLuongHD = soLuong;
                    else throw new Exception("Số lượng không hợp lệ.");

                    if (decimal.TryParse(row.Cells[COL_DonGia].Value.ToString(), out decimal donGia))
                        ct.DonGiaHD = donGia;
                    else throw new Exception("Đơn giá không hợp lệ.");

                    if (decimal.TryParse(row.Cells[COL_VAT].Value.ToString(), out decimal vatRate))
                        ct.VATHD = vatRate;
                    else throw new Exception("VAT không hợp lệ.");

                    danhSachCTHD.Add(ct);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi thu thập dữ liệu chi tiết hóa đơn: {ex.Message} tại dòng {row.Index}.", "Lỗi Dữ Liệu");
                    return;
                }
            }

            try
            {
                bool success = _hoaDonDAL.LuuHoaDon(
                    soHD, maNV, maKH, ngayLap, phuongThucTT, trangThaiHD,
                    triGiaHD, giamGia, danhSachCTHD);

                if (success)
                {
                    MessageBox.Show("Lưu Hóa đơn thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btDelete_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi Lưu Hóa Đơn");
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            ExcelExporter.ExportToExcel(dgv_CTHD, "Chi tiết hóa đơn bán hàng", "ChiTietHoaDon");
        }

        private void txtTriGiaHD_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            ReportDocument rpt = new ReportDocument();

            // Đường dẫn file (Dùng đường dẫn tương đối cho chắc ăn)
            string path = Path.Combine(Application.StartupPath, @"D:\HÂN\QLBH_BHX (C Sharp)\QLBH_BHX\UI\MainMenu\Reports\HoaDonBanHang.rpt");

            // Nạp file report
            rpt.Load(path);


            // -----------------------------------------------------------
            // BƯỚC 3: HIỂN THỊ LUÔN (Bỏ qua các bước kiểm tra rườm rà)
            // -----------------------------------------------------------

            InHoaDon frm = new InHoaDon();
            frm.crystalReportViewer1.ReportSource = rpt;
            frm.crystalReportViewer1.Refresh();
            frm.ShowDialog(); // Hiện lên luôn
        }
    }
}


