using dksApp.Bookkeeping.Invoice.InvoicePages.EditPage.Products;
using dksApp.Bookkeeping.LoadInfoWindow;
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

namespace dksApp.Bookkeeping.Invoice.InvoicePages.InvoiceDialog
{
    /// <summary>
    /// Interaction logic for ProductDialogWindow.xaml
    /// </summary>
    public partial class ProductDialogWindow : Window
    {
        ProductsInvoicePage? NewMotherPage;
        ProductsEditPage? EditMotherPage;

        public ProductDialogWindow(ProductsInvoicePage motherPage)
        {
            InitializeComponent();
            NewMotherPage = motherPage;
            EditMotherPage = null;
        }

        public ProductDialogWindow(ProductsEditPage motherPage)
        {
            InitializeComponent();
            EditMotherPage = motherPage;
            NewMotherPage = null;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if(NewMotherPage != null)
            {
                if (NewCheckBox.IsChecked == true)
                {
                    NewMotherPage!.isNew = true;
                    NewMotherPage.isLoad = false;
                }
                else if (SavedCheckBox.IsChecked == true)
                {
                    NewMotherPage!.isNew = false;
                    NewMotherPage.isLoad = true;
                }
            }
            else if(EditMotherPage != null)
            {
                if (NewCheckBox.IsChecked == true)
                {
                    EditMotherPage!.isNew = true;
                    EditMotherPage.isLoad = false;
                }
                else if (SavedCheckBox.IsChecked == true)
                {
                    EditMotherPage!.isNew = false;
                    EditMotherPage.isLoad = true;
                }
            }
            else
            {
                return;
            }

            this.Close();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
