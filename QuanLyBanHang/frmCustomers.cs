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
namespace QuanLyBanHang
{
    public partial class frmCustomers : Form
    {
        private string conStr = "Initial Catalog = QLBH; Persist Security Info=True;User ID = sa; Password=123456";
        private SqlConnection mySqlConnection;
        private SqlCommand mySqlCommand;
        public DataTable dtCustomers;
        private bool modeNew;
        public frmCustomers()
        {
            InitializeComponent();
        }

        private void frmCustomers_Load(object sender, EventArgs e)
        {
            mySqlConnection = new SqlConnection(conStr);
            mySqlConnection.Open();
            btnSearch.PerformClick();
            Dislay();
            setControls(false);

        }
        private void Dislay()
        {
            string sSql = " SELECT * FROM Customer Order by CustomerID";
            mySqlCommand = new SqlCommand(sSql, mySqlConnection);
            SqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
            dtCustomers = new DataTable();
            dtCustomers.Load(mySqlDataReader);
            //Hien thi len luoi
            dataGridView1.DataSource = dtCustomers;
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string sSql = "SELECT * FROM Customer WHERE FirstName + LastName Like N'%" + txtSearch.Text + "%'";
            //Tao doi tuong SqlCommand
            mySqlCommand = new SqlCommand(sSql, mySqlConnection);
            //Truy van du lieu
            SqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
            //Chuyen du lieu sang DataTable
            dtCustomers = new DataTable();
            dtCustomers.Load(mySqlDataReader);
            //Hien thi len luoi
            dataGridView1.DataSource = dtCustomers;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            btnSearch.PerformClick();
        }
        private void setControls(bool edit)
        {
            txtFirstName.Enabled = edit;
            txtLastName.Enabled = edit;
            txtPhone.Enabled = edit;
            txtEmail.Enabled = edit;
            txtAddress.Enabled = edit;
            btnNew.Enabled = !edit;
            btnEdit.Enabled = !edit;
            btnDelete.Enabled = !edit;
            btnSave.Enabled = edit;
            btnCancel.Enabled = edit;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            modeNew = true;
            setControls(true);
            txtFirstName.Clear();
            txtLastName.Clear();
            txtPhone.Clear();
            txtAddress.Clear();
            txtEmail.Clear();
            txtFirstName.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            modeNew = false;
            setControls(true);
            txtFirstName.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            DialogResult dr;
            dr = MessageBox.Show("Chắc chắn xóa dòng đã chọn không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == System.Windows.Forms.DialogResult.No) return;

            //Lay du lieu tren luoi
            int row = dataGridView1.CurrentRow.Index;
            string m_CustomerID = dataGridView1.Rows[row].Cells[0].Value.ToString();

            //Xoa
            string sSql = "DELETE FROM Customer WHERE CustomerID = " + m_CustomerID;
            mySqlCommand = new SqlCommand(sSql, mySqlConnection);
            mySqlCommand.ExecuteNonQuery();
            Dislay();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtFirstName.Text.Trim() == "")
            {
                MessageBox.Show("Đề nghị nhập Họ và Tên đệm của khách hàng!", "Thông báo");
                return;
            }
            if (txtLastName.Text.Trim() == "")
            {
                MessageBox.Show("Đề nghị nhập tên của khách hàng", "Thông báo");
                return;
            }
            if (modeNew)
            {
                string sSQL = "Insert into Customer(FirstName, LastName, Address, Phone, Email) values "+"(N'"+txtFirstName.Text+"', N'"+txtLastName.Text+"',  N'"+txtAddress.Text+"', '"+txtPhone.Text+"', '"+txtEmail.Text+"')";
                mySqlCommand = mySqlConnection.CreateCommand();
                mySqlCommand.CommandText = sSQL;
                mySqlCommand.ExecuteNonQuery();
            }
            else
            {
                //lay dong hien tai
                int row = dataGridView1.CurrentRow.Index;
                string m_CustomerID = dataGridView1.Rows[row].Cells[0].Value.ToString();
                string sSql = " UPDATE [dbo].[Customer] SET [FirstName] = N'"+txtFirstName.Text+"',[LastName] = N'"+txtLastName.Text+"' ,[Address] = N'"+txtAddress.Text+"' ,[Phone] = N'"+txtPhone.Text+"' ,[Email] = N'"+txtEmail.Text+"' WHERE CustomerID = " +  m_CustomerID;
                mySqlCommand = new SqlCommand(sSql, mySqlConnection);
                mySqlCommand.ExecuteNonQuery();
                
            }
            setControls(false);
            Dislay();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            setControls(false);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Bạn chắc chắn muốn thoát không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No) return;
            else this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtFirstName.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtLastName.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtPhone.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            txtEmail.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
            txtAddress.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
        }
    }
}
