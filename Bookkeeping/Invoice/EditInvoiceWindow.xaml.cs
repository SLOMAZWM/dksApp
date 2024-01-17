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
using dksApp.Bookkeeping.Invoice.InvoicePages;

namespace dksApp.Bookkeeping.Invoice
{
    /// <summary>
    /// Logika interakcji dla klasy EditInvoiceWindow.xaml
    /// </summary>
    public partial class EditInvoiceWindow : Window
    {
		private NavigatorManager navigator;
		Dictionary<string, Page> Pages = new Dictionary<string, Page>();
		public InvoiceClass EditInvoice { get; set; }

		public EditInvoiceWindow()
        {
            InitializeComponent();
			InitializePages(Pages);
        }

		//Navigation Manager
		private Dictionary<string, Page> InitializePages(Dictionary<string, Page> pages)
		{
			Dictionary<string, Page> newPage = new Dictionary<string, Page>()
			{
				{"Sprzedawca", new SellerInvoicePage(this)},
				{"Nabywca", new CompanyBuyerPage(this)},
				{"Informacje", new InformationInvoicePage(this)},

			};

			return newPage;
		}

		private void NavigationButton_Click(object sender,  RoutedEventArgs e)
		{
			if(sender is Button button && button.Tag is string gridName) 
			{
				navigator.ChangeInvoiceTabButton(button);
				navigator.NavigateToPage(gridName);
			}
		}

		//frontend settings

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

		private void ExitBtn_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
	}
}
