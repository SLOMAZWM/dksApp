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
using System.Xml;

namespace dksApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Page MainBook = new MainBookPage();
        Page Allegro = new AllegroPage();
        Page Calendar = new CalendarPage();
        Page Contractors = new CalendarPage();
        Page Magazine = new MagazinePage();
        Page Orders = new OrdersPage();

        List<Page> PageList = new List<Page>();

        public MainWindow()
        {
            InitializeComponent();
            PageList = InitializePageList();
            Navigate(MainBook);
        }

        private void ChangeColor()
        {
            SolidColorBrush buttonForegroundBrush = new SolidColorBrush(Color.FromRgb(208, 192, 255));

            foreach (var page in PageList)
            {
                page.Foreground = buttonForegroundBrush;
            }
        }
        private List<Page> InitializePageList()
        {
            List<Page> pageListCreate = new List<Page>
    {
        MainBook,
        Allegro,
        Calendar,
        Contractors,
        Magazine,
        Orders
    };
            return pageListCreate;
        }

        //#region navigate
        private void Navigate(Page page)
        {
            MainContentFrame.NavigationService.Navigate(page);
        }

        private void NavigationToBookKeepingBtn_Click(object sender, RoutedEventArgs e)
        {
            Navigate(MainBook);

            ChangeColor();
            NavigationToBookKeepingBtn.Foreground = Brushes.White;
        }

        private void NavigationToOrdersBtn_Click(object sender, RoutedEventArgs e)
        {
            Navigate(Orders);
        }

        private void NavigationToContractorsBtn_Click(object sender, RoutedEventArgs e)
        {
            Navigate(Contractors);
        }
        private void NavigationToMagazineBtn_Click(object sender, RoutedEventArgs e)
        {
            Navigate(Magazine);
        }

        private void NavigationToCalendarBtn_Click(object sender, RoutedEventArgs e)
        {
            Navigate(Calendar);
        }

        private void NavigationToAllegroBtn_Click(object sender, RoutedEventArgs e)
        {
            Navigate(Allegro);
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
            if (e.ClickCount == 2)
            {
                if (IsMaximized)
                {
                    this.WindowState = WindowState.Normal;
                    this.Width = 1080;
                    this.Height = 720;

                    IsMaximized = false;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;

                    IsMaximized = true;
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