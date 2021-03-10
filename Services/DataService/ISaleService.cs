using Models.Utility;
using Services.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using ViewModels;

namespace Services.DataService
{
    public interface ISaleService : IGenericRepository<Bill>
    {
        void CheckNumberPrice(string lotteryNumber, int lotteryPrice);
        void InsertSale(string deviceCode, string periodNumber, List<SaleViewModel> saleViewModelList);
        List<SaleViewModel> GetSellSetNumber(string lotteryNumber, int lotteryPrice);
    }
}
