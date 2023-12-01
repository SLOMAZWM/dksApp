using dksApp.Allegro;
using dksApp.Bookkeeping;
using dksApp.Calendar;
using dksApp.Contractors;
using dksApp.Magazine;
using dksApp.Orders;
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
    public class NavigatorManager
    {
        private Dictionary<string, Page> Pages = new Dictionary<string, Page>();
        private Frame ActuallyContentFrame;
        List<Button> ButtonList = new List<Button>();
        private StackPanel tabButtonStackPanel;

        public NavigatorManager(Frame frame, List<Button> ListOfButtons)
        {
            Pages = InitializePages();
            ActuallyContentFrame = frame;
            ButtonList = ListOfButtons;
        }

        public NavigatorManager(StackPanel SP)
        {
            tabButtonStackPanel = SP;
        }



        private Dictionary<string, Page> InitializePages()
        {
            Dictionary<string, Page> NewDictionaryOfPages = new Dictionary<string, Page>
            {
                { "MainBook", new MainBookPage() },
                {"Allegro", new AllegroPage() },
                {"Calendar", new CalendarPage() },
                {"Contractors", new ContractorsPage() },
                {"Magazine", new MagazinePage() },
                {"Orders", new OrdersPage() }
            };

            return NewDictionaryOfPages;
        }

        public void NavigateToPage(string pageName)
        {
            try
            {
                ActuallyContentFrame.NavigationService.Navigate(Pages[pageName]);
            }
            catch
            {
                MessageBox.Show("Błąd nawigacji, skontaktuj się z administratorem aplikacji!", "Krytyczny błąd Nawigacji", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void ChangeMenuButtonColor(Button clickedButton)
        {
            SolidColorBrush buttonForegroundBrush = new SolidColorBrush(Color.FromRgb(208, 192, 255));
            SolidColorBrush whiteForegroundBrush = Brushes.White;

            foreach (var button in ButtonList)
            {
                button.Foreground = buttonForegroundBrush;
            }

            clickedButton.Foreground = whiteForegroundBrush;
        }

        public void ChangeTabButton(Button clickedButton)
        {
            SolidColorBrush buttonBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x78, 0x4F, 0xF2));
            SolidColorBrush blackForegroundBrush = new SolidColorBrush(Color.FromRgb(0x12, 0x15, 0x18));
            SolidColorBrush transparentBrush = new SolidColorBrush(Color.FromArgb(0x00, 0x00, 0x00, 0x00));

            foreach (var child in tabButtonStackPanel.Children)
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
