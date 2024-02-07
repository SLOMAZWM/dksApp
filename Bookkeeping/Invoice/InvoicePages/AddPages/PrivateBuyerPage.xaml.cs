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

        private void LoadSellerBtn_Click(object sender, RoutedEventArgs e)
        {
            dksApp.Bookkeeping.LoadContractorsW.LoadContractorsW loadContractor = new dksApp.Bookkeeping.LoadContractorsW.LoadContractorsW(this);
            loadContractor.ShowDialog();
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
                if (!IsTextAllowed(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private bool IsTextAllowed(string text)
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
