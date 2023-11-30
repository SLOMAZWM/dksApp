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

        //NAPRAW KOLORY DLA BUTTONOW

        List<Button> ButtonList = new List<Button>();

        public MainWindow()
        {
            InitializeComponent();
            ButtonList = InitializeButtonList();
            Navigate(MainBook);
        }

        private void ChangeColor(Button clickedButton)
        {
            SolidColorBrush buttonForegroundBrush = new SolidColorBrush(Color.FromRgb(208, 192, 255));
            SolidColorBrush whiteForegroundBrush = Brushes.White;

            foreach (var Button in ButtonList)
            {
                Button.Foreground = buttonForegroundBrush;
            }

            clickedButton.Foreground = whiteForegroundBrush;
        }

        private List<Button> InitializeButtonList()
        {
            List<Button> newButtonList = new List<Button>
            {
                NavigationToBookKeepingBtn,
                NavigationToAllegroBtn,
                NavigationToOrdersBtn,
                NavigationToCalendarBtn,
                NavigationToContractorsBtn,
                NavigationToMagazineBtn
            };
            
            return newButtonList;
        }



        //#region navigate
        private void Navigate(Page page)
        {
            MainContentFrame.NavigationService.Navigate(page);
        }

        private void NavigationToBookKeepingBtn_Click(object sender, RoutedEventArgs e)
        {
            Navigate(MainBook);

            ChangeColor((Button)sender);
        }

        private void NavigationToOrdersBtn_Click(object sender, RoutedEventArgs e)
        {
            Navigate(Orders);

            ChangeColor((Button)sender);
        }

        private void NavigationToContractorsBtn_Click(object sender, RoutedEventArgs e)
        {
            Navigate(Contractors);

            ChangeColor((Button)sender);
        }
        private void NavigationToMagazineBtn_Click(object sender, RoutedEventArgs e)
        {
            Navigate(Magazine);

            ChangeColor((Button)sender);
        }

        private void NavigationToCalendarBtn_Click(object sender, RoutedEventArgs e)
        {
            Navigate(Calendar);

            ChangeColor((Button)sender);
        }

        private void NavigationToAllegroBtn_Click(object sender, RoutedEventArgs e)
        {
            Navigate(Allegro);

            ChangeColor((Button)sender);
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