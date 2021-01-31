using System;
using System.ComponentModel.DataAnnotations;

namespace Models.Utility
{
    public class Bill
    {
        [Key]
        public string bill_number { get; set; }
        public string period_number { get; set; }
        public string device_code { get; set; }
        public string device_ref { get; set; }
        public int bill_price { get; set; }
        public DateTime date_lottery { get; set; }
        public DateTime date_bill { get; set; }
        public TimeSpan time_bill { get; set; }
    }
}

