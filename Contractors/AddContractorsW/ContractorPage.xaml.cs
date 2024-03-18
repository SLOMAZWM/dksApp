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

namespace dksApp.Contractors.AddContractorsW
{
    /// <summary>
    /// Interaction logic for ContractorPage.xaml
    /// </summary>
    public partial class ContractorPage : Page
    {
        private MainWindow _mainWindow;
        private Buyer editBuyer = new Buyer();
        private bool editetBuyer;
        public ContractorPage(MainWindow mainWindow) //Add Contractor
        {
            InitializeComponent();
            editetBuyer = false;
            TitleNameTxt.Text = "Dodawanie Kontrahenta";
            _mainWindow = mainWindow;
        }

        public ContractorPage(Buyer buyer) //Edit Contractor
        {
            InitializeComponent();
            editBuyer = buyer;
            editetBuyer = true;
            InitializeEditWindow();
            TitleNameTxt.Text = "Edytowanie Kontrahenta";
        }

        private void InitializeEditWindow()
        {
            BuyerNameTxt.Text = editBuyer.BuyerName;
            BuyerCity.Text = editBuyer.BuyerCity;
            BuyerStreet.Text = editBuyer.BuyerStreet;
            BuyerZipCodeTxt.Text = editBuyer.BuyerZipCode;
            BuyerNIP.Text = editBuyer.BuyerNIP;
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
                        if (newBuyer.MinimalLettersNip(newBuyer.BuyerNIP) == false)
                        {
                            return;
                        }
                        else
                        {
                            if (newBuyer != null)
                            {
                                ContractorsServiceDataGrid.AddBuyerToDataBase(newBuyer);

                                MessageBox.Show("Poprawnie dodano kupującego do kontrahentów!", "Poprawny zapis kontrahenta", MessageBoxButton.OK, MessageBoxImage.Information);

                                //this.Close();
                            }
                            else
                            {
                                MessageBox.Show("Wypełnij wszystkie pola w okienku!", "Błąd wypełnienia", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Błąd dodawania nabywcy do bazy danych: " + ex.Message, "Błąd bazy danych", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                if (IsEmpty())
                {
                    MessageBox.Show("Wypełnij wszystkie pola!", "Błąd!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    try
                    {
                        editBuyer.BuyerName = BuyerNameTxt.Text;
                        editBuyer.BuyerStreet = BuyerStreet.Text;
                        editBuyer.BuyerCity = BuyerCity.Text;
                        editBuyer.BuyerZipCode = BuyerZipCodeTxt.Text;
                        editBuyer.BuyerNIP = BuyerNIP.Text;

                        if (editBuyer != null)
                        {
                            ContractorsServiceDataGrid.UpdateBuyerInDatabase(editBuyer);

                            //this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Wypełnij wszystkie pola w okienku!", "Błąd wypełnienia", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (SqlException ex)
                    {
                        //MessageBox.Sh
                    }
                }

            }

        }

        private void TextOnlyNumber_Check(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9,-]+");
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

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            //this.Close();
        }

    }
}
