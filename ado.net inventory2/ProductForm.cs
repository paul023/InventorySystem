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

    public partial class ProductForm : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\admin\OneDrive\Documents\dbI.mdf;Integrated Security=True;Connect Timeout=30");
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        public ProductForm()
        {
            InitializeComponent();
            LoadProduct();
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            ProductModuleForm productModule = new ProductModuleForm();
            productModule.btnSave.Enabled = true;
            productModule.btnupdate.Enabled = false;
            productModule.ShowDialog();
            LoadProduct();
        }


        public void LoadProduct()
        {
            int i = 0;
            dgvProduct.Rows.Clear();
            cm = new SqlCommand("SELECT * FROM tbproduct WHERE CONCAT(pid, pname,pprice,pdescription,pcategory) LIKE '%"+txtsearch.Text+"%' ", con);
            con.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvProduct.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString());
            }
            dr.Close();
            con.Close();
        }

        private void dgvProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvProduct.Columns[e.ColumnIndex].Name;
            if (colName == "Edit")
            {
                ProductModuleForm productModule = new ProductModuleForm();

                productModule.lblpid.Text = dgvProduct.Rows[e.RowIndex].Cells[1].Value.ToString();
                productModule.txtpname.Text = dgvProduct.Rows[e.RowIndex].Cells[2].Value.ToString();
                productModule.txtpquantity.Text = dgvProduct.Rows[e.RowIndex].Cells[3].Value.ToString();
                productModule.txtpprice.Text = dgvProduct.Rows[e.RowIndex].Cells[4].Value.ToString();
                productModule.txtpdescription.Text = dgvProduct.Rows[e.RowIndex].Cells[5].Value.ToString();
                productModule.cmbcategory.Text = dgvProduct.Rows[e.RowIndex].Cells[6].Value.ToString();
                

                
                
                productModule.btnSave.Enabled = false;
                productModule.btnupdate.Enabled = true;
               // productModule.txtuser.Enabled = false;
                productModule.ShowDialog();
            }
            else if (colName == "Delete")
            {
                if (MessageBox.Show("Are you sure you want to delete this product?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    con.Open();
                    cm = new SqlCommand("DELETE FROM tbproduct WHERE pid LIKE '" + dgvProduct.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", con);
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Record has been successfully deleted!");
                }
            }
            LoadProduct();
        }

        private void txtsearch_TextChanged(object sender, EventArgs e)
        {
            LoadProduct();
        }
    }
}
