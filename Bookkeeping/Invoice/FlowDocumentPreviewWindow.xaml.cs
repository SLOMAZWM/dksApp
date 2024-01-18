using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace dksApp.Bookkeeping.Invoice
{
	/// <summary>
	/// Interaction logic for FlowDocumentPreviewWindow.xaml
	/// </summary>
	public partial class FlowDocumentPreviewWindow : Window
	{
		public FlowDocumentPreviewWindow(FlowDocument flowDocument)
		{
			InitializeComponent();
			documentViewer.Document = flowDocument;
		}
	}
}
