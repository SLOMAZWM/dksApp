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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace dksApp.Bookkeeping.Invoice.InvoicePages
{
    /// <summary>
    /// Interaction logic for ProductsInvoicePage.xaml
    /// </summary>
    public partial class ProductsInvoicePage : Page
    {
        public ObservableCollection<Product> Products = new ObservableCollection<Product>();
        private uint LP = 2;
        private CreateInvoiceWindow parentWindow;

        public ProductsInvoicePage(CreateInvoiceWindow InvoiceWindow)
        {
            InitializeComponent();

            parentWindow = InvoiceWindow;

            Product first = new Product() { NumberOfItems = 1, IdProduct = 1, NameItem = "Testowy" };
            Products.Add(first);

            ProductsDataGrid.ItemsSource = Products;
        }

        private void AddProductBtn_Click(object sender, RoutedEventArgs e)
        {
            ProductDialogWindow dialog = new ProductDialogWindow();
            dialog.Owner = parentWindow;
            dialog.NewProductRequested += OnNewProductRequested;
            dialog.ShowDialog();
            dialog.NewProductRequested -= OnNewProductRequested;
        }

        private void OnNewProductRequested()
        {
            Product newProduct = new Product()
            {
                NumberOfItems = LP
            };
            Products.Add(newProduct);
            LP++;
        }
    }
}
