﻿using System;
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

namespace dksApp.Contractors
{
    /// <summary>
    /// Interaction logic for AllDataGridPage.xaml
    /// </summary>
    public partial class AllDataGridPage : Page
    {
        public AllDataGridPage()
        {
            InitializeComponent();
			AsyncInitialize();

			ContractorsDataGrid.ItemsSource = ContractorsServiceDataGrid.BuyerList;
        }

        public async Task AsyncInitialize()
        {
            await ContractorsServiceDataGrid.GetBuyersFromDataBase();
        }
    }
}
