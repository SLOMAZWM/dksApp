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

namespace dksApp.Bookkeeping.Invoice
{
    /// <summary>
    /// Interaction logic for SellerInvoicePage.xaml
    /// </summary>
    public partial class SellerInvoicePage : Page
    {
        CreateInvoiceWindow createInvoice;

        public SellerInvoicePage(CreateInvoiceWindow createInvoiceW)
        {
            InitializeComponent();
            createInvoice = createInvoiceW;
        }

        public void NextPageBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                createInvoice.WhichBuyer_Click(sender, e);
                createInvoice.HighlightBuyerButton();
            }
            catch
            {
                MessageBox.Show("Błąd nawigacji podstrony, skontaktuj się z administratorem aplikacji!", "Krytyczny błąd Nawigacji", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //Validation Data

        public bool IsEmpty()
        {
            if(String.IsNullOrEmpty(SellerNameTxt.Text))
            {
                MessageBox.Show("Uzupełnij Nazwę Sprzedawcy!", "Uzupełnij Sprzedawcę", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
            else if(String.IsNullOrEmpty(SellerZipCodeTxt.Text))
            {
                MessageBox.Show("Uzupełnij Kod Pocztowy Sprzedawcy!", "Uzupełnij Sprzedawcę", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
            else if (String.IsNullOrEmpty(SellerCity.Text))
            {
                MessageBox.Show("Uzupełnij Miasto Sprzedawcy!", "Uzupełnij Sprzedawcę", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
            else if (String.IsNullOrEmpty(SellerStreet.Text))
            {
                MessageBox.Show("Uzupełnij Ulicę Sprzedawcy!", "Uzupełnij Sprzedawcę", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
            else if (String.IsNullOrEmpty(SellerNIP.Text))
            {
                MessageBox.Show("Uzupełnij NIP Sprzedawcy!", "Uzupełnij Sprzedawcę", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
            else if (String.IsNullOrEmpty(SellerBankName.Text))
            {
                MessageBox.Show("Uzupełnij Nazwę Banku Sprzedawcy!", "Uzupełnij Sprzedawcę", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
            else if (String.IsNullOrEmpty(SellerBankName.Text))
            {
                MessageBox.Show("Uzupełnij Numer Rachunku Sprzedawcy!", "Uzupełnij Sprzedawcę", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }

            return false;
        }

        //Data Change Input

        private void SellerNameTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            createInvoice.NewInvoice.SellerName = SellerNameTxt.Text;
        }

        private void ZipCodeTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            createInvoice.NewInvoice.SellerZipCode = SellerZipCodeTxt.Text;
        }

        private void SellerCity_TextChanged(object sender, TextChangedEventArgs e)
        {
            createInvoice.NewInvoice.SellerCity = SellerCity.Text;
        }

        private void SellerStreet_TextChanged(object sender, TextChangedEventArgs e)
        {
            createInvoice.NewInvoice.SellerStreet = SellerStreet.Text;
        }

        private void SellerNIP_TextChanged(object sender, TextChangedEventArgs e)
        {
            createInvoice.NewInvoice.SellerNIP = SellerNIP.Text;
        }

        private void SellerBankName_TextChanged(object sender, TextChangedEventArgs e)
        {
            createInvoice.NewInvoice.SellerBankName = SellerBankName.Text;
        }

        private void SellerBankAccount_TextChanged(object sender, TextChangedEventArgs e)
        {
            createInvoice.NewInvoice.SellerBankAccount = SellerBankAccount.Text;
        }
    }
}
