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

namespace gproject
{
    public partial class Billing : Form
    {
        public Billing()
        {
            InitializeComponent();
            populate();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\MOHIT\Documents\GroceryDb.mdf;Integrated Security=True;Connect Timeout=30");
        private void populate()
        {
            Con.Open();
            string query = "select * from ItemTbl";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            ItemDGV.DataSource = ds.Tables[0];
            Con.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }
        int n = 0, GrdTotal = 0, Amount;
        private void AddToBillBtn_Click(object sender, EventArgs e)
        {
            
            if (  Convert.ToInt32(ItQtyTb.Text) > stock||ItNameTb.Text=="")
            {
                MessageBox.Show("Enter Quantity");
            }
            else
            {
                int total = Convert.ToInt32(ItQtyTb.Text) * Convert.ToInt32(ItPriceTb.Text);
                DataGridViewRow newRow = new DataGridViewRow();
                newRow.CreateCells(BillDGV);
                newRow.Cells[0].Value = n + 1;
                newRow.Cells[1].Value = ItNameTb.Text;
                newRow.Cells[2].Value = ItPriceTb.Text;
                newRow.Cells[3].Value = ItQtyTb.Text;
                newRow.Cells[4].Value = total;
                BillDGV.Rows.Add(newRow);
                GrdTotal = GrdTotal + total;
                Amount = GrdTotal;
                TotalLbl.Text = "Rs"+GrdTotal;
                n++;
                UpdateItem();
                Reset();
            }

        }
        private void UpdateItem()
        {
            try
            {
                int newQty = stock - Convert.ToInt32(ItQtyTb.Text);
                Con.Open();
                string query = "Update ItemTbl set ItQty=" + newQty + "where ItId=" + Key + ";";
                SqlCommand cmd = new SqlCommand(query,Con);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Item Updated Successfully...");
                Con.Close();
                populate();
               // Clear();
            }
            catch(Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }
        private void Reset()
        {
            ItPriceTb.Text = "";
            ItQtyTb.Text = "";
            ClientNameTb.Text = "";
            ItNameTb.Text = "";
        }
        private void ResetBtn_Click(object sender, EventArgs e)
        {
            Reset();
        }
 
        private void button2_Click(object sender, EventArgs e)
        {
            if (ClientNameTb.Text=="")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("insert into BillTbl values('" + EmployeeLbl.Text + "','" + ClientNameTb.Text + "'," + Amount + ")", Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Bill Saved Succesfully");
                    Con.Close();
                    populate();
                    //Clear();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
            if(printPreviewDialog1.ShowDialog()==DialogResult.OK)
            {
                printDocument1.Print();

            }
            
        }

        private void Billing_Load(object sender, EventArgs e)
        {
            EmployeeLbl.Text = Login.EmployeeName;

        }
        int prodid, prodqty, prodprice, tottal, pos = 60;

        private void label9_Click_1(object sender, EventArgs e)
        {
            Login obj = new Login();
            obj.Show();
            this.Hide();
        }

        string prodname;

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawString("Grocery Shop", new Font("Century Gothic", 18, FontStyle.Bold), Brushes.Blue,30,10);
            e.Graphics.DrawString("ID Product Prize Quantity Total", new Font("Century Gothic", 16, FontStyle.Bold),Brushes.Navy,70,40);
            foreach (DataGridViewRow row in BillDGV.Rows)
            {
                prodid = Convert.ToInt32(row.Cells [ "Column1" ].Value);
                prodname = "" + row.Cells["Column2"].Value;
                prodprice = Convert.ToInt32(row.Cells["Column3"].Value);
                prodqty = Convert.ToInt32(row.Cells["Column4"].Value);
                tottal = Convert.ToInt32(row.Cells["Column5"].Value);
                e.Graphics.DrawString("" + prodid,new Font("Century Gothic", 12, FontStyle.Bold), Brushes.Navy,80,80);
                e.Graphics.DrawString("" + prodname,new Font("Century Gothic", 12, FontStyle.Bold), Brushes.Navy, 100, 80);
                e.Graphics.DrawString("" + prodprice,new Font("Century Gothic",12, FontStyle.Bold), Brushes.Navy, 210, 80);
                e.Graphics.DrawString("" + prodqty,new Font("Century Gothic", 12, FontStyle.Bold), Brushes.Navy, 280, 80);
                e.Graphics.DrawString("" + tottal, new Font("Century Gothic", 12, FontStyle.Bold), Brushes.Navy, 340, 80);
                pos = pos + 20;
            }
            e.Graphics.DrawString("Grand Total:Rs" + Amount, new Font("Century Gothic", 16, FontStyle.Bold), Brushes.Navy, 200, 100);
            e.Graphics.DrawString("***************************Grocery Shop********************", new Font("Century Gothic", 18, FontStyle.Bold), Brushes.Navy, 240, 120);
            BillDGV.Rows.Clear();
            BillDGV.Refresh();
            pos = 100;
            Amount = 0;
        }

        int stock = 0 , Key=0;
        private void ItemDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ItNameTb.Text = ItemDGV.SelectedRows[0].Cells[1].Value.ToString();
            
            ItPriceTb.Text = ItemDGV.SelectedRows[0].Cells[3].Value.ToString();
            if (ItNameTb.Text == "")
            {
                stock = 0;
                Key = 0;
            }
            else
            {
                stock = Convert.ToInt32(ItemDGV.SelectedRows[0].Cells[2].Value.ToString());
                Key= Convert.ToInt32(ItemDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }
    }
}
