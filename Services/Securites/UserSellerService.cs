using Models.DataContext;
using Models.Securites;
using Services.Repository.Implementation;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Services.Securites
{
    public class UserSellerService : GenericRepository<UserSeller>, IUserSellerService
    {
        private readonly EfDbContext _context;
        
        public UserSellerService(EfDbContext context) : base(context)
        {
            _context = context;
        }

        public bool IsValidUserCredentials(string deviceCode, string password)
        {
            try
            {
                return _context.UserSeller.Any(t => t.device_code == deviceCode && t.us_pwd == password && t.us_status == 1);
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public string GetUserRole()
        {
            //if (!IsAnExistingUser(userName)) return string.Empty;
            //if (userName == "admin") return UserRoles.Admin;
            return UserRoles.BasicUser;
        }
    }

    public static class UserRoles
    {
        public const string Admin = nameof(Admin);
        public const string BasicUser = nameof(BasicUser);
    }
}
