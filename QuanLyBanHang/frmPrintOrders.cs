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
    public partial class frmPrintOrders : Form
    {
        public frmPrintOrders()
        {
            InitializeComponent();
        }
        private string conStr = "Initial Catalog = QLBH; Persist Security Info=True;User ID = sa; Password=123456";
        private SqlConnection mySqlConnection;
       
        private void frmPrintOrders_Load(object sender, EventArgs e)
        {
            mySqlConnection = new SqlConnection(conStr);
            mySqlConnection.Open();
            string sSql = "SELECT * FROM Orders";
            SqlDataAdapter mySqlDataAdapter = new SqlDataAdapter(sSql, mySqlConnection);
            DataSet ds = new DataSet();
            mySqlDataAdapter.Fill(ds, "Orders");
            ReportDataSource rds = new ReportDataSource();
            rds.Name = "DataSet1";
            rds.Value = ds.Tables[0];
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "QuanLyBanHang.ReportHoaDon.rdlc";
            this.reportViewer1.LocalReport.DataSources.Add(rds);
            this.reportViewer1.RefreshReport();
            this.reportViewer1.RefreshReport();
        }
    }
}
