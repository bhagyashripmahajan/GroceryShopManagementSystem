﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gproject
{
    public partial class AdminLogin : Form
    {
        public AdminLogin()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (PasswordTb.Text == "")
            {
                MessageBox.Show("Enter Password");
            }
            else if (PasswordTb.Text == "emp")
            {
                Employees Emp = new Employees();
                Emp.Show();
                this.Hide();
            }
            else if(PasswordTb.Text=="itm")
            {
                Items Itm = new Items();
                Itm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Wrong admin password...");
            }
        }
    }
}
