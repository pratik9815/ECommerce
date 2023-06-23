using DataAccessLayer.Common.Dashboard;
using DataAccessLayer.Common.Order;
using DataAccessLayer.DataContext;
using DataAccessLayer.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDbContext _context;

        public DashboardRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public  IQueryable<DashboardCommand> GetDataForDashboard()
        {

                var result =  _context.OrderDetails
                    .OrderBy(x => x.CreatedDate)
                    .Where(x => !x.IsDeleted)
                    .Join(_context.Products.Where(p => !p.IsDeleted), od => od.ProductId, p => p.Id, (od, p) => new { od, p })
                    .Join(_context.ProductCategories.Where(pc => !pc.IsDeleted), x => x.p.Id, pc => pc.ProductId, (x, pc) => new { x.od, x.p, pc })
                    .Join(_context.Categories.Where(c => !c.IsDeleted), x => x.pc.CategoryId, c => c.Id, (x, c) => new { x.od, x.p, x.pc, c })
                    .Join(_context.Orders.Where(o => !o.IsDeleted), x => x.od.OrderId, o => o.Id, (x, o) => new { x.od, x.p, x.pc, x.c, o })
                    .GroupBy(x => new { x.c.CategoryName, x.c.Id })
                    .Select(grouped => new DashboardCommand
                    {
                        Amount = grouped.Sum(x => x.od.Quantity * x.p.Price),
                        CategoryName = grouped.Key.CategoryName,
                        CategoryId = grouped.Key.Id
                    });

            return result;
        }

        public  IQueryable<GetPopularProducts> GetPopularProductForDashboard()
        {
            var popularProduct =  _context.OrderDetails
                .Where(o => !o.IsDeleted && !o.Order.IsDeleted)
                .GroupBy(od => new { od.Product.Name, od.ProductId, od.Product.Price })
                .Select(x => new GetPopularProducts
                {
                    Quantity = x.Sum(od => od.Quantity),
                    TotalPrice = x.Key.Price * x.Sum(od => od.Quantity),
                    ProductId = x.Key.ProductId,
                    ProductName = x.Key.Name
                }).OrderByDescending(o => o.Quantity);
            return popularProduct;

        }

       

    }
}

