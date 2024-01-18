using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Xps.Packaging;
using System.Windows.Xps;
using System.Xml;


namespace dksApp.Bookkeeping.Invoice
{
    /// <summary>
    /// Interaction logic for InvoiceWindow.xaml
    /// </summary>
    public partial class InvoiceWindow : Window
    {
        //public InvoiceClass DetailedInvoice { get; set; }
        public InvoiceWindow(InvoiceClass inv)
        {
            InitializeComponent();
            //DetailedInvoice = inv;
        }

		public InvoiceWindow()
		{
			InitializeComponent();
		}

		private void PrintBtn_Click(object sender, RoutedEventArgs e)
		{
			// Przygotuj treść faktury w formie FlowDocument
			FlowDocument flowDocument = new FlowDocument();

			// Dodaj treść faktury do FlowDocument
			Paragraph paragraph = new Paragraph();
			paragraph.Inlines.Add(new Run("Tutaj umieść treść faktury."));
			flowDocument.Blocks.Add(paragraph);

			// Utwórz drukowalny obiekt do użycia w PrintDialog
			IDocumentPaginatorSource paginatorSource = flowDocument as IDocumentPaginatorSource;
			if (paginatorSource != null)
			{
				PrintDialog printDialog = new PrintDialog();
				if (printDialog.ShowDialog() == true)
				{
					printDialog.PrintDocument(paginatorSource.DocumentPaginator, "Faktura");
				}
			}
		}


		private void exitBtn_Click(object sender, RoutedEventArgs e)
		{
            this.Close();
		}
	}
}
