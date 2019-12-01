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
    public partial class frmOrders : Form
    {
        public frmOrders()
        {
            InitializeComponent();
        }
        private string conStr = "Initial Catalog = QLBH; Persist Security Info=True;User ID = sa; Password=123456";
        private SqlConnection mySqlConnection;
        private SqlDataAdapter mySqlDataAdapter;
        private SqlCommandBuilder mySqlCommandBuilder;
        private DataTable dtOrders, dtCustomers;
        private bool modeNew;
        private void SetControls(bool edit)
        {
            cboCName.Enabled = edit;
            txtOrderID.Enabled = edit;
            txtOrderDate.Enabled = edit;
            txtTotalAmount.Enabled = edit;
            btnDelete.Enabled = !edit;
            btnNew.Enabled = !edit;
            btnEdit.Enabled = !edit;
            btnSave.Enabled = edit;
            btnCancel.Enabled = edit;
        }
        private void Display()
        {
            string sSql = "SELECT * FROM Orders   ORDER BY OrderID";
            mySqlDataAdapter = new SqlDataAdapter(sSql, mySqlConnection);
            mySqlCommandBuilder = new SqlCommandBuilder(mySqlDataAdapter);
            dtOrders = new DataTable();
            mySqlDataAdapter.Fill(dtOrders);
            //Hien thi len luoi
            dataGridView1.DataSource = dtOrders;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            modeNew = true;
            SetControls(true);
            txtOrderID.Clear();
            txtTotalAmount.Clear();
            txtOrderDate.Clear();
            txtTotalAmount.Clear();
            txtOrderDate.Clear();
            txtOrderID.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            modeNew = false;
            SetControls(true);
            txtOrderID.Focus();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetControls(false);
        }

        private bool IsDupplicate(string m_OrderID)
        {
            string sSql = "SELECT * FROM Orders WHERE OrderID = " + m_OrderID;
            SqlDataAdapter mySqlDataAdapter1 = new SqlDataAdapter(sSql, mySqlConnection);
            DataTable myDatatable = new DataTable();
            mySqlDataAdapter1.Fill(myDatatable);
            if (myDatatable.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult dr;
            dr = MessageBox.Show("Chắc chắn xóa dòng dữ liệu đã chọn không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == System.Windows.Forms.DialogResult.No) return;
            int pos = dataGridView1.CurrentRow.Index;
            dtOrders.Rows[pos].Delete();
            mySqlDataAdapter.Update(dtOrders);
            Display();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtOrderID.Text.Trim() == "")
            {
                MessageBox.Show("Mã sản phẩm không được nhập rỗng");
                txtOrderID.Focus();
                return;
            }
            if (txtOrderID.Text.Trim() == "")
            {
                MessageBox.Show("Tên sản phẩm không được nhập rỗng");
                txtOrderID.Focus();
                return;
            }
            if (modeNew == true)
            {
                //Kiem tra trung
                if (IsDupplicate(txtOrderID.Text))
                {
                    MessageBox.Show("Đã trùng mã đơn hàng");
                    txtOrderID.Clear();
                    txtOrderID.Focus();
                    return;
                }
                //Them moi
                DataRow newRow = dtOrders.NewRow();
                newRow["CustemerID"] = cboCName.SelectedValue.ToString();
                newRow["OrderID"] = txtOrderID.Text;
                newRow["OrderDate"] = txtOrderDate.Text;
                newRow["TotalAmount"] = txtTotalAmount.Text;
                dtOrders.Rows.Add(newRow);
                mySqlDataAdapter.Update(dtOrders);
            }
            else
            {
                //Kiem tra trung                                
                int pos = dataGridView1.CurrentRow.Index;
                //Lay ma truoc khi sua
                string m_OrderID = dataGridView1.Rows[pos].Cells[2].Value.ToString();
                if (txtOrderID.Text.Trim() != m_OrderID.Trim())
                {
                    //Kiem tra trung
                    if (IsDupplicate(txtOrderID.Text))
                    {
                        MessageBox.Show("Đã trùng mã sản phẩm");
                        txtOrderID.Clear();
                        txtOrderID.Focus();
                        return;
                    }
                }
                DataRow editRow = dtOrders.Rows[pos];
                editRow["CustemerID"] = cboCName.SelectedValue.ToString();
                editRow["OrderID"] = txtOrderID.Text;
                editRow["OrderDate"] = txtOrderID.Text;
                editRow["TotalAmount"] = txtTotalAmount.Text;
                mySqlDataAdapter.Update(dtOrders);
            }
            Display();
            SetControls(false);
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            cboCName.SelectedValue = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtOrderID.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtOrderDate.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtTotalAmount.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Bạn chắc chắn muốn thoát không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No) return;
            else this.Close();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void txtTotalAmount_TextChanged(object sender, EventArgs e)
        {

        }

        private void cboCName_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtOrderDate_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtOrderID_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void frmOrders_Load(object sender, EventArgs e)
        {
            mySqlConnection = new SqlConnection(conStr);
            mySqlConnection.Open();

            //Dưa du lieu vao COmboBox
            string sSql = "SELECT FirstName, LastName, CustomerID FROM Customer ";
            mySqlDataAdapter = new SqlDataAdapter(sSql, mySqlConnection);
            dtCustomers = new DataTable();
            mySqlDataAdapter.Fill(dtCustomers);
            //Hien thi du lieu ComboBox
            cboCName.DataSource = dtCustomers;
            //cboCName.DisplayMember = "FirstName";
            cboCName.DisplayMember = "LastName";
            
            cboCName.ValueMember = "CustomerID";
            //------------------
            Display();
            SetControls(false);
        }
    }
}
