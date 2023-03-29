using System;
using System.Windows.Forms;

namespace ATM
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) // Create Account
        {
            var createAccountForm = new Form2();
            createAccountForm.ShowDialog(this);
            createAccountForm.Dispose();

        }

        private void button2_Click(object sender, EventArgs e) // Login
        {
            var loginForm = new Form4();
            loginForm.ShowDialog(this);
            loginForm.Dispose();
        }
    }
}