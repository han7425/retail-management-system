using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBH_BHX.DAL
{
    internal class LoaiSPDAL
    {
        Ketnoi kn = new Ketnoi();

        public int ThemLoaiSanPham(string maLoaiSP, string tenLoaiSP)
        {
            string query = "INSERT INTO LOAISANPHAM (MaLoaiSP, TenLoaiSP) VALUES (@MaLoaiSP, @TenLoaiSP)";

            SqlParameter pMaLoaiSP = new SqlParameter("@MaLoaiSP", maLoaiSP);
            SqlParameter pTenLoaiSP = new SqlParameter("@TenLoaiSP", tenLoaiSP);

            return kn.ExcuteNonQueryWithParams(query, pMaLoaiSP, pTenLoaiSP);
        }

        public bool KiemTraTonTai(string maLoaiSP)
        {
            string query = "SELECT COUNT(*) FROM LOAISANPHAM WHERE MaLoaiSP = @MaLoaiSP";

            Ketnoi kn = new Ketnoi();
            try
            {
                using (SqlConnection conn = kn.GetConnect())
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaLoaiSP", maLoaiSP);

                        object result = cmd.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            // Trả về true nếu COUNT > 0
                            return Convert.ToInt32(result) > 0;
                        }
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                return false; 
            }
        }
    }
}
