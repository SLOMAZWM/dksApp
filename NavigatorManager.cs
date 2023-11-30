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

        public NavigatorManager(Frame frame, List<Button> ListOfButtons)
        {
            Pages = InitializePages();
            ActuallyContentFrame = frame;
            ButtonList = ListOfButtons;
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

        public void ChangeButtonColor(Button clickedButton)
        {
            SolidColorBrush buttonForegroundBrush = new SolidColorBrush(Color.FromRgb(208, 192, 255));
            SolidColorBrush whiteForegroundBrush = Brushes.White;

            foreach (var button in ButtonList)
            {
                button.Foreground = buttonForegroundBrush;
            }

            clickedButton.Foreground = whiteForegroundBrush;
        }

    }
}
