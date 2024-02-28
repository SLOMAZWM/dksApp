using dksApp.Bookkeeping.Invoice.InvoicePages.InvoiceDialog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
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
using dksApp.Bookkeeping.Invoice.InvoicePages.AddPages.Products;
using System.Security.Cryptography.X509Certificates;
using dksApp.Bookkeeping.LoadInfoWindow;

namespace dksApp.Bookkeeping.Invoice.InvoicePages
{
    /// <summary>
    /// Interaction logic for ProductsInvoicePage.xaml
    /// </summary>
    public partial class ProductsInvoicePage : Page
    {
        private int LP = 1;
        private readonly CreateInvoiceFrame parentWindow;

        public ProductsInvoicePage(CreateInvoiceFrame InvoiceFrame)
        {
            InitializeComponent();
            parentWindow = InvoiceFrame;

            ProductsDataGrid.ItemsSource = parentWindow.NewInvoice.Products;
        }

        public bool isNew { get; set; }
        public bool isLoad { get; set; }

        private void AddProductBtn_Click(object sender, RoutedEventArgs e)
        {
            ProductDialogWindow dialog = new ProductDialogWindow(this);
            dialog.ShowDialog();

            if(isNew == true)
            {
                OnNewProductRequested();
            }
            else if(isLoad == true)
            {
                LoadWindow loadW = new LoadWindow(this);
                loadW.ShowDialog();
            }
        }

        public void OnLoadProductRequested(Product databaseProduct)
        {
            databaseProduct.NumberOfItems = LP;
            AddNewProduct loadedProductW = new AddNewProduct(databaseProduct);
            loadedProductW.ShowDialog();
            if(loadedProductW.IsCreated == true) 
            {
                parentWindow.NewInvoice.Products.Add(databaseProduct);
                LP++;
            }
            else
            {
                return;
            }
        }

        private void OnNewProductRequested()
        {
            Product newProduct = new Product()
            {
                NumberOfItems = LP
            };
            AddNewProduct newProductWindow = new AddNewProduct(newProduct);
            newProductWindow.ShowDialog();
            if(newProductWindow.IsCreated == true)
            {
                parentWindow.NewInvoice.Products.Add(newProduct);
                LP++;
            }
            else
            {
                return;
            }
        }

