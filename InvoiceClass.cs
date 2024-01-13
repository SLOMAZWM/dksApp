using dksApp.Bookkeeping.Invoice;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
			get { return _isSelected; }
			set
			{
				if (_isSelected != value)
				{
					_isSelected = value;
					OnPropertyChanged(nameof(IsSelected));
				}
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
        public string? Comments { get; set; }

        //SELLER INFO
        public uint IdSeller { get; set; }
        public string? SellerName { get; set; }
        public string? SellerStreet { get; set; }
        public string? SellerCity { get; set; }
        public string? SellerZipCode { get; set; }
        public string? SellerNIP { get; set; }
        public string? SellerBankName { get; set; }
        public string? SellerBankAccount { get; set; } //Change DataBase

        //BUYER INFO
        public uint IdBuyer { get; set; }
        public string? BuyerName { get; set; }
        public string? BuyerStreet { get; set; }
        public string? BuyerCity { get; set; }
        public string? BuyerZipCode { get; set; }
        public string? BuyerNIP { get; set; }

        //PRODUCT INFO - Product.cs
        public ObservableCollection<Product> Products { get; set; }

        public InvoiceClass()
        {
            Products = new ObservableCollection<Product>();
        }

        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(IssueDate)
                || string.IsNullOrEmpty(ExecutionDate)
                || string.IsNullOrEmpty(PaymentType)
                || string.IsNullOrEmpty(PaymentDate)
                || string.IsNullOrEmpty(SellerName)
                || string.IsNullOrEmpty(SellerStreet)
                || string.IsNullOrEmpty(SellerCity)
                || string.IsNullOrEmpty(SellerZipCode)
                || string.IsNullOrEmpty(SellerNIP)
                || string.IsNullOrEmpty(SellerBankName)
                || string.IsNullOrEmpty(SellerBankAccount)
                || string.IsNullOrEmpty(BuyerName)
                || string.IsNullOrEmpty(BuyerStreet)
                || string.IsNullOrEmpty(BuyerCity)
                || string.IsNullOrEmpty(BuyerZipCode);
        }

    }
}
