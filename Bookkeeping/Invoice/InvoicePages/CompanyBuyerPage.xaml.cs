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
    /// Interaction logic for CompanyBuyerPage.xaml
    /// </summary>
    public partial class CompanyBuyerPage : Page
    {
        CreateInvoiceWindow createInvoice;

        public CompanyBuyerPage(CreateInvoiceWindow createInvoiceW)
        {
            InitializeComponent();
            createInvoice = createInvoiceW;
        }

        private void NextPageBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                createInvoice.Navigator.NavigateToGrid("Produkty");
                createInvoice.HighlightProductButton();
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
        }

        private void BuyerZipCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            createInvoice.NewInvoice.BuyerZipCode = BuyerZipCode.Text;
        }

        private void BuyerCity_TextChanged(object sender, TextChangedEventArgs e)
        {
            createInvoice.NewInvoice.BuyerCity = BuyerCity.Text;
        }

        private void BuyerStreet_TextChanged(object sender, TextChangedEventArgs e)
        {
            createInvoice.NewInvoice.BuyerCity = BuyerStreet.Text;
        }

        private void BuyerNIP_TextChanged(object sender, TextChangedEventArgs e)
        {
            createInvoice.NewInvoice.BuyerNIP = BuyerNIP.Text;
        }

        private void BuyerPaymentType_TextChanged(object sender, TextChangedEventArgs e)
        {
            createInvoice.NewInvoice.BuyerPaymentType = BuyerPaymentType.Text;
        }

        private void BuyerPaymentDate_TextChanged(object sender, TextChangedEventArgs e)
        {
            createInvoice.NewInvoice.BuyerPaymentDate = BuyerPaymentDate.Text;
        }
    }
}
