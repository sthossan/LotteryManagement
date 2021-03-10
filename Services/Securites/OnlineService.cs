using Models.DataContext;
using Models.Utility;
using Services.Repository.Implementation;
using System;
using System.Linq;

namespace Services
{
    public class OnlineService : GenericRepository<Online>, IOnlineService
    {
        private readonly EfDbContext _context;
        public OnlineService(EfDbContext context) : base(context)
        {
            _context = context;
        }

        public bool IsSeverOnline()
        {
            try
            {
                var data = _context.Online.Where(t => t.online_status == 1).OrderByDescending(t => t.date_online).FirstOrDefault();
                return data != null;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public object Get(string deviceCode)
        {
            try
            {
                var drawNumber = _context.Online.Where(t => t.online_status == 1).OrderByDescending(t => t.date_online).ThenBy(t => t.time_online).Select(t => t.period_number).FirstOrDefault();
                var billNumber = _context.Bill.Where(t => t.period_number == drawNumber && t.device_code == deviceCode).Select(t => t.bill_number).FirstOrDefault();
                var billDetailList = _context.BillDetail.Where(t => t.bill_number == billNumber)
                    .Select(t => new
                    {
                        t.bill_number,
                        t.lottery_number,
                        t.lottery_price
                    }).ToList();
                var totalSale = billDetailList.Count();
                var totalCancel = _context.BillCancel.Where(t => t.period_number == drawNumber && t.bill_number == billNumber).Count();

                object obj = new
                {
                    drawNumber,
                    billNumber,
                    totalSale,
                    totalCancel,
                    billDetailList
                };
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
