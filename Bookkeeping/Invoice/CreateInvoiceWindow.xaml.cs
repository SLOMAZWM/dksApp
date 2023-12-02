using dksApp.Bookkeeping.Invoice.InvoicePages.InvoiceDialog;
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
using System.Windows.Shapes;

namespace dksApp.Bookkeeping.Invoice
{
    /// <summary>
    /// Interaction logic for CreateInvoiceWindow.xaml
    /// </summary>
    public partial class CreateInvoiceWindow : Window
    {

        private NavigatorManager navigator;
        private Dictionary<string, Page> GridPage = new Dictionary<string, Page>();
        public ObservableCollection<Product> Products = new ObservableCollection<Product>();
        public uint LP = 1;

        public CreateInvoiceWindow()
        {
            InitializeComponent();
            GridPage = InitializeGridPages();
            navigator = new NavigatorManager(GridPage, tabButtonSP, GridFrame);

            Product first = new Product(){ Id = 1, NameItem = "Testowy" };
            Products.Add(first);

            ProductsDataGrid.ItemsSource = Products;
        }

        //Function's

        private void NavigationButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string gridName)
            {
                navigator.ChangeInvoiceTabButton(button);
                navigator.NavigateToGrid(gridName);
            }
        }

        private void AddProductBtn_Click(object sender, RoutedEventArgs e)
        {

            ProductDialogWindow dialog = new ProductDialogWindow();
            dialog.Owner = this;
            dialog.ShowDialog();
        }

        private Dictionary<string, Page> InitializeGridPages()
        {
            Dictionary<string, Page> NewDictionaryOfPages = new Dictionary<string, Page>
            {
                { "Sprzedawca", new SellerInvoicePage() }
            };

            return NewDictionaryOfPages;
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //#settingsfrontend
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private bool IsMaximized = false;
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (IsMaximized)
                {
                    this.WindowState = WindowState.Normal;
                    this.Width = 650;
                    this.Height = 400;

                    IsMaximized = false;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;

                    IsMaximized = true;
                }
            }
        }

    }
}
