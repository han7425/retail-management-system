using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBH_BHX.DAL
{
    public class KhachHangDAL
    {
        Ketnoi kn = new Ketnoi();
        public int CapNhatKhachHang(string maKH, string tenKH, string diaChi, string sdt, string gioiTinh, string hangTV, string mst, string ngaySinh, string diemTL)
        {
            string query = "UPDATE KHACHHANG SET " +
                           "TenKH = @TenKH, DiaChiKH = @DiaChi, SDTKH = @Sdt, GioiTinhKH = @GioiTinh, " +
                           "HangThanhVien = @HangTV, MSTKH = @MST, NgaySinh = @NgaySinh, DiemTichLuy = @DiemTL " +
                           "WHERE MaKH = @MaKH"; 

            SqlParameter pTenKH = new SqlParameter("@TenKH", tenKH);
            SqlParameter pDiaChi = new SqlParameter("@DiaChi", diaChi);
            SqlParameter pSdt = new SqlParameter("@Sdt", sdt);
            SqlParameter pGioiTinh = new SqlParameter("@GioiTinh", gioiTinh);
            SqlParameter pHangTV = new SqlParameter("@HangTV", hangTV);
            SqlParameter pMST = new SqlParameter("@MST", mst);
            SqlParameter pNgaySinh = new SqlParameter("@NgaySinh", ngaySinh);
            SqlParameter pDiemTL = new SqlParameter("@DiemTL", diemTL);
            SqlParameter pMaKH = new SqlParameter("@MaKH", maKH);

            return kn.ExcuteNonQueryWithParams(query, pTenKH, pDiaChi, pSdt, pGioiTinh, pHangTV, pMST, pNgaySinh, pDiemTL, pMaKH);
        }

            public int XoaKhachHang(string maKH)
            {
                string query = "DELETE FROM KHACHHANG WHERE MaKH = @MaKH";

                SqlParameter pMaKH = new SqlParameter("@MaKH", SqlDbType.VarChar);
                pMaKH.Value = maKH;

                return kn.ExcuteNonQueryWithParams(query, pMaKH);
            }
        
    }
}
