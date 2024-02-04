using System;
using dksApp.Contractors.AddContractorsW;
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
            if (CurrentPage < ContractorsServiceDataGrid.GetTotalPages())
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
            int totalItems = ContractorsServiceDataGrid.GetTotalPages();
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
            GeneratePaginationButtons();
        }

        private void UpdateAmountOfBuyers()
        {
            NumberOfContractors.Text = $"Liczba kontrahentów: {ContractorsServiceDataGrid.BuyerList.Count}";
        }

        private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filterText = txtFilter.Text;

            if (filterText.Length > 0 ) 
            {
                ContractorsDataGrid.ItemsSource = ContractorsServiceDataGrid.FilterContractors(filterText);
            }
            else
            {
                UpdateDisplayedBuyers();
            }

        }

        private async void DeleteContractorBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedContractor = (Buyer)ContractorsDataGrid.SelectedItem;

            if (selectedContractor != null)
            {
                MessageBoxResult result = MessageBox.Show($"Czy aby na pewno chcesz usunąć nabywcę o id: {selectedContractor.IdBuyer}?", "Potwierdź wybór", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    ContractorsServiceDataGrid.DeleteContractor(selectedContractor.IdBuyer);

                    await ContractorsServiceDataGrid.GetBuyersFromDataBase();
                }
                else
                {
                    return;
                }
            }
            else
            {
                MessageBox.Show("Wybierz nabywcę do usunięcia!", "Błąd usuwania", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            await AsyncInitialize();
        }

        private async void DeleteContractorsBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedContractors = ContractorsDataGrid.Items.Cast<Buyer>().ToList().Where(p => p.IsSelected).ToList();

            foreach(Buyer buyer in selectedContractors)
            {
                MessageBoxResult result = MessageBox.Show($"Czy aby na pewno chcesz usunąc nabywcę o ID: {buyer.IdBuyer}?", "Potwierdź wybór", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if(result == MessageBoxResult.Yes)
                {
                    ContractorsServiceDataGrid.DeleteContractor(buyer.IdBuyer);
                }
                else
                {
                    MessageBox.Show($"Anulowano usunięcie produktu o ID: {buyer.IdBuyer}", "Usuwanie nie powiodło się!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            await AsyncInitialize();
        }

        private async void EditContractorBtn_Click(object sender, RoutedEventArgs e) 
        {
            var selectedContractor = (Buyer)ContractorsDataGrid.SelectedItem;

            if(selectedContractor != null) 
            {
                AddBuyerW editContractor = new AddBuyerW(selectedContractor);
                editContractor.ShowDialog();
            }
            else
            {
                MessageBox.Show("Wybierz nabywcę do edycji!", "Błąd wyboru", MessageBoxButton.OK, MessageBoxImage.Error);
            }

           await AsyncInitialize();
        }
    }
}
