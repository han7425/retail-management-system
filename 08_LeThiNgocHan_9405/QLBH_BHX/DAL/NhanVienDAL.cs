using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBH_BHX.DAL
{
    internal class NhanVienDAL
    {
        Ketnoi kn = new Ketnoi();

        public int CapNhatNhanVien(
            string maNV, string tenNV, string maBP, string maCV, string maTK,
            string gioiTinhNV, string sdtNV, string emailNV)
        {
            string query = "UPDATE NHANVIEN SET " +
                           "TenNV = @TenNV, MaBP = @MaBP, MaCV = @MaCV, MaTK = @MaTK, " +
                           "GioiTinhNV = @GioiTinhNV, SDTNV = @SDTNV, EmailNV = @EmailNV " +
                           "WHERE MaNV = @MaNV";

            SqlParameter pTenNV = new SqlParameter("@TenNV", tenNV);
            SqlParameter pMaBP = new SqlParameter("@MaBP", maBP);
            SqlParameter pMaCV = new SqlParameter("@MaCV", maCV);
            SqlParameter pMaTK = new SqlParameter("@MaTK", maTK);
            SqlParameter pGioiTinhNV = new SqlParameter("@GioiTinhNV", gioiTinhNV);
            SqlParameter pSDTNV = new SqlParameter("@SDTNV", sdtNV);
            SqlParameter pEmailNV = new SqlParameter("@EmailNV", emailNV);

            SqlParameter pMaNV = new SqlParameter("@MaNV", maNV); 

            return kn.ExcuteNonQueryWithParams(query,
                pTenNV, pMaBP, pMaCV, pMaTK, pGioiTinhNV, pSDTNV, pEmailNV, pMaNV);
        }
        public int XoaNhanVien(string maNV)
        {
            string query = "DELETE FROM NHANVIEN WHERE MaNV = @MaNV";
            SqlParameter pMaNV = new SqlParameter("@MaNV", maNV);
            return kn.ExcuteNonQueryWithParams(query, pMaNV);
        }
    }
}
