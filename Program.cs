using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ATM
{

    internal class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
        
        public static bool CheckValidPin(string newPin)
        {
            if (newPin.Length != 4) return false;
            char[] availableChars = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            foreach (var number in newPin)
            {
                if (!availableChars.Contains(number)) return false;
                
            }

            return true;
        }

        private static (bool, int) ExecuteQuery(string query)
        {
            const string connString = "datasource=127.0.0.1;port=3306;username=root;password=;database=bank";
            var connection = new MySqlConnection(connString);
            var command = new MySqlCommand(query, connection);
            try
            {
                connection.Open();
                var success = command.ExecuteNonQuery();
                if (success == -1)
                {
                    success = 0;
                    if (command.ExecuteReader().Read())
                    {
                        success = 1;
                    }
                }
                connection.Close();
                return (true, success);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error with database: " + e.Message);
                return (false, -1);
            }
        }

        private static (bool, List<string>) ExecuteSelect(string query)
        {
            const string connString = "datasource=127.0.0.1;port=3306;username=root;password=;database=bank";
            var connection = new MySqlConnection(connString);
            var command = new MySqlCommand(query, connection);
            try
            {
                connection.Open();
                var reader = command.ExecuteReader();
                var data = new List<string>();
                if (reader.Read())
                {
                    data.Add(reader.GetString(1));
                    data.Add(reader.GetString(2));
                    data.Add(reader.GetString(3));
                    data.Add((reader.GetDouble(5)).ToString(CultureInfo.CurrentCulture));
                }
                else
                {
                    data = null;
                }
                connection.Close();
                return (true, data);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error with database: " + e.Message);
                return (false, null);
            }
        }
        
        public static (bool, string) CreateAccount(string firstName, string lastName, string pinCode)
        {
            var cardNumber = GenerateCardNumber();

            if (!cardNumber.Item1) return (false, "");
            
            /* Establish Connection and save all data */
            string idQuery = "SELECT MIN(ID + 1) FROM Accounts WHERE NOT EXISTS (SELECT * FROM Accounts t2 WHERE t2.ID = Accounts.ID + 1)";
            string query = $"INSERT INTO Accounts SET ID = -1, FirstName = '{firstName}', LastName = '{lastName}', " +
                           $"PinCode = '{pinCode}', CardNumber = '{cardNumber.Item2}', Balance = 0.00";
            string updateQuery = $"UPDATE Accounts SET ID = ({idQuery}) WHERE CardNumber = '{cardNumber.Item2}'";
            string removeQuery = $"DELETE FROM Accounts WHERE CardNumber = '{cardNumber.Item2}'";

            var success = ExecuteQuery(query).Item1 && ExecuteQuery(updateQuery).Item2 == 1;
            if (!success)
            {
                ExecuteQuery(removeQuery);
                return (false, "");
            }
            return (true, cardNumber.Item2);
        }
        
        private static (bool, string) GenerateCardNumber()
        {
            Random rnd = new Random();
            string cardNumber = rnd.Next(1000000000, 1999999999).ToString();
            string query = $"SELECT CardNumber FROM Accounts WHERE CardNumber = {cardNumber}";
            var success = ExecuteQuery(query);
            if (success.Item1)
            {
                if (success.Item2 == 0) return (true, cardNumber);
                return GenerateCardNumber();
            }

            return (false, "");

        }

        public static bool FindAccount(string cardNumber, string pin)
        {
            
            var query = $"SELECT * FROM Accounts WHERE CardNumber = {cardNumber}";
            var result = ExecuteSelect(query);
            if (result.Item2 == null)
            {
                MessageBox.Show(
                    "Your account wasn't found in the database\nCheck if your card number was inputted correctly",
                    "ERRROR - Missing Account");
                return false;
            }
            if (!result.Item1)
            {
                MessageBox.Show("There was an error trying to reach SQL server!", "ERROR - SQL Connection");
                return false;
            }

            var name = result.Item2[0];
            var surname = result.Item2[1];
            var pinCode = result.Item2[2];
            var balance = result.Item2[3];
            MessageBox.Show($"Name: {name}\nSurname: {surname}\nPin Code: {pinCode}\nBalance: {balance}");
            return false;
        }
        
    }
    
}