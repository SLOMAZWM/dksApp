using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using dksApp.Bookkeeping;
using dksApp.Orders;
using dksApp.Contractors;
using System.Xml;

namespace dksApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private NavigatorManager navigator;

        public MainWindow()
        {
            InitializeComponent();
            navigator = new NavigatorManager(MainContentFrame, InitializeButtonList());
        }

private List<Button> InitializeButtonList()
        {
            List<Button> newButtonList = new List<Button>
            {
                NavigationToBookKeepingBtn,
                NavigationToOrdersBtn,
                NavigationToContractorsBtn,
            };
            
            return newButtonList;
        }


        //#Navigation
        private void NavigationButton_Click(object sender, RoutedEventArgs e) 
        {
            if (sender is Button button && button.Tag is string pageName)
            {
                navigator.ChangeMenuButtonColor(button);
                navigator.NavigateToPage(pageName);
            }
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