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

namespace dksApp.Bookkeeping.Invoice
{
	/// <summary>
	/// Interaction logic for EditInvoiceWindow.xaml
	/// </summary>
	public partial class EditInvoiceWindow : Window
	{
		public InvoiceClass EditInvoice { get; set; }
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

		public EditInvoiceWindow()
		{
			InitializeComponent();
			EditInvoice = new InvoiceClass();
			//gridPage = 
		}

		//private Dictionary<string, Page> InitializePages()
		//{
		//	Dictionary<string, Page> newPages = new Dictionary<string, Page>()
		//	{
		//		{"Sprzedawca",  }
		//	};
		//}

	}
}
