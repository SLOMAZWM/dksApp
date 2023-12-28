using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace dksApp.Contractors.AddContractorsW
{
    /// <summary>
    /// Interaction logic for AddBuyerW.xaml
    /// </summary>
    public partial class AddBuyerW : Window
    {
        private Buyer buyer = new Buyer();
        public AddBuyerW()
        {
            InitializeComponent();
        }

        private bool isEmpty()
        {
            if (string.IsNullOrEmpty(BuyerNameTxt.Text))
            {
                return true;
            }
            else if (string.IsNullOrEmpty(BuyerZipCodeTxt.Text))
            {
                return true;
            }
            else if (string.IsNullOrEmpty(BuyerCity.Text))
            {
                return true;
            }
            else if (string.IsNullOrEmpty(BuyerStreet.Text))
            {
                return true;
            }
            else if (string.IsNullOrEmpty(BuyerNIP.Text))
            {
                return true;
            }
            else if (string.IsNullOrEmpty(BuyerBankName.Text))
            {
                return true;
            }
            else if (string.IsNullOrEmpty(BuyerBankAccount.Text))
            {
                return true;
            }
            else if (string.IsNullOrEmpty(NameToSave.Text))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void SaveBuyerBtn_Click(object sender, RoutedEventArgs e)
        {
            if(isEmpty() == false)
            {
                buyer.BuyerName = BuyerNameTxt.Text;
                buyer.BuyerZipCode = BuyerZipCodeTxt.Text;
                buyer.BuyerCity = BuyerCity.Text;
                buyer.BuyerStreet = BuyerStreet.Text;
                buyer.BuyerNIP = BuyerNIP.Text;
                buyer.BuyerBankName = BuyerBankName.Text;
                buyer.BuyerBankAccount = BuyerBankAccount.Text;
                buyer.BuyerTitle = NameToSave.Text;

                string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        string query = "INSERT INTO Buyer (BuyerName, BuyerStreet, BuyerCity, BuyerZipCode, BuyerNIP, BuyerBankName, BuyerBankAccount, BuyerTitle)  OUTPUT INSERTED.IdBuyer VALUES (@BuyerName, @BuyerStreet, @BuyerCity, @BuyerZipCode, @BuyerNIP, @BuyerBankName, @BuyerBankAccount, @BuyerTitle)";
                        try
                        {
                            using (SqlCommand cmd = new SqlCommand(query, conn))
                            {
                                int SellerID = 0;

                                cmd.Parameters.AddWithValue("@BuyerName", buyer.BuyerName);
                                cmd.Parameters.AddWithValue("@BuyerZipCode", buyer.BuyerZipCode);
                                cmd.Parameters.AddWithValue("@BuyerCity", buyer.BuyerCity);
                                cmd.Parameters.AddWithValue("@BuyerStreet", buyer.BuyerStreet);
                                cmd.Parameters.AddWithValue("@BuyerNIP", buyer.BuyerNIP);
                                cmd.Parameters.AddWithValue("@BuyerBankName", buyer.BuyerBankName);
                                cmd.Parameters.AddWithValue("@BuyerBankAccount", buyer.BuyerBankAccount);
                                cmd.Parameters.AddWithValue("@BuyerTitle", buyer.BuyerTitle);

                                SellerID = (int)cmd.ExecuteScalar();
                            }
                            MessageBox.Show("Poprawnie dodano Nabywcę!", "Dodano Kontrahenta", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        catch (SqlException ex)
                        {
                            MessageBox.Show("Błąd wykonania bazy danych: " + ex.Message, "Błąd bazy danych!", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        finally
                        {
                            conn.Close();
                        }
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Błąd połączenia z bazą danych: " + ex.Message, "Błąd bazy danych!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Wypełnij wszystkie pola!", "Błąd zapisu!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }


        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //front

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }


    }
}
