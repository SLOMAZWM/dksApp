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

        private bool AreTextBoxesEmpty(Grid grid)
        {
            foreach (UIElement element in grid.Children) 
            {
                if (element is StackPanel stackPanel)
                {
                    foreach(var child in stackPanel.Children)
                    {
                        if(child is TextBox textBox && string.IsNullOrWhiteSpace(textBox.Text))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private void AddProductBtn_Click(object sender, RoutedEventArgs e)
        {
            if(AreTextBoxesEmpty(InputGrid) == false) 
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

                if (Product.isEmpty() == false && Product.isZero() == false)
                {
                    IsCreated = true;
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Wypełnij wszystkie pola!", "Błąd wypełnienia", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
