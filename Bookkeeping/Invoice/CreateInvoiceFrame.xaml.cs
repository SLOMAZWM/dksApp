﻿using dksApp.Bookkeeping.Invoice.InvoicePages.InvoiceDialog;
using dksApp.Bookkeeping.Invoice.InvoicePages;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace dksApp.Bookkeeping.Invoice
{
    /// <summary>
    /// Interaction logic for CreateInvoiceFrame.xaml
    /// </summary>
    public partial class CreateInvoiceFrame : Page
    {
        private NavigatorManager navigator;
        public Dictionary<string, Page> GridPage;
        private string selectedGrid;
        private bool isSelected;
        public InvoiceClass NewInvoice { get; set; }

        public CreateInvoiceFrame()
        {
            InitializeComponent();
            NewInvoice = new InvoiceClass();
            GridPage = InitializeGridPages();
            navigator = new NavigatorManager(GridPage, tabButtonSP, GridFrame);
        }

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
                    choiceBuyerType.Navigator = navigator;

                    choiceBuyerType.BuyerTypeSelected += BuyerTypeSelected;
                    choiceBuyerType.ShowDialog();
                }
            }
            else if (isSelected == true && sender is Button button)
            {
                navigator.ChangeInvoiceTabButton(button);
                navigator.NavigateToGrid(selectedGrid);
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
            if(NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
        }
    }
}
