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

namespace dksApp.Contractors.Dialog
{
    /// <summary>
    /// Interaction logic for ContractorsDialog.xaml
    /// </summary>
    public partial class ContractorsDialog : Window
    {
        private byte userChoice; // 1 for Seller || 2 for Buyer || 0 For Nothing
        public ContractorsDialog(byte ch)
        {
            InitializeComponent();
            userChoice = ch;
        }

        private void SellerChb_Checked(object sender, RoutedEventArgs e)
        {
            BuyerChb.IsChecked = false;
            userChoice = 1;
        }

        private void BuyerChb_Checked(object sender, RoutedEventArgs e)
        {
            SellerChb.IsChecked = false;
            userChoice = 2;
        }

        private void SellerChb_Unchecked(object sender, RoutedEventArgs e)
        {
            userChoice = 0;
        }

        private void BuyerChb_Unchecked(object sender, RoutedEventArgs e)
        {
            userChoice = 0;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            userChoice = 0;
            this.Close();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if(userChoice == 0) 
            {
                MessageBox.Show("Nie wybrałeś żadnej opcji!", "Błąd wyboru", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                this.Close();
            }
        }

        //front
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
    }
}
