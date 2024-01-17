using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

		public void DeleteInvoiceWithProducts(int invoiceId)
		{
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				using (SqlTransaction transaction = connection.BeginTransaction())
				{
					try
					{
						List<int> productIds = GetProductIds(invoiceId, connection, transaction);

						DeleteInvoiceProducts(invoiceId, connection, transaction);

						DeleteProducts(productIds, connection, transaction);

	
						DeleteInvoice(invoiceId, connection, transaction);

						transaction.Commit();
					}
					catch
					{
						transaction.Rollback();
						throw;
					}
				}
			}
		}

	}
}
