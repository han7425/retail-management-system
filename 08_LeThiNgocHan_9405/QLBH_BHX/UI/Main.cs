using Microsoft.Reporting.WinForms;
using QLBH_BHX.MainMenu.Products_Management;
using QLBH_BHX.MainMenu.Profile;
using QLBH_BHX.MainMenu.Sales_Management;
using QLBH_BHX.UI.MainMenu.CTKM;
using QLBH_BHX.UI.MainMenu.Employee;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLBH_BHX
{
    public partial class Main : Form
    {

          
        public Main(string username, string password)
        {
            InitializeComponent();  
            this.username = username;
            this.password = password;
            PhanQuyenForm(username, password);
        }
        public void PhanQuyenForm(string username, string password)
        {
            if (username == "Admin")
            {
                foreach (ToolStripItem item in menuStrip1.Items)
                {
                    item.Visible = true;
                }
            }

            else if (username.StartsWith("NV"))
            {
                tls_CTKM.Visible = false;
                tls_QLNV.Visible = false;

            }

        }

        public Main()
        {
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
        public event EventHandler Logout;
        private void DangXuat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn thoát?",    
                "Thông báo",                       
                MessageBoxButtons.YesNoCancel,      
                MessageBoxIcon.Information         
    );

            if (result == DialogResult.Yes)
            {
                isThoat = false;               // báo là logout, không exit app
                Logout?.Invoke(this, EventArgs.Empty);  // phát event cho Login biết
                this.Close();                  // đóng Main
            }
        }
        public bool isThoat = true;
        private string username;
        private string password;

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isThoat)
                Application.Exit();
        }
        private void panel25_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Main_Load(object sender, EventArgs e)
        {

        }
        private void đổiMậtKhẩuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
           if (Application.OpenForms.OfType<ChangePassword>().Count() > 0)
            {
                Application.OpenForms.OfType<ChangePassword>().First().BringToFront();
                return;
            }
            ChangePassword dmk = new ChangePassword(username);
            dmk.Show();
        }
    
        private void tls_SP_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Products>().Count() > 0)
            {
                Application.OpenForms.OfType<Products>().First().BringToFront();
                return;
            }
            Products prd = new Products();
            prd.Show();
        }

        private void tls_KH_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Customers>().Count() > 0)
            {
                Application.OpenForms.OfType<Customers>().First().BringToFront();
                return;
            }
            Customers ctm = new Customers();            
            ctm.Show();
        }

        private void tls_HD_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Invoice>().Count() > 0)
            {
                Application.OpenForms.OfType<Invoice>().First().BringToFront();
                return;
            }
            Invoice invoice = new Invoice();
            invoice.Show();
        }

        private void tls_Kho_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<WareHouse>().Count() > 0)
            {
                Application.OpenForms.OfType<WareHouse>().First().BringToFront();
                return;
            }
            WareHouse wrh = new WareHouse();
            wrh.Show();
        }

        private void tls_BCTK_Click(object sender, EventArgs e)
        {
            QLBH_BHX.UI.MainMenu.Reports.Report rpt = new QLBH_BHX.UI.MainMenu.Reports.Report("tls_BCTK");
            rpt.ShowDialog();
        }

        private void tls_CTKM_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<CTKM>().Count() > 0)
            {
                Application.OpenForms.OfType<CTKM>().First().BringToFront();
                return;
            }
            CTKM ctkm = new CTKM();
            ctkm.Show();
        }

        private void tls_QLNV_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Employee>().Count() > 0)
            {
                Application.OpenForms.OfType<Employee>().First().BringToFront();
                return;
            }
            Employee nv = new Employee();
            nv.Show();
        }
        private void dgvList_LastOrder_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void báoCáoThốngKêToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
