using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBH_BHX.DAL
{
    internal class SanPhamDAL
    {
        Ketnoi kn = new Ketnoi();

        public int CapNhatSanPham(
            string maSP, string maKho, string maLoaiSP, string maDVT, string maCTKM,
            decimal giaBan, string barcode, string tenSP, DateTime nsx, DateTime hsd,
            string xuatXu, string ncc, decimal vat, decimal trongLuong, int slToiThieu,
            string moTaSP)
        {
            string query = "UPDATE SANPHAM SET " +
                           "MaKho = @MaKho, MaLoaiSP = @MaLoaiSP, MaDVT = @MaDVT, MaCTKM = @MaCTKM, " +
                           "GiaBan = @GiaBan, Barcode = @Barcode, TenSP = @TenSP, NSXSP = @NSXSP, " +
                           "HSDSP = @HSDSP, XuatXu = @XuatXu, NhaCungCap = @NCC, VAT = @VAT, " +
                           "TrongLuong = @TrongLuong, SoLuongToiThieu = @SLToiThieu, MoTaSP = @MoTaSP " +
                           "WHERE MaSP = @MaSP";

            SqlParameter pMaKho = new SqlParameter("@MaKho", maKho);
            SqlParameter pMaLoaiSP = new SqlParameter("@MaLoaiSP", maLoaiSP);
            SqlParameter pMaDVT = new SqlParameter("@MaDVT", maDVT);
            SqlParameter pMaCTKM = new SqlParameter("@MaCTKM", maCTKM);
            SqlParameter pGiaBan = new SqlParameter("@GiaBan", SqlDbType.Decimal);
            pGiaBan.Value = giaBan;
            SqlParameter pBarcode = new SqlParameter("@Barcode", barcode);
            SqlParameter pTenSP = new SqlParameter("@TenSP", tenSP);
            SqlParameter pNSXSP = new SqlParameter("@NSXSP", SqlDbType.Date);
            pNSXSP.Value = nsx;
            SqlParameter pHSDSP = new SqlParameter("@HSDSP", SqlDbType.Date);
            pHSDSP.Value = hsd;
            SqlParameter pXuatXu = new SqlParameter("@XuatXu", xuatXu);
            SqlParameter pNCC = new SqlParameter("@NCC", ncc);
            SqlParameter pVAT = new SqlParameter("@VAT", SqlDbType.Decimal);
            pVAT.Value = vat;
            SqlParameter pTrongLuong = new SqlParameter("@TrongLuong", SqlDbType.Decimal);
            pTrongLuong.Value = trongLuong;
            SqlParameter pSLToiThieu = new SqlParameter("@SLToiThieu", SqlDbType.Int);
            pSLToiThieu.Value = slToiThieu;
            SqlParameter pMoTaSP = new SqlParameter("@MoTaSP", moTaSP);
            SqlParameter pMaSP = new SqlParameter("@MaSP", maSP);

            return kn.ExcuteNonQueryWithParams(query,
                pMaKho, pMaLoaiSP, pMaDVT, pMaCTKM, pGiaBan, pBarcode, pTenSP,
                pNSXSP, pHSDSP, pXuatXu, pNCC, pVAT, pTrongLuong, pSLToiThieu,
                pMoTaSP, pMaSP); // pMaSP luôn đặt cuối cùng cho WHERE
        }


        public int XoaSanPham(string maSP)
        {
            string query = "DELETE FROM SANPHAM WHERE MaSP = @MaSP";

            SqlParameter pMaSP = new SqlParameter("@MaSP", SqlDbType.Char, 10);
            pMaSP.Value = maSP;

            return kn.ExcuteNonQueryWithParams(query, pMaSP);
        }

    }
}
