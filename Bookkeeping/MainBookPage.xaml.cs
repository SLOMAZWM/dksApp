using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace dksApp.Bookkeeping
{
    /// <summary>
    /// Interaction logic for MainBookPage.xaml
    /// </summary>
    public partial class MainBookPage : Page
    {

        private NavigatorManager navigator;

        public MainBookPage()
        {
            InitializeComponent();

            ObservableCollection<Invoice> invoices = new ObservableCollection<Invoice>();

            //przykładowa kolekcja

            invoices.Add(new Invoice { Id = 1, SellerName = "DARKAS", BuyerName = "Ja", Payment = "Przelew", Price = 1290, Type = "WŁASNA" });
            invoices.Add(new Invoice { Id = 2, SellerName = "DARKAS", BuyerName = "Ty", Payment = "Przelew", Price = 521, Type = "ALLEGRO" });

            BookKeepingDataGrid.ItemsSource = invoices;

            navigator = new NavigatorManager(tabButtonSP);
        }

        private void NavigationButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                navigator.ChangeTabButton(button);
                //navigator.NavigateToPage(DataGridName); // Dodaj nawigacje - przemysl system SQL/APLIKACJA sortowanie
            }
        }
    }
}
