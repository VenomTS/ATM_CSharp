using System;
using System.Windows.Forms;

namespace ATM
{
    public partial class Form5 : Form
    {

        private double _maxWithdraw;
        private readonly bool _isDeposit;
        private readonly string _cardNumber;
        private readonly Form3 _parent;
        
        public Form5(Form3 parent, double maxWithdraw, bool isDeposit, string cardNumber)
        {
            InitializeComponent();
            _maxWithdraw = Math.Round(maxWithdraw, 2);
            _isDeposit = isDeposit;
            _cardNumber = cardNumber;
            _parent = parent;
            var text = $"Amount to withdraw (MAX: {_maxWithdraw}$)";
            var title = "ATM - Withdraw";
            if (isDeposit)
            {
                text = "Amount to Deposit: ";
                title = "ATM - Deposit";
            }

            label1.Text = title;
            label2.Text = text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {
                MessageBox.Show("You need to input amount you want to deposit/withdraw", "ERROR - No Input");
                return;
            }
            var amount = Math.Round(double.Parse(textBox1.Text), 2);
            
            
            if (_isDeposit)
            {
                var query =
                    $"UPDATE Accounts SET Balance = {_maxWithdraw + amount} WHERE CardNumber = {_cardNumber}";
                if (!Program.ExecuteQuery(query).Item1)
                {
                    MessageBox.Show(
                        "An error occurred while trying to communicate with the database\nPlease try again later",
                        "ERROR - Database Unreachable");
                    return;
                }

                _maxWithdraw += amount;

                MessageBox.Show($"You successfully deposited {amount}$ to your bank account!",
                    "DEPOSIT - Successful");
                Close();
            }
            else
            {
                if(amount > _maxWithdraw)
                {
                    MessageBox.Show(
                        $"You do not have that amount of money in your bank account!\nYou can withdraw at most {_maxWithdraw}$",
                        "WITHDRAW - Insufficient Funds");
                    return;
                }
                var query =
                    $"UPDATE Accounts SET Balance = {_maxWithdraw - amount} WHERE CardNumber = {_cardNumber}";
                if (!Program.ExecuteQuery(query).Item1)
                {
                    MessageBox.Show(
                        "An error occurred while trying to communicate with the database\nPlease try again later",
                        "ERROR - Database Unreachable");
                    return;
                }

                _maxWithdraw -= amount;

                MessageBox.Show($"You successfully withdrew {amount}$ from your bank account!",
                    "WITHDRAW - Successful");
                Close();
                
            }
            _parent.UpdateFunds(_maxWithdraw);
        }
    }
}