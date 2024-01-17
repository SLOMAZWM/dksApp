using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.ComponentModel;
using System.Collections;
using dksApp.Services;

namespace dksApp.Bookkeeping
{
	/// <summary>
	/// Interaction logic for MainDataGrid.xaml
	/// </summary>
	public partial class MainDataGrid : Page, INotifyPropertyChanged
	{
		private ObservableCollection<InvoiceClass> Invoices;
		private ObservableCollection<InvoiceClass> DisplayedInvoices;
		private int selectedInvoice = 0;
		private int CurrentPage;
		private int PageSize = 7;
		private uint allDocuments;
		public int InvoiceId { get; set; }
		public uint AllDocuments
		{
			get { return allDocuments; }
			set
			{
				if (allDocuments != value)
				{
					allDocuments = value;
					OnPropertyChanged(nameof(AllDocuments));
				}
			}
		}


		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged(string name)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}

		public MainDataGrid()
		{
			InitializeComponent();
			DeleteSelectedInvoicesBtn.Visibility = Visibility.Collapsed;
			this.DataContext = this;
			Invoices = new ObservableCollection<InvoiceClass>();
			DisplayedInvoices = new ObservableCollection<InvoiceClass>();
			BookKeepingDataGrid.ItemsSource = DisplayedInvoices;
			InitializeAllInvoices();

			CurrentPage = 1;
			UpdateDisplayedInvoices();
			txtFilter.TextChanged += TxtFilter_TextChanged;
		}


