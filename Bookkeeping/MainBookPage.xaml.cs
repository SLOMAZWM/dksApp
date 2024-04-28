using dksApp.Bookkeeping.Invoice;
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
        private MainWindow _mainWindow;
        private NavigatorManager navigatorDataGrid;
        private Dictionary<string, Page> DataGridPage = new Dictionary<string, Page>();

        public MainBookPage(MainWindow mainWindow)
        {
            InitializeComponent();

            DataGridPage = InitializeDataGridPages();
            navigatorDataGrid = new NavigatorManager(tabButtonSP, DataGridSelectedFrame, DataGridPage);
            _mainWindow = mainWindow;
        }

        private Dictionary<string, Page> InitializeDataGridPages()
        {
            Dictionary<string, Page> NewDictionaryOfPages = new Dictionary<string, Page>
            {
                { "Wszystkie", new MainDataGrid() },
                {"Allegro", new AllegroDataGrid() },
                {"Własne", new UserDataGrid() },
            };

            return NewDictionaryOfPages;
        }

        private void NavigationButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string dataGridName)
            {
                navigatorDataGrid.ChangeTabButton(button);
                navigatorDataGrid.NavigateToDataGrid(dataGridName);
            }
        }

        private void NavigationToCreateInvoice_Click(object sender, RoutedEventArgs e)
        {
            CreateInvoiceFrame createInvoiceFrame = new CreateInvoiceFrame();
            createInvoiceFrame.InvoiceAdded += RefreshDataGrids;
            _mainWindow.MainContentFrame.NavigationService.Navigate(createInvoiceFrame);
        }

        private void RefreshDataGrids()
        {
            DataGridPage["Wszystkie"] = new MainDataGrid();
            SimulateButtonClick("Wszystkie");
        }

        private void SimulateButtonClick(string dataGridName)
        {
            Button button = tabButtonSP.Children
                .OfType<Button>()
                .FirstOrDefault(b => b.Tag as string == dataGridName);

            if (button != null)
            {
                navigatorDataGrid.ChangeTabButton(button);
                navigatorDataGrid.NavigateToDataGrid(dataGridName);
            }
        }

    }
}
