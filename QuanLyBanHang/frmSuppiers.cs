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
using System.Text.RegularExpressions;
namespace QuanLyBanHang
{
    public partial class frmSuppiers : Form
    {
        public frmSuppiers()
        {
            InitializeComponent();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
        private string conStr = "Initial Catalog = QLBH; Persist Security Info=True;User ID = sa; Password=123456";
        private SqlConnection mySqlConnection;
        private SqlCommand mySqlCommand;
        private SqlCommandBuilder mySqlCommandBuilder;
        private SqlDataAdapter mySqlDataAdapter;
        public DataTable Suppliers;
        private bool modeNew;
        private void setControls(bool edit)
        {
            txtSupplierID.Enabled = edit;
            txtCompanyName.Enabled = edit;
            txtContactName.Enabled = edit;
            txtPhone.Enabled = edit;
            txtEmail.Enabled = edit;
            txtAddress.Enabled = edit;
            btnNew.Enabled = !edit;
            btnEdit.Enabled = !edit;
            btnDelete.Enabled = !edit;
            btnSave.Enabled = edit;
            btnCancel.Enabled = edit;
        }
        private void frmSuppiers_Load(object sender, EventArgs e)
        {
            mySqlConnection = new SqlConnection(conStr);
            mySqlConnection.Open();
            btnSearch.PerformClick();
            Dislay();
            setControls(false);
        }
        private void Dislay()
        {
            string sSql = " SELECT * FROM Suppliers Order by SupplierID";
            mySqlDataAdapter = new SqlDataAdapter(sSql, mySqlConnection);
            mySqlCommandBuilder = new SqlCommandBuilder();
            Suppliers = new DataTable();
            mySqlDataAdapter.Fill(Suppliers);
            dataGridView1.DataSource = Suppliers;
            dataGridView1.AutoGenerateColumns = false;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string sSql = "SELECT * FROM Suppliers WHERE CompanyName + CompanyName Like N'%" + txtSearch.Text + "%'";
            mySqlCommand = new SqlCommand(sSql, mySqlConnection);
            SqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
            Suppliers = new DataTable();
            Suppliers.Load(mySqlDataReader);
            dataGridView1.DataSource = Suppliers;
        }

        

        private void btnNew_Click(object sender, EventArgs e)
        {
            modeNew = true;
            setControls(true);
            txtSupplierID.Clear();
            txtCompanyName.Clear();
            txtContactName.Clear();
            txtPhone.Clear();
            txtEmail.Clear();
            txtAddress.Clear();
            txtSupplierID.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            modeNew = false;
            setControls(true);
            txtSupplierID.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult dr;
            dr = MessageBox.Show("Chắc chắn xóa dòng đã chọn không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == System.Windows.Forms.DialogResult.No) return;
            int row = dataGridView1.CurrentRow.Index;
            string m_SupplierID = dataGridView1.Rows[row].Cells[0].Value.ToString();
            string sSql = "DELETE FROM Suppliers WHERE SupplierID = " + m_SupplierID;
            SqlCommand mySqlCommand = new SqlCommand(sSql, mySqlConnection);
            mySqlCommand.ExecuteNonQuery();
            Dislay();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Bạn chắc chắn muốn thoát không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No) return;
            else this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            setControls(false);
        }

        private bool myConvert(TextBox myTextBox, string error, out int num)
        {
            bool ktra = int.TryParse(myTextBox.Text, out num);
            if (ktra == false)
            {
                MessageBox.Show(error, "thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                myTextBox.Focus();
                myTextBox.Clear();
                return false;
            }
            return true;
        }
        //isvalidEmail
        private bool checkFormatEmail(TextBox myTextBox, string error)
        {
            string Email = txtEmail.Text;
            Regex r = new Regex("^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$");

            if (!r.IsMatch(Email))
            {
                MessageBox.Show(error, "thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                myTextBox.Focus();
                myTextBox.Clear();
                return false;
            }
            return true;
        }

        //isvalidPhone
        private bool checkFormatPhone(TextBox myTextBox, string error)
        {
            string Phone = txtPhone.Text;
            Regex r = new Regex("^\\(?(\\d{3})\\)?[- ]?(\\d{3})[- ]?(\\d{4})$");

            if (!r.IsMatch(Phone))
            {
                MessageBox.Show(error, "thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                myTextBox.Focus();
                myTextBox.Clear();
                return false;
            }
            return true;
        }
        //isUnique key
        private bool IsDublicateID(string m_SupplierID)
        {
            //Kiem tra ma trung
            string SSql = "SELECT SupplierID FROM Suppliers WHERE SupplierID = " + m_SupplierID;
            mySqlCommand = new SqlCommand(SSql, mySqlConnection);
            SqlDataReader drSuppliers = mySqlCommand.ExecuteReader();
            if (drSuppliers.HasRows == true)
            {
                MessageBox.Show("Đã nhập trùng mã khách hàng", "Thông báo");
                txtSupplierID.Clear();
                txtSupplierID.Focus();
                drSuppliers.Close();
                return true;
            }
            drSuppliers.Close();
            return false;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            int SupplierID;
            if (!myConvert(txtSupplierID, "Dữ liệu nhập mã nhà cung cấp phải là số!", out SupplierID)) return;
            if (!checkFormatEmail(txtEmail, "Email sai định dạng. Xin mời nhập lại!")) return;
            if (!checkFormatPhone(txtPhone, "Điện thoại sai định dạng. Xin mời nhập lại!")) return;
            if (txtCompanyName.Text.Trim() == "")
            {
                MessageBox.Show("Đề nghị nhập tên công ty!", "Thông báo");
                return;
            }
            if (txtContactName.Text.Trim() == "")
            {
                MessageBox.Show("Đề nghị nhập họ tên người liên lạc!", "Thông báo");
                return;
            }
            if (txtAddress.Text.Trim() == "")
            {
                MessageBox.Show("Đề nghị nhập địa chỉ công ty!", "Thông báo");
                return;
            }
            if (modeNew)
            {
                string sSQL = "INSERT INTO Suppliers(SupplierID,CompanyName,ContactName,Address,Phone,Email) VALUES('" + txtSupplierID.Text + "',N'" + txtCompanyName.Text+"',N'"+txtContactName.Text+"',N'"+txtAddress.Text+"','"+txtPhone.Text+"',N'"+txtEmail.Text+"')";
                SqlCommand command = new SqlCommand(sSQL);
                command.Connection = mySqlConnection;
                command.ExecuteNonQuery();
            }
            else
            {
                //Lay dong hien thoi
                int curRow = dataGridView1.CurrentRow.Index;
                //Lay ma truoc luc sua
                string m_SupplierID = dataGridView1.Rows[curRow].Cells[0].Value.ToString();
                if (txtSupplierID.Text.Trim() != m_SupplierID.Trim())
                {
                    if (IsDublicateID(txtSupplierID.Text)) return;
                }
                string sSql = "UPDATE Suppliers SET CompanyName = N'" + txtCompanyName.Text + "', ContactName = N'" + txtContactName.Text + "', Address = N'" + txtAddress.Text + "', Phone = N'" + txtPhone.Text + "', Email = N'" + txtEmail.Text + "' WHERE SupplierID = " + txtSupplierID.Text;
                SqlCommand mySqlCommand = new SqlCommand(sSql, mySqlConnection);
                mySqlCommand.ExecuteNonQuery();

            }
            setControls(false);
            Dislay();
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {

            txtSupplierID.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtCompanyName.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtContactName.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtAddress.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            txtPhone.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            txtEmail.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();

        }

        private void btnSearch_TextChanged(object sender, EventArgs e)
        {
            btnSearch.PerformClick();
        }

      
    }
}
