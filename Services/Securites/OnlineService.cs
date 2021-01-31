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

    }
}
