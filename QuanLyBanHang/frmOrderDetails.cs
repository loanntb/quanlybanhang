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
    public partial class frmOrderDetails : Form
    {
        public frmOrderDetails()
        {
            InitializeComponent();
        }
        private string conStr = "Initial Catalog = QLBH; Persist Security Info=True;User ID = sa; Password=123456";
        private SqlConnection mySqlConnection;
        private SqlDataAdapter mySqlDataAdapter;
        private SqlCommandBuilder mySqlCommandBuilder;
        private DataTable dtOrderDetails, dtProducts, dtCustomers;
        private bool modeNew;
        private void SetControls(bool edit)
        {
            cboOrderID.Enabled = edit;
            txtDetailID.Enabled = edit;
            txtNote.Enabled = edit;
            cboProductName.Enabled = edit;
            txtQuantity.Enabled = edit;
            txtNote.Enabled = edit;
            btnDelete.Enabled = !edit;
            btnNew.Enabled = !edit;
            btnEdit.Enabled = !edit;
            btnSave.Enabled = edit;
            btnCancel.Enabled = edit;
        }
        private void Display()
        {
            string sSql = "SELECT * FROM OrderDetails  ORDER BY DetailID";
            mySqlDataAdapter = new SqlDataAdapter(sSql, mySqlConnection);
            mySqlCommandBuilder = new SqlCommandBuilder(mySqlDataAdapter);
            dtOrderDetails = new DataTable();
            mySqlDataAdapter.Fill(dtOrderDetails);
            //Hien thi len luoi
            dataGridView1.DataSource = dtOrderDetails;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            modeNew = true;
            SetControls(true);
            txtDetailID.Clear();
            txtQuantity.Clear();
            txtNote.Clear();
            txtDetailID.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            SetControls(false);
        }
        private bool IsDupplicate(string m_DetailID)
        {
            string sSql = "SELECT * FROM OrderDetails WHERE DetailID = " + m_DetailID;
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
            dtOrderDetails.Rows[pos].Delete();
            mySqlDataAdapter.Update(dtOrderDetails);
            Display();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (txtDetailID.Text.Trim() == "")
            {
                MessageBox.Show("Mã sản phẩm không được nhập rỗng");
                txtDetailID.Focus();
                return;
            }
            if (txtQuantity.Text.Trim() == "")
            {
                MessageBox.Show("Số lượng sản phẩm không được nhập rỗng");
                txtQuantity.Focus();
                return;
            }
            
            if (modeNew == true)
            {
                //Kiem tra trung
                if (IsDupplicate(txtDetailID.Text))
                {
                    MessageBox.Show("Đã trùng mã đơn hàng");
                    txtDetailID.Clear();
                    txtDetailID.Focus();
                    return;
                }
                //Them moi
                DataRow newRow = dtOrderDetails.NewRow();
                newRow["OrderID"] = cboOrderID.SelectedValue.ToString();
                newRow["DetailID"] = txtDetailID.Text;
                newRow["ProductID"] = cboOrderID.SelectedValue.ToString();
                newRow["Quantity"] = txtQuantity.Text;
                newRow["Note"] = txtNote.Text;
                dtOrderDetails.Rows.Add(newRow);
                mySqlDataAdapter.Update(dtOrderDetails);
            }
            else
            {
                //Kiem tra trung                                
                int pos = dataGridView1.CurrentRow.Index;
                //Lay ma truoc khi sua
                string m_DetailID = dataGridView1.Rows[pos].Cells[1].Value.ToString();
                if (txtDetailID.Text.Trim() != m_DetailID.Trim())
                {
                    //Kiem tra trung
                    if (IsDupplicate(txtDetailID.Text))
                    {
                        MessageBox.Show("Đã trùng mã sản phẩm");
                        txtDetailID.Clear();
                        txtDetailID.Focus();
                        return;
                    }
                }
                DataRow editRow = dtOrderDetails.Rows[pos];
                editRow["OrderID"] = cboOrderID.SelectedValue.ToString();
                editRow["DetailID"] = txtDetailID.Text;
                editRow["ProductName"] = cboProductName.SelectedValue.ToString();
                editRow["Quantity"] = txtQuantity.Text;
                editRow["Note"] = txtNote.Text;
                mySqlDataAdapter.Update(dtOrderDetails);
            }
            Display();
            SetControls(false);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Bạn chắc chắn muốn thoát không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No) return;
            else this.Close();
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            cboOrderID.SelectedValue = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtDetailID.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            cboProductName.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtQuantity.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            txtNote.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
        }

        private void frmOrderDetails_Load(object sender, EventArgs e)
        {
            mySqlConnection = new SqlConnection(conStr);
            mySqlConnection.Open();

            //Dưa du lieu vao COmboBox
            string sSql = "SELECT * FROM Products ";
            mySqlDataAdapter = new SqlDataAdapter(sSql, mySqlConnection);
            dtProducts = new DataTable();
            mySqlDataAdapter.Fill(dtProducts);
            //Hien thi du lieu ComboBox
            cboProductName.DataSource = dtProducts;
            cboProductName.DisplayMember = "ProductName";

            cboProductName.ValueMember = "ProductID";

            //----------
            sSql = "SELECT *FROM Customer ";
            mySqlDataAdapter = new SqlDataAdapter(sSql, mySqlConnection);
            dtCustomers = new DataTable();
            mySqlDataAdapter.Fill(dtCustomers);
            //Hien thi du lieu ComboBox
            cboOrderID.DataSource = dtCustomers;
            cboOrderID.DisplayMember = "CustomerID";
            cboOrderID.ValueMember = "CustomerID";
            //------------------
            Display();
            SetControls(false);
        }
    }
}
