using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
namespace KingsCafeApp.Models
{
    public partial class Order
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public TimeSpan? OrderTime { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public String Name { get; set; }
        public String Phone { get; set; }
        public String Email { get; set; }
        public String Address { get; set; }
        public String AddressType { get; set; }
        public double Latitude { get; set; }
        public double Logitude { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
        public string AssignedRider { get; set; }

    }
}
