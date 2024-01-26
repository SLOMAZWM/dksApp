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
using System.Windows.Shapes;

namespace dksApp.Magazine.Product
{
    /// <summary>
    /// Interaction logic for ProductWindow.xaml
    /// </summary>
    public partial class ProductWindow : Window
    {
        public ProductWindow()
        {
            InitializeComponent();
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

		private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left && e.ButtonState == MouseButtonState.Pressed)
			{
				this.DragMove();
			}
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

		private bool AreAllInputsValid()
		{
			return !string.IsNullOrWhiteSpace(ProductNameTxt.Text) &&
						   !string.IsNullOrWhiteSpace(TypeAmountTxt.Text) &&
						   !string.IsNullOrWhiteSpace(PKWiUTxt.Text) &&
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
						VatTxt.Text = "100";
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



	}
}
