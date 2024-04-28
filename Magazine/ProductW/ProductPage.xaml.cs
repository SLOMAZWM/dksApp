using dksApp.Magazine.MagazineDataGrid;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace dksApp.Magazine.ProductW
{
    /// <summary>
    /// Interaction logic for ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        private MainWindow _mainWindow;
        private dksApp.Product editProduct;
        private bool editetProduct;
        public ProductPage(MainWindow mainWindow) //Create New Product
        {
            InitializeComponent();
            editetProduct = false;
            TitleNameTxt.Text = "Dodawanie Produktu";
            ConfirmBtnTxt.Text = "Zapisz Produkt";
            _mainWindow = mainWindow;
        }

        public ProductPage(ref bool ed, ref dksApp.Product p, MainWindow mainWindow) //Edit Product
        {
            InitializeComponent();
            editetProduct = ed;
            editProduct = p;
            InitializeEditWindow(ref p);
            TitleNameTxt.Text = "Edytowanie Produktu";
            ConfirmBtnTxt.Text = "Edytuj Produkt";
            _mainWindow = mainWindow;
        }

        private void InitializeEditWindow(ref dksApp.Product product)
        {
            ProductNameTxt.Text = product.NameItem;
            TypeAmountTxt.Text = product.QuantityType;
            PKWiUTxt.Text = product.PKWiU;
            NettoOneTxt.Text = product.NettoPrice.ToString();
            VatTxt.Text = product.VATPercent;
            decimal ValueVat = ProductServiceDataGrid.CalculateValueVAT(product.NettoPrice, Convert.ToDecimal(product.VATPercent));
            ValueVatTxt.Text = ValueVat.ToString();
            ProductServiceDataGrid.CalculateValueBrutto(product.NettoPrice, ValueVat);
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
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


        private void RememberProductBtn_Click(object sender, RoutedEventArgs e)
        {
            if (editetProduct == false)
            {
                if (!AreAllInputsValid())
                {
                    MessageBox.Show("Wypełnij wszystkie pola!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                else
                {
                    dksApp.Product newProduct = new dksApp.Product();
                    try
                    {
                        newProduct.NameItem = ProductNameTxt.Text;
                        newProduct.QuantityType = TypeAmountTxt.Text;
                        newProduct.PKWiU = PKWiUTxt.Text;
                        newProduct.NettoPrice = decimal.Parse(NettoOneTxt.Text);
                        newProduct.VATPercent = VatTxt.Text;
                        newProduct.VATValue = decimal.Parse(ValueVatTxt.Text);
                        newProduct.BruttoValue = decimal.Parse(ValueBruttoTxt.Text);
                        newProduct.ShowIt = true;

                        if (newProduct != null)
                        {
                            ProductServiceDataGrid.AddProductToDataBase(newProduct);

                            MessageBox.Show("Poprawnie dodano produkt do magazynu!", "Poprawny zapis produktu", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Wypełnij wszystkie pola w produkcie!", "Błąd wypełnienia", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Błąd dodawania produktu do bazy danych: " + ex.Message, "Błąd bazy danych", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                if (!AreAllInputsValid())
                {
                    MessageBox.Show("Wypełnij wszystkie pola!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                else
                {
                    {
                        try
                        {
                            editProduct.NameItem = ProductNameTxt.Text;
                            editProduct.QuantityType = TypeAmountTxt.Text;
                            editProduct.PKWiU = PKWiUTxt.Text;
                            editProduct.NettoPrice = decimal.Parse(NettoOneTxt.Text);
                            editProduct.VATPercent = VatTxt.Text;
                            editProduct.VATValue = decimal.Parse(ValueVatTxt.Text);
                            editProduct.BruttoValue = decimal.Parse(ValueBruttoTxt.Text);

                            if (editProduct != null)
                            {
                                ProductServiceDataGrid.UpdateProductDataBase(editProduct);

                                MessageBox.Show("Poprawnie zmieniono produkt w magazynie!", "Poprawny zapis produktu", MessageBoxButton.OK, MessageBoxImage.Information);

                                MainMagazineDataGrid.Instance.InitializeAsync();

                                //this.Close();
                            }
                            else
                            {
                                MessageBox.Show("Wypełnij wszystkie pola w produkcie!", "Błąd wypełnienia", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        catch (SqlException ex)
                        {
                            MessageBox.Show("Błąd edycji produktu" + ex.Message, "Błąd bazy danych", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }

                }
            }
        }

        private bool AreAllInputsValid()
        {
            return !string.IsNullOrWhiteSpace(ProductNameTxt.Text) &&
                           !string.IsNullOrWhiteSpace(TypeAmountTxt.Text) &&
                           !string.IsNullOrWhiteSpace(NettoOneTxt.Text) &&
                           !string.IsNullOrWhiteSpace(VatTxt.Text) &&
                           !string.IsNullOrWhiteSpace(ValueVatTxt.Text) &&
                           !string.IsNullOrWhiteSpace(ValueBruttoTxt.Text);
        }

        private void ValueVATAndBrutto_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (decimal.TryParse(NettoOneTxt.Text, out decimal nettoPrice))
            {
                if (decimal.TryParse(VatTxt.Text, out decimal vatPercent))
                {
                    if (vatPercent > 100)
                    {
                        vatPercent = 100;
                        VatTxt.Text = vatPercent.ToString();
                    }

                    decimal vatValue = ProductServiceDataGrid.CalculateValueVAT(nettoPrice, vatPercent);
                    ValueVatTxt.Text = vatValue.ToString();

                    decimal bruttoValue = ProductServiceDataGrid.CalculateValueBrutto(nettoPrice, vatValue);
                    ValueBruttoTxt.Text = bruttoValue.ToString();
                }
                else
                {
                    ValueVatTxt.Text = "0";
                    ValueBruttoTxt.Text = "0";
                }
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
    }
}
