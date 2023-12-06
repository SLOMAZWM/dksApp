using dksApp.Bookkeeping.Invoice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace dksApp
{
    public class InvoiceClass
    {
        //Invoice Info
        public uint IDInvoice { get; }
        public string ? IssueDate { get; set; }
        public string ? ExecutionDate { get; set; }

        public string? PaymentType { get; set; }
        public string? PaymentDate { get; set; }
        public decimal Paid { get; set; }
        public decimal PaidYet { get; set; }
        public string? BruttoValueInWords { get; set; }

        //SELLER INFO
        
        Seller ? seller { get; set; }

        //BUYER INFO
    
        Buyer ? buyer { get; set; }

        //PRODUCT INFO - Product.cs

        Product ? product { get; set; }

    }
}
