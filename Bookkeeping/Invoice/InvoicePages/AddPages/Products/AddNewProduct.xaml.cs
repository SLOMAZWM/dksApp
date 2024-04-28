using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace dksApp.Bookkeeping.Invoice.InvoicePages.AddPages.Products
{
    /// <summary>
    /// Interaction logic for AddNewProduct.xaml
    /// </summary>
    /// 
    public partial class AddNewProduct : Window
    {
        public bool IsCreated { get; set; }
        Product Product { get; set; }
        public AddNewProduct(Product p)
        {
            InitializeComponent();
            Product = p;
            IsCreated = false;
            InitializeProduct(Product);
        }

        private void InitializeProduct(Product Product)
        {
            ProductNameTxt.Text = Product.NameItem;
            TypeAmountTxt.Text = Product.QuantityType;
            AmountTxt.Text = Convert.ToString(Product.Quantity);
            PKWiUTxt.Text = Product.PKWiU;
            NettoOneTxt.Text = Convert.ToString(Product.NettoPrice);
            ValueNettoTxt.Text = Convert.ToString(Product.NettoValue);
            VatTxt.Text = Product.VATPercent;
            ValueVatTxt.Text = Convert.ToString(Product.VATValue);
            ValueBruttoTxt.Text = Convert.ToString(Product.BruttoValue);
        }

        //frontSETTINGS
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
                    this.Height = 720;

                    IsMaximized = false;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;

                    IsMaximized = true;
                }
            }
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TextOnlyNumber_Check(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9,]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TextOnlyNumber_CheckPaste(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!IsTextAllowed(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9,.]+");
            return !regex.IsMatch(text);
        }

        private void GetAllPropertiesFromInputs()
        {
            Product.NameItem = ProductNameTxt.Text;
            Product.QuantityType = TypeAmountTxt.Text;
            Product.Quantity = double.Parse(AmountTxt.Text);
            Product.PKWiU = PKWiUTxt.Text;
            Product.NettoPrice = Convert.ToDecimal(NettoOneTxt.Text);
            Product.NettoValue = Convert.ToDecimal(ValueNettoTxt.Text);
            Product.VATPercent = VatTxt.Text;
            Product.VATValue = Convert.ToDecimal(ValueVatTxt.Text);
            Product.BruttoValue = Convert.ToDecimal(ValueBruttoTxt.Text);
        }

        private void AddProductBtn_Click(object sender, RoutedEventArgs e)
        {
            GetAllPropertiesFromInputs();
            if(Product.isEmpty() == false)
            {
                Product.NameItem = ProductNameTxt.Text;
                Product.QuantityType = TypeAmountTxt.Text;
                Product.Quantity = double.Parse(AmountTxt.Text);
                Product.PKWiU = PKWiUTxt.Text;
                Product.NettoPrice = Convert.ToDecimal(NettoOneTxt.Text);
                Product.NettoValue = Convert.ToDecimal(ValueNettoTxt.Text);
                Product.VATPercent = VatTxt.Text;
                Product.VATValue = Convert.ToDecimal(ValueVatTxt.Text);
                Product.BruttoValue = Convert.ToDecimal(ValueBruttoTxt.Text);

                if (Product.isZero() == false)
                {
                    IsCreated = true;
                    this.Close();
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }

        private void PlusAmountBtn_Click(object sender, RoutedEventArgs e)
        {
            if(AmountTxt.Text != string.Empty)
            {
                decimal amount = Convert.ToDecimal(AmountTxt.Text);
                amount++;
                AmountTxt.Text = amount.ToString();
            }
            else
            {
                return;
            }
        }

        private void MinusAmountBtn_Click(object sender, RoutedEventArgs e)
        {
            if(AmountTxt.Text != string.Empty)
            {
                decimal amount = Convert.ToDecimal(AmountTxt.Text);
                amount--;
                AmountTxt.Text = amount.ToString();
            }
            else
            {
                return;
            }
        }

        private void TypeAmountChb_UnChecked(object sender, RoutedEventArgs e)
        {
            TypeAmountTxt.Text = "";
            TypeAmountTxt.IsEnabled = true;
            TypeAmountTxt.Background = new SolidColorBrush(Colors.White);
            TypeAmountTxt.Foreground = new SolidColorBrush(Colors.Black);
        }

        private void TypeAmountChb_Checked(object sender, RoutedEventArgs e)
        {
            TypeAmountTxt.Text = "Szt.";
            TypeAmountTxt.IsEnabled = false;
            TypeAmountTxt.Background = new SolidColorBrush(Colors.Gray);
            TypeAmountTxt.Foreground = new SolidColorBrush(Colors.White);
        }

        private string CalculateValueNetto(string Amount, string Price)
        {
            decimal amount = Convert.ToDecimal(Amount);
            decimal price = Convert.ToDecimal(Price);

            decimal valueNetto = price * amount;
            string ValueNetto = valueNetto.ToString();
            return ValueNetto;
        }

        private string CalculateValueVat(string Vat, string Price, string Amount)
        {
            decimal vat = Convert.ToDecimal(Vat) / 100;
            decimal price = Convert.ToDecimal(Price);
            decimal amount = Convert.ToDecimal(Amount);

            decimal valueVat = price * amount * vat;
            string ValueVat = valueVat.ToString();

            return ValueVat;
        }

        private void CalculateAmountFromTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(!string.IsNullOrEmpty(VatTxt.Text)) 
            {
                decimal vatAmount = Convert.ToDecimal(VatTxt.Text);

                if (vatAmount > 100)
                {
                    vatAmount = 100;
                    VatTxt.Text = vatAmount.ToString();
                }
                else if (vatAmount < 0)
                {
                    vatAmount = 0;
                    VatTxt.Text = vatAmount.ToString();
                }
            }

            if (AmountTxt.Text != string.Empty && NettoOneTxt.Text != string.Empty && VatTxt.Text != string.Empty)
            {
                ValueNettoTxt.Text = CalculateValueNetto(AmountTxt.Text, NettoOneTxt.Text);
                ValueVatTxt.Text = CalculateValueVat(VatTxt.Text, NettoOneTxt.Text, AmountTxt.Text);
                ValueBruttoTxt.Text = (Convert.ToDecimal(ValueVatTxt.Text) + Convert.ToDecimal(ValueNettoTxt.Text)).ToString();
            }
            else
            {
                return;
            }
        }

        private void RememberProductBtn_Click(object sender, RoutedEventArgs e)
        {
            dksApp.Product rememberProduct = new dksApp.Product();
            rememberProduct.ShowIt = true;

            rememberProduct.NameItem = ProductNameTxt.Text;
            rememberProduct.QuantityType = TypeAmountTxt.Text;
            rememberProduct.Quantity = Convert.ToDouble(AmountTxt.Text);
            rememberProduct.PKWiU = PKWiUTxt.Text;
            rememberProduct.NettoPrice = Convert.ToDecimal(NettoOneTxt.Text);
            rememberProduct.NettoValue = Convert.ToDecimal(ValueNettoTxt.Text);
            rememberProduct.VATPercent = VatTxt.Text;
            rememberProduct.VATValue = Convert.ToDecimal(ValueVatTxt.Text);
            rememberProduct.BruttoValue = Convert.ToDecimal(ValueBruttoTxt.Text);
            rememberProduct.NumberOfItems = 1;

            if(rememberProduct.isEmpty() == false && rememberProduct.isZero() == false)
            {
                ProductServiceDataGrid.AddProductToDataBase(rememberProduct);
                MessageBox.Show("Poprawnie zapisano produkt do bazy danych!", "Poprawny zapis", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Uzupełnij wszystkie pola!", "Błąd wypełnienia", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
