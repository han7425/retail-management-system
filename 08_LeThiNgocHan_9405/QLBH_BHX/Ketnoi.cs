using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBH_BHX
{
    internal class Ketnoi
    {
        //Khai báo chuỗi kết nối CSDL
        public string connectString = "Data Source=LAPTOP-L3RFR7M6\\SQLEXPRESS; Initial Catalog=QLBH; Integrated Security=True";


        //Tạo và mở kết nối CSDL
        public SqlConnection GetConnect()
        {
            SqlConnection conn = new SqlConnection(connectString);
            conn.Open();
            return conn;
        }


        //Thực hiện câu lệnh SQL không trả về dữ liệu (INSERT, UPDATE, DELETE)
        public int ExcuteNonQuery(string query)
        {
            int affectRow = 0;
            using (SqlConnection ketnoi = new SqlConnection(connectString)) //sử dụng kết nối
            {
                ketnoi.Open();
                SqlCommand excute = new SqlCommand(query, ketnoi); //tạo đối tượng để thực thi câu lệnh
                affectRow = excute.ExecuteNonQuery(); //thực thi câu lệnh và trả về số dòng bị ảnh hưởng
                ketnoi.Close();
            }
            return affectRow; //trả về số dòng bị ảnh hưởng
        }


        //Thực hiện câu lệnh SQL trả về một bảng dữ liệu (SELECT)
        public DataTable ExcuteQuery(string query)
        {
            DataTable dt = new DataTable(); //tạo đối tượng để chứa dữ liệu kết quả trả về
            using (SqlConnection ketnoi = new SqlConnection(connectString)) //sử dụng kết nối
            {
                ketnoi.Open();
                SqlCommand excute = new SqlCommand(query, ketnoi); //tạo đối tượng thực thi câu lệnh
                SqlDataAdapter getdata = new SqlDataAdapter(excute); //sử dụng SqlDataAdapter để lấy dữ liệu từ cơ sở dữ liệu và điền vào DataTable
                getdata.Fill(dt); //điền dữ liệu vào DataTable
                ketnoi.Close();
            }
            return dt; //trả về datatable chứa dữ liệu kết quả 
        }

        public int ExcuteNonQueryWithParams(string query, params SqlParameter[] parameters)
        {
            int affectedRows = 0;

            // Sử dụng khối 'using' để đảm bảo kết nối được đóng và giải phóng
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                // Sử dụng khối 'using' để đảm bảo đối tượng Command được giải phóng
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Thêm các tham số vào command
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    try
                    {
                        connection.Open();
                        // Thực thi câu lệnh và nhận số dòng bị ảnh hưởng
                        affectedRows = command.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        // Ghi log lỗi SQL chi tiết
                        Console.WriteLine("Lỗi SQL khi thực thi NonQuery: " + ex.Message);
                        // Có thể throw lại ngoại lệ để lớp gọi biết rằng thao tác thất bại
                        // throw; 
                    }
                    // Khối 'finally' không cần thiết vì 'using' tự động đóng connection và command
                }
            }
            return affectedRows;
        }
        public DataTable ExcuteQueryWithParams(string query, params SqlParameter[] parameters)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connectString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Thêm các tham số vào command
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }

                        // Dùng Adapter để đổ dữ liệu
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    // Xử lý lỗi nếu cần (hoặc throw)
                    // Console.WriteLine(ex.Message);
                }
            }
            return dt;
        }
    }
}
