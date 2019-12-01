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
    public partial class frmUsers : Form
    {
        private string conStr = "Initial Catalog = QLBH; Persist Security Info=True;User ID = sa; Password=123456";
        private SqlConnection mySqlConnection;
        private SqlCommand mySqlCommand;
        public DataTable dtUsers;
        private bool modeNew;
        public frmUsers()
        {
            InitializeComponent();
        }

        private void frmUsers_Load(object sender, EventArgs e)
        {
            mySqlConnection = new SqlConnection(conStr);
            mySqlConnection.Open();
            Dislay();
            setControls(false);
        }
        private void Dislay()
        {
            string sSql = " SELECT * FROM [Users] Order by UserID";
            mySqlCommand = new SqlCommand(sSql, mySqlConnection);
            SqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
            dtUsers = new DataTable();
            dtUsers.Load(mySqlDataReader);
            dataGridView1.DataSource = dtUsers;
        }
        private void setControls(bool edit)
        {
            txtFullName.Enabled = edit;
            txtUserName.Enabled = edit;
            txtPassword.Enabled = edit;
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
            txtFullName.Clear();
            txtUserName.Clear();
            txtPassword.Clear();
            txtFullName.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            modeNew = false;
            setControls(true);
            txtFullName.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult dr;
            dr = MessageBox.Show("Chắc chắn xóa dòng đã chọn không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == System.Windows.Forms.DialogResult.No) return;

            //Lay du lieu tren luoi
            int row = dataGridView1.CurrentRow.Index;
            string m_UserID = dataGridView1.Rows[row].Cells[0].Value.ToString();

            //Xoa
            string sSql = "DELETE FROM [Users] WHERE UserID = " + m_UserID;
            mySqlCommand = new SqlCommand(sSql, mySqlConnection);
            mySqlCommand.ExecuteNonQuery();
            Dislay();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtFullName.Text.Trim() == "")
            {
                MessageBox.Show("Đề nghị nhập họ và tên của người dùng!", "Thông báo");
                return;
            }
            if (txtUserName.Text.Trim() == "")
            {
                MessageBox.Show("Đề nghị nhập tên của của người dùng", "Thông báo");
                return;
            }
            if (modeNew)
            {
                string sSQL = "Insert into [Users](FullName, UserName, Password) values " + "(N'" + txtFullName.Text + "', N'" + txtUserName.Text + "',  '" + txtPassword.Text + "')";
                mySqlCommand = mySqlConnection.CreateCommand();
                mySqlCommand.CommandText = sSQL;
                mySqlCommand.ExecuteNonQuery();
            }
            else
            {
                //lay dong hien tai
                int row = dataGridView1.CurrentRow.Index;
                string m_UserID = dataGridView1.Rows[row].Cells[0].Value.ToString();
                string sSql = " UPDATE [dbo].[Users] SET [FullName] = N'" + txtFullName.Text + "',[UserName] = N'" + txtUserName.Text + "' ,[Password] = N'" + txtPassword.Text + "'  WHERE UserID = " + m_UserID;
                mySqlCommand = new SqlCommand(sSql, mySqlConnection);
                mySqlCommand.ExecuteNonQuery();

            }
            setControls(false);
            Dislay();
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtFullName.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtUserName.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtPassword.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Bạn chắc chắn thoát không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No) return;
            else this.Close();
        }
    }
}
