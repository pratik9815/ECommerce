using DataAccessLayer.Common.Customer;
using DataAccessLayer.Common.Order;
using DataAccessLayer.DataContext;
using DataAccessLayer.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext context)
        {
                _context = context;
        }

        public async Task<List<GetCustomerCommand>> GetCustomerById(Guid customerId)
        {
            var customer = await _context.Customers.AsNoTracking()
                                          .Where(c =>c.Id == customerId)
                                          .Select(x => new GetCustomerCommand
                                          {
                                              Id = x.Id,
                                              FullName = x.FullName,
                                              Email = x.Email,  
                                              Address = x.Address,
                                              Orders = _context.Orders.AsNoTracking()
                                                                .Select(x =>new GetOrderCommand
                                                                {
                                                                    Id= x.Id,

                                                                }).ToList(),   
                                          }).ToListAsync();
            return customer;
        }

    }
}
