using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows;
using dksApp.Magazine.ProductW;
using dksApp.Magazine.MagazineDataGrid;

namespace dksApp
{
	public static class ProductServiceDataGrid
	{
		private static string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;
		public static ObservableCollection<Product> Products { get; set; }
        public static event Action ProductsUpdated;
        private static int PageSize = 7;

        public static void NotifyProductsUpdated()
        {
            ProductsUpdated?.Invoke();
        }

        public static void AddProductToDataBase(Product product)
		{
				try
				{
					using (SqlConnection con = new SqlConnection(connectionString))
					{
						string query = @"INSERT INTO Product (NumberOfItems, NameItem, Quantity, QuantityType, PKWiU, NettoPrice, NettoValue, VATPercent, VATValue, BruttoValue, ShowIt)
                                 VALUES (@NumberOfItems, @NameItem, @Quantity, @QuantityType, @PKWiU, @NettoPrice, @NettoValue, @VATPercent, @VATValue, @BruttoValue, @ShowIt)";
						try
						{
							using (SqlCommand cmd = new SqlCommand(query, con))
							{
								cmd.Parameters.AddWithValue("@NumberOfItems", 0);
								cmd.Parameters.AddWithValue("@NameItem", product.NameItem ?? (object)DBNull.Value);
								cmd.Parameters.AddWithValue("@Quantity", 0);
								cmd.Parameters.AddWithValue("@QuantityType", product.QuantityType ?? (object)DBNull.Value);
								cmd.Parameters.AddWithValue("@PKWiU", product.PKWiU ?? (object)DBNull.Value);
								cmd.Parameters.AddWithValue("@NettoPrice", product.NettoPrice);
								cmd.Parameters.AddWithValue("@NettoValue", product.NettoValue);
								cmd.Parameters.AddWithValue("@VATPercent", product.VATPercent ?? (object)DBNull.Value);
								cmd.Parameters.AddWithValue("@VATValue", product.VATValue);
								cmd.Parameters.AddWithValue("@BruttoValue", product.BruttoValue);
								cmd.Parameters.AddWithValue("@ShowIt", product.ShowIt);


								con.Open();
								cmd.ExecuteNonQuery();
								NotifyProductsUpdated();
                        }
                    }
						catch(SqlException ex) 
						{
							MessageBox.Show("Błąd dodawania produktu do bazy danych: " + ex.Message, "Błąd bazy danych", MessageBoxButton.OK, MessageBoxImage.Error);
						}
					}
				}
				catch(SqlException ex) 
				{
					MessageBox.Show("Błąd połączenia z bazą danych: " + ex.Message, "Błąd bazy danych", MessageBoxButton.OK, MessageBoxImage.Error);
				}
		}

		public static async Task GetAllFromDataBaseAsync()
		{
			using (SqlConnection con = new SqlConnection(connectionString)) 
			{
				Products = new ObservableCollection<Product>();
				string query = "SELECT * FROM Product WHERE ShowIt = 1";
				await con.OpenAsync();
				using (SqlCommand cmd = new SqlCommand(query, con)) 
				{
					SqlDataReader reader = await cmd.ExecuteReaderAsync();

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
						Products.Add(product);
					}
				}
			}
		}

		public static ObservableCollection<Product> GetProductsPage(int currentPage)
		{
			int startIndex = (currentPage - 1) * PageSize;
			int endIndex = Math.Min(startIndex + PageSize, Products.Count);
			var pageProducts = new ObservableCollection<Product>();

			for (int i = startIndex; i < endIndex; i++)
			{
				if (i >= 0 && i < Products.Count)
				{
					pageProducts.Add(Products[i]);
				}
			}

			return pageProducts;
		}

        public static ObservableCollection<Product> GetProductsPage(int currentPage, int PageSize)
        {
            int startIndex = (currentPage - 1) * PageSize;
            int endIndex = Math.Min(startIndex + PageSize, Products.Count);
            var pageProducts = new ObservableCollection<Product>();

            for (int i = startIndex; i < endIndex; i++)
            {
                if (i >= 0 && i < Products.Count)
                {
                    pageProducts.Add(Products[i]);
                }
            }

            return pageProducts;
        }

