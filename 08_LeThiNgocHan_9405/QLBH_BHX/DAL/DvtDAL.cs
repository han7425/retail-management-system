using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBH_BHX.DAL
{
    internal class DvtDAL
    {
        
    
        Ketnoi kn = new Ketnoi();

        public int ThemDVT(string maDVT, string tenDVT)
        {
            string query = "INSERT INTO DVT (MaDVT, TenDVT) VALUES (@MaDVT, @TenDVT)";
            SqlParameter pMaDVTP = new SqlParameter("@MaDVT", maDVT);
            SqlParameter pTenDVT = new SqlParameter("@TenDVT", tenDVT);

            return kn.ExcuteNonQueryWithParams(query, pMaDVTP, pTenDVT);
        }

        public bool KiemTraTonTai(string MaDVT)
        {
            string query = "SELECT COUNT(*) FROM DVT WHERE MaDVT = @MaDVT";
            try
            {
                using (SqlConnection conn = kn.GetConnect())
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaDVT", MaDVT);

                        object result = cmd.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
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
