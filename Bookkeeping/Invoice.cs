using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace dksApp
{
    public class Invoice
    {
        public int Id { get; set; } //NR.
        public string Type { get; set; }
        public string SellerName { get; set; } //Sprzedawca
        public string SellerAddress { get; set; }
        public string SellerNIP { get; set; }
        public string SellerPhoneNumber { get; set; }
        public string SellerEmail { get; set; }
        public string BuyerName { get; set; } // Nabywca
        public string BuyerAddress { get; set; }
        public string BuyerNIP { get; set; }
        public string BuyerPhoneNumber { get; set; }
        public string BuyerEmail { get; set; }
        public string Payment { get; set; } // Płatność
        public decimal Price { get; set; } // Kwota
        public DateTime Date { get; set; } // Data wystawienia
    }
}
