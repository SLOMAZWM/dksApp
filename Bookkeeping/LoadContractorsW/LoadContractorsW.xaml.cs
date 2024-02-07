using dksApp.Bookkeeping.Invoice.InvoicePages.EditPage.Products;
using dksApp.Bookkeeping.Invoice.InvoicePages;
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
using System.Windows.Shapes;
using dksApp.Contractors.AddContractorsW;
using dksApp.Contractors;
using dksApp.Bookkeeping.Invoice;
using dksApp.Bookkeeping.Invoice.InvoicePages.EditPage;

namespace dksApp.Bookkeeping.LoadContractorsW
{
    /// <summary>
    /// Interaction logic for LoadContractorsW.xaml
    /// </summary>
    public partial class LoadContractorsW : Window
    {
        PrivateBuyerPage motherPageAddInvoice { get; set; }
        CompanyBuyerPage motherCompanyPageAddInvoice { get; set; }
        BuyerEditPage motherEditPage { get; set; }
        
        private int CurrentPage;
        public bool IsAddedContractor = false;

        public LoadContractorsW(BuyerEditPage page)
        {
            InitializeComponent();
            ContractorDataGrid.ItemsSource = ContractorsServiceDataGrid.BuyerList;
            GeneratePaginationButtons();
            CurrentPage = 1;
            UpdateDisplayedContractors();

            motherEditPage = page;
        }

        public LoadContractorsW(PrivateBuyerPage page)
        {
            InitializeComponent();
            ContractorDataGrid.ItemsSource = ContractorsServiceDataGrid.BuyerList;
            GeneratePaginationButtons();
            CurrentPage = 1;
            UpdateDisplayedContractors();

            motherPageAddInvoice = page;
        }

        public LoadContractorsW(CompanyBuyerPage page)
        {
            InitializeComponent();
            ContractorDataGrid.ItemsSource = ContractorsServiceDataGrid.BuyerList;
            GeneratePaginationButtons();
            CurrentPage = 1;
            UpdateDisplayedContractors();

            motherCompanyPageAddInvoice = page;
        }

        //public LoadContractorsW(ProductsEditPage page)
        //{
        //    InitializeComponent();
        //    ContractorDataGrid.ItemsSource = ContractorsServiceDataGrid.BuyerList;
        //    GeneratePaginationButtons();
        //    CurrentPage = 1;
        //    UpdateDisplayedContractors();

        //    motherPageEditInvoice = page;
        //}

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filterText = txtFilter.Text;

            if (filterText.Length > 0)
            {
                ContractorDataGrid.ItemsSource = ContractorsServiceDataGrid.FilterContractors(filterText);
            }
            else
            {
                ContractorDataGrid.ItemsSource = ContractorsServiceDataGrid.GetBuyerPage(1);
            }
        }

        private void UpdateDisplayedContractors()
        {
            var displayedContractors = ContractorsServiceDataGrid.GetBuyerPage(CurrentPage);
            ContractorDataGrid.ItemsSource = displayedContractors;
            UpdatePaginationButtonStyles();
            GeneratePaginationButtons();
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage < ContractorsServiceDataGrid.GetTotalPages())
            {
                CurrentPage++;
                UpdateDisplayedContractors();
            }
        }

        private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                UpdateDisplayedContractors();
            }
        }
        private void FirstPageButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage = 1;
            UpdateDisplayedContractors();
        }

        private void LastPageButton_Click(object sender, RoutedEventArgs e)
        {
            int totalItems = ContractorsServiceDataGrid.GetTotalPages();
            CurrentPage = totalItems;
            UpdateDisplayedContractors();
        }

        private void PageButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button pageButton && int.TryParse(pageButton.Content.ToString(), out int pageNumber))
            {
                CurrentPage = pageNumber;
                UpdateDisplayedContractors();
            }
        }

        private void GeneratePaginationButtons()
        {
            int totalPages = ContractorsServiceDataGrid.GetTotalPages();
            PaginationItemsControl.Items.Clear();

            int halfRange = 1;
            int startPage = Math.Max(1, CurrentPage - halfRange);
            int endPage = Math.Min(totalPages, CurrentPage + halfRange);

            if (endPage - startPage < 2)
            {
                if (startPage > 1) startPage = Math.Max(1, endPage - 2);
                else endPage = Math.Min(totalPages, startPage + 2);
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

        private void LoadContentBtn_Click(object sender, RoutedEventArgs e)
        {
            IsAddedContractor = true;
            var selectedItem = ContractorDataGrid.SelectedItem;

            Buyer buyer = (Buyer)selectedItem;

            if(motherPageAddInvoice != null)
            {
                motherPageAddInvoice.BuyerName.Text = buyer.BuyerName;
                motherPageAddInvoice.BuyerCity.Text = buyer.BuyerCity;
                motherPageAddInvoice.BuyerStreet.Text = buyer.BuyerStreet;
                motherPageAddInvoice.BuyerZipCode.Text = buyer.BuyerZipCode;
                buyer.BuyerNIP = "Brak";
            }
            else if(motherCompanyPageAddInvoice != null) 
            {
                motherCompanyPageAddInvoice.BuyerName.Text = buyer.BuyerName;
                motherCompanyPageAddInvoice.BuyerCity.Text = buyer.BuyerCity;
                motherCompanyPageAddInvoice.BuyerStreet.Text = buyer.BuyerStreet;
                motherCompanyPageAddInvoice.BuyerZipCode.Text = buyer.BuyerZipCode;
                motherCompanyPageAddInvoice.BuyerNIP.Text = buyer.BuyerNIP;
            }
            else if(motherEditPage != null)
            {
                motherEditPage.BuyerName.Text = buyer.BuyerName;
                motherEditPage.BuyerCity.Text = buyer.BuyerCity;
                motherEditPage.BuyerStreet.Text = buyer.BuyerStreet;
                motherEditPage.BuyerZipCode.Text = buyer.BuyerZipCode;
                motherEditPage.BuyerNIP.Text = buyer.BuyerNIP;
            }
            

            this.Close();
        }
    
}
}
