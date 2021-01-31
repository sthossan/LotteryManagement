using System.ComponentModel.DataAnnotations;

namespace Models.Utility
{
    public class BillDetail
    {
        [Key]
        public int bd_id { get; set; }
        public string bill_number { get; set; }
        public int lottery_number { get; set; }
        public int lottery_price { get; set; }
    }
}
