using dksApp.Bookkeeping.Invoice;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace dksApp
{
    public class InvoiceClass : INotifyPropertyChanged
    {
        //Pagination
        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //INVOICE FROM?

        public string ? From { get; set; }

        //Invoice Info
        public uint IDInvoice { get; set; }
        public string ? IssueDate { get; set; }
        public string ? ExecutionDate { get; set; }
        public string? PaymentType { get; set; }
        public string? PaymentDate { get; set; }
        public decimal Paid { get; set; }
        public decimal PaidYet { get; set; }
        public string? BruttoValueInWords { get; set; }

        //SELLER INFO
        public uint IdSeller { get; set; }
        public string? SellerName { get; set; }
        public string? SellerStreet { get; set; }
        public string? SellerCity { get; set; }
        public string? SellerZipCode { get; set; }
        public string? SellerNIP { get; set; }
        public string? SellerBankName { get; set; }
        public string? SellerBankAccount { get; set; } //Change DataBase
        public string? SellerNumberBank { get; set; }
        public string? Comments { get; set; }

        //BUYER INFO
        public uint IdBuyer { get; set; }
        public string? BuyerName { get; set; }
        public string? BuyerStreet { get; set; }
        public string? BuyerCity { get; set; }
        public string? BuyerZipCode { get; set; }
        public string? BuyerNIP { get; set; }
        public string? BuyerBankName { get; set; }
        public string? BuyerBankNumber { get; set; }

        //PRODUCT INFO - Product.cs
        public List<Product> Products { get; set; }

        public InvoiceClass()
        {
            Products = new List<Product>();
        }

    }
}
