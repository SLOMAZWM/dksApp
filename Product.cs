using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dksApp
{
    public class Product
    {
        public int IdProduct { get; set; }
        public int NumberOfItems { get; set; }
        public string? NameItem { get; set; }
        public long Quantity { get; set; }
        public string? QuantityType { get; set; }
        public string? PKWiU { get; set; }
        public decimal NettoPrice { get; set; }
        public decimal NettoValue { get; set; }
        public string ? VATPercent { get; set; }
        public decimal VATValue { get; set; }
        public decimal BruttoValue { get; set; }
        public bool ShowIt { get; set; }

    }
}
