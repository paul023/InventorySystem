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
    public partial class SupplierForm : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\admin\OneDrive\Documents\dbI.mdf;Integrated Security=True;Connect Timeout=30");
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        public SupplierForm()
        {
            InitializeComponent();
            LoadSupplier();

        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            SupplierModuleForm supplierModule = new SupplierModuleForm();
            supplierModule.btnSave.Enabled = true;
            supplierModule.btnupdate.Enabled = false;
            supplierModule.ShowDialog();
            LoadSupplier();
        }
        public void LoadSupplier()
        {
            int i = 0;
            dgvSupplier.Rows.Clear();
            cm = new SqlCommand("SELECT * FROM tbSupplier WHERE CONCAT(sid, sname,sphone,semailaddress,saddress) LIKE '%" + txtsearch.Text + "%' ", con);
            con.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvSupplier.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString());
            }
            dr.Close();
            con.Close();
        }

       

        private void txtsearch_TextChanged(object sender, EventArgs e)
        {
            LoadSupplier();
        }

        private void dgvSupplier_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvSupplier.Columns[e.ColumnIndex].Name;
            if (colName == "Edit")
            {
                SupplierModuleForm supplierModule = new SupplierModuleForm();

                supplierModule.lblsid.Text = dgvSupplier.Rows[e.RowIndex].Cells[1].Value.ToString();
                supplierModule.txtsname.Text = dgvSupplier.Rows[e.RowIndex].Cells[2].Value.ToString();
                supplierModule.txtsphone.Text = dgvSupplier.Rows[e.RowIndex].Cells[3].Value.ToString();
                supplierModule.txtEmail.Text = dgvSupplier.Rows[e.RowIndex].Cells[4].Value.ToString();
                supplierModule.txtsaddress.Text = dgvSupplier.Rows[e.RowIndex].Cells[5].Value.ToString();

                supplierModule.btnSave.Enabled = false;
                supplierModule.btnupdate.Enabled = true;
                supplierModule.ShowDialog();
            }
            else if (colName == "Delete")
            {
                if (MessageBox.Show("Are you sure you want to delete this supplier?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    con.Open();
                    cm = new SqlCommand("DELETE FROM tbSupplier WHERE Sid LIKE '" + dgvSupplier.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", con);
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Record has been successfully deleted!");
                }
            }
            LoadSupplier();
        }
    }
}
