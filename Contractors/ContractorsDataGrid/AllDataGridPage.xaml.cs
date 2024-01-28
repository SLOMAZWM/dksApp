using System;
using System.Collections.Generic;
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

namespace dksApp.Contractors
{
    /// <summary>
    /// Interaction logic for AllDataGridPage.xaml
    /// </summary>
    public partial class AllDataGridPage : Page
    {
        private int CurrentPage;
		private List<Buyer> BuyerList = new List<Buyer>();

        public AllDataGridPage()
        {
            InitializeComponent();
			DeleteContractorBtn.Visibility = Visibility.Collapsed;
			AsyncInitialize();

			ContractorsDataGrid.ItemsSource = ContractorsServiceDataGrid.BuyerList;

            CurrentPage = 1;
        }

        public async Task AsyncInitialize()
        {
            await ContractorsServiceDataGrid.GetBuyersFromDataBase();
			UpdateDisplayedBuyers();
			GeneratePaginationButtons();
			UpdatePaginationButtonStyles();
			
        }

		private void Contractors_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			UpdateAmountOfBuyers();
		}

		private void GeneratePaginationButtons()
        {
            int totalPages = ContractorsServiceDataGrid.GetTotalPages();
            PaginationItemsControl.Items.Clear();
            
            for (int i = 1; i<= totalPages; i++) 
            {
                var pageButton = new Button
                {
                    Content = i.ToString(),
                    Style = this.FindResource("pagingButton") as Style
                };
                pageButton.Click += PageButton_Click;
                PaginationItemsControl.Items.Add(pageButton);
            }
        }

		private void ContractorsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			UpdateDeleteButtonVisibility();
		}

		private void CheckBox_Checked(object sender, RoutedEventArgs e)
		{
			var selectedBuyer = ContractorsDataGrid.SelectedItem;
			BuyerList.Add((Buyer)selectedBuyer);
			UpdateDeleteButtonVisibility();
		}

		private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
		{
			var unSelectedBuyer = ContractorsDataGrid.SelectedItem;
			BuyerList.Remove((Buyer)unSelectedBuyer);
			UpdateDeleteButtonVisibility();
		}

		private void UpdateDeleteButtonVisibility()
		{
			DeleteContractorBtn.Visibility = BuyerList.Count >= 2 ? Visibility.Visible : Visibility.Collapsed;
		}

		private void CheckBoxHeader_Click(object sender, RoutedEventArgs e)
		{
			if (sender is CheckBox checkBox)
			{
				if (checkBox.IsChecked == true)
				{
					foreach (Buyer buyer in ContractorsDataGrid.Items)
					{
						buyer.IsSelected = true;
					}
				}
				else
				{
					foreach (Buyer buyer in ContractorsDataGrid.Items)
					{
						buyer.IsSelected = false;
					}
				}
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

		private void NextPageButton_Click(object sender, RoutedEventArgs e)
		{
			if (CurrentPage < ProductServiceDataGrid.GetTotalPages())
			{
				CurrentPage++;
				UpdateDisplayedBuyers();
			}
		}

		private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
		{
			if (CurrentPage > 1)
			{
				CurrentPage--;
				UpdateDisplayedBuyers();
			}
		}
		private void FirstPageButton_Click(object sender, RoutedEventArgs e)
		{
			CurrentPage = 1;
			UpdateDisplayedBuyers();
		}

		private void LastPageButton_Click(object sender, RoutedEventArgs e)
		{
			int totalItems = ProductServiceDataGrid.GetTotalPages();
			CurrentPage = totalItems;
			UpdateDisplayedBuyers();
		}

		private void PageButton_Click(object sender, RoutedEventArgs e)
		{
			if (sender is Button pageButton && int.TryParse(pageButton.Content.ToString(), out int pageNumber))
			{
				CurrentPage = pageNumber;
				UpdateDisplayedBuyers();
			}
		}

		private void UpdateDisplayedBuyers()
		{
			var displayedBuyers = ContractorsServiceDataGrid.GetBuyerPage(CurrentPage);
			ContractorsDataGrid.ItemsSource = displayedBuyers;

			UpdatePaginationButtonStyles();
			UpdateAmountOfBuyers();
		}

		private void UpdateAmountOfBuyers()
		{
			NumberOfContractors.Text = $"Liczba kontrahentów: {ContractorsServiceDataGrid.BuyerList.Count}";
		}
	}
}
