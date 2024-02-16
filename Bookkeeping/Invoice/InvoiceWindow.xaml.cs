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
            SellerStreetTxt.Text = DetailedInvoice.SellerStreet;
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
            PaidTxt.Text = $"{DetailedInvoice.Paid.ToString()} PLN";
            PaidYetTxt.Text = $"{CalculateToPay()} PLN";
            DateIssueTxt.Text = DetailedInvoice.IssueDate;
            DateDeliveryTxt.Text = DetailedInvoice.ExecutionDate;
            InvoiceNumberTxt.Text = DetailedInvoice.InvoiceNumber;
            BruttoValueTxt.Text = $"{DetailedInvoice.PaidYet.ToString()} PLN";
            BruttoValueInWordsTxt.Text = NumberToWord(DetailedInvoice.PaidYet);
        }

        private string NumberToWord(decimal number)
        {
            if (number == 0) return "zero złotych";

            var words = new StringBuilder();
            var zlotowki = (int)number;
            var grosze = (int)((number - zlotowki) * 100);

            words.Append(NumberToWordsInternal(zlotowki));
            words.Append(" ");
            words.Append(ChooseCorrectForm(zlotowki, "złoty", "złote", "złotych"));

            if (grosze > 0)
            {
                words.Append(" i ");
                words.Append(NumberToWordsInternal(grosze));
                words.Append(" ");
                words.Append(ChooseCorrectForm(grosze, "grosz", "grosze", "groszy"));
            }

            return words.ToString().Trim();
        }

        private string NumberToWordsInternal(int number)
        {
            if (number == 0) return "zero";

            var jednosci = new[] { "", "jeden", "dwa", "trzy", "cztery", "pięć", "sześć", "siedem", "osiem", "dziewięć" };
            var nascie = new[] { "dziesięć", "jedenaście", "dwanaście", "trzynaście", "czternaście", "piętnaście", "szesnaście", "siedemnaście", "osiemnaście", "dziewiętnaście" };
            var dziesiatki = new[] { "", "dziesięć", "dwadzieścia", "trzydzieści", "czterdzieści", "pięćdziesiąt", "sześćdziesiąt", "siedemdziesiąt", "osiemdziesiąt", "dziewięćdziesiąt" };
            var setki = new[] { "", "sto", "dwieście", "trzysta", "czterysta", "pięćset", "sześćset", "siedemset", "osiemset", "dziewięćset" };
            var grupy = new[,]
            {
            {"", "", "" },
            { "tysiąc", "tysiące", "tysięcy" },
            { "milion", "miliony", "milionów" },
            { "miliard", "miliardy", "miliardów" }
        };

            var words = new List<string>();
            int groupNumber = 0;

            while (number > 0)
            {
                var temp = number % 1000;
                number /= 1000;

                if (temp > 0)
                {
                    var groupWords = new List<string>();

                    var hundreds = temp / 100;
                    var tens = (temp % 100) / 10;
                    var units = temp % 10;

                    if (hundreds > 0)
                    {
                        groupWords.Add(setki[hundreds]);
                    }

                    if (tens >= 2)
                    {
                        groupWords.Add(dziesiatki[tens]);
                        if (units > 0)
                        {
                            groupWords.Add(jednosci[units]);
                        }
                    }
                    else if (temp % 100 >= 10)
                    {
                        groupWords.Add(nascie[temp % 10]);
                    }
                    else if (units > 0)
                    {
                        groupWords.Add(jednosci[units]);
                    }

                    if (groupNumber > 0)
                    {
                        var grupa = grupy[Math.Min(groupNumber, grupy.GetLength(0) - 1), GetGroupIndex(units, tens, hundreds)];
                        if (!string.IsNullOrEmpty(grupa))
                        {
                            groupWords.Add(grupa);
                        }
                    }

                    words.InsertRange(0, groupWords);
                }

                groupNumber++;
            }

            return string.Join(" ", words);
        }

        private int GetGroupIndex(int units, int tens, int hundreds)
        {
            if (units == 1 && tens != 1 && hundreds != 1) return 0;
            if (units >= 2 && units <= 4 && tens != 1) return 1;
            return 2;
        }

        private string ChooseCorrectForm(int number, string form1, string form2, string form3)
        {
            if (number % 100 >= 12 && number % 100 <= 14)
            {
                return form3;
            }

            switch (number % 10)
            {
                case 1:
                    return form1;
                case 2:
                case 3:
                case 4:
                    return form2;
                default:
                    return form3;
            }
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
