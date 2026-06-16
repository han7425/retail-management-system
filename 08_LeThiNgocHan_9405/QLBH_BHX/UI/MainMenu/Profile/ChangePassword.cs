using QLBH_BHX.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace QLBH_BHX.MainMenu.Profile
{
    public partial class ChangePassword : Form
    {
        Ketnoi kn = new Ketnoi();  
        private string _username;
        public ChangePassword(string username)
        {
            InitializeComponent();
            _username = username;
        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            string currentPass = tbCurrent.Text.Trim();
            string newPass = tbNew.Text.Trim();
            string confirmPass = tbConfirm.Text.Trim();

            if (newPass != confirmPass)
            {
                MessageBox.Show("Mật khẩu xác nhận không trùng khớp!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbConfirm.Clear();
                tbConfirm.Focus();
                return;
            }

            string queryCheck = "SELECT COUNT(*) FROM TAIKHOAN WHERE TenDN = @user AND MatKhau = @pass";

            using (SqlConnection conn = kn.GetConnect())
            using (SqlCommand cmd = new SqlCommand(queryCheck, conn))
            {
                cmd.Parameters.AddWithValue("@user", _username);
                cmd.Parameters.AddWithValue("@pass", currentPass);

                //conn.Open();
                int count = (int)cmd.ExecuteScalar();

                if (count == 0)
                {
                    MessageBox.Show("Mật khẩu hiện tại không chính xác!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tbCurrent.Clear();
                    tbCurrent.Focus();
                    return;
                }
            }

            string queryUpdate = "UPDATE TAIKHOAN SET MatKhau = @newPass WHERE TenDN = @user";
            using (SqlConnection conn = kn.GetConnect())
            using (SqlCommand cmd = new SqlCommand(queryUpdate, conn))
            {
                cmd.Parameters.AddWithValue("@newPass", newPass);
                cmd.Parameters.AddWithValue("@user", _username);
                //conn.Open();
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Đổi mật khẩu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void chbShowPass_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void ck_display_CheckedChanged(object sender, EventArgs e)
        {
            tbNew.UseSystemPasswordChar = !ck_display.Checked;
            tbCurrent.UseSystemPasswordChar = !ck_display.Checked;
            tbConfirm.UseSystemPasswordChar = !ck_display.Checked;
        }
    }

}
