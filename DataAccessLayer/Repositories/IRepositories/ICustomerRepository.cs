using DataAccessLayer.Common.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.IRepositories
{
    public interface ICustomerRepository
    {
        Task<GetCustomerCommand> GetCustomerById(Guid customerId);
    }
}
