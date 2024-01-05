using Microsoft.Xaml.Behaviors.Media;
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
        public double Quantity { get; set; }
        public string? QuantityType { get; set; }
        public string? PKWiU { get; set; }
        public decimal NettoPrice { get; set; }
        public decimal NettoValue { get; set; }
        public string ? VATPercent { get; set; }
        public decimal VATValue { get; set; }
        public decimal BruttoValue { get; set; }
        public bool ShowIt { get; set; } //ADDED TO MAGAZINE!

        public bool isEmpty()
        {
            return string.IsNullOrEmpty(NameItem)
                || string.IsNullOrEmpty(QuantityType)
                || string.IsNullOrEmpty(PKWiU)
                || string.IsNullOrEmpty(VATPercent);
        }

        public bool isZero()
        {
            if (NumberOfItems == 0)
            {
                return true;
            }
            else if (Quantity == 0)
            {
                return true;
            }
            else if (NettoPrice == 0)
            {
                return true;
            }
            else if(NettoValue == 0)
            {
                return true;
            }
            else if(NettoPrice == 0)
            {
                return true;
            }
            else if(VATValue == 0)
            {
                return true;
            }
            else if(BruttoValue == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
