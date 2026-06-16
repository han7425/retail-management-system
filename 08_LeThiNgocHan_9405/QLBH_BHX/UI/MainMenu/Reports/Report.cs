using CrystalDecisions.CrystalReports.Engine;
using Microsoft.Reporting.WinForms;
using QLBH_BHX.DAL;
using QLBH_BHX.QLBHDataSetTableAdapters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace QLBH_BHX.UI.MainMenu.Reports
{
    public partial class Report : Form
    {
        Ketnoi kn = new Ketnoi();
        private string _option;
        private string _paramID;
        public Report(string option, string id = "")
        {
            InitializeComponent();
            this.reportViewer1.RefreshReport();
            _option = option;
            _paramID = id;
        }

        private void Report_Load(object sender, EventArgs e)
        { 
            if (_option == "tls_BCTK")
            {
                try
                {
                    reportViewer1.Visible = true;
                    reportViewer1.Reset();
                    string query = @"SELECT * FROM TONKHO";
                    DataTable dt = kn.ExcuteQuery(query); // dt chứa tất cả sản phẩm                  
                    reportViewer1.LocalReport.ReportPath = @"D:\HÂN\QLBH_BHX (C Sharp)\QLBH_BHX\UI\MainMenu\Reports\ReportTonKho.rdlc";
                    reportViewer1.LocalReport.DataSources.Clear();
                    reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSetTonKho", dt));
                    reportViewer1.RefreshReport();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
            else if (_option == "tls_BCDT")
            { 
                try
                {
                    reportViewer1.Visible = true;
                    reportViewer1.LocalReport.ReportEmbeddedResource = "QLBH_BHX.ReportDoanhThu.rdlc";
                    ReportDataSource rds = new ReportDataSource();
                    reportViewer1.RefreshReport();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải báo cáo: " + ex.Message, "Lỗi Báo Cáo");
                }
            }
  
        }
        

        private void reportViewer1_Load(object sender, EventArgs e)
        {

        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}
