using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;

namespace dksApp.Services
{
	public class InvoiceDataGridService
	{
		private string connectionString;

		public InvoiceDataGridService(string connectionString)
		{
			this.connectionString = connectionString;
		}

		public void DeleteInvoiceWithProducts(int invoiceId)
		{
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				SqlTransaction transaction = connection.BeginTransaction();

				try
				{
					List<int> productIds = GetProductIds(invoiceId, connection, transaction);

					DeleteInvoiceProducts(invoiceId, connection, transaction);

					DeleteProducts(productIds, connection, transaction);

					DeleteInvoice(invoiceId, connection, transaction);

					transaction.Commit();
					MessageBox.Show("Faktura i powiązane produkty zostały usunięte.");
				}
				catch (SqlException ex)
				{
					transaction.Rollback();
					MessageBox.Show("Wystąpił błąd podczas usuwania faktury i powiązanych produktów: " + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
		}

		private List<int> GetProductIds(int invoiceId, SqlConnection connection, SqlTransaction transaction)
		{
			List<int> productIds = new List<int>();
			string selectProductIdsQuery = "SELECT ProductID FROM InvoiceProducts WHERE InvoiceID = @InvoiceID";
			using (SqlCommand command = new SqlCommand(selectProductIdsQuery, connection, transaction))
			{
				command.Parameters.AddWithValue("@InvoiceID", invoiceId);
				using (SqlDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						productIds.Add(reader.GetInt32(0));
					}
				}
			}

			return productIds;
		}

		private void DeleteInvoiceProducts(int invoiceId, SqlConnection connection, SqlTransaction transaction)
		{
			string deleteInvoiceProductsQuery = "DELETE FROM InvoiceProducts WHERE InvoiceID = @InvoiceID";
			using (SqlCommand command = new SqlCommand(deleteInvoiceProductsQuery, connection, transaction))
			{
				command.Parameters.AddWithValue("@InvoiceID", invoiceId);
				command.ExecuteNonQuery();
			}
		}

		private void DeleteProducts(List<int> productIds, SqlConnection connection, SqlTransaction transaction)
		{
			foreach (int productId in productIds)
			{
				string deleteProductQuery = "DELETE FROM Product WHERE ProductID = @ProductID";
				using (SqlCommand command = new SqlCommand(deleteProductQuery, connection, transaction))
				{
					command.Parameters.AddWithValue("@ProductID", productId);
					command.ExecuteNonQuery();
				}
			}
		}

		private void DeleteInvoice(int invoiceId, SqlConnection connection, SqlTransaction transaction)
		{
			string deleteInvoiceQuery = "DELETE FROM Invoice WHERE InvoiceID = @InvoiceID";
			using (SqlCommand command = new SqlCommand(deleteInvoiceQuery, connection, transaction))
			{
				command.Parameters.AddWithValue("@InvoiceID", invoiceId);
				command.ExecuteNonQuery();
			}
		}

		public InvoiceClass GetInvoiceWithProducts(uint invoiceId)
		{
			InvoiceClass invoice = new InvoiceClass();

			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();

				string invoiceQuery = "SELECT * FROM Invoice WHERE InvoiceID = @InvoiceID";
				using (SqlCommand command = new SqlCommand(invoiceQuery, connection))
				{
					command.Parameters.AddWithValue("@InvoiceID", (int)invoiceId);
					using (SqlDataReader reader = command.ExecuteReader())
					{
						if (reader.Read())
						{
							invoice.IDInvoice = (uint)reader.GetInt32(reader.GetOrdinal("InvoiceID"));
							invoice.IssueDate = reader.IsDBNull(reader.GetOrdinal("IssueDate")) ? null : reader.GetString(reader.GetOrdinal("IssueDate"));
							invoice.ExecutionDate = reader.IsDBNull(reader.GetOrdinal("ExecutionDate")) ? null : reader.GetString(reader.GetOrdinal("ExecutionDate"));
							invoice.PaymentType = reader.IsDBNull(reader.GetOrdinal("PaymentType")) ? null : reader.GetString(reader.GetOrdinal("PaymentType"));
							invoice.PaymentDate = reader.IsDBNull(reader.GetOrdinal("PaymentDate")) ? null : reader.GetString(reader.GetOrdinal("PaymentDate"));
							invoice.Paid = reader.IsDBNull(reader.GetOrdinal("Paid")) ? 0 : reader.GetDecimal(reader.GetOrdinal("Paid"));
							invoice.PaidYet = reader.IsDBNull(reader.GetOrdinal("PaidYet")) ? 0 : reader.GetDecimal(reader.GetOrdinal("PaidYet"));
							invoice.IdSeller = reader.IsDBNull(reader.GetOrdinal("IdSeller")) ? 0 : (uint)reader.GetInt32(reader.GetOrdinal("IdSeller"));
							invoice.SellerName = reader.IsDBNull(reader.GetOrdinal("SellerName")) ? null : reader.GetString(reader.GetOrdinal("SellerName"));
							invoice.SellerStreet = reader.IsDBNull(reader.GetOrdinal("SellerStreet")) ? null : reader.GetString(reader.GetOrdinal("SellerStreet"));
							invoice.SellerCity = reader.IsDBNull(reader.GetOrdinal("SellerCity")) ? null : reader.GetString(reader.GetOrdinal("SellerCity"));
							invoice.SellerZipCode = reader.IsDBNull(reader.GetOrdinal("SellerZipCode")) ? null : reader.GetString(reader.GetOrdinal("SellerZipCode"));
							invoice.SellerNIP = reader.IsDBNull(reader.GetOrdinal("SellerNIP")) ? null : reader.GetString(reader.GetOrdinal("SellerNIP"));
							invoice.SellerBankName = reader.IsDBNull(reader.GetOrdinal("SellerBankName")) ? null : reader.GetString(reader.GetOrdinal("SellerBankName"));
							invoice.SellerBankAccount = reader.IsDBNull(reader.GetOrdinal("SellerBankAccount")) ? null : reader.GetString(reader.GetOrdinal("SellerBankAccount"));
							invoice.Comments = reader.IsDBNull(reader.GetOrdinal("Comments")) ? null : reader.GetString(reader.GetOrdinal("Comments"));
							invoice.BuyerName = reader.IsDBNull(reader.GetOrdinal("BuyerName")) ? null : reader.GetString(reader.GetOrdinal("BuyerName"));
							invoice.BuyerStreet = reader.IsDBNull(reader.GetOrdinal("BuyerStreet")) ? null : reader.GetString(reader.GetOrdinal("BuyerStreet"));
							invoice.BuyerCity = reader.IsDBNull(reader.GetOrdinal("BuyerCity")) ? null : reader.GetString(reader.GetOrdinal("BuyerCity"));
							invoice.BuyerZipCode = reader.IsDBNull(reader.GetOrdinal("BuyerZipCode")) ? null : reader.GetString(reader.GetOrdinal("BuyerZipCode"));
							invoice.BuyerNIP = reader.IsDBNull(reader.GetOrdinal("BuyerNIP")) ? null : reader.GetString(reader.GetOrdinal("BuyerNIP"));
							invoice.From = reader.IsDBNull(reader.GetOrdinal("InvoiceFrom")) ? null : reader.GetString(reader.GetOrdinal("InvoiceFrom"));
						}
					}
				}

				string productsQuery = @"SELECT p.* FROM Product p 
                                    INNER JOIN InvoiceProducts ip ON p.ProductID = ip.ProductID 
                                    WHERE ip.InvoiceID = @InvoiceID";
				using (SqlCommand command = new SqlCommand(productsQuery, connection))
				{
					command.Parameters.AddWithValue("@InvoiceID", (int)invoiceId);
					using (SqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							Product product = new Product
							{
								IdProduct = reader.GetInt32(reader.GetOrdinal("ProductID")),
								NumberOfItems = reader.IsDBNull(reader.GetOrdinal("NumberOfItems")) ? 0 : reader.GetInt32(reader.GetOrdinal("NumberOfItems")),
								NameItem = reader.IsDBNull(reader.GetOrdinal("NameItem")) ? null : reader.GetString(reader.GetOrdinal("NameItem")),
								Quantity = reader.IsDBNull(reader.GetOrdinal("Quantity")) ? 0 : reader.GetInt64(reader.GetOrdinal("Quantity")),
								QuantityType = reader.IsDBNull(reader.GetOrdinal("QuantityType")) ? null : reader.GetString(reader.GetOrdinal("QuantityType")),
								PKWiU = reader.IsDBNull(reader.GetOrdinal("PKWiU")) ? null : reader.GetString(reader.GetOrdinal("PKWiU")),
								NettoPrice = reader.IsDBNull(reader.GetOrdinal("NettoPrice")) ? 0 : reader.GetDecimal(reader.GetOrdinal("NettoPrice")),
								NettoValue = reader.IsDBNull(reader.GetOrdinal("NettoValue")) ? 0 : reader.GetDecimal(reader.GetOrdinal("NettoValue")),
								VATPercent = reader.IsDBNull(reader.GetOrdinal("VATPercent")) ? null : reader.GetString(reader.GetOrdinal("VATPercent")),
								VATValue = reader.IsDBNull(reader.GetOrdinal("VATValue")) ? 0 : reader.GetDecimal(reader.GetOrdinal("VATValue")),
								BruttoValue = reader.IsDBNull(reader.GetOrdinal("BruttoValue")) ? 0 : reader.GetDecimal(reader.GetOrdinal("BruttoValue")),
								ShowIt = reader.IsDBNull(reader.GetOrdinal("ShowIt")) ? false : reader.GetBoolean(reader.GetOrdinal("ShowIt"))
							};
							invoice.Products.Add(product);
						}
					}
				}
			}

			return invoice;
		}
	}
}
