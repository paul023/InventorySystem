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
    public partial class CategoryModuleForm : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\admin\OneDrive\Documents\dbI.mdf;Integrated Security=True;Connect Timeout=30");
        SqlDataReader dr;
        SqlCommand cm = new SqlCommand();
        public CategoryModuleForm()
        {
            InitializeComponent();


        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if category name is empty or null
                if (string.IsNullOrEmpty(txtcatname.Text))
                {
                    MessageBox.Show("Category name cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Check if category already exists
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM tbCategory WHERE catname = @catname", con))
                {
                    cmd.Parameters.AddWithValue("@catname", txtcatname.Text);
                    con.Open();
                    int count = (int)cmd.ExecuteScalar();
                    if (count > 0)
                    {
                        MessageBox.Show("Category already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        con.Close();
                        return;
                    }
                    con.Close();
                }

                // If category name is valid and category does not exist, save the category
                if (MessageBox.Show("Are you sure you want to save this Category?", "Saving Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    using (SqlCommand cm = new SqlCommand("INSERT INTO tbCategory(catname)VALUES(@catname)", con))
                    {
                        cm.Parameters.AddWithValue("@catname", txtcatname.Text);
                        con.Open();
                        cm.ExecuteNonQuery();
                        con.Close();
                        MessageBox.Show("Category has been successfully saved.");

                    }
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
        public void Clear()
        {
            txtcatname.Clear();
        }
        private void btnupdate_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if category name is empty or null
                if (string.IsNullOrEmpty(txtcatname.Text))
                {
                    MessageBox.Show("Category name cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Check if category already exists
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM tbCategory WHERE catname = @catname AND catid <> @catid", con))
                {
                    cmd.Parameters.AddWithValue("@catname", txtcatname.Text);
                    cmd.Parameters.AddWithValue("@Catid", lblcategory.Text);

                    // cmd.Parameters.AddWithValue("@catid", categoryId);
                    con.Open();
                    int count = (int)cmd.ExecuteScalar();
                    if (count > 1)
                    {
                        MessageBox.Show("Category name already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        con.Close();
                        return;
                    }
                    con.Close();
                }

                // If category name is valid and category does not exist, update the category
                if (MessageBox.Show("Are you sure you want to update this Category?", "Updating Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    using (SqlCommand cm = new SqlCommand("UPDATE tbCategory SET catname = @catname WHERE catid = @catid", con))
                    {
                        cm.Parameters.AddWithValue("@catname", txtcatname.Text);
                        cm.Parameters.AddWithValue("@Catid", lblcategory.Text);
                        //  cm.Parameters.AddWithValue("@catid", categoryId);
                        con.Open();
                        cm.ExecuteNonQuery();
                        con.Close();
                        MessageBox.Show("Category has been successfully updated.");
                        Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void pictureBoxclose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}