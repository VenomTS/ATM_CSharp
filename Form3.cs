using System;
using System.Windows.Forms;

namespace ATM
{
    public partial class Form3 : Form
    {
        private static double _userFunds;
        private readonly string _cardNumber;
        private readonly string _fullName;
        
        public Form3(string name, string surname, string cardNumber, double userFunds)
        {
            InitializeComponent();
            _userFunds = userFunds;
            _cardNumber = cardNumber;
            _fullName = name + " " + surname;
            UpdateText(_fullName, _userFunds);
        }

        private void UpdateText(string fName, double uFunds)
        {
            label2.Text = $"Account Details:\nFull Name: {fName}\nBalance {Math.Round(uFunds, 2)}$";
        }

        public void UpdateFunds(double newFunds)
        {
            _userFunds = newFunds;
            UpdateText(_fullName, _userFunds);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e) // Deposit
        {
            var depositForm = new Form5(this, _userFunds, true, _cardNumber);
            depositForm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var depositForm = new Form5(this, _userFunds, false, _cardNumber);
            depositForm.Show();
        }
    }
}