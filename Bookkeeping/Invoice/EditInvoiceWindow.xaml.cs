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
using System.Windows.Shapes;
using dksApp.Bookkeeping.Invoice.InvoicePages.EditPage;
using dksApp.Bookkeeping.Invoice.InvoicePages.EditPage.Products;

namespace dksApp.Bookkeeping.Invoice
{
	/// <summary>
	/// Interaction logic for EditInvoiceWindow.xaml
	/// </summary>
	public partial class EditInvoiceWindow : Window
	{
		public InvoiceClass NewInvoice { get; set; }
		private NavigatorManager navigator { get; set; }
		private Dictionary<string, Page> gridPage = new Dictionary<string, Page>();
		public NavigatorManager Navigator
		{
			get
			{
				return navigator;
			}
			set { navigator = value; }
		}

		public EditInvoiceWindow(InvoiceClass editInvoice)
		{
			NewInvoice = editInvoice;
			gridPage = InitializePages();
			InitializeComponent();
			navigator = new NavigatorManager(gridPage, tabButtonSP, GridFrame);
		}

		private Dictionary<string, Page> InitializePages()
		{
			Dictionary<string, Page> newPages = new Dictionary<string, Page>()
			{
				{"Sprzedawca", new SellerEditPage(this)},
				{"Nabywca", new BuyerEditPage(this) },
				{"Informacje", new InformationEditPage(this) },
				{"Produkty", new ProductsEditPage(this)
        }
			};

			return newPages;
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
