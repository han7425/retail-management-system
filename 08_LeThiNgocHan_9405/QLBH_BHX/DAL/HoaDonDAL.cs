using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBH_BHX.DAL
{
    internal class HoaDonDAL
    {
        private Ketnoi kn = new Ketnoi();
        public DataTable ExcuteQueryWithParams(string query, params SqlParameter[] parameters)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = kn.GetConnect())
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }


        public DataTable TraCuuSanPhamTheoMa(string maSP)
        {
            string query = "SELECT GiaBan, VAT FROM SANPHAM WHERE MaSP = @MaSP";
            SqlParameter pMaSP = new SqlParameter("@MaSP", maSP);

            return ExcuteQueryWithParams(query, pMaSP);
        }
        public DataTable TraCuuKhachHangTheoMa(string maKH)
        {
            string query = "SELECT MaKH, TenKH FROM KHACHHANG WHERE MaKH = @MaKH";
            SqlParameter pMaKH = new SqlParameter("@MaKH", maKH);
            return ExcuteQueryWithParams(query, pMaKH);
        }

        public int LuuHoaDon(
            string soHD, string maNV, string maKH, DateTime ngayLap,
            string pttt, string trangThai, decimal triGiaHD, decimal giamGia,
            DataTable dtChiTiet)
        {
            using (SqlConnection conn = kn.GetConnect())
            {
                SqlTransaction transaction = conn.BeginTransaction();
                int rowsAffected = 0;

                try
                {
                    
                    string queryHD = "INSERT INTO HOADON (SoHD, MaNV, MaKH, NgayLap, PhuongThucTT, TrangThaiHD, TriGiaHD, GiamGia) " +
                                     "VALUES (@SoHD, @MaNV, @MaKH, @NgayLap, @PhuongThucTT, @TrangThaiHD, @TriGiaHD, @GiamGia)";

                    SqlCommand cmdHD = new SqlCommand(queryHD, conn, transaction);
                    cmdHD.Parameters.AddWithValue("@SoHD", soHD);
                    cmdHD.Parameters.AddWithValue("@MaNV", maNV);
                    cmdHD.Parameters.AddWithValue("@MaKH", maKH);
                    cmdHD.Parameters.AddWithValue("@NgayLap", ngayLap);
                    cmdHD.Parameters.AddWithValue("@PhuongThucTT", pttt);
                    cmdHD.Parameters.AddWithValue("@TrangThaiHD", trangThai);
                    cmdHD.Parameters.AddWithValue("@TriGiaHD", triGiaHD);
                    cmdHD.Parameters.AddWithValue("@GiamGia", giamGia);

                    rowsAffected += cmdHD.ExecuteNonQuery();

                    string queryCT = "INSERT INTO CT_HOADON (SoHD, MaSP, SoLuongHD, DonGiaHD, VATHD) " +
                                     "VALUES (@SoHD, @MaSP, @SL, @DG, @VAT)";

                    foreach (DataRow row in dtChiTiet.Rows)
                    {
                        SqlCommand cmdCT = new SqlCommand(queryCT, conn, transaction);

                        cmdCT.Parameters.AddWithValue("@SoHD", soHD);
                        cmdCT.Parameters.AddWithValue("@MaSP", row["MaSP"]);
                        cmdCT.Parameters.AddWithValue("@SL", row["SoLuong"]);
                        cmdCT.Parameters.AddWithValue("@DG", row["DonGia"]);
                        cmdCT.Parameters.AddWithValue("@VAT", row["VAT"]);

                        rowsAffected += cmdCT.ExecuteNonQuery();
                    }


                    transaction.Commit();
                    return rowsAffected;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Lỗi hệ thống khi lưu hóa đơn. Đã hoàn tác: " + ex.Message);
                }
            }
        }

         public string TraCuuTenSP(string maSP)
        {
            using (SqlConnection conn = kn.GetConnect())
            {
                string query = "SELECT TOP 1 TenSP FROM SANPHAM WHERE MaSP = @MaSP";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaSP", maSP);

                object result = cmd.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    return result.ToString();
                }
            }
            return "Không tìm thấy SP";
        }

        public class ChiTietHoaDon
        {
            public string MaSP { get; set; }
            public short SoLuongHD { get; set; } 
            public decimal DonGiaHD { get; set; }
            public decimal VATHD { get; set; } 
        }

        public bool LuuHoaDon(
    string soHD, string maNV, string maKH, DateTime ngayLap,
    string phuongThucTT, string trangThaiHD, decimal triGiaHD,
    decimal giamGia, List<ChiTietHoaDon> danhSachCTHD)
        {
            Ketnoi kn = new Ketnoi();
            SqlConnection conn = kn.GetConnect();
            SqlTransaction transaction = null;

            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                transaction = conn.BeginTransaction();
                string insertHDQuery =
                    "INSERT INTO HOADON (SoHD, MaNV, MaKH, NgayLap, PhuongThucTT, TrangThaiHD, TriGiaHD, GiamGia) " +
                    "VALUES (@SoHD, @MaNV, @MaKH, @NgayLap, @PhuongThucTT, @TrangThaiHD, @TriGiaHD, @GiamGia)";

                using (SqlCommand cmdHD = new SqlCommand(insertHDQuery, conn, transaction))
                {
                    cmdHD.Parameters.AddWithValue("@SoHD", soHD);
                    cmdHD.Parameters.AddWithValue("@MaNV", maNV);
                    cmdHD.Parameters.AddWithValue("@MaKH", maKH);
                    cmdHD.Parameters.AddWithValue("@NgayLap", ngayLap);
                    cmdHD.Parameters.AddWithValue("@PhuongThucTT", phuongThucTT);
                    cmdHD.Parameters.AddWithValue("@TrangThaiHD", trangThaiHD);
                    cmdHD.Parameters.AddWithValue("@TriGiaHD", triGiaHD);

                    cmdHD.Parameters.AddWithValue("@GiamGia", giamGia == 0 ? (object)DBNull.Value : giamGia);

                    cmdHD.ExecuteNonQuery();
                }

                string insertCTHDQuery =
                    "INSERT INTO CT_HOADON (SoHD, MaSP, SoLuongHD, DonGiaHD, VATHD) " +
                    "VALUES (@SoHD, @MaSP, @SoLuongHD, @DonGiaHD, @VATHD)";

                foreach (var ct in danhSachCTHD)
                {
                    using (SqlCommand cmdCTHD = new SqlCommand(insertCTHDQuery, conn, transaction))
                    {
                        cmdCTHD.Parameters.AddWithValue("@SoHD", soHD); 
                        cmdCTHD.Parameters.AddWithValue("@MaSP", ct.MaSP);
                        cmdCTHD.Parameters.AddWithValue("@SoLuongHD", ct.SoLuongHD);
                        cmdCTHD.Parameters.AddWithValue("@DonGiaHD", ct.DonGiaHD);
                        cmdCTHD.Parameters.AddWithValue("@VATHD", ct.VATHD);

                        cmdCTHD.ExecuteNonQuery();
                    }

                }

                transaction.Commit();
                return true;

            }
            catch (Exception ex)
            {
                transaction?.Rollback();
                throw new Exception($"Lỗi khi lưu Hóa Đơn: {ex.Message}");
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public int ThemHoaDon(string soHD, string maNV, string maKH, DateTime ngayLap,
                          string pttt, string trangThai, decimal triGia, decimal giamGia)
        {
            string query = "INSERT INTO HOADON (SoHD, MaNV, MaKH, NgayLap, PhuongThucTT, TrangThaiHD, TriGiaHD, GiamGia) " +
                           "VALUES (@SoHD, @MaNV, @MaKH, @NgayLap, @PTTT, @TrangThai, @TriGia, @GiamGia)";

            SqlParameter[] param = new SqlParameter[]
            {
            new SqlParameter("@SoHD", soHD),
            new SqlParameter("@MaNV", maNV),
            new SqlParameter("@MaKH", maKH),
            new SqlParameter("@NgayLap", ngayLap),
            new SqlParameter("@PTTT", pttt),
            new SqlParameter("@TrangThai", trangThai),
            new SqlParameter("@TriGia", triGia), 
            new SqlParameter("@GiamGia", giamGia) // Mặc định 0 nếu không nhập
            };

            return kn.ExcuteNonQueryWithParams(query, param);
        }
        public DataTable LayDuLieuInHoaDon(string soHD)
        {
            // Câu lệnh SQL JOIN 4 bảng để lấy đầy đủ thông tin hiển thị
            // Lưu ý: VAT và DonGia lấy từ bảng CT_HOADON (giá tại thời điểm bán)
            // Nếu bạn muốn lấy giá hiện tại từ kho thì đổi thành sp.GiaBan, sp.VAT (nhưng hóa đơn thì nên lấy giá lúc bán)

            string query = @"
        SELECT 
            -- Thông tin chung (Header)
            hd.SoHD, 
            hd.NgayLap, 
            nv.TenNV, 
            kh.TenKH, 
            
            -- Thông tin chi tiết sản phẩm (Body)
            ct.MaSP,
            sp.TenSP,          -- Lấy Tên SP từ bảng SANPHAM
            ct.SoLuongHD,      -- Số lượng
            ct.DonGiaHD AS GiaBan,,       -- Giá bán (Lưu trong CT_HOADON)
            ct.VATHD AS VAT,          -- VAT (Lưu trong CT_HOADON)
            
            -- Tính Thành tiền: (SL * Giá) * (1 + VAT)
            (ct.SoLuongHD * ct.DonGiaHD * (1 + ct.VATHD)) as ThanhTien 

        FROM HOADON hd
        JOIN CT_HOADON ct ON hd.SoHD = ct.SoHD
        JOIN SANPHAM sp ON ct.MaSP = sp.MaSP      -- Join để lấy TenSP
        LEFT JOIN NHANVIEN nv ON hd.MaNV = nv.MaNV -- Left Join phòng trường hợp MaNV null
        LEFT JOIN KHACHHANG kh ON hd.MaKH = kh.MaKH -- Left Join phòng trường hợp khách lẻ (null)
        
        WHERE hd.SoHD = @SoHD";

            SqlParameter pSoHD = new SqlParameter("@SoHD", soHD);

            return kn.ExcuteQueryWithParams(query, pSoHD);
        }
    }
}
