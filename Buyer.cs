using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace dksApp
{
    public class Buyer : INotifyPropertyChanged
	{
        public long IdBuyer { get; set; }
        public string? BuyerName { get; set; }
        public string? BuyerStreet { get; set; }
        public string? BuyerCity { get; set; }
        public string? BuyerZipCode { get; set; }
        public string? BuyerNIP { get; set; }
        public string? BuyerBankName { get; set; }
        public string? BuyerBankAccount { get; set; }
        public string? BuyerTitle { get; set; }
		private bool _isSelected;
		public bool IsSelected
		{
			get => _isSelected;
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
	}
}
