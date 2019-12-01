using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft.Reporting.WinForms;
namespace QuanLyBanHang
{
    public partial class frmPrintCurrentdates : Form
    {
        public frmPrintCurrentdates()
        {
            InitializeComponent();
        }
        private string conStr = "Initial Catalog = QLBH; Persist Security Info=True;User ID = sa; Password=123456";
        private SqlConnection mySqlConnection;
        private void frmPrintCurrentdates_Load(object sender, EventArgs e)
        {
            mySqlConnection = new SqlConnection(conStr);
            mySqlConnection.Open();
            string sSql = " Select *from HoaDon Where OrderDate = '14/12/2018';";
            SqlDataAdapter mySqlDataAdapter = new SqlDataAdapter(sSql, mySqlConnection);
            DataSet ds = new DataSet();
            mySqlDataAdapter.Fill(ds, "HoaDon");
            ReportDataSource rds = new ReportDataSource();
            rds.Name = "DataSet2";
            rds.Value = ds.Tables[0];
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "QuanLyBanHang.ReportHoaDonTheoNgay.rdlc";
            this.reportViewer1.LocalReport.DataSources.Add(rds);
            this.reportViewer1.RefreshReport();
            this.reportViewer1.RefreshReport();
            this.reportViewer1.RefreshReport();
        }
    }
}
