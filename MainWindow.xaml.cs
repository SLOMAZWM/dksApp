using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using dksApp.Bookkeeping;

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
            Navigate(new MainBookPage());
        }

        //#region navigate
        private void Navigate(Page page)
        {
            //MainContentFrame.NavigationService.Navigate(page);
        }

        private void NavigationToBookKeepingBtn_Click(object sender, RoutedEventArgs e)
        {
            Navigate(new MainBookPage());
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