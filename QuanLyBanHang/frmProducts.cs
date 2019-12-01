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
    public partial class frmProducts : Form
    {
        public frmProducts()
        {
            InitializeComponent();
        }
        private string conStr = "Initial Catalog = QLBH; Persist Security Info=True;User ID = sa; Password=123456";
        private SqlConnection mySqlConnection;
        private SqlDataAdapter mySqlDataAdapter;
        private SqlCommandBuilder mySqlCommandBuilder;
        private DataTable tbCategories, tbSuppliers, dtProducts;
        private bool modeNew;
        private void SetControls(bool edit)
        {
            cboCategory.Enabled = edit;
            cboSupplier.Enabled = edit;
            txtProductID.Enabled = edit;
            txtProductName.Enabled = edit;
            txtUnit.Enabled = edit;
            txtPrice.Enabled = edit;
            txtDescription.Enabled = edit;

            btnDelete.Enabled = !edit;
            btnNew.Enabled = !edit;
            btnEdit.Enabled = !edit;
            btnSave.Enabled = edit;
            btnCancel.Enabled = edit;
        }
        private void Display()
        {
            string sSql = "SELECT * FROM Products  ORDER BY ProductName";
            mySqlDataAdapter = new SqlDataAdapter(sSql, mySqlConnection);
            mySqlCommandBuilder = new SqlCommandBuilder(mySqlDataAdapter);
            dtProducts = new DataTable();
            mySqlDataAdapter.Fill(dtProducts);
            //Hien thi len luoi
            dataGridView1.DataSource = dtProducts;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            modeNew = true;
            SetControls(true);
            txtProductID.Clear();
            txtProductName.Clear();
            txtUnit.Clear();
            txtPrice.Clear();
            txtDescription.Clear();
            txtProductID.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            txtProductID.Focus();
            modeNew = false;
            SetControls(true);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetControls(false);
        }
        private bool IsDupplicate(string m_ProductID)
        {
            string sSql = "SELECT * FROM Products WHERE ProductID = " + m_ProductID;
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtProductID.Text.Trim() == "")
            {
                MessageBox.Show("Mã sản phẩm không được nhập rỗng");
                txtProductID.Focus();
                return;
            }
            if (txtProductName.Text.Trim() == "")
            {
                MessageBox.Show("Tên sản phẩm không được nhập rỗng");
                txtProductName.Focus();
                return;
            }
            if (modeNew == true)
            {
                //Kiem tra trung
                if (IsDupplicate(txtProductID.Text))
                {
                    MessageBox.Show("Đã trùng mã sản phẩm");
                    txtProductID.Clear();
                    txtProductID.Focus();
                    return;
                }
                //Them moi
                DataRow newRow = dtProducts.NewRow();
                newRow["CategoryID"] = cboCategory.SelectedValue.ToString();
                newRow["SupplierID"] = cboSupplier.SelectedValue.ToString();
                newRow["ProductID"] = txtProductID.Text;
                newRow["ProductName"] = txtProductName.Text;
                newRow["Unit"] = txtUnit.Text;
                newRow["Price"] = txtPrice.Text;
                newRow["Description"] = txtDescription.Text;
                dtProducts.Rows.Add(newRow);
                mySqlDataAdapter.Update(dtProducts);
            }
            else
            {
                //Kiem tra trung                                
                int pos = dataGridView1.CurrentRow.Index;
                //Lay ma truoc khi sua
                string m_ProductID = dataGridView1.Rows[pos].Cells[2].Value.ToString();
                if (txtProductID.Text.Trim() != m_ProductID.Trim())
                {
                    //Kiem tra trung
                    if (IsDupplicate(txtProductID.Text))
                    {
                        MessageBox.Show("Đã trùng mã sản phẩm");
                        txtProductID.Clear();
                        txtProductID.Focus();
                        return;
                    }
                }
                DataRow editRow = dtProducts.Rows[pos];
                editRow["CategoryID"] = cboCategory.SelectedValue.ToString();
                editRow["SupplierID"] = cboSupplier.SelectedValue.ToString();
                editRow["ProductID"] = txtProductID.Text;
                editRow["ProductName"] = txtProductName.Text;
                editRow["Unit"] = txtUnit.Text;
                editRow["Price"] = txtPrice.Text;
                editRow["Description"] = txtDescription.Text;
                mySqlDataAdapter.Update(dtProducts);
            }
            Display();
            SetControls(false);
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            cboCategory.SelectedValue = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            cboSupplier.SelectedValue = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtProductID.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtProductName.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            txtUnit.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            txtPrice.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
            txtDescription.Text = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult dr;
            dr = MessageBox.Show("Chắc chắn xóa dòng dữ liệu đã chọn không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == System.Windows.Forms.DialogResult.No) return;
            int pos = dataGridView1.CurrentRow.Index;
            dtProducts.Rows[pos].Delete();
            mySqlDataAdapter.Update(dtProducts);
            Display();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            btnSearch.PerformClick();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string sSql = "SELECT * FROM Products WHERE ProductName + ProductName Like N'%" + txtSearch.Text + "%'";
            mySqlDataAdapter = new SqlDataAdapter(sSql, mySqlConnection);
            mySqlCommandBuilder = new SqlCommandBuilder(mySqlDataAdapter);
            dtProducts = new DataTable();
            mySqlDataAdapter.Fill(dtProducts);
            dataGridView1.DataSource = dtProducts;
        }

       

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Bạn chắc chắn muốn thoát không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No) return;
            else this.Close();
        }

        
        private void frmProducts_Load(object sender, EventArgs e)
        {
            mySqlConnection = new SqlConnection(conStr);
            mySqlConnection.Open();

            //Dưa du lieu vao COmboBox
            string sSql = "SELECT * FROM Categories";
            mySqlDataAdapter = new SqlDataAdapter(sSql, mySqlConnection);
            tbCategories = new DataTable();
            mySqlDataAdapter.Fill(tbCategories);
            //Hien thi du lieu ComboBox
            cboCategory.DataSource = tbCategories;
            cboCategory.DisplayMember = "CategoryName";
            cboCategory.ValueMember = "CategoryID";


            sSql = "SELECT * FROM Suppliers";
            mySqlDataAdapter = new SqlDataAdapter(sSql, mySqlConnection);
            tbSuppliers = new DataTable();
            mySqlDataAdapter.Fill(tbSuppliers);
            //Hien thi du lieu ComboBox
            cboSupplier.DataSource = tbSuppliers;
            cboSupplier.DisplayMember = "CompanyName";
            cboSupplier.ValueMember = "SupplierID";
            //------------------
            Display();
            SetControls(false);
        }
    }
}
