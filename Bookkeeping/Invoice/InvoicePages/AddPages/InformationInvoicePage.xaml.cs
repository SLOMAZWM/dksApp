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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace dksApp.Bookkeeping.Invoice.InvoicePages
{
    /// <summary>
    /// Interaction logic for InformationInvoicePage.xaml
    /// </summary>
    public partial class InformationInvoicePage : Page
    {
        private CreateInvoiceFrame? parentWindow;

        public InformationInvoicePage(CreateInvoiceFrame parentF)
        {
            parentWindow = parentF;
            InitializeComponent();
            CommentsTxt.Text = "Brak";
        }

        private void TodayCHB_Checked(object sender, RoutedEventArgs e)
        {
            string issueDate = DateTime.Now.ToString("yyyy-MM-dd");
            parentWindow.NewInvoice.IssueDate = issueDate;
            IssueDate.Text = issueDate;
            IssueDate.IsEnabled = false;
        }

        private void TodayCHB_Unchecked(object sender, RoutedEventArgs e)
        {
            IssueDate.IsEnabled = true;
        }

        private void OnCommentsChb_Checked(object sender, RoutedEventArgs e)
        {
            CommentsTxt.IsEnabled = true;
        }

        private void OnCommentsChb_Unchecked(object sender, RoutedEventArgs e)
        {
            CommentsTxt.IsEnabled = false;
        }

        private void PreviousPageBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                parentWindow.WhichBuyer_Click(sender, e);
                parentWindow.HighlightBuyerButton();
            }
            catch
            {
                MessageBox.Show("Błąd nawigacji podstrony, skontaktuj się z administratorem aplikacji!", "Krytyczny błąd Nawigacji", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void NextPageBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                parentWindow.Navigator.NavigateToGrid("Produkty");
                parentWindow.HighlightProductButton();
            }
            catch
            {
                MessageBox.Show("Błąd nawigacji podstrony, skontaktuj się z administratorem aplikacji!", "Krytyczny błąd Nawigacji", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void IssueDate_TextChanged(object sender, TextChangedEventArgs e)
        {
            parentWindow.NewInvoice.IssueDate = IssueDate.Text;
        }

        private void ExecutionDateTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            parentWindow.NewInvoice.ExecutionDate = ExecutionDateTxt.Text;
        }

        private void PaidYet_TextChanged(object sender, TextChangedEventArgs e)
        {
            parentWindow.NewInvoice.Paid = Convert.ToDecimal(PaidYet.Text);
        }

        private void CommentsTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            parentWindow.NewInvoice.Comments = CommentsTxt.Text;
        }

        private void Days7PayCHB_Unchecked(object sender, RoutedEventArgs e)
        {
            PaymentDateTxt.IsEnabled = true;
        }

        private void Days14PayCHB_Unchecked(object sender, RoutedEventArgs e)
        {
            PaymentDateTxt.IsEnabled = true;
        }

        private void Days30PayCHB_Unchecked(object sender, RoutedEventArgs e)
        {
            PaymentDateTxt.IsEnabled = true;
        }

        private void Days30PayCHB_Checked(object sender, RoutedEventArgs e)
        {
            string issueDatePlus30Days = DateTime.Now.AddDays(30).ToString("yyyy-MM-dd");
            PaymentDateTxt.Text = issueDatePlus30Days;
            Days7PayCHB.IsChecked = false;
            Days14PayCHB.IsChecked = false;
            PaymentDateTxt.IsEnabled = false;
        }

        private void Days14PayCHB_Checked(object sender, RoutedEventArgs e)
        {
            string issueDatePlus14Days = DateTime.Now.AddDays(14).ToString("yyyy-MM-dd");
            PaymentDateTxt.Text = issueDatePlus14Days;
            Days7PayCHB.IsChecked = false;
            Days30PayCHB.IsChecked = false;
            PaymentDateTxt.IsEnabled = false;
        }

        private void Days7PayCHB_Checked(object sender, RoutedEventArgs e)
        {
            string issueDatePlus7Days = DateTime.Now.AddDays(7).ToString("yyyy-MM-dd");
            PaymentDateTxt.Text = issueDatePlus7Days;
            Days14PayCHB.IsChecked = false;
            Days30PayCHB.IsChecked = false;
            PaymentDateTxt.IsEnabled = false;
        }

        private void TodayPayCHB_Checked(object sender, RoutedEventArgs e)
        {
            string executionDate = DateTime.Now.ToString("yyyy-MM-dd");
            parentWindow.NewInvoice.ExecutionDate = executionDate;
            ExecutionDateTxt.Text = executionDate;
            ExecutionDateTxt.IsEnabled = false;
        }

        private void TodayPayCHB_Unchecked(object sender, RoutedEventArgs e)
        {
            ExecutionDateTxt.IsEnabled = true;
        }

        private void InvoiceFromCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox ? comboBox = sender as ComboBox;
            if (comboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string ? selectedValue = selectedItem.Content.ToString();

                if(selectedValue == "WŁASNE")
                {
                    selectedValue = "WLASNE";
                }
                parentWindow.NewInvoice.From = selectedValue;
            }
        }

        private void PaymentType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox? comboBox = sender as ComboBox;
            if(comboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string? selectedValue = selectedItem.Content.ToString();

                parentWindow.NewInvoice.PaymentType = selectedValue;
            }
        }

        private void PaymentDateTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            parentWindow.NewInvoice.PaymentDate = PaymentDateTxt.Text;
        }

        private void PaidYetCHB_Unchecked(object sender, RoutedEventArgs e)
        {
            PaidYet.IsEnabled = true;
        }

        private void PaidYetCHB_Checked(object sender, RoutedEventArgs e)
        {
            PaidYet.Text = "0";
            PaidYet.IsEnabled = false;
        }

        private void PaidYet_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void PaidYet_OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!IsTextAllowedPaidYet(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private bool IsTextAllowedPaidYet(string text)
        {
            Regex regex = new Regex("[^0-9]+");
            return !regex.IsMatch(text);
        }

        private void NumbersDash_PreviewInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9-]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void NumbersDash_Paste(object sender, DataObjectPastingEventArgs e)
        {
            if(e.DataObject.GetDataPresent(typeof(String))) 
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if(!IsTextAllowedNumbersDash(text)) 
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private bool IsTextAllowedNumbersDash(string text)
        {
            Regex regex = new Regex("[^0-9-]");
            return !regex.IsMatch(text);
        }

        private void PaidYet_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }
    }
}
