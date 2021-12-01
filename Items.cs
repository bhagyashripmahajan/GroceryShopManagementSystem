﻿using System;
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
    public partial class Items : Form
    {
        public Items()
        {
            InitializeComponent();
            populate();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

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
        private void Clear()
        {
            ItNameTb.Text = "";
            ItQtyTb.Text = "";
            PriceTb.Text = "";
            CatCb.SelectedIndex = -1;
            Key = 0;
        }
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (ItNameTb.Text == "" || ItQtyTb.Text == "" || PriceTb.Text == "" || CatCb.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("insert into ItemTbl values('" + ItNameTb.Text + "'," + ItQtyTb.Text + "," + PriceTb.Text + ",'" + CatCb.SelectedItem + "')", Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Item Saved Succesfully");
                    Con.Close();
                    populate();
                    Clear();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }

        }
        int Key = 0;
        private void ItemDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ItNameTb.Text = ItemDGV.SelectedRows[0].Cells[1].Value.ToString();
            ItQtyTb.Text = ItemDGV.SelectedRows[0].Cells[2].Value.ToString();
            PriceTb.Text = ItemDGV.SelectedRows[0].Cells[3].Value.ToString();
            CatCb.SelectedItem = ItemDGV.SelectedRows[0].Cells[4].Value.ToString();
            if (ItNameTb.Text == "")
            {
                Key = 0;
            }
            else
            {
                Key = Convert.ToInt32(ItemDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void ClearBtn_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void EmployeeLbl_Click(object sender, EventArgs e)
        {
            Login obj = new Login();
            obj.Show();
            this.Hide();
        }
    }
}
