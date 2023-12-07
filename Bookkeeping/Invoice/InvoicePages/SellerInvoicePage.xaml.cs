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
    }
}
