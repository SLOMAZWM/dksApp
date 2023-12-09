using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace dksApp.Bookkeeping
{


    public partial class AllegroDataGrid : Page, INotifyPropertyChanged
    {
        private ObservableCollection<InvoiceClass> Invoices { get; set; }
        private ObservableCollection<InvoiceClass> DisplayedInvoices { get; set; }
        private int CurrentPage { get; set; }
        private int PageSize { get; set; } = 7; // Liczba wierszy na stronie
        private uint allDocuments;
        public uint AllDocuments
        {
            get { return allDocuments; }
            set
            {
                if (allDocuments != value)
                {
                    allDocuments = value;
                    OnPropertyChanged(nameof(AllDocuments));
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public AllegroDataGrid()
        {
            InitializeComponent();
            this.DataContext = this;
            Invoices = new ObservableCollection<InvoiceClass>();
            DisplayedInvoices = new ObservableCollection<InvoiceClass>();
            BookKeepingDataGrid.ItemsSource = DisplayedInvoices;
            InitializeAllInvoices();

            CurrentPage = 1;
            UpdateDisplayedInvoices();
        }

        private void InitializeAllInvoices()
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\frees\\Desktop\\Moje\\Programowanie\\Visual\\C#\\Projekty\\dksApp\\dksApp.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=False";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT InvoiceID, SellerName, BuyerName, PaymentType, Paid, IssueDate FROM Invoice WHERE InvoiceFrom = 'ALLEGRO'";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var invoice = new InvoiceClass
                            {
                                IDInvoice = (uint)reader.GetInt32(reader.GetOrdinal("InvoiceID")),
                                SellerName = reader.IsDBNull(reader.GetOrdinal("SellerName")) ? null : reader.GetString(reader.GetOrdinal("SellerName")),
                                BuyerName = reader.IsDBNull(reader.GetOrdinal("BuyerName")) ? null : reader.GetString(reader.GetOrdinal("BuyerName")),
                                PaymentType = reader.IsDBNull(reader.GetOrdinal("PaymentType")) ? null : reader.GetString(reader.GetOrdinal("PaymentType")),
                                Paid = reader.IsDBNull(reader.GetOrdinal("Paid")) ? 0 : reader.GetDecimal(reader.GetOrdinal("Paid")),
                                IssueDate = reader.IsDBNull(reader.GetOrdinal("IssueDate")) ? null : reader.GetString(reader.GetOrdinal("IssueDate"))
                            };
                            Invoices.Add(invoice);
                            allDocuments++;
                        }
                    }
                }
            }

            int maxPage = (int)Math.Ceiling((double)Invoices.Count / PageSize);

            for (int pageNumber = 1; pageNumber <= maxPage; pageNumber++)
            {
                var button = new Button
                {
                    Content = pageNumber.ToString(),
                    Style = FindResource("pagingButton") as Style
                };
                button.Click += PageButton_Click;

                PaginationItemsControl.Items.Add(button);
            }
        }

        private void UpdateDisplayedInvoices()
        {
            int startIndex = (CurrentPage - 1) * PageSize;
            int endIndex = startIndex + PageSize;

            DisplayedInvoices.Clear();

            for (int i = startIndex; i < endIndex && i < Invoices.Count; i++)
            {
                DisplayedInvoices.Add(Invoices[i]);
            }

            UpdatePaginationButtonStyles();
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            int maxPage = (int)Math.Ceiling((double)Invoices.Count / PageSize);
            if (CurrentPage < maxPage)
            {
                CurrentPage++;
                UpdateDisplayedInvoices();
            }
        }

        private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                UpdateDisplayedInvoices();
            }
        }

        private void UpdatePaginationButtonStyles()
        {
            foreach (UIElement element in PaginationItemsControl.Items)
            {
                if (element is Button button)
                {
                    int pageNumber;
                    if (int.TryParse(button.Content?.ToString(), out pageNumber))
                    {
                        Brush actuallyButtonBackground = new SolidColorBrush(Color.FromRgb(0x79, 0x50, 0xF2));
                        Brush normalButtonForeground = new SolidColorBrush(Color.FromRgb(0x6C, 0x76, 0x82));

                        if (pageNumber == CurrentPage)
                        {
                            button.Background = actuallyButtonBackground;
                            button.Foreground = Brushes.White;
                        }
                        else
                        {
                            button.Background = Brushes.Transparent;
                            button.Foreground = normalButtonForeground;
                        }
                    }
                }
            }
        }

        private void PageButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && int.TryParse(button.Content?.ToString(), out int pageNumber))
            {
                CurrentPage = pageNumber;
                UpdateDisplayedInvoices();
            }
        }

        //HEADER CHECKBOX ALL SELECTED IN DATAGRID
        private void CheckBoxHeader_Click(object sender, RoutedEventArgs e)
        {
            var checkBoxHeader = (CheckBox)sender;
            bool isChecked = checkBoxHeader.IsChecked ?? false;

            foreach (var invoice in DisplayedInvoices)
            {
                invoice.IsSelected = isChecked;
            }
        }

    }
}
