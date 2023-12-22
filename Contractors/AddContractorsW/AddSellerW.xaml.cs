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
    /// Interaction logic for AddSellerW.xaml
    /// </summary>
    public partial class AddSellerW : Window
    {
        Seller seller { get; set; }
        public AddSellerW()
        {
            InitializeComponent();
            seller = new Seller();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SaveSellerBtn_Click(object sender, RoutedEventArgs e)
        {
            seller.SellerName = SellerNameTxt.Text;
            seller.SellerZipCode = SellerZipCodeTxt.Text;
            seller.SellerCity = SellerCity.Text;
            seller.SellerStreet = SellerStreet.Text;
            seller.SellerNIP = SellerNIP.Text;
            seller.SellerBankName = SellerBankName.Text;
            seller.SellerBankAccount = SellerBankAccount.Text;
            seller.SellerTitle = NameToSave.Text;

            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Seller (SellerName, SellerZipCode, SellerCity, SellerStreet, SellerNIP, SellerBankName, SellerBankAccount, SellerTitle) OUTPUT INSERTED.IdSeller VALUES (@SellerName, @SellerZipCode, @SellerCity, @SellerStreet, @SellerNIP, @SellerBankName, @SellerBankAccount, @SellerTitle)";
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            int SellerID = 0;

                            cmd.Parameters.AddWithValue("@SellerName", seller.SellerName);
                            cmd.Parameters.AddWithValue("@SellerZipCode", seller.SellerZipCode);
                            cmd.Parameters.AddWithValue("@SellerCity", seller.SellerCity);
                            cmd.Parameters.AddWithValue("@SellerStreet", seller.SellerStreet);
                            cmd.Parameters.AddWithValue("@SellerNIP", seller.SellerNIP);
                            cmd.Parameters.AddWithValue("@SellerBankName", seller.SellerBankName);
                            cmd.Parameters.AddWithValue("@SellerBankAccount", seller.SellerBankAccount);
                            cmd.Parameters.AddWithValue("@SellerTitle", seller.SellerTitle);

                            SellerID = (int)cmd.ExecuteScalar();
                        }

                        MessageBox.Show("Poprawnie dodano Sprzedawcę!", "Dodano Kontrahenta", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Błąd wykonania polecenia: " + ex.Message, "Błąd bazy danych", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    finally
                    {
                        conn.Close();                    }
                }
                
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Wystąpił błąd połączenia: " + ex.Message, "Błąd bazy danych", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
