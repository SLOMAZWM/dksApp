using dksApp.Bookkeeping.Invoice.InvoicePages;
using dksApp.Bookkeeping.Invoice.InvoicePages.EditPage.Products;
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

namespace dksApp.Bookkeeping.LoadInfoWindow
{
    /// <summary>
    /// Interaction logic for LoadWindow.xaml
    /// </summary>
    public partial class LoadWindow : Window
    {
        ProductsInvoicePage motherPageAddInvoice;
        ProductsEditPage motherPageEditInvoice;
        private int CurrentPage;
        public bool isAddedProduct = false;

        public LoadWindow(ProductsInvoicePage page)
        {
            InitializeComponent();
            ProductDataGrid.ItemsSource = ProductServiceDataGrid.Products;
            GeneratePaginationButtons();
            CurrentPage = 1;
            UpdateDisplayedProducts();

            motherPageAddInvoice = page;
        }

        public LoadWindow(ProductsEditPage page)
        {
            InitializeComponent();
            ProductDataGrid.ItemsSource = ProductServiceDataGrid.Products;
            GeneratePaginationButtons();
            CurrentPage = 1;
            UpdateDisplayedProducts();

            motherPageEditInvoice = page;
        }

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
                ProductDataGrid.ItemsSource = ProductServiceDataGrid.FilterProducts(filterText);
            }
            else
            {
                ProductDataGrid.ItemsSource = ProductServiceDataGrid.Products;
            }
        }

        private void UpdateDisplayedProducts()
        {
            var displayedProducts = ProductServiceDataGrid.GetProductsPage(CurrentPage);
            ProductDataGrid.ItemsSource = displayedProducts;
            UpdatePaginationButtonStyles();
            GeneratePaginationButtons();
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
            var selectedItem = ProductDataGrid.SelectedItem;
            isAddedProduct = true;

            if (selectedItem != null && motherPageAddInvoice != null) 
            {
                motherPageAddInvoice.OnLoadProductRequested((Product)selectedItem);
                this.Close();
            }
            else if(selectedItem != null && motherPageEditInvoice != null) 
            {
                motherPageEditInvoice.OnLoadProductRequested((Product)selectedItem);
                this.Close();
            }
        }
    }
}
