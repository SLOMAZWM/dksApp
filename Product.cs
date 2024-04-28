using System.ComponentModel;
using System.Text;
using System.Windows;

namespace dksApp
{
	public class Product : INotifyPropertyChanged
	{
		public int IdProduct { get; set; }
		public int NumberOfItems { get; set; }
		public string? NameItem { get; set; }
		public double Quantity { get; set; }
		public string? QuantityType { get; set; }
		public string? PKWiU { get; set; }
		public decimal NettoPrice { get; set; } // 1 Item
		public decimal NettoValue { get; set; } // All Items
		public string? VATPercent { get; set; }
		public decimal VATValue { get; set; }
		public decimal BruttoValue { get; set; }
		public bool ShowIt { get; set; } //ADDED TO MAGAZINE!
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

        public bool isEmpty()
        {
            bool hasError = false;
            StringBuilder errorMessage = new StringBuilder("Proszę uzupełnić następujące pola:\n");

            if (string.IsNullOrEmpty(NameItem))
            {
                errorMessage.AppendLine("- Nazwa Produktu/Usługi");
                hasError = true;
            }
            if (string.IsNullOrEmpty(QuantityType))
            {
                errorMessage.AppendLine("- Miara ilości");
                hasError = true;
            }
            if (string.IsNullOrEmpty(VATPercent))
            {
                errorMessage.AppendLine("- VAT [%]");
                hasError = true;
            }

            if (hasError)
            {
                MessageBox.Show(errorMessage.ToString(), "Błąd wypełnienia", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return hasError;
        }


        public bool isZero()
        {
            bool hasError = false;
            StringBuilder errorMessage = new StringBuilder("Następujące pola liczbowe nie mogą być zerowe:\n");

            if (NumberOfItems == 0)
            {
                errorMessage.AppendLine("- Numer pozycji");
                hasError = true;
            }
            if (Quantity == 0)
            {
                errorMessage.AppendLine("- Ilość");
                hasError = true;
            }
            if (NettoPrice == 0)
            {
                errorMessage.AppendLine("- Cena Netto za jednostkę");
                hasError = true;
            }
            if (NettoValue == 0)
            {
                errorMessage.AppendLine("- Wartość Netto");
                hasError = true;
            }
            if (VATValue == 0)
            {
                errorMessage.AppendLine("- Wartość VAT");
                hasError = true;
            }
            if (BruttoValue == 0)
            {
                errorMessage.AppendLine("- Wartość Brutto");
                hasError = true;
            }

            if (hasError)
            {
                MessageBox.Show(errorMessage.ToString(), "Błąd wypełnienia", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return hasError;
        }


    }
}
