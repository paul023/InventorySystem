using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ado.net_inventory2
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        private Form activeForm = null;
        private void openChildForm(Form childForm)
        {
            if (activeForm != null)
                activeForm.Close();
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelmain.Controls.Add(childForm);
            panelmain.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();

        }

     
        private void custombutton1_Click_1(object sender, EventArgs e)
        {
            openChildForm(new UserForm());
        }

        private void btncategory_Click(object sender, EventArgs e)
        {
            openChildForm(new CategoryForm());
        }

        private void btnorder_Click(object sender, EventArgs e)
        {
            openChildForm(new OrderForm());
        }

        private void btncustomer_Click(object sender, EventArgs e)
        {
            openChildForm(new CustomerForm());
        }

        private void btnproduct_Click(object sender, EventArgs e)
        {
            openChildForm(new ProductForm());
        }

        private void pictureBoxclose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void custombutton1_Click(object sender, EventArgs e)
        {
            openChildForm(new SupplierForm());
        }
    }
}
