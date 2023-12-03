using dksApp.Bookkeeping.Invoice.InvoicePages;
using dksApp.Bookkeeping.Invoice.InvoicePages.InvoiceDialog;
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
using System.Windows.Shapes;

namespace dksApp.Bookkeeping.Invoice
{
    /// <summary>
    /// Interaction logic for CreateInvoiceWindow.xaml
    /// </summary>
    public partial class CreateInvoiceWindow : Window
    {

        private NavigatorManager navigator;
        private Dictionary<string, Page> GridPage = new Dictionary<string, Page>();
        private string selectedGrid;
        private bool isSelected;
        public string SelectedGrid 
        {
            get 
            { 
                return selectedGrid;
            } 
            set 
            { 
                selectedGrid = value; 
            } 
        }
        public bool IsSelected { get { return isSelected; } set { isSelected = value; } }

        public NavigatorManager Navigator 
        { 
            get 
            { 
                return navigator; 
            }
            set { navigator = value; }
        }


        public CreateInvoiceWindow()
        {
            InitializeComponent();
            GridPage = InitializeGridPages();
            navigator = new NavigatorManager(GridPage, tabButtonSP, GridFrame);
        }

        //Function's

        private void NavigationButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string gridName)
            {
                navigator.ChangeInvoiceTabButton(button);
                navigator.NavigateToGrid(gridName);
            }
        }

        private Dictionary<string, Page> InitializeGridPages()
        {
            Dictionary<string, Page> NewDictionaryOfPages = new Dictionary<string, Page>
            {
                { "Sprzedawca", new SellerInvoicePage() },
                { "Produkty", new ProductsInvoicePage(this) },
                { "NabywcaFirmowy", new CompanyBuyerPage() },
                { "NabywcaPrywatny", new PrivateBuyerPage() }
            };

            return NewDictionaryOfPages;
        }

        private void WhichBuyer_Click(object sender, RoutedEventArgs e) 
        {
            if (isSelected == false)
            {
                if (sender is Button button)
                {
                    navigator.ChangeInvoiceTabButton(button);
                    WhichBuyerDialog choiceBuyerType = new WhichBuyerDialog();
                    choiceBuyerType.Owner = this;
                    choiceBuyerType.Navigator = navigator;
                    choiceBuyerType.ShowDialog();
                }
            }
            else if(isSelected == true && sender is Button button)
            {
                navigator.ChangeInvoiceTabButton(button);
                navigator.NavigateToGrid(SelectedGrid);
            }
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
                    this.Width = 650;
                    this.Height = 400;

                    IsMaximized = false;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;

                    IsMaximized = true;
                }
            }
        }

    }
}
