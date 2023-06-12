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
    public partial class ProductModuleForm : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\admin\OneDrive\Documents\dbI.mdf;Integrated Security=True;Connect Timeout=30");

        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        public ProductModuleForm()
        {
            InitializeComponent();
            loadCategory();
        }
        
        public void loadCategory()
        {
            cmbcategory.Items.Clear();
            cm = new SqlCommand("Select catname FROM tbCategory", con);
            con.Open();
            dr= cm.ExecuteReader();
            while (dr.Read())
            {
                cmbcategory.Items.Add(dr[0].ToString());
            }
            dr.Close();
            con.Close();
        }

        private void pictureBoxclose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if any input fields are null
                if (string.IsNullOrEmpty(txtpname.Text) ||
                    string.IsNullOrEmpty(txtpdescription.Text) ||
                    string.IsNullOrEmpty(txtpprice.Text) ||
                    string.IsNullOrEmpty(txtpquantity.Text) ||
                    string.IsNullOrEmpty(cmbcategory.Text))
                {
                    MessageBox.Show("Please fill out all fields.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Check if the username or fullname already exists in the database
                cm = new SqlCommand("SELECT COUNT(*) FROM tbproduct WHERE pname=@pname", con);
                cm.Parameters.AddWithValue("@pname", txtpname.Text);
               
                con.Open();
                int count = (int)cm.ExecuteScalar();
                con.Close();

                if (count > 0)
                {
                    MessageBox.Show("Product Name already exists.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Check if the password and confirm password match
                
                
                // If all checks pass, insert the new user into the database
                if (MessageBox.Show("Are you sure you want to save this Product?", "Saving Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cm = new SqlCommand("INSERT INTO tbproduct(pname,pqty,pprice,pdescription,pcategory)VALUES(@pname,@pqty,@pprice,@pdescription,@pcategory)", con);
                    cm.Parameters.AddWithValue("@pname", txtpname.Text);
                    cm.Parameters.AddWithValue("@pqty", Convert.ToInt32(txtpquantity.Text));
                    cm.Parameters.AddWithValue("@pprice", Convert.ToInt32(txtpprice.Text));
                    cm.Parameters.AddWithValue("@pdescription", txtpdescription.Text);
                    cm.Parameters.AddWithValue("@pcategory", cmbcategory.Text);
                    con.Open();
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Product has been successfully saved.");
                    loadCategory();
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
            txtpname.Clear();
            txtpdescription.Clear();
            txtpprice.Clear();
            txtpquantity.Clear();
            cmbcategory.Items.Clear();
        }

        private void ProductModuleForm_Load(object sender, EventArgs e)
        {

        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if any input fields are null
                if (string.IsNullOrEmpty(txtpname.Text) ||
                    string.IsNullOrEmpty(txtpdescription.Text) ||
                    string.IsNullOrEmpty(txtpquantity.Text) ||
                    string.IsNullOrEmpty(cmbcategory.Text) ||
                    string.IsNullOrEmpty(txtpprice.Text))
                {
                    MessageBox.Show("Please fill out all required fields.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Check if the user exists in the database
                cm = new SqlCommand("SELECT COUNT(*) FROM tbproduct WHERE pname=@pname", con);
                cm.Parameters.AddWithValue("@pname", txtpname.Text);
                con.Open();
                int count = (int)cm.ExecuteScalar();
                con.Close();

                if (count > 1)
                {
                    MessageBox.Show("Product Name does not exist.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // If the user exists, update the user's information in the database
                if (MessageBox.Show("Are you sure you want to update this user?", "Updating Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cm = new SqlCommand("UPDATE tbproduct SET pname=@pname, pqty=@pqty,pprice=@pprice, pdescription=@pdescription, pcategory = @pcategory WHERE pid=@pid", con);
                    cm.Parameters.AddWithValue("@pid", lblpid.Text);
                    cm.Parameters.AddWithValue("@pname", txtpname.Text);
                    cm.Parameters.AddWithValue("@pqty", txtpquantity.Text);
                    cm.Parameters.AddWithValue("@pprice", txtpprice.Text);
                    cm.Parameters.AddWithValue("@pdescription", txtpdescription.Text);
                    cm.Parameters.AddWithValue("@pcategory", cmbcategory.Text);
                       

                    con.Open();
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Product has been successfully updated.");
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

        private void cmbcategory_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

