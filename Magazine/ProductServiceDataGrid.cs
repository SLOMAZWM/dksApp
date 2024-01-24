using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Controls;
using System.Collections.ObjectModel;

namespace dksApp.Magazine
{
	public static class ProductServiceDataGrid
	{
		private static string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;
		private static ObservableCollection<Product> products;
		private static int PageSize = 7;
		public static void GetAllFromDataBase(ref DataGrid dataGrid)
		{
			using (SqlConnection con = new SqlConnection(connectionString)) 
			{
				products = new ObservableCollection<Product>();
				string query = "SELECT * FROM Product WHERE ShowIt = 1";
				con.Open();
				using (SqlCommand cmd = new SqlCommand(query, con)) 
				{
					SqlDataReader reader = cmd.ExecuteReader();

					while(reader.Read()) 
					{
						Product product = new Product
						{
							IdProduct = Convert.ToInt32(reader["ProductID"]),
							NumberOfItems = reader["NumberOfItems"] as int? ?? default,
							NameItem = reader["NameItem"].ToString(),
							Quantity = Convert.ToDouble(reader["Quantity"]),
							QuantityType = reader["QuantityType"].ToString(),
							PKWiU = reader["PKWiU"].ToString(),
							NettoPrice = Convert.ToDecimal(reader["NettoPrice"]),
							NettoValue = Convert.ToDecimal(reader["NettoValue"]),
							VATPercent = reader["VATPercent"].ToString(),
							VATValue = Convert.ToDecimal(reader["VATValue"]),
							BruttoValue = Convert.ToDecimal(reader["BruttoValue"]),
							ShowIt = Convert.ToBoolean(reader["ShowIt"])
						};
						products.Add(product);
					}

					dataGrid.ItemsSource = products;
				}
			}
		}

		public static ObservableCollection<Product> GetProductsPage(int currentPage)
		{
			int startIndex = (currentPage - 1) * PageSize;
			int endIndex = Math.Min(startIndex + PageSize, products.Count);
			var pageProducts = new ObservableCollection<Product>();

			for (int i = startIndex; i < endIndex; i++)
			{
				if (i >= 0 && i < products.Count)
				{
					pageProducts.Add(products[i]);
				}
			}

			return pageProducts;
		}

		public static int GetTotalPages()
		{
			return (int)Math.Ceiling((double)products.Count / PageSize);
		}
	}
}
