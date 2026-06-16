using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLBH_BHX
{
    public partial class Login : Form
    {
        Ketnoi kn = new Ketnoi(); // Khởi tạo lớp kết nối
        public Login()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = DateTime.Now.ToString("dddd, dd/MM/yyyy - HH:mm:ss",
                    new CultureInfo("vi-VN"));

        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            string username = txt_username.Text.Trim();
            string password = txt_password.Text.Trim();

            // Kiểm tra nhập thiếu
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu!");
                return;
            }

            string query = "SELECT MaTK, MaNV FROM TaiKhoan WHERE TenDN=@TenDN AND MatKhau=@MatKhau";

            DataTable dt = new DataTable();

            using (SqlConnection conn = kn.GetConnect())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TenDN", username);
                    cmd.Parameters.AddWithValue("@MatKhau", password);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
                conn.Close();
            }

            if (dt.Rows.Count > 0)
            {
                string maTK = dt.Rows[0]["MaTK"].ToString();

                Main main = new Main(username, password);
                main.Logout += Main_Logout;
                main.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!");
            }
        }
            private void Main_Logout(object sender, EventArgs e)
            {
            (sender as Main).isThoat = false;
            (sender as Main).Close();
            this.Show();
            }

        private void ck_display_CheckedChanged(object sender, EventArgs e)
        {
            txt_password.UseSystemPasswordChar = !ck_display.Checked;
        }

        private void txt_username_TextChanged(object sender, EventArgs e)
        {

        }
    }    
}
