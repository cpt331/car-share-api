//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CarShareApi.Models.Repositories.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Receipt
    {
        public int ReceiptID { get; set; }
        public string Name { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Suburb { get; set; }
        public string State { get; set; }
        public string Postcode { get; set; }
        public string CarDescription { get; set; }
        public decimal BillingRate { get; set; }
        public decimal BilledAmount { get; set; }
        public System.DateTime ReceiptDate { get; set; }
        public string CityDropOff { get; set; }
        public Nullable<int> TransactionID { get; set; }
    
        public virtual TransactionHistory TransactionHistory { get; set; }
    }
}
