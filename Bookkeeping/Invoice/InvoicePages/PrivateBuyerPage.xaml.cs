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

namespace dksApp.Bookkeeping.Invoice.InvoicePages
{
    /// <summary>
    /// Interaction logic for PrivateBuyerPage.xaml
    /// </summary>
    public partial class PrivateBuyerPage : Page
    {
        CreateInvoiceWindow createInvoice;
        public PrivateBuyerPage(CreateInvoiceWindow createInvoiceW)
        {
            InitializeComponent();
            createInvoice = createInvoiceW;
        }

        private void NextPageBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                createInvoice.Navigator.NavigateToGrid("Informacje");
                createInvoice.HighlightInformationButton();
            }
            catch
            {
                MessageBox.Show("Błąd nawigacji podstrony, skontaktuj się z administratorem aplikacji!", "Krytyczny błąd Nawigacji", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PreviousPageBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                createInvoice.Navigator.NavigateToGrid("Sprzedawca");
                createInvoice.HighlightSellerButton();
            }
            catch
            {
                MessageBox.Show("Błąd nawigacji podstrony, skontaktuj się z administratorem aplikacji!", "Krytyczny błąd Nawigacji", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //Change Data Input

        private void BuyerName_TextChanged(object sender, TextChangedEventArgs e)
        {
            createInvoice.NewInvoice.BuyerName = BuyerName.Text;
            createInvoice.NewInvoice.BuyerNIP = "Brak";
        }

        private void BuyerZipCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            createInvoice.NewInvoice.BuyerZipCode = BuyerZipCode.Text;
            createInvoice.NewInvoice.BuyerNIP = "Brak";
        }

        private void BuyerCity_TextChanged(object sender, TextChangedEventArgs e)
        {
            createInvoice.NewInvoice.BuyerCity = BuyerCity.Text;
            createInvoice.NewInvoice.BuyerNIP = "Brak";
        }

        private void BuyerStreet_TextChanged(object sender, TextChangedEventArgs e)
        {
            createInvoice.NewInvoice.BuyerStreet = BuyerStreet.Text;
            createInvoice.NewInvoice.BuyerNIP = "Brak";
        }
    }
}
