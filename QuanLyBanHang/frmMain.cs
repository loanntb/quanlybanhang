using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyBanHang
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void thêmMớiToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void mởUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUsers frmUser = new frmUsers();
            frmUser.MdiParent = this;
            frmUser.Show();
        }

        private void kháchHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCustomers frmCustomer = new frmCustomers();
            frmCustomer.MdiParent = this;
            frmCustomer.Show();
        }

        private void nhàCungCấpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSuppiers frmSuppier = new frmSuppiers();
            frmSuppier.MdiParent = this;
            frmSuppier.Show();
        }

        private void sảnPhẩmToolStripMenuItem_Click(object sender, EventArgs e)
        {
             frmProducts fProduct = new frmProducts();
            fProduct.MdiParent = this;
            fProduct.Show();
        }

        private void thoátToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Bạn chắc chắn muốn thoát không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No) return;
            else this.Close();
        }

        private void thoátToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Bạn chắc chắn muốn thoát không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No) return;
            else this.Close();
        }

        private void thoátToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Bạn chắc chắn muốn thoát không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No) return;
            else this.Close();
        }

        

        private void xemHóaĐơnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPrintOrders frmOrder = new frmPrintOrders();
            frmOrder.MdiParent = this;
            frmOrder.Show();
        }

        private void hHóaĐơnMuaHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmOrders frmOrderss = new frmOrders();
            frmOrderss.MdiParent = this;
            frmOrderss.Show();
        }

        private void chiTiếtĐơnHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmOrderDetails frmOrderDe = new frmOrderDetails();
            frmOrderDe.MdiParent = this;
            frmOrderDe.Show();
        }

        private void inHóaĐơnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPrintOrders frmpOrder = new frmPrintOrders();
            frmpOrder.MdiParent = this;
            frmpOrder.Show();
        }

        private void thoátToolStripMenuItem3_Click_1(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Bạn chắc chắn muốn thoát không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No) return;
            else this.Close();
        }

        private void xemHóaĐơnBaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPrintCurrentdates frmpOrderdd = new frmPrintCurrentdates();
            frmpOrderdd.MdiParent = this;
            frmpOrderdd.Show();
        }

        private void btnSupplier_Click(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Bạn chắc chắn muốn thoát không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No) return;
            else this.Close();
        }

        private void btnUsers_Click(object sender, EventArgs e)
        {

        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            frmCustomers frmCustomer1 = new frmCustomers();
            frmCustomer1.MdiParent = this;
            frmCustomer1.Show();
        }
    }
}
