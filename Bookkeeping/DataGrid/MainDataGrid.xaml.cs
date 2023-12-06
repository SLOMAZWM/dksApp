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
using System.Data.SqlClient;
using System.Configuration;

namespace dksApp.Bookkeeping
{
    /// <summary>
    /// Interaction logic for MainDataGrid.xaml
    /// </summary>
    public partial class MainDataGrid : Page
    {
        private ObservableCollection<InvoiceClass> Invoices {get; set;}

        public MainDataGrid()
        {
            InitializeComponent();
        }

        private void InitializeAllInvoices()
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\frees\\Documents\\dksApp.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=True";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {

            }

        }
    }
}
