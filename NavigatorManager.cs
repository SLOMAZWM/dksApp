using dksApp.Bookkeeping;
using dksApp.Bookkeeping.Invoice;
using dksApp.Contractors;
using dksApp.Orders;
using dksApp.Magazine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace dksApp
{
	public class NavigatorManager
	{
		private Dictionary<string, Page> Pages = new Dictionary<string, Page>();
		private Dictionary<string, Page> DataGridPage = new Dictionary<string, Page>();
		private Dictionary<string, Page> GridPage = new Dictionary<string, Page>();
		private Frame ActuallyContentFrame;
		List<Button> ButtonListMenu = new List<Button>();
		private StackPanel tabButtonStackPanel;
		private StackPanel invoiceTabButton;
		private MainWindow _mainWindow;

		//MainWindow Navigation
		public NavigatorManager(Frame frame, List<Button> ListOfButtonsMenu, MainWindow mainWindow)
		{
			_mainWindow = mainWindow;
			Pages = InitializePages();
			ActuallyContentFrame = frame;
			ButtonListMenu = ListOfButtonsMenu;
		}

		//GridNavigation
		public NavigatorManager(StackPanel SP, Frame frame, Dictionary<string, Page> GridPages)
		{
			DataGridPage = GridPages;
			tabButtonStackPanel = SP;
			ActuallyContentFrame = frame;
		}

		//CreateInvoiceNavigation
		public NavigatorManager(Dictionary<string, Page> GridPages, StackPanel SP, Frame frame)
		{
			ActuallyContentFrame = frame;
			invoiceTabButton = SP;
			GridPage = GridPages;
		}


		public void NavigateToDataGrid(string dataGridName)
		{
			try
			{
				ActuallyContentFrame.NavigationService.Navigate(DataGridPage[dataGridName]);
			}
			catch
			{
				MessageBox.Show("Błąd nawigacji tabeli, skontaktuj się z administratorem aplikacji!", "Krytyczny błąd Nawigacji", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		public void NavigateToGrid(string gridName)
		{
			try
			{
				ActuallyContentFrame.NavigationService.Navigate(GridPage[gridName]);
			}
			catch
			{
				MessageBox.Show("Błąd nawigacji podstrony, skontaktuj się z administratorem aplikacji!", "Krytyczny błąd Nawigacji", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private Dictionary<string, Page> InitializePages()
		{
			Dictionary<string, Page> NewDictionaryOfPages = new Dictionary<string, Page>
			{
				{ "MainBook", new MainBookPage(_mainWindow) },
				{"Contractors", new ContractorsPage(_mainWindow) },
				{"Orders", new OrdersPage() },
				{"Magazine", new MagazinePage(_mainWindow) }
			};

			return NewDictionaryOfPages;
		}

		public void NavigateToPage(string pageName)
		{
			try
			{
				ActuallyContentFrame.NavigationService.Navigate(Pages[pageName]);
			}
			catch
			{
				MessageBox.Show("Błąd nawigacji strony, skontaktuj się z administratorem aplikacji!", "Krytyczny błąd Nawigacji", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		public void ChangeMenuButtonColor(Button clickedButton)
		{
			SolidColorBrush buttonForegroundBrush = new SolidColorBrush(Color.FromRgb(208, 192, 255));
			SolidColorBrush whiteForegroundBrush = Brushes.White;

			foreach (var button in ButtonListMenu)
			{
				button.Foreground = buttonForegroundBrush;
			}

			clickedButton.Foreground = whiteForegroundBrush;
		}

		public void ChangeTabButton(Button clickedButton)
		{
			SolidColorBrush buttonBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x78, 0x4F, 0xF2));
			SolidColorBrush blackForegroundBrush = new SolidColorBrush(Color.FromRgb(0x12, 0x15, 0x18));
			SolidColorBrush transparentBrush = new SolidColorBrush(Color.FromArgb(0x00, 0x00, 0x00, 0x00));

			foreach (var child in tabButtonStackPanel.Children)
			{
				if (child is Button button)
				{
					button.Foreground = (button == clickedButton) ? buttonBrush : blackForegroundBrush;
					button.BorderBrush = (button == clickedButton) ? buttonBrush : transparentBrush;
				}
			}
		}

		public void ChangeInvoiceTabButton(Button clickedButton)
		{
			SolidColorBrush buttonBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x78, 0x4F, 0xF2));
			SolidColorBrush blackForegroundBrush = new SolidColorBrush(Color.FromRgb(0x12, 0x15, 0x18));
			SolidColorBrush transparentBrush = new SolidColorBrush(Color.FromArgb(0x00, 0x00, 0x00, 0x00));

			foreach (var child in invoiceTabButton.Children)
			{
				if (child is Button button)
				{
					button.Foreground = (button == clickedButton) ? buttonBrush : blackForegroundBrush;
					button.BorderBrush = (button == clickedButton) ? buttonBrush : transparentBrush;
				}
			}
		}

	}
}
