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
    /// Interaction logic for WhichBuyerDialog.xaml
    /// </summary>
    public partial class WhichBuyerDialog : Window
    {
        public NavigatorManager Navigator { get; set; }
        //public string SelectedGrid { get; set; }
        public WhichBuyerDialog()
        {
            InitializeComponent();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
        
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (PrivateCheckBox.IsChecked == true)
            {
                // Ustaw SelectedGrid właściciela (który jest CreateInvoiceWindow)
                if (Owner is CreateInvoiceWindow ownerWindow)
                {
                    ownerWindow.IsSelected = true;
                    ownerWindow.SelectedGrid = "NabywcaPrywatny";
                    Navigator?.NavigateToGrid(ownerWindow.SelectedGrid);
                }
                this.Close();
            }
            else if(CompanyCheckBox.IsChecked == true) 
            {
                if(Owner is CreateInvoiceWindow ownerWindow) 
                {
                    ownerWindow.IsSelected = true;
                    ownerWindow.SelectedGrid = "NabywcaFirmowy";
                    Navigator?.NavigateToGrid(ownerWindow.SelectedGrid);
                }
                this.Close();
            }
            else
            {
                if(Owner is CreateInvoiceWindow ownerWindow)
                {
                    ownerWindow.IsSelected = false;
                    MessageBox.Show("Nie wybrałeś żadnej opcji!", "Błąd wyboru", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;

            if (checkBox == null) return;

            if (checkBox.Name == "PrivateCheckBox")
            {
                CompanyCheckBox.IsChecked = false;
            }
            else if (checkBox.Name == "CompanyCheckBox")
            {
                PrivateCheckBox.IsChecked = false;
            }
        }

        private void CancelButton_Click(Object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
