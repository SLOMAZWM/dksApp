using dksApp.Bookkeeping;
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
using dksApp.Magazine.MagazineDataGrid;
using PdfSharp;
using dksApp.Magazine.ProductW;
using dksApp.Bookkeeping.Invoice;

namespace dksApp.Magazine
{
	/// <summary>
	/// Interaction logic for MagazinePage.xaml
	/// </summary>
	public partial class MagazinePage : Page
	{
		private MainWindow _mainWindow;
		private NavigatorManager navigator;
		private Dictionary<string, Page> DataGridPage = new Dictionary<string, Page>();

		public MagazinePage(MainWindow mainWindow)
		{
			InitializeComponent();
			DataGridPage = InitializeDataGridPages();
			navigator = new NavigatorManager(tabButtonSP, DataGridSelectedFrame, DataGridPage);
            _mainWindow = mainWindow;
        }

        private void NavigationButton_Click(object sender, RoutedEventArgs e)
		{
			if (sender is Button button && button.Tag is string dataGridName)
			{
				navigator.ChangeTabButton(button);
				navigator.NavigateToDataGrid(dataGridName);
			}
		}

		private Dictionary<string, Page> InitializeDataGridPages()
		{
			Dictionary<string, Page> NewDictionaryOfPages = new Dictionary<string, Page>
			{
				{ "Wszystkie", new MainMagazineDataGrid(_mainWindow) }
			};

			return NewDictionaryOfPages;
		}

		private void AddBookKeepingBtn_Click(object sender, RoutedEventArgs e)
		{
            ProductPage createNewProduct = new ProductPage(_mainWindow);
            _mainWindow.MainContentFrame.NavigationService.Navigate(createNewProduct);
        }
    }
}
