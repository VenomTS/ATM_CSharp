using System;
using System.Windows.Forms;

namespace ATM
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var cardNumber = textBox1.Text;
            var pinCode = textBox2.Text;
            if (cardNumber.Length == 0 || pinCode.Length != 4)
            {
                MessageBox.Show("Your input was incorrect", "ERROR - Incorrect Input");
                return;
            }

            Program.findAccount(cardNumber, pinCode);
        }
    }
}