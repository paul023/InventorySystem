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
using System.Security.Cryptography.X509Certificates;

namespace ado.net_inventory2
{
    public partial class OrderModuleForm : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\admin\OneDrive\Documents\dbI.mdf;Integrated Security=True;Connect Timeout=30");
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        int qty = 0;
        public OrderModuleForm()
        {
            InitializeComponent();
            LoadCustomer();
            LoadProduct();
        }
        public void LoadProduct()
        {
            int i = 0;
            dgvProduct.Rows.Clear();
            cm = new SqlCommand("SELECT * FROM tbproduct WHERE CONCAT(pid, pname,pprice,pdescription,pcategory) LIKE '%" + txtsearchproduct.Text + "%' ", con);
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

        public void LoadCustomer()
        {
            int i = 0;
            dgvCustomer.Rows.Clear();
            cm = new SqlCommand("SELECT * FROM tbCustomer WHERE CONCAT(cid,cname) LIKE '%" +txtseacrhcustomer.Text+"%'", con);
            con.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvCustomer.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString());
            }
            dr.Close();
            con.Close();
        }

        private void pictureBoxclose_Click(object sender, EventArgs e)
        {
            this.Dispose(); 
        }

        
        
       
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Getqty();
            if(Convert.ToInt32(numericUpDown1.Value) > qty)
            {
                MessageBox.Show("Instock quantity is not enough!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numericUpDown1.Value = numericUpDown1.Value - 1;
                return;
            }
            if (Convert.ToInt32(numericUpDown1.Value) > 0)
            {
                int total = Convert.ToInt32(txtprice.Text) * Convert.ToInt32(numericUpDown1.Value);
                txttotal.Text = total.ToString();
            }

        }

        private void dgvCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvProduct.Rows.Count)
            {
                txtcid.Text = dgvCustomer.Rows[e.RowIndex].Cells[1].Value.ToString();
                txtcname.Text = dgvCustomer.Rows[e.RowIndex].Cells[2].Value.ToString();
            }
        }

        private void dgvProduct_CellClick(object sender, DataGridViewCellEventArgs e)
        { 
            if (e.RowIndex >= 0 && e.RowIndex < dgvProduct.Rows.Count)
            {
                txtpid.Text = dgvProduct.Rows[e.RowIndex].Cells[1].Value?.ToString();
                txtpname.Text = dgvProduct.Rows[e.RowIndex].Cells[2].Value?.ToString();
                txtprice.Text = dgvProduct.Rows[e.RowIndex].Cells[4].Value?.ToString();
            }

        }

        private void txtsearch_TextChanged(object sender, EventArgs e)
        {
            LoadProduct();
        }

        private void txtseacrhcustomer_TextChanged(object sender, EventArgs e)
        {
            LoadCustomer();
        }

        

      
        public void Clear()
        {
            txtcid.Clear();
            txtprice.Clear();
            txtpname.Clear();
            txtcname.Clear();
            txttotal.Clear();
            numericUpDown1.Value = 0;
            datenow.Value = DateTime.Now;
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtprice.Text) ||
                   string.IsNullOrEmpty(txtpname.Text) ||
                   string.IsNullOrEmpty(txtcname.Text) ||
                   string.IsNullOrEmpty(txttotal.Text))

                {
                    MessageBox.Show("Please fill out all fields.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                if (txtcid.Text == "")
                {
                    MessageBox.Show("Please select cutomer!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (txtpid.Text == "")
                {
                    MessageBox.Show("Please select product!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (MessageBox.Show("Are you sure you want to save this Order?", "Saving Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cm = new SqlCommand("INSERT INTO tbOrder(odate,pid,cid,qty,price,total)VALUES(@odate,@pid,@cid,@qty,@price,@total)", con);
                    cm.Parameters.AddWithValue("@odate", datenow.Value);
                    cm.Parameters.AddWithValue("@pid", Convert.ToInt32(txtpid.Text));
                    cm.Parameters.AddWithValue("@cid", Convert.ToInt32(txtcid.Text));
                    cm.Parameters.AddWithValue("@qty", Convert.ToInt32(numericUpDown1.Value));
                    cm.Parameters.AddWithValue("@price", Convert.ToInt32(txtprice.Text));
                    cm.Parameters.AddWithValue("@total", Convert.ToInt32(txttotal.Text));
                    con.Open();
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Order has been successfully saved.");


                    cm = new SqlCommand("UPDATE tbProduct SET pqty = (pqty-@pqty) WHERE pid LIKE '" + txtpid.Text + "' ", con);
                    cm.Parameters.AddWithValue("@pqty", Convert.ToInt32(numericUpDown1.Text));
                    
                    con.Open();
                    cm.ExecuteNonQuery();
                    con.Close();
                    Clear();
                    LoadProduct();

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
            btnInsert.Enabled = true;
        ;
        }
        public void Getqty()
        {
            
            cm = new SqlCommand("SELECT * FROM tbproduct WHERE pid = '"+ txtpid.Text + "'", con);
            con.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                qty = Convert.ToInt32(dr[0].ToString());
            }   
            dr.Close();
            con.Close();
        }
        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
