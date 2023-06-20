using DataAccessLayer.Common.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.IRepositories
{
    public interface IDashboardRepository
    {
        IQueryable<DashboardCommand> GetDataForDashboard();
        IQueryable<GetPopularProducts> GetPopularProductForDashboard();
    }
}
