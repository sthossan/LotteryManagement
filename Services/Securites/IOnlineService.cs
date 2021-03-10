using Models.Utility;
using Services.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services
{
    public interface IOnlineService : IGenericRepository<Online>
    {
        bool IsSeverOnline();

        object Get(string deviceCode);
    }
}
