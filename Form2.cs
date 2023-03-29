using System;
using System.Windows.Forms;

namespace ATM
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var firstName = textBox1.Text;
            var lastName = textBox2.Text;
            var pinCode = textBox3.Text;
            if (!Program.CheckValidPin(pinCode))
            {
                MessageBox.Show("You inputted an invalid pin code\nYour pin code must be 4 digits",
                    "Account Creation - Pin Error");
                return;
            }
            var result = Program.CreateAccount(firstName, lastName, pinCode);
            if (!result.Item1)
            {
                MessageBox.Show("An issue occured while creating your account\nPlease try again later", "Account Creation - Failure");
            }
            else
            {
                MessageBox.Show($"You successfully created an account\nYour card number: {result.Item2}", "Account Creation - Success");
            }
            Close();
        }
    }
}