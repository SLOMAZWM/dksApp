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

namespace dksApp.Contractors.AddContractorsW
{
    /// <summary>
    /// Interaction logic for AddSellerW.xaml
    /// </summary>
    public partial class AddSellerW : Window
    {
        Seller seller { get; set; }
        public AddSellerW()
        {
            InitializeComponent();
            seller = new Seller();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SaveSellerBtn_Click(object sender, RoutedEventArgs e)
        {
            //Przypisanie z textinputow do obiektu seller
        }
    }
}
