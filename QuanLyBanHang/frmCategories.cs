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
    public partial class frmCategories : Form
    {
        private string conStr = "Initial Catalog = QLBH; Persist Security Info=True;User ID = sa; Password=123456";
        private SqlConnection mySqlConnection;
        private SqlCommand mySqlCommand;
        public DataTable dtCategories;
        private bool modeNew;
        public frmCategories()
        {
            InitializeComponent();
        }

        private void frmCategories_Load(object sender, EventArgs e)
        {
            mySqlConnection = new SqlConnection(conStr);
            mySqlConnection.Open();
            Dislay();
            SetControls(false);
        }
        private void SetControls(bool edit)
        {
            txtCategoryID.Enabled = edit;
            txtCategoryName.Enabled = edit;
            txtDescription.Enabled = edit;
            btnSave.Enabled = edit;
            btnCancel.Enabled = edit;

            btnNew.Enabled = !edit;
            btnEdit.Enabled = !edit;
            btnDelete.Enabled = !edit;
        }
        private void Dislay()
        {
            string sSql = "SELECT * FROM Categories ORDER BY CategoryName";
            mySqlCommand = new SqlCommand(sSql, mySqlConnection);
            SqlDataReader drCategories = mySqlCommand.ExecuteReader();
            dtCategories = new DataTable();
            dtCategories.Load(drCategories);
            //Hien thi len luoi
            dataGridView1.DataSource = dtCategories;
        }


        private bool IsDublicateID(string m_CategoryID)
        {
            //Kiem tra ma trung
            string SSql = "SELECT CategoryID FROM Categories WHERE CategoryID = " + m_CategoryID;
            mySqlCommand = new SqlCommand(SSql, mySqlConnection);
            SqlDataReader drCategories = mySqlCommand.ExecuteReader();
            if (drCategories.HasRows == true)
            {
                MessageBox.Show("Đã nhập trùng mã loại hàng", "Thông báo");
                txtCategoryID.Clear();
                txtCategoryID.Focus();
                drCategories.Close();
                return true;
            }
            drCategories.Close();
            return false;
        }



        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtCategoryID.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtCategoryName.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtDescription.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
        }

        private void btnNew_Click_1(object sender, EventArgs e)
        {
            modeNew = true;
            SetControls(true);
            txtCategoryID.Clear();
            txtCategoryName.Clear();
            txtDescription.Clear();
            txtCategoryID.Focus();
        }

        private void btnEdit_Click_1(object sender, EventArgs e)
        {
            modeNew = false;
            SetControls(true);
            txtCategoryID.Focus();
        }

        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            DialogResult dr;
            dr = MessageBox.Show("Xóa dữ liệu loại hàng đã chọn, các dữ liệu liên quan sẽ bị mất. Chắc chắn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == System.Windows.Forms.DialogResult.No) return;

            string sSql = "DELETE FROM Categories WHERE CategoryID = " + txtCategoryID.Text;
            SqlCommand mySqlCommand = new SqlCommand(sSql, mySqlConnection);
            mySqlCommand.ExecuteNonQuery();
            Dislay();
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {

            //Kiem tra du lieu nhap/sua
            int CategoryID;
            bool ktra = int.TryParse(txtCategoryID.Text, out CategoryID);
            if (ktra == false)
            {
                MessageBox.Show("Mã loại hàng phải là kiểu số!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtCategoryID.Clear();
                txtCategoryID.Focus();
                return;
            }
            if (txtCategoryName.Text.Trim() == "")
            {
                MessageBox.Show("Đề nghị nhập tên loại hàng!", "Thông báo");
                txtCategoryName.Focus();
                return;
            }
            if (modeNew == true)
            {
                //Kiem tra trung ma
                if (IsDublicateID(txtCategoryID.Text)) return;
                //Them moi
                string SSql = "INSERT INTO Categories VALUES (" + txtCategoryID.Text + ", N'" + txtCategoryName.Text + "', N'" + txtDescription.Text + "')";
                SqlCommand mySqlCommand = new SqlCommand(SSql, mySqlConnection);
                mySqlCommand.ExecuteNonQuery();
            }
            else
            {
                //Sua du lieu
                //Lay dong hien thoi
                int curRow = dataGridView1.CurrentRow.Index;
                //Lay ma truoc luc sua
                string m_CategoryID = dataGridView1.Rows[curRow].Cells[0].Value.ToString();
                if (txtCategoryID.Text.Trim() != m_CategoryID.Trim())
                {
                    if (IsDublicateID(txtCategoryID.Text)) return;
                }
                //Cap nhat du lieu
                string sSql = "UPDATE Categories SET CategoryID = " + txtCategoryID.Text + ", CategoryName = N'" + txtCategoryName.Text + "', Description = N'" + txtDescription.Text + "' WHERE CategoryID = " + m_CategoryID;
                SqlCommand mySqlCommand = new SqlCommand(sSql, mySqlConnection);
                mySqlCommand.ExecuteNonQuery();
            }

            Dislay();
            SetControls(false);
        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            SetControls(false);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Bạn chắc chắn muốn thoát không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No) return;
            else this.Close();
        }

        private void dataGridView1_RowEnter_1(object sender, DataGridViewCellEventArgs e)
        {
            txtCategoryID.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtCategoryName.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtDescription.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
        }
    }

}
