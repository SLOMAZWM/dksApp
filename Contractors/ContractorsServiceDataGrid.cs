using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dksApp.Contractors
{
	public static class ContractorsServiceDataGrid
	{
		private static string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

		public static ObservableCollection<Buyer> BuyerList { get; set; }

		public static int TotalItemsCount => BuyerList.Count;

		private static int PageSize = 7;

		public static async Task GetBuyersFromDataBase()
		{
			using(SqlConnection con = new SqlConnection(connectionString)) 
			{
				BuyerList = new ObservableCollection<Buyer>();
				string query = "SELECT * FROM Buyer";
				await con.OpenAsync();
				
				using (SqlCommand cmd = new SqlCommand(query, con)) 
				{
					SqlDataReader reader = await cmd.ExecuteReaderAsync();

					while(reader.Read())
					{
						Buyer buyer = new Buyer()
						{
							IdBuyer = Convert.ToInt64(reader["IdBuyer"]),
							BuyerName = reader["BuyerName"].ToString(),
							BuyerStreet = reader["BuyerStreet"].ToString(),
							BuyerCity = reader["BuyerCity"].ToString(),
							BuyerZipCode = reader["BuyerZipCode"].ToString(),
							BuyerNIP = reader["BuyerNIP"].ToString(),
							BuyerBankName = reader["BuyerBankName"].ToString(),
							BuyerBankAccount = reader["BuyerBankAccount"].ToString(),
							BuyerTitle = reader["BuyerTitle"].ToString()
						};

						BuyerList.Add(buyer);
					}
				}
			}
		}

	}
}
