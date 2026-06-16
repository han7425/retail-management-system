using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBH_BHX.DAL
{
    internal class TonKhoDAL
    {
        Ketnoi kn = new Ketnoi();

        public int CapNhatTonKho(
            string maSP, string maKho, DateTime ngayCapNhat, string thangTK, string namTK,
            decimal tonDauKy, decimal triGiaTDK, decimal nhapTK, decimal triGiaNTK,
            decimal xuatTK, decimal triGiaXTK, decimal tonCK, decimal triGiaTCK)
        {

            string query = "UPDATE TONKHO SET " +
                           "NgayCapNhat = @NgayCN, ThangTK = @ThangTK, NamTK = @NamTK, " +
                           "TonDauKy = @TDK, TriGiaTDK = @TriGiaTDK, NhapTK = @NhapTK, " +
                           "TriGiaNTK = @TriGiaNTK, XuatTK = @XuatTK, TriGiaXTK = @TriGiaXTK, " +
                           "TonCK = @TCK, TriGiaTCK = @TriGiaTCK " +
                           "WHERE MaSP = @MaSP AND MaKho = @MaKho"; 

            SqlParameter pMaSP = new SqlParameter("@MaSP", maSP);
            SqlParameter pMaKho = new SqlParameter("@MaKho", maKho);

            SqlParameter pNgayCapNhat = new SqlParameter("@NgayCN", SqlDbType.DateTime);
            pNgayCapNhat.Value = ngayCapNhat;

            SqlParameter pThangTK = new SqlParameter("@ThangTK", thangTK);
            SqlParameter pNamTK = new SqlParameter("@NamTK", namTK);

            SqlParameter pTonDauKy = new SqlParameter("@TDK", SqlDbType.Decimal);
            pTonDauKy.Value = tonDauKy;
            SqlParameter pTriGiaTDK = new SqlParameter("@TriGiaTDK", SqlDbType.Decimal);
            pTriGiaTDK.Value = triGiaTDK;

            SqlParameter pNhapTK = new SqlParameter("@NhapTK", SqlDbType.Decimal);
            pNhapTK.Value = nhapTK;
            SqlParameter pTriGiaNTK = new SqlParameter("@TriGiaNTK", SqlDbType.Decimal);
            pTriGiaNTK.Value = triGiaNTK;

            SqlParameter pXuatTK = new SqlParameter("@XuatTK", SqlDbType.Decimal);
            pXuatTK.Value = xuatTK;
            SqlParameter pTriGiaXTK = new SqlParameter("@TriGiaXTK", SqlDbType.Decimal);
            pTriGiaXTK.Value = triGiaXTK;

            SqlParameter pTonCK = new SqlParameter("@TCK", SqlDbType.Decimal);
            pTonCK.Value = tonCK;
            SqlParameter pTriGiaTCK = new SqlParameter("@TriGiaTCK", SqlDbType.Decimal);
            pTriGiaTCK.Value = triGiaTCK;

            return kn.ExcuteNonQueryWithParams(query,
                pNgayCapNhat, pThangTK, pNamTK, pTonDauKy, pTriGiaTDK, pNhapTK,
                pTriGiaNTK, pXuatTK, pTriGiaXTK, pTonCK, pTriGiaTCK, pMaSP, pMaKho);
        }



        public int XoaTonKho(string maSP, string maKho)
        {

            string query = "DELETE FROM TONKHO WHERE MaSP = @MaSP AND MaKho = @MaKho";

            SqlParameter pMaSP = new SqlParameter("@MaSP", maSP);
            SqlParameter pMaKho = new SqlParameter("@MaKho", maKho);

            return kn.ExcuteNonQueryWithParams(query, pMaSP, pMaKho);
        }
    }
}