        public static int GetTotalPages()
		{
			return (int)Math.Ceiling((double)Products.Count / PageSize);
		}

		public static void DeleteProduct(int productId)
		{
			using (SqlConnection con = new SqlConnection(connectionString))
			{
				string query = "DELETE FROM Product WHERE ProductID = @ProductId";
				con.Open();

				using (SqlCommand cmd = new SqlCommand(query, con))
				{
					cmd.Parameters.AddWithValue("@ProductId", productId);
					cmd.ExecuteNonQuery();
                    NotifyProductsUpdated();
                }
            }
		}

		public static decimal CalculateValueVAT(decimal nettoPrice, decimal vatPercent)
		{
			decimal ValueVat = nettoPrice * (vatPercent / 100);

			return Math.Round(ValueVat, 2, MidpointRounding.AwayFromZero);
		}

		public static decimal CalculateValueBrutto(decimal nettoPrice, decimal vatValue)
		{
			decimal ValueBrutto = nettoPrice + vatValue;

			return Math.Round(ValueBrutto, 2, MidpointRounding.AwayFromZero);
		}

		public static ObservableCollection<Product> FilterProducts(string filterText)
		{
			var filteredProducts = new ObservableCollection<Product> ();
			
			foreach(var product in Products)
			{
				if (product.NameItem.Contains(filterText, StringComparison.OrdinalIgnoreCase) || product.NettoPrice.ToString().Contains(filterText, StringComparison.OrdinalIgnoreCase))
					{
					filteredProducts.Add (product);
				}
			}

			return filteredProducts;
		}

		public static void InitializeEditWindow(ref bool ed, ref dksApp.Product p, MainWindow mainW)
		{
			ed = true;
			ProductPage editetProduct = new ProductPage(ref ed, ref p, mainW);
			mainW.MainContentFrame.Navigate(editetProduct);
		}

		public static void UpdateProductDataBase(dksApp.Product editedProduct)
		{
			try
			{
				using (SqlConnection con = new SqlConnection(connectionString))
				{
					string query = @"UPDATE Product SET 
                             NumberOfItems = @NumberOfItems, 
                             NameItem = @NameItem, 
                             Quantity = @Quantity, 
                             QuantityType = @QuantityType, 
                             PKWiU = @PKWiU, 
                             NettoPrice = @NettoPrice, 
                             NettoValue = @NettoValue, 
                             VATPercent = @VATPercent, 
                             VATValue = @VATValue, 
                             BruttoValue = @BruttoValue, 
                             ShowIt = @ShowIt 
                             WHERE ProductID = @ProductId";

					using (SqlCommand cmd = new SqlCommand(query, con))
					{
					
						cmd.Parameters.AddWithValue("@ProductId", editedProduct.IdProduct);
						cmd.Parameters.AddWithValue("@NumberOfItems", editedProduct.NumberOfItems);
						cmd.Parameters.AddWithValue("@NameItem", editedProduct.NameItem ?? (object)DBNull.Value);
						cmd.Parameters.AddWithValue("@Quantity", editedProduct.Quantity);
						cmd.Parameters.AddWithValue("@QuantityType", editedProduct.QuantityType ?? (object)DBNull.Value);
						cmd.Parameters.AddWithValue("@PKWiU", editedProduct.PKWiU ?? (object)DBNull.Value);
						cmd.Parameters.AddWithValue("@NettoPrice", editedProduct.NettoPrice);
						cmd.Parameters.AddWithValue("@NettoValue", editedProduct.NettoValue);
						cmd.Parameters.AddWithValue("@VATPercent", editedProduct.VATPercent ?? (object)DBNull.Value);
						cmd.Parameters.AddWithValue("@VATValue", editedProduct.VATValue);
						cmd.Parameters.AddWithValue("@BruttoValue", editedProduct.BruttoValue);
						cmd.Parameters.AddWithValue("@ShowIt", editedProduct.ShowIt);

						con.Open();
						cmd.ExecuteNonQuery();
                        NotifyProductsUpdated();
                    }
                }
			}
			catch (SqlException ex)
			{
				MessageBox.Show("Błąd aktualizacji produktu w bazie danych: " + ex.Message, "Błąd bazy danych", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

	}
}
