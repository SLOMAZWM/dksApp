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
        //Invoice Info
        public uint ID { get; }
        public string ? IssueDate { get; set; }
        public string ? ExecutionDate { get; set; }

        //SELLER INFO
        public string ? SellerName { get; set; }
        public string ? SellerStreet {  get; set; }
        public string ? SellerCity { get; set; }
        public string ? SellerZipCode { get; set; }
        public string ? SellerNIP { get; set; }
        public string ? SellerBank { get; set; }
        public string ? SellerNumberBank { get; set; }
        public string ? Comments {  get; set; }

        //BUYER INFO

        public string? BuyerName { get; set; }
        public string? BuyerStreet { get; set; }
        public string? BuyerCity { get; set; }
        public string? BuyerZipCode { get; set; }
        public string? BuyerNIP { get; set; }
        public string? BuyerBank { get; set; }
        public string? BuyerNumberBank { get; set; }

        //PRODUCT INFO - Product.cs

        Product ? product { get; set; }

        //Other Info

        public string ? PaymentType {  get; set; }
        public string ? PaymentDate {  get; set; }
        public decimal Paid { get; set; }
        public decimal PaidYet { get; set; }
        public string ? BruttoValueInWords { get; set; }

    }
}
