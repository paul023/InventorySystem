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

namespace ado.net_inventory2
{
    public partial class CustomerModuleForm : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\admin\OneDrive\Documents\dbI.mdf;Integrated Security=True;Connect Timeout=30");

        SqlCommand cm = new SqlCommand();
        public CustomerModuleForm()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if any input fields are null
                if (string.IsNullOrEmpty(txtcname.Text))
                {
                    MessageBox.Show("Category name cannot be empty", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

               
                cm = new SqlCommand("SELECT COUNT(*) FROM tbCustomer WHERE cname = @cname", con);
                cm.Parameters.AddWithValue("@cname", txtcname.Text);
              
                con.Open();
                int count = (int)cm.ExecuteScalar();
                con.Close();

                if (count > 0)
                {
                    MessageBox.Show("Customer name already exists.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Check if the password and confirm password match
                
                // If all checks pass, insert the new user into the database
                if (MessageBox.Show("Are you sure you want to save this user?", "Saving Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cm = new SqlCommand("INSERT INTO tbCustomer(cname,cphone)VALUES(@cname,@cphone)", con);
                    cm.Parameters.AddWithValue("@cname", txtcname.Text);
                    cm.Parameters.AddWithValue("@cphone", txtcphone.Text);
                    
                    con.Open();
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("User has been successfully saved.");
                    Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
                try
            {
                // Check if any input fields are null
                if (string.IsNullOrEmpty(txtcname.Text))
                {
                    MessageBox.Show("Please fill out all required fields.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Check if the user exists in the database
                cm = new SqlCommand("SELECT COUNT(*) FROM tbCustomer WHERE cname=@cname", con);
                cm.Parameters.AddWithValue("@cid", lblcid.Text);
                cm.Parameters.AddWithValue("@cname", txtcname.Text);
                con.Open();
                int count = (int)cm.ExecuteScalar();
                con.Close();

                if (count > 1)
                {
                    MessageBox.Show("Customer name already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // If the user exists, update the user's information in the database
                if (MessageBox.Show("Are you sure you want to update this user?", "Updating Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cm = new SqlCommand("UPDATE tbCustomer SET cname=@cname, cphone=@cphone WHERE cid=@cid", con);
                    cm.Parameters.AddWithValue("@cid", lblcid.Text);
                    cm.Parameters.AddWithValue("@cname", txtcname.Text);
                    cm.Parameters.AddWithValue("@cphone", txtcphone.Text);
                    con.Open();
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("User has been successfully updated.");
                    Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Clear()
        {
            txtcname.Clear();
            txtcphone.Clear();
            
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            Clear();
            btnSave.Enabled = true;
            btnupdate.Enabled = false;
        }

        private void pictureBoxclose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void CustomerModuleForm_Load(object sender, EventArgs e)
        {

        }
    }
}
