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
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Fonts;
using System.Runtime.Intrinsics.Arm;


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
			// Ukryj Grid.Row=0
			var firstRow = MainGrid.Children
							.Cast<UIElement>()
							.Where(x => Grid.GetRow(x) == 0);
			foreach (var element in firstRow)
			{
				element.Visibility = Visibility.Collapsed;
			}

			// Renderowanie WPF UI do obrazu
			int dpi = 300; // Wyższa wartość DPI dla lepszej jakości
			RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(
				(int)(MainGrid.ActualWidth * dpi / 96.0),
				(int)(MainGrid.ActualHeight * dpi / 96.0),
				dpi, dpi,
				PixelFormats.Pbgra32);
			renderTargetBitmap.Render(MainGrid);

			var scale = dpi / 96.0;
			MainGrid.LayoutTransform = new ScaleTransform(scale, scale);

			// Przywróć widoczność Grid.Row=0
			foreach (var element in firstRow)
			{
				element.Visibility = Visibility.Visible;
			}

			// Zapisanie obrazu do strumienia
			PngBitmapEncoder pngImage = new PngBitmapEncoder();
			pngImage.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
			MemoryStream imageStream = new MemoryStream();
			pngImage.Save(imageStream);
			imageStream.Position = 0;

			// Tworzenie nowego dokumentu PDF
			PdfDocument document = new PdfDocument();
			PdfPage page = document.AddPage();
			page.Width = renderTargetBitmap.Width;
			page.Height = renderTargetBitmap.Height;
			XGraphics gfx = XGraphics.FromPdfPage(page);
			XImage image = XImage.FromStream(imageStream);

			// Dodanie obrazu do dokumentu PDF, pomijając Grid.Row=0
			gfx.DrawImage(image, 0, -MainGrid.RowDefinitions[0].ActualHeight, page.Width, page.Height);

			// Zapisywanie dokumentu PDF do pliku
			string fileName = "Faktura.pdf";
			document.Save(fileName);

			// Otwieranie pliku PDF w domyślnej przeglądarce PDF
			string filePath = System.IO.Path.Combine(Environment.CurrentDirectory, fileName);
			System.Diagnostics.Process.Start("explorer.exe", filePath);

			MainGrid.LayoutTransform = null;
		}



		private void exitBtn_Click(object sender, RoutedEventArgs e)
		{
            this.Close();
		}
	}
}
