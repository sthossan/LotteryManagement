using Models.Utility;
using Services.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services
{
    public interface IMobileVersionService : IGenericRepository<MobileVersion>
    {
        bool IsMobileVersionLatest(string versionName);
    }
}