		private void InitializeAllInvoices()
		{
			string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;
			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					conn.Open();
					string query = "SELECT InvoiceID, SellerName, BuyerName, PaymentType, Paid, IssueDate FROM Invoice ORDER BY IssueDate DESC";

					try
					{
						using (SqlCommand cmd = new SqlCommand(query, conn))
						{
							using (SqlDataReader reader = cmd.ExecuteReader())
							{
								while (reader.Read())
								{
									var invoice = new InvoiceClass
									{
										IDInvoice = (uint)reader.GetInt32(reader.GetOrdinal("InvoiceID")),
										SellerName = reader.IsDBNull(reader.GetOrdinal("SellerName")) ? null : reader.GetString(reader.GetOrdinal("SellerName")),
										BuyerName = reader.IsDBNull(reader.GetOrdinal("BuyerName")) ? null : reader.GetString(reader.GetOrdinal("BuyerName")),
										PaymentType = reader.IsDBNull(reader.GetOrdinal("PaymentType")) ? null : reader.GetString(reader.GetOrdinal("PaymentType")),
										Paid = reader.IsDBNull(reader.GetOrdinal("Paid")) ? 0 : reader.GetDecimal(reader.GetOrdinal("Paid")),
										IssueDate = reader.IsDBNull(reader.GetOrdinal("IssueDate")) ? null : reader.GetString(reader.GetOrdinal("IssueDate"))
									};
									Invoices.Add(invoice);
									AllDocuments++;
								}
							}
						}
					}
					catch (SqlException ex)
					{
						MessageBox.Show("Błąd odczytu z bazy danych!" + ex.Message, "Błąd bazy danych!", MessageBoxButton.OK, MessageBoxImage.Error);
					}

					UpdatePaginationButtons();
				}
			}
			catch (SqlException ex)
			{
				MessageBox.Show("Błąd połączenia z bazą danych!" + ex.Message, "Błąd bazy danych!", MessageBoxButton.OK, MessageBoxImage.Error);
			}

		}

		private void UpdatePaginationButtons()
		{
			int maxPage = (int)Math.Ceiling((double)Invoices.Count / PageSize);
			PaginationItemsControl.Items.Clear();

			int halfRange = 3;
			int startPage = Math.Max(1, CurrentPage - halfRange);
			int endPage = Math.Min(maxPage, CurrentPage + halfRange);

			if (endPage - startPage < 5)
			{
				if (startPage > 1) startPage = Math.Max(1, endPage - 5);
				else endPage = Math.Min(maxPage, startPage + 5);
			}

			for (int pageNumber = startPage; pageNumber <= endPage; pageNumber++)
			{
				var button = new Button
				{
					Content = pageNumber.ToString(),
					Style = FindResource("pagingButton") as Style
				};
				button.Click += PageButton_Click;

				PaginationItemsControl.Items.Add(button);
			}

			UpdatePaginationButtonStyles();
		}


		private void UpdateDisplayedInvoices()
		{
			int startIndex = (CurrentPage - 1) * PageSize;
			int endIndex = Math.Min(startIndex + PageSize, Invoices.Count);

			DisplayedInvoices.Clear();

			for (int i = startIndex; i < endIndex; i++)
			{
				DisplayedInvoices.Add(Invoices[i]);
			}

			UpdatePaginationButtonStyles();
			UpdatePaginationButtons();

		}

		private void NextPageButton_Click(object sender, RoutedEventArgs e)
		{
			int maxPage = (int)Math.Ceiling((double)Invoices.Count / PageSize);
			if (CurrentPage < maxPage)
			{
				CurrentPage++;
				UpdateDisplayedInvoices();
			}
		}

		private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
		{
			if (CurrentPage > 1)
			{
				CurrentPage--;
				UpdateDisplayedInvoices();
			}
		}

		private void UpdatePaginationButtonStyles()
		{
			foreach (UIElement element in PaginationItemsControl.Items)
			{
				if (element is Button button)
				{
					int pageNumber;
					if (int.TryParse(button.Content?.ToString(), out pageNumber))
					{
						Brush actuallyButtonBackground = new SolidColorBrush(Color.FromRgb(0x79, 0x50, 0xF2));
						Brush normalButtonForeground = new SolidColorBrush(Color.FromRgb(0x6C, 0x76, 0x82));

						if (pageNumber == CurrentPage)
						{
							button.Background = actuallyButtonBackground;
							button.Foreground = Brushes.White;
						}
						else
						{
							button.Background = Brushes.Transparent;
							button.Foreground = normalButtonForeground;
						}
					}
				}
			}
		}

		private void PageButton_Click(object sender, RoutedEventArgs e)
		{
			if (sender is Button button && int.TryParse(button.Content?.ToString(), out int pageNumber))
			{
				CurrentPage = pageNumber;
				UpdateDisplayedInvoices();
			}
		}

		//HEADER CHECKBOX ALL SELECTED IN DATAGRID
		private void CheckBoxHeader_Click(object sender, RoutedEventArgs e)
		{
			var checkBoxHeader = (CheckBox)sender;
			bool isChecked = checkBoxHeader.IsChecked ?? false;

			foreach (var invoice in DisplayedInvoices)
			{
				invoice.IsSelected = isChecked;
			}
		}

		//FILTER

		private void TxtFilter_TextChanged(object sender, TextChangedEventArgs e)
		{
			FilterInvoices();
		}

		private void FilterInvoices()
		{
			var filterText = txtFilter.Text?.ToLower() ?? string.Empty;
			var filteredInvoices = string.IsNullOrWhiteSpace(filterText)
				? Invoices
				: new ObservableCollection<InvoiceClass>(Invoices.Where(invoice =>
					invoice.SellerName?.ToLower().Contains(filterText) == true ||
					invoice.BuyerName?.ToLower().Contains(filterText) == true ||
					invoice.IDInvoice.ToString().Contains(filterText) ||
					invoice.PaymentType?.ToLower().Contains(filterText) == true ||
					invoice.PaymentDate?.ToLower().Contains(filterText) == true ||
					invoice.Paid.ToString().Contains(filterText) ||
					invoice.IssueDate?.ToLower().Contains(filterText) == true
				));

			DisplayedInvoices.Clear();
			foreach (var invoice in filteredInvoices)
			{
				DisplayedInvoices.Add(invoice);
			}

			AllDocuments = (uint)filteredInvoices.Count;
		}

		private void GridRemoveButton_Click(object sender, RoutedEventArgs e)
		{
			if (BookKeepingDataGrid.SelectedItem is InvoiceClass selectedInvoice)
			{
				MessageBoxResult result = MessageBox.Show($"Czy na pewno chcesz usunąć fakturę o ID: {selectedInvoice.IDInvoice}?", "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Question);

				if (result == MessageBoxResult.Yes)
				{
					var invoiceService = new InvoiceDataGridService(ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString);
					invoiceService.DeleteInvoiceWithProducts((int)selectedInvoice.IDInvoice);

					Invoices.Remove(selectedInvoice);
					DisplayedInvoices.Remove(selectedInvoice);

					AllDocuments = (uint)Invoices.Count;
					UpdateDisplayedInvoices();
				}
			}
			else
			{
				MessageBox.Show("Proszę wybrać fakturę do usunięcia.");
			}
		}

		private void DeleteSelectedInvoices_Click(object sender, RoutedEventArgs e)
		{
			var selectedInvoices = DisplayedInvoices.Where(invoice => invoice.IsSelected).ToList();

			if (selectedInvoices.Count <= 1)
			{
				MessageBox.Show("Proszę wybrać co najmniej dwie faktury do usunięcia.");
				return;
			}
			else
			{
				MessageBoxResult confirmation = MessageBox.Show($"Czy na pewno chcesz usunąć {selectedInvoices.Count} faktur?", "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Question);

				if (confirmation == MessageBoxResult.Yes) 
				{
					var invoiceService = new InvoiceDataGridService(ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString);

					foreach(var selectedInvoice in selectedInvoices) 
					{
						invoiceService.DeleteInvoiceWithProducts((int)selectedInvoice.IDInvoice);
						DisplayedInvoices.Remove(selectedInvoice);

						Invoices.Remove(selectedInvoice);
						DisplayedInvoices.Remove(selectedInvoice);

						AllDocuments = (uint)Invoices.Count;
						UpdateDisplayedInvoices();
					}
					
				}
			}
		}

		private void FirstPageButton_Click(object sender, RoutedEventArgs e) 
		{
			CurrentPage = 1;
			UpdateDisplayedInvoices();
		}

		private void LastPageButton_Click(object sender, RoutedEventArgs e)
		{
			int totalItems = Invoices.Count;
			int maxPage = (int)Math.Ceiling((double)totalItems / PageSize);

			CurrentPage = maxPage;
			UpdateDisplayedInvoices();
		}

		private void UpdateDeleteButtonVisibility()
		{
			int selectedCount = DisplayedInvoices.Count(invoice => invoice.IsSelected);
			DeleteSelectedInvoicesBtn.Visibility = selectedCount >= 2 ? Visibility.Visible : Visibility.Collapsed;
		}

		private void CheckBox_Checked(object sender, RoutedEventArgs e)
		{
			selectedInvoice++;

			UpdateDeleteButtonVisibility();
		}

		private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
		{
			selectedInvoice--;

			UpdateDeleteButtonVisibility();
		}

		private void GridEditButton_Click(object sender, RoutedEventArgs e)
		{
			var selectedProduct = BookKeepingDataGrid.SelectedItem;

			//Get the product from database
		}
	}
}
