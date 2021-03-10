using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Utility
{
    public class BillDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int bd_id { get; set; }
        public string bill_number { get; set; }
        public int lottery_number { get; set; }
        public int lottery_price { get; set; }
        public DateTime date_bill_detail { get; set; }
    }
}
