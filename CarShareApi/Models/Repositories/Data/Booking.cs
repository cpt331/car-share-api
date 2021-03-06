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
    
    public partial class Booking
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Booking()
        {
            this.TransactionHistories = new HashSet<TransactionHistory>();
        }
    
        public int BookingID { get; set; }
        public int VehicleID { get; set; }
        public int AccountID { get; set; }
        public string BookingStatus { get; set; }
        public System.DateTime CheckOut { get; set; }
        public Nullable<System.DateTime> CheckIn { get; set; }
        public Nullable<decimal> TimeBilled { get; set; }
        public decimal BillingRate { get; set; }
        public Nullable<decimal> AmountBilled { get; set; }
        public string CityPickUp { get; set; }
        public string CityDropOff { get; set; }
    
        public virtual Booking Booking1 { get; set; }
        public virtual Booking Booking2 { get; set; }
        public virtual BookingStatu BookingStatu { get; set; }
        public virtual Car Car { get; set; }
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TransactionHistory> TransactionHistories { get; set; }
    }
}
