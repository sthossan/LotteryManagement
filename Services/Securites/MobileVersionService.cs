using Models.DataContext;
using Models.Utility;
using Services.Repository.Implementation;
using System;
using System.Linq;

namespace Services
{
    public class MobileVersionService : GenericRepository<MobileVersion>, IMobileVersionService
    {
        private readonly EfDbContext _context;
        public MobileVersionService(EfDbContext context) : base(context)
        {
            _context = context;
        }
        public bool IsMobileVersionLatest(string versionName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(versionName)) return false;

                if (_context.MobileVersion.OrderByDescending(t => t.version_date).FirstOrDefault(t => t.version_status == 1).version_name == versionName)
                    return true;
                else return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

    }
}
