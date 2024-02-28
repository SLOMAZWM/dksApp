using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace dksApp
{
    public static class NavigationBookKeeping
    {
        private static Dictionary<string, Page> DataGridPage = new Dictionary<string, Page>();
        private static Frame ActuallyContentFrame;
        private static StackPanel invoiceTabButton;

        public static void Initialize(Dictionary<string, Page> DataGridP, StackPanel SP, Frame frame)
        {
            ActuallyContentFrame = frame;
            invoiceTabButton = SP;
            DataGridPage = DataGridP;
        }

        public static void NavigateToDataGrid(string dataGridName)
        {
            try
            {
                ActuallyContentFrame.NavigationService.Navigate(DataGridPage[dataGridName]);
            }
            catch
            {
                MessageBox.Show("Błąd nawigacji tabeli, skontaktuj się z administratorem aplikacji!", "Krytyczny błąd Nawigacji", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void ChangeInvoiceTabButton(Button clickedButton)
        {
            SolidColorBrush buttonBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x78, 0x4F, 0xF2));
            SolidColorBrush blackForegroundBrush = new SolidColorBrush(Color.FromRgb(0x12, 0x15, 0x18));
            SolidColorBrush transparentBrush = new SolidColorBrush(Color.FromArgb(0x00, 0x00, 0x00, 0x00));

            foreach (var child in invoiceTabButton.Children)
            {
                if (child is Button button)
                {
                    button.Foreground = (button == clickedButton) ? buttonBrush : blackForegroundBrush;
                    button.BorderBrush = (button == clickedButton) ? buttonBrush : transparentBrush;
                }
            }
        }
    }
}
