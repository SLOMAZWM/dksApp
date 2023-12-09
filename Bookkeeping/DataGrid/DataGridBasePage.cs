using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace dksApp.Bookkeeping.DataGrid
{
    public abstract class DataGridBasePage : Page, INotifyPropertyChanged
    {
        protected ObservableCollection<InvoiceClass> Invoices { get; set; }
        protected ObservableCollection<InvoiceClass> DisplayedInvoices { get; set; }
        protected int CurrentPage { get; set; }
        protected int PageSize { get; set; } = 7; // Liczba wierszy na stronie
        protected uint allDocuments;
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

        public DataGridBasePage()
        {
            Invoices = new ObservableCollection<InvoiceClass>();
            DisplayedInvoices = new ObservableCollection<InvoiceClass>();
            this.DataContext = this;
            InitializeAllInvoices();
        }
        protected abstract void InitializeAllInvoices();

        protected void UpdateDisplayedInvoices()
        {
            int startIndex = (CurrentPage - 1) * PageSize;
            int endIndex = startIndex + PageSize;

            DisplayedInvoices.Clear();

            for (int i = startIndex; i < endIndex && i < Invoices.Count; i++)
            {
                DisplayedInvoices.Add(Invoices[i]);
            }
        }

        protected void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            int maxPage = (int)Math.Ceiling((double)Invoices.Count / PageSize);
            if (CurrentPage < maxPage)
            {
                CurrentPage++;
                UpdateDisplayedInvoices();
                //UpdatePaginationButtonStyles();
            }
        }

        protected void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                UpdateDisplayedInvoices();
                //UpdatePaginationButtonStyles();
            }
        }

        protected void PageButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && int.TryParse(button.Content?.ToString(), out int pageNumber))
            {
                CurrentPage = pageNumber;
                UpdateDisplayedInvoices();
                //UpdatePaginationButtonStyles();
            }
        }

        protected void CheckBoxHeader_Click(object sender, RoutedEventArgs e)
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
