using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using dksApp.Bookkeeping;
using dksApp.Orders;
using dksApp.Contractors;
using dksApp.Magazine;
using dksApp.Calendar;
using dksApp.Allegro;

namespace dksApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Navigate(new AllegroPage());
        }

        //#region navigate
        private void Navigate(Page page)
        {
            MainContentFrame.NavigationService.Navigate(page);
        }

        private void NavigationToBookKeepingBtn_Click(object sender, RoutedEventArgs e)
        {
            Navigate(new MainBookPage());
        }

        private void NavigationToOrdersBtn_Click(object sender, RoutedEventArgs e)
        {
            Navigate(new OrdersPage());
        }

        private void NavigationToContractorsBtn_Click(object sender, RoutedEventArgs e)
        {
            Navigate(new ContractorsPage());
        }
        private void NavigationToMagazineBtn_Click(object sender, RoutedEventArgs e)
        {
            Navigate(new MagazinePage());
        }

        private void NavigationToCalendarBtn_Click(object sender, RoutedEventArgs e)
        {
            Navigate(new CalendarPage());
        }

        private void NavigationToAllegroBtn_Click(object sender, RoutedEventArgs e)
        {
            Navigate(new AllegroPage());
        }

        //#settingsfrontend
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) 
            {
                this.DragMove();
            }
        }

        private bool IsMaximized = false;
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) 
        {
            if(e.ClickCount == 2)
            {
                if(IsMaximized) 
                {
                    this.WindowState = WindowState.Normal;
                    this.Width = 1080;
                    this.Height = 720;

                    IsMaximized = false;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;

                    IsMaximized= true;
                }
            }
        }

        //Function's
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}