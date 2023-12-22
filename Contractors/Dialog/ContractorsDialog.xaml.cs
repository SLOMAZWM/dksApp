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
        private ContractorsPage ParentPage { get; set; }
        public ContractorsDialog(ContractorsPage Contractor)
        {
            InitializeComponent();
            ParentPage = Contractor;
        }

        private void SellerChb_Checked(object sender, RoutedEventArgs e)
        {
            BuyerChb.IsChecked = false;
        }

        private void BuyerChb_Checked(object sender, RoutedEventArgs e)
        {
            SellerChb.IsChecked = false;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ParentPage.userChoice = 0;
            this.Close();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
           if(SellerChb.IsChecked == true)
            {
                ParentPage.userChoice = 1;
            }
           else if(BuyerChb.IsChecked == true) 
            {
                ParentPage.userChoice = 2;
            }
           else
            {
                MessageBox.Show("Nie dokonałeś wyboru - zaznacz coś, lub kliknij anuluj!", "Błąd wyboru", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
           this.Close();
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
