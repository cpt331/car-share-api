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
    
    public partial class PaymentMethod
    {
        public int AccountID { get; set; }
        public string CardNumber { get; set; }
        public string CardName { get; set; }
        public string Expiry { get; set; }
        public string CVV { get; set; }
        public string CardType { get; set; }
    
        public virtual CardType CardType1 { get; set; }
        public virtual User User { get; set; }
    }
}