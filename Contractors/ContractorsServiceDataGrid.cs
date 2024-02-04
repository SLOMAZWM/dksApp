using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace dksApp.Contractors
{
    public static class ContractorsServiceDataGrid
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

        public static ObservableCollection<Buyer> BuyerList { get; set; }

        public static int TotalItemsCount => BuyerList.Count;

        private static int PageSize = 7;

        public static ObservableCollection<Buyer> FilterContractors(string text)
        {
            var filteredContractors = new ObservableCollection<Buyer>();

            foreach (var contractor in BuyerList)
            {
                if (contractor.BuyerName.Contains(text, StringComparison.OrdinalIgnoreCase) || contractor.BuyerCity.Contains(text, StringComparison.OrdinalIgnoreCase) || contractor.BuyerZipCode.Contains(text, StringComparison.OrdinalIgnoreCase))
                {
                    filteredContractors.Add(contractor);
                }
            }

            return filteredContractors;
        }

        public static void AddBuyerToDataBase(Buyer buyer)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = @"INSERT INTO Buyer (BuyerName, BuyerStreet, BuyerCity, BuyerZipCode, BuyerNIP) 
                             VALUES (@BuyerName, @BuyerStreet, @BuyerCity, @BuyerZipCode, @BuyerNIP)";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@BuyerName", buyer.BuyerName ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@BuyerStreet", buyer.BuyerStreet ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@BuyerCity", buyer.BuyerCity ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@BuyerZipCode", buyer.BuyerZipCode ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@BuyerNIP", buyer.BuyerNIP ?? (object)DBNull.Value);

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Błąd dodawania nabywcy do bazy danych: {ex.Message}", "Błąd bazy danych", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Nieoczekiwany błąd: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static async Task GetBuyersFromDataBase()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                BuyerList = new ObservableCollection<Buyer>();
                string query = "SELECT * FROM Buyer";
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        Buyer buyer = new Buyer()
                        {
                            IdBuyer = Convert.ToInt64(reader["IdBuyer"]),
                            BuyerName = reader["BuyerName"].ToString(),
                            BuyerStreet = reader["BuyerStreet"].ToString(),
                            BuyerCity = reader["BuyerCity"].ToString(),
                            BuyerZipCode = reader["BuyerZipCode"].ToString(),
                            BuyerNIP = reader["BuyerNIP"].ToString()
                        };

                        BuyerList.Add(buyer);
                    }
                }
            }
        }

        public static ObservableCollection<Buyer> GetBuyerPage(int currentPage)
        {
            int startIndex = (currentPage - 1) * PageSize;
            int endIndex = Math.Min(startIndex + PageSize, BuyerList.Count);
            var pageBuyer = new ObservableCollection<Buyer>();

            for (int i = startIndex; i < endIndex; i++)
            {
                if (i >= 0 && i < BuyerList.Count)
                {
                    pageBuyer.Add(BuyerList[i]);
                }
            }

            return pageBuyer;
        }

        public static int GetTotalPages()
        {
            return (int)Math.Ceiling((double)BuyerList.Count / PageSize);
        }

        public static void DeleteContractor(long contractorID)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Buyer WHERE IdBuyer = @IdBuyer";
                con.Open();

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@IdBuyer", contractorID);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateBuyerInDatabase(Buyer updatedBuyer)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = @"UPDATE Buyer SET 
                             BuyerName = @BuyerName, 
                             BuyerStreet = @BuyerStreet, 
                             BuyerCity = @BuyerCity, 
                             BuyerZipCode = @BuyerZipCode, 
                             BuyerNIP = @BuyerNIP
                             WHERE IdBuyer = @IdBuyer";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@IdBuyer", updatedBuyer.IdBuyer);
                        cmd.Parameters.AddWithValue("@BuyerName", updatedBuyer.BuyerName ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@BuyerStreet", updatedBuyer.BuyerStreet ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@BuyerCity", updatedBuyer.BuyerCity ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@BuyerZipCode", updatedBuyer.BuyerZipCode ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@BuyerNIP", updatedBuyer.BuyerNIP ?? (object)DBNull.Value);

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Kontrahent został zaktualizowany.", "Aktualizacja", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Błąd aktualizacji danych kontrahenta w bazie danych: {ex.Message}", "Błąd bazy danych", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Nieoczekiwany błąd: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}



