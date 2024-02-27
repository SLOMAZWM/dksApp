using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

		public bool IsEmpty()
		{
			if(string.IsNullOrEmpty(BuyerName))
			{
				return true;
			}
			else if(string.IsNullOrEmpty(BuyerStreet))
			{
				return true;
			}
            else if (string.IsNullOrEmpty(BuyerCity))
            {
                return true;
            }
            else if (string.IsNullOrEmpty(BuyerZipCode))
            {
                return true;
            }
            else if (string.IsNullOrEmpty(BuyerNIP))
            {
                return true;
            }
            else
			{
                return false;
            }
		}

        public bool MinimalLettersNip(string NIP)
        {
            if (NIP.Length < 10)
            {
                MessageBox.Show("Użyto mniej niż 10 znaków w NIP!", "Błąd uzupełnienia NIP", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
