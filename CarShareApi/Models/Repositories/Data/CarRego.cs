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
    
    public partial class CarRego
    {
        public int VehicleID { get; set; }
        public string RegistrationNum { get; set; }
        public string State { get; set; }
        public System.DateTime ExpiryDate { get; set; }
    
        public virtual Car Car { get; set; }
    }
}
