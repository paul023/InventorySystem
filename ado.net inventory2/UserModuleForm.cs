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
using System.Data.SqlClient;


namespace ado.net_inventory2
{
    public partial class UserModuleForm : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\admin\OneDrive\Documents\dbI.mdf;Integrated Security=True;Connect Timeout=30");

        SqlCommand cm = new SqlCommand();
        public UserModuleForm()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if any input fields are null
                if (string.IsNullOrEmpty(txtuser.Text) || 
                    string.IsNullOrEmpty(txtfullname.Text) || 
                    string.IsNullOrEmpty(txtpassword.Text) || 
                    string.IsNullOrEmpty(txtrepass.Text) || 
                    string.IsNullOrEmpty(txtphone.Text))
                {
                    MessageBox.Show("Please fill out all fields.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Check if the username or fullname already exists in the database
                cm = new SqlCommand("SELECT COUNT(*) FROM TbUser WHERE username=@username OR fullname=@fullname", con);
                cm.Parameters.AddWithValue("@username", txtuser.Text);
                cm.Parameters.AddWithValue("@fullname", txtfullname.Text);
                con.Open();
                int count = (int)cm.ExecuteScalar();
                con.Close();

                if (count > 0)
                {
                    MessageBox.Show("Username or Fullname already exists.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Check if the password and confirm password match
                if (txtpassword.Text != txtrepass.Text)
                {
                    MessageBox.Show("Password did not Match!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // If all checks pass, insert the new user into the database
                if (MessageBox.Show("Are you sure you want to save this user?", "Saving Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cm = new SqlCommand("INSERT INTO tbUser(username,fullname,password,phone)VALUES(@username,@fullname,@password,@phone)", con);
                    cm.Parameters.AddWithValue("@username", txtuser.Text);
                    cm.Parameters.AddWithValue("@fullname", txtfullname.Text);
                    cm.Parameters.AddWithValue("@password", txtpassword.Text);
                    cm.Parameters.AddWithValue("@phone", txtphone.Text);
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
        public void Clear()
        {
            txtuser.Clear();
            txtfullname.Clear();
            txtpassword.Clear();
            txtphone.Clear();
            txtrepass.Clear();
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

        private void btnupdate_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if any input fields are null
                if (string.IsNullOrEmpty(txtuser.Text) ||
                    string.IsNullOrEmpty(txtfullname.Text) ||
                    string.IsNullOrEmpty(txtphone.Text))
                {
                    MessageBox.Show("Please fill out all required fields.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Check if the user exists in the database
                cm = new SqlCommand("SELECT COUNT(*) FROM tbUser WHERE username=@username", con);
                cm.Parameters.AddWithValue("@username", txtuser.Text);
                con.Open();
                int count = (int)cm.ExecuteScalar();
                con.Close();

                if (count == 0)
                {
                    MessageBox.Show("User does not exist.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // If the user exists, update the user's information in the database
                if (MessageBox.Show("Are you sure you want to update this user?", "Updating Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cm = new SqlCommand("UPDATE tbUser SET fullname=@fullname, phone=@phone WHERE username=@username", con);
                    cm.Parameters.AddWithValue("@fullname", txtfullname.Text);
                    cm.Parameters.AddWithValue("@phone", txtphone.Text);
                    cm.Parameters.AddWithValue("@username", txtuser.Text);
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

        private void UserModuleForm_Load(object sender, EventArgs e)
        {

        }
    }
}
