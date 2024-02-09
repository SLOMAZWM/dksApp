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
		public InvoiceClass DetailedInvoice { get; set; }
		public InvoiceWindow(InvoiceClass inv)
        {
            InitializeComponent();
			DetailedInvoice = inv;
			InitializeInvoiceData();
		}

		private void InitializeInvoiceData()
		{
			//Seller Info
			SellerNameTxt.Text = DetailedInvoice.SellerName;
			SellerStreetTxt.Text= DetailedInvoice.SellerStreet;
			SellerZipCodeTxt.Text = DetailedInvoice.SellerZipCode;
			SellerCityTxt.Text = DetailedInvoice.SellerCity;
			SellerNIPTxt.Text = DetailedInvoice.SellerNIP;
			SellerBankNumberTxt.Text = DetailedInvoice.SellerBankAccount;

			//Buyer Info
			BuyerNameTxt.Text = DetailedInvoice.BuyerName;
			BuyerStreetTxt.Text = DetailedInvoice.BuyerStreet;
			BuyerZipCodeTxt.Text = DetailedInvoice.BuyerZipCode;
			BuyerCityTxt.Text = DetailedInvoice.BuyerCity;
			BuyerNIPTxt.Text = DetailedInvoice.BuyerNIP;

			//Warning Display

			WarningTxt.Text = DetailedInvoice.Comments;

			//Products
			InitializeProductList();

			//Information

			PaymentTypeTxt.Text = DetailedInvoice.PaymentType;
			PaymentDateTxt.Text = DetailedInvoice.PaymentDate;
			PaidTxt.Text = DetailedInvoice.Paid.ToString();
			PaidYetTxt.Text = CalculateToPay();
			DateIssueTxt.Text = DetailedInvoice.IssueDate;
			DateDeliveryTxt.Text = DetailedInvoice.ExecutionDate;

			BruttoValueTxt.Text = DetailedInvoice.PaidYet.ToString();
		}

		private void InitializeProductList()
		{
			productsDataGrid.ItemsSource = DetailedInvoice.Products;
		}

		private string CalculateBruttoValue()
		{
			decimal BruttoValue = DetailedInvoice.Products.Sum(product => product.BruttoValue);

            string CalculatedBruttoValue = BruttoValue.ToString();

			return CalculatedBruttoValue;
		}

		private string CalculateToPay()
		{
			string BruttoValue = CalculateBruttoValue();

			decimal CalculateToPaidYet = Convert.ToDecimal(BruttoValue) - DetailedInvoice.Paid;

			return CalculateToPaidYet.ToString();
		}

		private void PrintBtn_Click(object sender, RoutedEventArgs e)
		{
			var firstRow = MainGrid.Children
							.Cast<UIElement>()
							.Where(x => Grid.GetRow(x) == 0);
			foreach (var element in firstRow)
			{
				element.Visibility = Visibility.Collapsed;
			}

			
			int dpi = 400; 
			RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(
				(int)(MainGrid.ActualWidth * dpi / 96.0),
				(int)(MainGrid.ActualHeight * dpi / 96.0),
				dpi, dpi,
				PixelFormats.Pbgra32);
			renderTargetBitmap.Render(MainGrid);

			var scale = dpi / 96.0;
			MainGrid.LayoutTransform = new ScaleTransform(scale, scale);

			
			foreach (var element in firstRow)
			{
				element.Visibility = Visibility.Visible;
			}

			
			PngBitmapEncoder pngImage = new PngBitmapEncoder();
			pngImage.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
			MemoryStream imageStream = new MemoryStream();
			pngImage.Save(imageStream);
			imageStream.Position = 0;

			
			PdfDocument document = new PdfDocument();
			PdfPage page = document.AddPage();
			page.Width = renderTargetBitmap.Width;
			page.Height = renderTargetBitmap.Height;
			XGraphics gfx = XGraphics.FromPdfPage(page);
			XImage image = XImage.FromStream(imageStream);

			gfx.DrawImage(image, 0, -MainGrid.RowDefinitions[0].ActualHeight, page.Width, page.Height);

			string fileName = "Faktura.pdf";
			document.Save(fileName);

			string filePath = System.IO.Path.Combine(Environment.CurrentDirectory, fileName);
			System.Diagnostics.Process.Start("explorer.exe", filePath);

			MainGrid.LayoutTransform = null;
		}

		private void exitBtn_Click(object sender, RoutedEventArgs e)
		{
            this.Close();
		}

		private void Border_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
			{
				this.DragMove();
			}
		}
	}
}
