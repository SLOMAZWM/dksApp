using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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

namespace dksApp.Contractors.AddContractorsW
{
	/// <summary>
	/// Interaction logic for AddBuyerW.xaml
	/// </summary>
	public partial class AddBuyerW : Window
	{
		private Buyer editBuyer = new Buyer();
		private bool editetBuyer;
		public AddBuyerW()
		{
			InitializeComponent();
		}

		public AddBuyerW(ref bool ed, ref Buyer buyer)
		{
			InitializeComponent();
			editetBuyer = ed;
			editBuyer = buyer;
			//InitializeEditWindow
		}

		private bool IsEmpty()
		{
			if (string.IsNullOrEmpty(BuyerNameTxt.Text))
			{
				return true;
			}
			else if (string.IsNullOrEmpty(BuyerZipCodeTxt.Text))
			{
				return true;
			}
			else if (string.IsNullOrEmpty(BuyerCity.Text))
			{
				return true;
			}
			else if (string.IsNullOrEmpty(BuyerStreet.Text))
			{
				return true;
			}
			else if (string.IsNullOrEmpty(BuyerNIP.Text))
			{
				return true;
			}
			else if (string.IsNullOrEmpty(BuyerBankName.Text))
			{
				return true;
			}
			else if (string.IsNullOrEmpty(BuyerBankAccount.Text))
			{
				return true;
			}
			else if (string.IsNullOrEmpty(NameToSave.Text))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		private void SaveBuyerBtn_Click(object sender, RoutedEventArgs e)
		{
			if (editetBuyer == false)
			{
				if (IsEmpty())
				{
					MessageBox.Show("Wypełnij wszystkie pola!", "Błąd!", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}
				else
				{
					Buyer newBuyer = new Buyer();
					try
					{
						newBuyer.BuyerName = BuyerNameTxt.Text;
						newBuyer.BuyerStreet = BuyerStreet.Text;
						newBuyer.BuyerCity = BuyerCity.Text;
						newBuyer.BuyerZipCode = BuyerZipCodeTxt.Text;
						newBuyer.BuyerNIP = BuyerNIP.Text;
						newBuyer.BuyerBankName = BuyerBankName.Text;
						newBuyer.BuyerBankAccount = BuyerBankAccount.Text;
						newBuyer.BuyerTitle = NameToSave.Text;

						if(newBuyer != null) 
						{
							ContractorsServiceDataGrid.AddBuyerToDataBase(newBuyer);

							MessageBox.Show("Poprawnie dodano kupującego do kontrahentów!", "Poprawny zapis kontrahenta", MessageBoxButton.OK, MessageBoxImage.Information);

							this.Close();
						}
						else
						{
							MessageBox.Show("Wypełnij wszystkie pola w okienku!", "Błąd wypełnienia", MessageBoxButton.OK, MessageBoxImage.Error);
						}
					}
					catch (SqlException ex)
					{
						MessageBox.Show("Błąd dodawania nabywcy do bazy danych: " + ex.Message, "Błąd bazy danych", MessageBoxButton.OK, MessageBoxImage.Error);
					}
				}
			}

		}


		private void CancelBtn_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		//front

		private void Border_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
			{
				this.DragMove();
			}
		}


	}
}
