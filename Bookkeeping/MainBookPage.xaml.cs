using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// <summary>
    /// Interaction logic for MainBookPage.xaml
    /// </summary>
    public partial class MainBookPage : Page
    {

        private NavigatorManager navigator;
        private Dictionary<string, Page> DataGridPage = new Dictionary<string, Page>();

        public MainBookPage()
        {
            InitializeComponent();

            DataGridPage = InitializeDataGridPages();
            navigator = new NavigatorManager(tabButtonSP, DataGridSelectedFrame, DataGridPage);
        }

        private Dictionary<string, Page> InitializeDataGridPages()
        {
            Dictionary<string, Page> NewDictionaryOfPages = new Dictionary<string, Page>
            {
                { "Wszystkie", new MainDataGrid() },
                {"Allegro", new AllegroDataGrid() },
                {"Własne", new UserDataGrid() }
            };

            return NewDictionaryOfPages;
        }

        private void NavigationButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string dataGridName)
            {
                navigator.ChangeTabButton(button);
                navigator.NavigateToDataGrid(dataGridName);
            }
        }
    }
}
