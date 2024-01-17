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
        public InvoiceClass NewInvoice {  get; set; }

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
            NewInvoice = new InvoiceClass();
            GridPage = InitializeGridPages();
            navigator = new NavigatorManager(tabButtonSP, GridFrame, GridPage);
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

        public void HighlightBuyerButton()
        {
            navigator.ChangeInvoiceTabButton(BuyerBtn);
        }

        public void HighlightProductButton()
        {
            navigator.ChangeInvoiceTabButton(ProductBtn);
        }

        public void HighlightInformationButton()
        {
            navigator.ChangeInvoiceTabButton(InformationBtn);
        }

        public void HighlightSellerButton()
        {
            navigator.ChangeInvoiceTabButton(SellerBtn);
        }

        private Dictionary<string, Page> InitializeGridPages()
        {
            Dictionary<string, Page> NewDictionaryOfPages = new Dictionary<string, Page>
            {
                { "Sprzedawca", new SellerInvoicePage(this) },
                { "Produkty", new ProductsInvoicePage(this) },
                { "Informacje", new InformationInvoicePage(this) }
            };

            return NewDictionaryOfPages;
        }

        public void WhichBuyer_Click(object sender, RoutedEventArgs e) 
        {
            if (isSelected == false)
            {
                if (sender is Button button)
                {
                    navigator.ChangeInvoiceTabButton(button);
                    WhichBuyerDialog choiceBuyerType = new WhichBuyerDialog();
                    choiceBuyerType.Owner = this;
                    choiceBuyerType.Navigator = navigator;

                    choiceBuyerType.BuyerTypeSelected += BuyerTypeSelected;
                    choiceBuyerType.ShowDialog();
                }
            }
            else if(isSelected == true && sender is Button button)
            {
                navigator.ChangeInvoiceTabButton(button);
                navigator.NavigateToGrid(SelectedGrid);
            }
        }

        private void BuyerTypeSelected(string buyerType)
        {
            if (buyerType != null)
            {
                IsSelected = true;
                SelectedGrid = buyerType;

                if (!GridPage.ContainsKey(buyerType))
                {
                    GridPage.Add(buyerType, buyerType == "NabywcaPrywatny"
                        ? new PrivateBuyerPage(this)
                        : new CompanyBuyerPage(this));
                }

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
                    this.Width = 1080;
                    this.Height = 700;

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
