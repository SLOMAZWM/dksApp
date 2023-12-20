﻿using dksApp.Bookkeeping;
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
using dksApp.Contractors.Dialog;

namespace dksApp.Contractors
{
    /// <summary>
    /// Interaction logic for ContractorsPage.xaml
    /// </summary>
    public partial class ContractorsPage : Page
    {
        private NavigatorManager navigator;
        private Dictionary<string, Page> DataGridPage = new Dictionary<string, Page>();

        public ContractorsPage()
        {
            InitializeComponent();

            DataGridPage = InitializeDataGridPages();
            navigator = new NavigatorManager(tabButtonSP, DataGridSelectedFrame, DataGridPage);
        }

        private void AddContractor_Click(object sender, RoutedEventArgs e)
        {
            byte choice = 0;
            ContractorsDialog contractorsDialog = new ContractorsDialog(choice);
            contractorsDialog.ShowDialog();

            if(choice == 1)
            {
                
            }
            else if(choice == 2) 
            {

            }
        }

        private Dictionary<string, Page> InitializeDataGridPages()
        {
            Dictionary<string, Page> NewDictionaryOfPages = new Dictionary<string, Page>
            {
                { "Wszyscy", new AllDataGridPage() },
                {"Sprzedawcy", new SuppliersPage() },
                {"Nabywcy", new RecipientsPage() }
            };

            return NewDictionaryOfPages;
        }

        private void NavigationButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string dataGridName)
            {
                navigator.ChangeTabButton(button);
                navigator.NavigateToDataGrid(dataGridName);
            }
        }
    }
}
