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

namespace dksApp.Bookkeeping.Invoice.InvoicePages
{
    /// <summary>
    /// Interaction logic for ProductsInvoicePage.xaml
    /// </summary>
    public partial class ProductsInvoicePage : Page
    {
        public ObservableCollection<Product> Products = new ObservableCollection<Product>();
        private int LP = 2;
        private CreateInvoiceWindow parentWindow;


        public ProductsInvoicePage(CreateInvoiceWindow InvoiceWindow)
        {
            parentWindow = InvoiceWindow;
            InitializeComponent();

            Product first = new Product() { NumberOfItems = 1, IdProduct = 1, NameItem = "Testowy" };
            Products.Add(first);

            ProductsDataGrid.ItemsSource = Products;
        }

        private void AddProductBtn_Click(object sender, RoutedEventArgs e)
        {
            ProductDialogWindow dialog = new ProductDialogWindow();
            dialog.Owner = parentWindow;
            dialog.NewProductRequested += OnNewProductRequested;
            dialog.ShowDialog();
            dialog.NewProductRequested -= OnNewProductRequested;
        }

        private void OnNewProductRequested()
        {
            Product newProduct = new Product()
            {
                NumberOfItems = LP
            };
            AddNewProduct newProductWindow = new AddNewProduct(newProduct);
            newProductWindow.ShowDialog();
            Products.Add(newProduct);
            LP++;
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

        private void ProductsDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var product = e.Row.Item as Product;
                var editingTextBox = e.EditingElement as TextBox;

                switch (e.Column.Header.ToString())
                {
                    case "L.p.":
                        if (int.TryParse(editingTextBox.Text, out int numberOfItems))
                        {
                            product.NumberOfItems = numberOfItems;
                        }
                        break;
                    case "Nazwa Produktu/Usługi":
                        product.NameItem = editingTextBox.Text;
                        break;
                    case "Ilość":
                        if (long.TryParse(editingTextBox.Text, out long quantity))
                        {
                            product.Quantity = quantity;
                        }
                        break;
                    case "J.m.":
                        product.QuantityType = editingTextBox.Text;
                        break;
                    case "PKWiU":
                        product.PKWiU = editingTextBox.Text;
                        break;
                    case "Cena netto [PLN]":
                        if (decimal.TryParse(editingTextBox.Text, out decimal nettoPrice))
                        {
                            product.NettoPrice = nettoPrice;
                        }
                        break;
                    case "Wartość netto [PLN]":
                        if (decimal.TryParse(editingTextBox.Text, out decimal nettoValue))
                        {
                            product.NettoValue = nettoValue;
                        }
                        break;
                    case "Vat [%]":
                        product.VATPercent = editingTextBox.Text;
                        break;
                    case "Wartość VAT [PLN]":
                        if (decimal.TryParse(editingTextBox.Text, out decimal vatValue))
                        {
                            product.VATValue = vatValue;
                        }
                        break;
                    case "Wartość Brutto [PLN]":
                        if (decimal.TryParse(editingTextBox.Text, out decimal bruttoValue))
                        {
                            product.BruttoValue = bruttoValue;
                        }
                        break;
                }

                UpdateProductInInvoice(product);
            
        }
        }

        private void UpdateProductInInvoice(Product updatedProduct)
        {
            var invoice = parentWindow.NewInvoice;
            var existingProduct = invoice.Products.FirstOrDefault(p => p.IdProduct == updatedProduct.IdProduct);

            if (existingProduct != null)
            {
                existingProduct.NumberOfItems = updatedProduct.NumberOfItems;
                existingProduct.NameItem = updatedProduct.NameItem;
                existingProduct.Quantity = updatedProduct.Quantity;
                existingProduct.QuantityType = updatedProduct.QuantityType;
                existingProduct.PKWiU = updatedProduct.PKWiU;
                existingProduct.NettoPrice = updatedProduct.NettoPrice;
                existingProduct.NettoValue = updatedProduct.NettoValue;
                existingProduct.VATPercent = updatedProduct.VATPercent;
                existingProduct.VATValue = updatedProduct.VATValue;
                existingProduct.BruttoValue = updatedProduct.BruttoValue;
            }
            else
            {
                invoice.Products.Add(updatedProduct);
            }
        }

        private void Save_Invoice(object sender, RoutedEventArgs e)
        {
            if(parentWindow.NewInvoice.IsEmpty() == true)
            {
                MessageBox.Show("Pole jest puste!", "Błąd wypełniania!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Dodawanie produktów i zapisywanie ich ID
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

                            int productId = (int)command.ExecuteScalar(); // Zapisuje ID dodanego produktu
                            productIds.Add(productId);
                        }
                    }

                    // Dodawanie faktury

                    int invoiceId;

                    string invoiceQuery = "INSERT INTO Invoice (IssueDate, ExecutionDate, PaymentType, PaymentDate, Paid, PaidYet, IdSeller, SellerName, SellerStreet, SellerCity, SellerZipCode, SellerNIP, SellerBankName, SellerBankAccount, Comments, BuyerName, BuyerStreet, BuyerCity, BuyerZipCode, BuyerNIP, InvoiceFrom) OUTPUT INSERTED.InvoiceID VALUES (@IssueDate, @ExecutionDate, @PaymentType, @PaymentDate, @Paid, @PaidYet, @IdSeller, @SellerName, @SellerStreet, @SellerCity, @SellerZipCode, @SellerNIP, @SellerBankName, @SellerBankAccount, @Comments, @BuyerName, @BuyerStreet, @BuyerCity, @BuyerZipCode, @BuyerNIP, @InvoiceFrom)";
                    using (SqlCommand command = new SqlCommand(invoiceQuery, connection))
                    {
                        long idSeller = Convert.ToInt64(parentWindow.NewInvoice.IdSeller);

                        command.Parameters.AddWithValue("@IssueDate", parentWindow.NewInvoice.IssueDate);
                        command.Parameters.AddWithValue("@ExecutionDate", parentWindow.NewInvoice.ExecutionDate);
                        command.Parameters.AddWithValue("@PaymentType", parentWindow.NewInvoice.PaymentType);
                        command.Parameters.AddWithValue("@PaymentDate", parentWindow.NewInvoice.PaymentDate);
                        command.Parameters.AddWithValue("@Paid", parentWindow.NewInvoice.Paid);
                        command.Parameters.AddWithValue("@PaidYet", parentWindow.NewInvoice.PaidYet);
                        command.Parameters.AddWithValue("@IdSeller", idSeller);
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

        //private void IsValidPagesInput()
        //{
        //    parentWindow.GridPage
        //}


    }
}
