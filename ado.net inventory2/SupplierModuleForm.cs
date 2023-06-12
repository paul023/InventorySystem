using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ado.net_inventory2
{
    public partial class SupplierModuleForm : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\admin\OneDrive\Documents\dbI.mdf;Integrated Security=True;Connect Timeout=30");

        SqlCommand cm = new SqlCommand();
        public SupplierModuleForm()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            try
            {
                // Check if any input fields are null
                if (string.IsNullOrEmpty(txtsname.Text) ||
                   string.IsNullOrEmpty(txtsphone.Text) ||
                   string.IsNullOrEmpty(txtEmail.Text) ||
                   string.IsNullOrEmpty(txtsaddress.Text))

                {
                    MessageBox.Show("Please fill out all fields", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }



                cm = new SqlCommand("SELECT COUNT(*) FROM tbSupplier WHERE sname = @sname OR semailaddress=@semailaddress", con);
                cm.Parameters.AddWithValue("@sname", txtsname.Text);
                cm.Parameters.AddWithValue("@semailaddress", txtEmail.Text);

                con.Open();
                int count = (int)cm.ExecuteScalar();
                con.Close();

                if (count > 0)
                {
                    MessageBox.Show("Supplier name or emaill address already exists.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Check if the password and confirm password match

                // If all checks pass, insert the new user into the database
                if (MessageBox.Show("Are you sure you want to save this Supplier?", "Saving Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cm = new SqlCommand("INSERT INTO tbSupplier(sname,sphone,semailaddress,saddress)VALUES(@sname,@sphone,@semailaddress,@saddress)", con);
                    cm.Parameters.AddWithValue("@sname", txtsname.Text);
                    cm.Parameters.AddWithValue("@sphone", txtsphone.Text);
                    cm.Parameters.AddWithValue("@semailaddress", txtEmail.Text);
                    cm.Parameters.AddWithValue("@saddress", txtsaddress.Text);

                    con.Open();
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Supplier has been successfully saved.");
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
                txtsname.Clear();
                txtsphone.Clear();
                txtsaddress.Clear();
                txtEmail.Clear();
                
            }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if any input fields are null
                if (string.IsNullOrEmpty(txtsname.Text) ||
                    string.IsNullOrEmpty(txtsphone.Text) ||
                    string.IsNullOrEmpty(txtEmail.Text) ||
                    string.IsNullOrEmpty(txtsaddress.Text))
                    
                {
                    MessageBox.Show("Please fill out all required fields.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Check if the user exists in the database
                cm = new SqlCommand("SELECT COUNT(*) FROM tbSupplier WHERE sname=@sname OR semailaddress=@semailaddress", con);
                cm.Parameters.AddWithValue("@sname", txtsname.Text);
                cm.Parameters.AddWithValue("@semailaddress", txtEmail.Text);

                con.Open();
                int count = (int)cm.ExecuteScalar();
                con.Close();

                if (count > 1)
                {
                    MessageBox.Show("Supplier Name or Email address Already exist.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // If the user exists, update the user's information in the database
                if (MessageBox.Show("Are you sure you want to update this user?", "Updating Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cm = new SqlCommand("UPDATE tbSupplier SET sname=@sname, sphone=@sphone,semailaddress=@semailaddress, saddress=@saddress WHERE Sid=@Sid", con);
                    cm.Parameters.AddWithValue("@Sid", lblsid.Text);
                    cm.Parameters.AddWithValue("@sname", txtsname.Text);
                    cm.Parameters.AddWithValue("@sphone", txtsphone.Text);
                    cm.Parameters.AddWithValue("@semailaddress", txtEmail.Text);
                    cm.Parameters.AddWithValue("@saddress", txtsaddress.Text);
                 
                    con.Open();
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Supplier has been successfully updated.");
                    Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
    }
    }

