using dksApp.Bookkeeping.Invoice;
using PdfSharp.Fonts;
using System.Configuration;
using System.Data;
using System.Windows;

namespace dksApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

			string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;
		}
	}

}
