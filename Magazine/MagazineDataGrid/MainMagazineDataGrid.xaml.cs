using System;
using dksApp;
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
using dksApp.Contractors;


namespace dksApp.Magazine.MagazineDataGrid
{
	/// <summary>
	/// Interaction logic for MainMagazineDataGrid.xaml
	/// </summary>
	public partial class MainMagazineDataGrid : Page
	{
		private MainWindow _mainWindow;
		private int CurrentPage;
		private List<dksApp.Product> products = new List<dksApp.Product>();
		public static MainMagazineDataGrid Instance { get; private set; }


		public MainMagazineDataGrid(MainWindow mainWindow)
		{
			InitializeComponent();
			InitializeAsync();
			ProductDataGrid.ItemsSource = ProductServiceDataGrid.Products;

			DeleteProductsBtn.Visibility = Visibility.Collapsed;

			Instance = this;
			CurrentPage = 1;
			_mainWindow = mainWindow;
		}

		public async Task InitializeAsync()
		{
			await ProductServiceDataGrid.GetAllFromDataBaseAsync();
			UpdateDisplayedProducts();
			GeneratePaginationButtons();
			UpdatePaginationButtonStyles();
            ProductServiceDataGrid.Products.CollectionChanged += Products_CollectionChanged;
		}

		private void UpdateDisplayedProducts()
		{
			var displayedProducts = ProductServiceDataGrid.GetProductsPage(CurrentPage);
			ProductDataGrid.ItemsSource = displayedProducts;
			UpdatePaginationButtonStyles();
			UpdateAmountOfItems();
			GeneratePaginationButtons();
		}

		private void Products_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			UpdateAmountOfItems();
		}

		private void UpdateAmountOfItems()
		{
			NumberOfItems.Text = $"Liczba produktów: {ProductServiceDataGrid.Products.Count}";
		}

		private void NextPageButton_Click(object sender, RoutedEventArgs e)
		{
			if (CurrentPage < ProductServiceDataGrid.GetTotalPages())
			{
				CurrentPage++;
				UpdateDisplayedProducts();
			}
		}

		private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
		{
			if (CurrentPage > 1)
			{
				CurrentPage--;
				UpdateDisplayedProducts();
			}
		}
		private void FirstPageButton_Click(object sender, RoutedEventArgs e)
		{
			CurrentPage = 1;
			UpdateDisplayedProducts();
		}

		private void LastPageButton_Click(object sender, RoutedEventArgs e)
		{
			int totalItems = ProductServiceDataGrid.GetTotalPages();
			CurrentPage = totalItems;
			UpdateDisplayedProducts();
		}

		private void PageButton_Click(object sender, RoutedEventArgs e)
		{
			if (sender is Button pageButton && int.TryParse(pageButton.Content.ToString(), out int pageNumber))
			{
				CurrentPage = pageNumber;
				UpdateDisplayedProducts();
			}
		}

		private void GeneratePaginationButtons()
		{
            int totalPages = ProductServiceDataGrid.GetTotalPages();
            PaginationItemsControl.Items.Clear();

            int halfRange = 3;
            int startPage = Math.Max(1, CurrentPage - halfRange);
            int endPage = Math.Min(totalPages, CurrentPage + halfRange);

            if (endPage - startPage < 5)
            {
                if (startPage > 1) startPage = Math.Max(1, endPage - 5);
                else endPage = Math.Min(totalPages, startPage + 5);
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

		private async void GridRemoveButton_Click(object sender, RoutedEventArgs e)
		{
			var selectedProduct = (dksApp.Product)ProductDataGrid.SelectedItem;

			if (selectedProduct != null)
			{
				MessageBoxResult result = MessageBox.Show($"Czy na pewno chcesz usunąć produkt o id: {selectedProduct.IdProduct}?", "Potwierdź wybór", MessageBoxButton.YesNo, MessageBoxImage.Question);

				if (result == MessageBoxResult.Yes)
				{
					ProductServiceDataGrid.DeleteProduct(selectedProduct.IdProduct);
				}
				else
				{
					return;
				}
			}
			else
			{
				MessageBox.Show("Wybierz produkt do usunięcia!", "Błąd usuwania", MessageBoxButton.OK, MessageBoxImage.Error);
			}

			await InitializeAsync();
		}

		private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
		{
			var filterText = txtFilter.Text;

			if(filterText.Length > 0) 
			{
                ProductDataGrid.ItemsSource = ProductServiceDataGrid.FilterProducts(filterText);
            }
			else
			{
				UpdateDisplayedProducts();
			}
		}

		private void CheckBox_Checked(object sender, RoutedEventArgs e) 
		{
			var selectedProduct = ProductDataGrid.SelectedItem;
			products.Add((dksApp.Product)selectedProduct);
			UpdateDeleteButtonVisibility();
		}

		private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
		{
			var unSelectedProduct = ProductDataGrid.SelectedItem;
			products.Remove((dksApp.Product)unSelectedProduct);
			UpdateDeleteButtonVisibility();
		}

		private void UpdateDeleteButtonVisibility()
		{
			DeleteProductsBtn.Visibility = products.Count >= 2 ? Visibility.Visible : Visibility.Collapsed;
		}

		private async void DeleteProducts_Click(object sender, RoutedEventArgs e)
		{
			var selectedProducts = ProductDataGrid.Items.Cast<dksApp.Product>().ToList().Where(p => p.IsSelected).ToList();

			foreach(dksApp.Product product in selectedProducts)
			{
				MessageBoxResult result = MessageBox.Show($"Czy aby na pewno chcesz usunąć produkt o ID: {product.IdProduct}", "Potwierdzenie usunięcia", MessageBoxButton.YesNo, MessageBoxImage.Question);
				if (result == MessageBoxResult.Yes) 
				{
					ProductServiceDataGrid.DeleteProduct(product.IdProduct);
				}
				else
				{
					MessageBox.Show($"Anulowano usunięcie produktu o ID: {product.IdProduct}", "Usuwanie nie powiodło się!", MessageBoxButton.OK, MessageBoxImage.Warning);
				}
			}
            await InitializeAsync();
		}

		private void ProductDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			UpdateDeleteButtonVisibility();
		}

		private void CheckBoxHeader_Click(object sender, RoutedEventArgs e)
		{
			if(sender is CheckBox checkBox) 
			{
				if(checkBox.IsChecked == true) 
				{
					foreach (dksApp.Product product in ProductDataGrid.Items)
					{
						product.IsSelected = true;
					}
				}
				else
				{
					foreach (dksApp.Product product in ProductDataGrid.Items)
					{
						product.IsSelected = false;
					}
				}
			}
		}

		private void GridEditButton_Click(object sender, RoutedEventArgs e) 
		{
			if(ProductDataGrid.SelectedItem != null) 
			{
				dksApp.Product selectedProduct = (dksApp.Product)ProductDataGrid.SelectedItem;
				bool edited = false;
				ProductServiceDataGrid.InitializeEditWindow(ref edited, ref selectedProduct, _mainWindow);
			}
			else
			{
				MessageBox.Show("Wybierz produkt do edycji!", "Błąd edycji", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
    }
}
