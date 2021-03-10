using Models.DataContext;
using Models.Utility;
using Services.Repository.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ViewModels;

namespace Services.DataService
{
    public class SaleService : GenericRepository<Bill>, ISaleService
    {
        private readonly EfDbContext _context;
        public SaleService(EfDbContext context) : base(context)
        {
            _context = context;
        }

        public void CheckNumberPrice(string lotteryNumber, int lotteryPrice)
        {
            var max_lenght = _context.DigitLenght.FirstOrDefault().max_lenght;
            if (lotteryNumber.Length > max_lenght)
                throw new Exception($"lottery number can't over {max_lenght} digit");

            if (lotteryPrice < 1000)
                throw new Exception($"Price can\'t be lower 1000 kip");

            if (lotteryPrice % 1000 != 0)
                throw new Exception($"price should be multiple 1000 kip");

            var ln_Data = _context.LotteryNumber.FirstOrDefault(t => t.lottery_number == lotteryNumber);

            if (ln_Data != null)
            {
                //throw new Exception("invalid lottery number");
                if (ln_Data.ln_status != 1)
                    throw new Exception("this number is over limit sale");

                var lottery_price_db = _context.BillDetail.Where(t => t.lottery_number == Convert.ToInt32(lotteryNumber) && t.date_bill_detail == DateTime.Today).Sum(t => t.lottery_price);

                var realTimeMaxPrice = ln_Data.max_sell - lottery_price_db;

                if (lottery_price_db == 0)
                {
                    if (lotteryPrice > ln_Data.max_sell)
                        throw new Exception($"{lotteryNumber} \"number input\" can buy \"{realTimeMaxPrice}\" please modify it");
                }
                else if (realTimeMaxPrice == 0)
                {
                    throw new Exception($"can not sell {lotteryNumber}");
                }
                else if (lotteryPrice + realTimeMaxPrice > ln_Data.max_sell)
                {
                    throw new Exception($"{lotteryNumber} \"number input\" can buy \"{realTimeMaxPrice}\" please modify it");
                }

            }
        }

        public List<SaleViewModel> GetSellSetNumber(string lotteryNumber, int lotteryPrice)
        {
            var lotteryName = _context.SaleSetNumber.FirstOrDefault(t => t.lottery_number == lotteryNumber && t.lottery_digit == lotteryNumber.Length).lottery_name;
            var saleSetNumberList = _context.SaleSetNumber.Where(t => t.lottery_name == lotteryName && t.lottery_digit == lotteryNumber.Length).ToList();
            if (saleSetNumberList.Count() == 0)
                throw new Exception($"invalid lotteryNumber");

            List<SaleViewModel> saleViewModelList = new List<SaleViewModel>();

            foreach (var item in saleSetNumberList)
            {
                SaleViewModel saleViewModel = new SaleViewModel
                {
                    lotteryNumber = item.lottery_number,
                    lotteryPrice = lotteryPrice
                };
                saleViewModelList.Add(saleViewModel);
            }

            return saleViewModelList;
        }

        public void InsertSale(string deviceCode, string periodNumber, List<SaleViewModel> saleViewModelList)
        {
            try
            {
                // check period_number online
                if (!_context.Online.Any(t => t.period_number == periodNumber && t.online_status == 1))
                    throw new Exception("Invalid draw number");

                var totalBillDetails = _context.Bill.Where(t => t.period_number == periodNumber && t.device_code == deviceCode && t.date_bill == DateTime.Today)
                    .Join(_context.BillDetail, t => t.bill_number, y => y.bill_number, (t, y) => new
                    {
                        billDetails = y

                    }).Count();

                if (totalBillDetails >= 12)
                {
                    throw new Exception("Today your sell limit is over");
                }
                if ((totalBillDetails + saleViewModelList.Count()) > 12)
                {
                    throw new Exception("Today already you sell " + totalBillDetails);
                }

                var lotteryNumberArray = saleViewModelList.Select(t => Convert.ToInt32(t.lotteryNumber)).ToList();

                var ln_list = _context.LotteryNumber.Where(t => lotteryNumberArray.Contains(Convert.ToInt32(t.lottery_number))).ToList();
                var max_lenght = _context.DigitLenght.FirstOrDefault().max_lenght;

                var lotteryPriceList = _context.BillDetail.Where(t => lotteryNumberArray.Contains(t.lottery_number) && t.date_bill_detail == DateTime.Today).ToList();


                foreach (var item in saleViewModelList)
                {
                    if (item.lotteryNumber.Length > max_lenght)
                        throw new Exception($"{item.lotteryNumber} can't over {max_lenght} digit");

                    if (item.lotteryPrice < 1000)
                        throw new Exception($"Price can\'t be lower 1000 kip");

                    if (item.lotteryPrice % 500 != 0)
                        throw new Exception($"price should be multiple 1000 kip");

                    var ln_Data = ln_list.FirstOrDefault(t => t.lottery_number == item.lotteryNumber);

                    if (ln_Data != null)
                    {
                        //throw new Exception($"invalid lottery number {item.lotteryNumber}");

                        if (ln_Data.ln_status != 1)
                            throw new Exception($"{item.lotteryNumber} number is over limit sale");

                        var lottery_price_db = lotteryPriceList.Where(t => t.lottery_number == Convert.ToInt32(item.lotteryNumber)).Sum(t => t.lottery_price);

                        var realTimeMaxPrice = ln_Data.max_sell - lottery_price_db;

                        if (lottery_price_db == 0)
                        {
                            if (item.lotteryPrice > ln_Data.max_sell)
                                throw new Exception($"{item.lotteryNumber} \"number input\" can buy \"{realTimeMaxPrice}\" please modify it");
                        }
                        else if (realTimeMaxPrice == 0)
                        {
                            throw new Exception($"can not sell {item.lotteryNumber}");
                        }
                        else if (item.lotteryPrice + realTimeMaxPrice > ln_Data.max_sell)
                        {
                            throw new Exception($"{item.lotteryNumber} \"number input\" can buy \"{realTimeMaxPrice}\" please modify it");
                        }

                    }

                }


                var billAutoNumber = _context.Bill.Where(t => t.period_number == periodNumber && t.device_code == deviceCode && t.date_bill == DateTime.Today).Count() + 1;
                var newbillId = periodNumber + deviceCode + (billAutoNumber.ToString().Length == 1 ? "000" + billAutoNumber : "00" + billAutoNumber);

                var devRefCode = _context.Device.FirstOrDefault(t => t.device_code == deviceCode).device_ref;

                var bill = new Bill
                {
                    bill_number = newbillId,
                    period_number = periodNumber,
                    device_code = deviceCode,
                    device_ref = devRefCode,
                    bill_price = saleViewModelList.Sum(t => t.lotteryPrice),
                    date_bill = DateTime.Now,
                    time_bill = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)
                };
                _context.Add(bill);

                var list = new List<BillDetail>();
                foreach (var item in saleViewModelList)
                {
                    var model = new BillDetail
                    {
                        //bd_id=(int)DateTime.Now.Ticks,
                        bill_number = newbillId,
                        lottery_number = int.Parse(item.lotteryNumber),
                        lottery_price = item.lotteryPrice,
                    };
                    list.Add(model);
                }
                _context.AddRange(list);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
