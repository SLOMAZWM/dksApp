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
        public delegate void NewProductEventHandler();

        public event NewProductEventHandler NewProductRequested;

        public ProductDialogWindow()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (NewCheckBox.IsChecked == true)
            {
                NewProductRequested?.Invoke();
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
