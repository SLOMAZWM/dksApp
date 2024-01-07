using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            createInvoice.NewInvoice.BuyerStreet = BuyerStreet.Text;
        }

        private void BuyerNIP_TextChanged(object sender, TextChangedEventArgs e)
        {
            createInvoice.NewInvoice.BuyerNIP = BuyerNIP.Text;
        }

        private void BuyerZipCodeTxt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9-]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TextOnlyNumber_CheckPasteZipCode(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!IsTextAllowedZipCode(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private bool IsTextAllowedZipCode(string text)
        {
            Regex regex = new Regex("[^0-9-]");
            return !regex.IsMatch(text);
        }

        private void BuyerCityTxt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[a-zA-Z]+$");
            e.Handled = !regex.IsMatch(e.Text);
        }

        private void TextOnlyNumber_CheckPasteCity(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!IsTextAllowedCity(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private bool IsTextAllowedCity(string text)
        {
            Regex regex = new Regex("[^a-z, A-Z]+$");
            return !regex.IsMatch(text);
        }
    }
}