        private bool IsProductsDataGridEmpty()
        {
            if(ProductsDataGrid.Items.IsEmpty)
            {
                MessageBox.Show("Brak produktów - kliknij dodaj produkt!", "Błąd dodawania faktury", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
            else
            {
                return false;
            }
        }
        private void PreviousPageBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                parentWindow.Navigator.NavigateToGrid("Informacje");
                parentWindow.HighlightInformationButton();
            }
            catch
            {
                MessageBox.Show("Błąd nawigacji podstrony, skontaktuj się z administratorem aplikacji!", "Krytyczny błąd Nawigacji", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private decimal CalculatePaidYet()
        {
            return parentWindow.NewInvoice.Products.Sum(p => p.BruttoValue);
        }

        private string GenerateNumberInvoice()
        {
            var currentDate = DateTime.Now;
            var year = currentDate.Year.ToString();
            var month = currentDate.Month.ToString("D2");

            int lastInvoiceSequence = GetLastInvoiceSequence(year, month);
            int newInvoiceSequence = lastInvoiceSequence + 1;

            var newInvoiceNumber = $"{year}/{month}/{newInvoiceSequence:D3}";

            return newInvoiceNumber;
        }

        private int GetLastInvoiceSequence(string year, string month)
        {
            int lastSequence = 0;
            string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

            using(SqlConnection connection = new SqlConnection(connectionString)) 
            {
                string query = @"
            SELECT TOP 1 InvoiceNumber 
            FROM Invoice 
            WHERE InvoiceNumber LIKE @YearMonthPattern 
            ORDER BY InvoiceNumber DESC";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@YearMonthPattern", $"{year}/{month}/%");
                    connection.Open();
                    var result = command.ExecuteScalar();

                    if (result != null)
                    {
                        var lastInvoiceNumber = result.ToString();
                        lastSequence = int.Parse(lastInvoiceNumber.Split('/').Last());
                    }
                }
            }
            return lastSequence;
        }

        private void Save_Invoice(object sender, RoutedEventArgs e)
        {
            if(IsProductsDataGridEmpty() == true)
            {
                return;
            }
            else
            {
                if (parentWindow.NewInvoice.IsEmpty() == true)
                {
                    MessageBox.Show("Pole jest puste!", "Błąd wypełniania!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;
                    string invoiceNumber = GenerateNumberInvoice();

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        List<int> productIds = new List<int>();
                        foreach (var product in parentWindow.NewInvoice.Products)
                        {
                            bool showIt = false;
                            string productQuery = "INSERT INTO Product (NumberOfItems, NameItem, Quantity, QuantityType, PKWiU, NettoPrice, NettoValue, VATPercent, VATValue, BruttoValue, ShowIt) OUTPUT INSERTED.ProductID VALUES (@NumberOfItems, @NameItem, @Quantity, @QuantityType, @PKWiU, @NettoPrice, @NettoValue, @VATPercent, @VATValue, @BruttoValue, @ShowIt)";

                            using (SqlCommand command = new SqlCommand(productQuery, connection))
                            {
                                command.Parameters.AddWithValue("@NumberOfItems", product.NumberOfItems);
                                command.Parameters.AddWithValue("@NameItem", product.NameItem);
                                command.Parameters.AddWithValue("@Quantity", product.Quantity);
                                command.Parameters.AddWithValue("@QuantityType", product.QuantityType);
                                command.Parameters.AddWithValue("@PKWiU", product.PKWiU);
                                command.Parameters.AddWithValue("@NettoPrice", product.NettoPrice);
                                command.Parameters.AddWithValue("@NettoValue", product.NettoValue);
                                command.Parameters.AddWithValue("@VATPercent", product.VATPercent);
                                command.Parameters.AddWithValue("@VATValue", product.VATValue);
                                command.Parameters.AddWithValue("@BruttoValue", product.BruttoValue);
                                command.Parameters.AddWithValue("@ShowIt", showIt);

                                int productId = (int)command.ExecuteScalar();
                                productIds.Add(productId);
                            }
                        }

                        int invoiceId;

                        string invoiceQuery = "INSERT INTO Invoice (InvoiceNumber, IssueDate, ExecutionDate, PaymentType, PaymentDate, Paid, PaidYet, SellerName, SellerStreet, SellerCity, SellerZipCode, SellerNIP, SellerBankName, SellerBankAccount, Comments, BuyerName, BuyerStreet, BuyerCity, BuyerZipCode, BuyerNIP, InvoiceFrom) OUTPUT INSERTED.InvoiceID VALUES (@InvoiceNumber, @IssueDate, @ExecutionDate, @PaymentType, @PaymentDate, @Paid, @PaidYet, @SellerName, @SellerStreet, @SellerCity, @SellerZipCode, @SellerNIP, @SellerBankName, @SellerBankAccount, @Comments, @BuyerName, @BuyerStreet, @BuyerCity, @BuyerZipCode, @BuyerNIP, @InvoiceFrom)";
                        using (SqlCommand command = new SqlCommand(invoiceQuery, connection))
                        {
                            command.Parameters.AddWithValue("@InvoiceNumber", invoiceNumber);
                            command.Parameters.AddWithValue("@IssueDate", parentWindow.NewInvoice.IssueDate);
                            command.Parameters.AddWithValue("@ExecutionDate", parentWindow.NewInvoice.ExecutionDate);
                            command.Parameters.AddWithValue("@PaymentType", parentWindow.NewInvoice.PaymentType);
                            command.Parameters.AddWithValue("@PaymentDate", parentWindow.NewInvoice.PaymentDate);
                            command.Parameters.AddWithValue("@Paid", parentWindow.NewInvoice.Paid);
                            command.Parameters.AddWithValue("@PaidYet", CalculatePaidYet());
                            command.Parameters.AddWithValue("@SellerName", parentWindow.NewInvoice.SellerName);
                            command.Parameters.AddWithValue("@SellerStreet", parentWindow.NewInvoice.SellerStreet);
                            command.Parameters.AddWithValue("@SellerCity", parentWindow.NewInvoice.SellerCity);
                            command.Parameters.AddWithValue("@SellerZipCode", parentWindow.NewInvoice.SellerZipCode);
                            command.Parameters.AddWithValue("@SellerNIP", parentWindow.NewInvoice.SellerNIP);
                            command.Parameters.AddWithValue("@SellerBankName", parentWindow.NewInvoice.SellerBankName);
                            command.Parameters.AddWithValue("@SellerBankAccount", parentWindow.NewInvoice.SellerBankAccount);
                            command.Parameters.AddWithValue("@Comments", parentWindow.NewInvoice.Comments);
                            command.Parameters.AddWithValue("@BuyerName", parentWindow.NewInvoice.BuyerName);
                            command.Parameters.AddWithValue("@BuyerStreet", parentWindow.NewInvoice.BuyerStreet);
                            command.Parameters.AddWithValue("@BuyerCity", parentWindow.NewInvoice.BuyerCity);
                            command.Parameters.AddWithValue("@BuyerZipCode", parentWindow.NewInvoice.BuyerZipCode);
                            command.Parameters.AddWithValue("@BuyerNIP", parentWindow.NewInvoice.BuyerNIP);
                            command.Parameters.AddWithValue("@InvoiceFrom", parentWindow.NewInvoice.From);

                            invoiceId = (int)command.ExecuteScalar();
                        }

                        // Dodawanie powiązań w InvoiceProducts
                        foreach (int productId in productIds)
                        {
                            string invoiceProductQuery = "INSERT INTO InvoiceProducts (InvoiceID, ProductID) VALUES (@InvoiceID, @ProductID)";

                            using (SqlCommand command = new SqlCommand(invoiceProductQuery, connection))
                            {
                                command.Parameters.AddWithValue("@InvoiceID", invoiceId);
                                command.Parameters.AddWithValue("@ProductID", productId);

                                command.ExecuteNonQuery();
                            }
                        }

                        MessageBox.Show("Poprawnie dodano fakturę!", "Operacja wykonana pomyślnie!", MessageBoxButton.OK, MessageBoxImage.Information);
                        connection.Close();
                    
                    }

                }
            }
            
        }

        private void EditProductBtn_Click(object sender, RoutedEventArgs e)
        {
			try 
            {
				var selectedProduct = ProductsDataGrid.SelectedItem;
                if(selectedProduct != null)
                {
					EditProductW editProduct = new EditProductW((Product)selectedProduct, this);
					editProduct.ShowDialog();
					ProductsDataGrid.Items.Refresh();
					editProduct.Close();
				}
				else
                {
                    MessageBox.Show("Wybierz produkt do edycji!", "Brak wyboru produktu", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
			}
            catch(Exception ex) //test
            {
                MessageBox.Show("Błąd wybierania produktu do edycji: " + ex.Message, "Błąd edycji", MessageBoxButton.OK, MessageBoxImage.Error);
                
            }
        }

        private void DeleteProductBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedProduct = ProductsDataGrid.SelectedItem;
            if(selectedProduct != null) 
            {
                Product deleteProduct = (Product)selectedProduct;
				var result = MessageBox.Show($"Czy chcesz usunąć produkt: {deleteProduct.NameItem}?", "Usuwanie produktu", MessageBoxButton.YesNo, MessageBoxImage.Question);
				if (result == MessageBoxResult.Yes)
				{
					parentWindow.NewInvoice.Products.Remove((Product)selectedProduct);
				}
				else
				{
					return;
				}
			}
			else
            {
                MessageBox.Show("Wybierz produkt!", "Błąd wybrania!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
