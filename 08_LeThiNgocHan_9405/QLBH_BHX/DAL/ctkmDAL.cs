using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBH_BHX.DAL
{
    internal class ctkmDAL
    {
        Ketnoi kn = new Ketnoi();
        public int CapNhatCTKM(
        string maCTKM, string tenCTKM, string moTaCTKM,
        DateTime ngayBatDau, DateTime ngayKetThuc)
        {
            string query = "UPDATE CTKM SET " +
                           "TenCTKM = @TenCTKM, MoTaCTKM = @MoTaCTKM, " +
                           "NgayBatDau = @NgayBD, NgayKetThuc = @NgayKT " +
                           "WHERE MaCTKM = @MaCTKM";

            SqlParameter pTenCTKM = new SqlParameter("@TenCTKM", tenCTKM);
            SqlParameter pMoTaCTKM = new SqlParameter("@MoTaCTKM", moTaCTKM);
            SqlParameter pNgayBatDau = new SqlParameter("@NgayBD", SqlDbType.DateTime);
            pNgayBatDau.Value = ngayBatDau;
            SqlParameter pNgayKetThuc = new SqlParameter("@NgayKT", SqlDbType.DateTime);
            pNgayKetThuc.Value = ngayKetThuc;
            SqlParameter pMaCTKM = new SqlParameter("@MaCTKM", maCTKM);
            return kn.ExcuteNonQueryWithParams(query,
                pTenCTKM, pMoTaCTKM, pNgayBatDau, pNgayKetThuc, pMaCTKM);
        }



        public int ThemCTKM(
        string maCTKM, string tenCTKM, string moTaCTKM, DateTime ngayBatDau, DateTime ngayKetThuc)
        {
            string insertQuery = "INSERT INTO CTKM (MaCTKM, TenCTKM, MoTaCTKM, NgayBatDau, NgayKetThuc) " +
                                 "VALUES (@MaCTKM, @TenCTKM, @MoTaCTKM, @NgayBD, @NgayKT)";
            SqlParameter pTenCTKM = new SqlParameter("@TenCTKM", tenCTKM);
            SqlParameter pMoTaCTKM = new SqlParameter("@MoTaCTKM", moTaCTKM);
            SqlParameter pNgayBatDau = new SqlParameter("@NgayBD", SqlDbType.DateTime);
            pNgayBatDau.Value = ngayBatDau;
            SqlParameter pNgayKetThuc = new SqlParameter("@NgayKT", SqlDbType.DateTime);
            pNgayKetThuc.Value = ngayKetThuc;
            SqlParameter pMaCTKM = new SqlParameter("@MaCTKM", maCTKM);
            return kn.ExcuteNonQueryWithParams(maCTKM,
                pTenCTKM, pMoTaCTKM, pNgayBatDau, pNgayKetThuc, pMaCTKM);
        }
        public int XoaCTKM(string maCTKM)
        {
            string query = "DELETE FROM CTKM WHERE MaCTKM = @MaCTKM";

            SqlParameter pMaCTKM = new SqlParameter("@MaCTKM", SqlDbType.Char, 10);
            pMaCTKM.Value = maCTKM;

            return kn.ExcuteNonQueryWithParams(query, pMaCTKM);
        }
    }
}

