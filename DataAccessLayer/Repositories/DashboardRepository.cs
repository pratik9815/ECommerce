using DataAccessLayer.Common;
using DataAccessLayer.Common.Dashboard;
using DataAccessLayer.Common.Order;
using DataAccessLayer.DataContext;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        public IQueryable<DashboardCommand> GetDataForDashboard()
        {

            var result = _context.OrderDetails
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

        public IQueryable<GetPopularProducts> GetPopularProductForDashboard()
        {
            var popularProduct = _context.OrderDetails
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


        public IQueryable<DashboardCommand> GetDataForDashboardUsingMethodSyntax()
        {
            //The select many will select all the data and the group by will group the data and gives out the value 

            var result = _context.OrderDetails
                .OrderBy(x => x.CreatedDate)
                .Include(od => od.Order)
                .Where(od => !od.IsDeleted)
                .Include(od => od.Product)
                    .ThenInclude(p => p.ProductCategories)
                        .ThenInclude(pc => pc.Category)
                .Where(pc => !pc.Product.IsDeleted)
                .Where(od => !od.Order.IsDeleted)
                .Where(pc => !pc.Product.ProductCategories.Any(cat => cat.Category.IsDeleted))
                .Where(pc => !pc.Product.ProductCategories.First().IsDeleted)
                .SelectMany(od => od.Product.ProductCategories.Select(pc => new DashboardCommand
                {
                    Amount = od.Quantity * od.Product.Price,
                    CategoryName = pc.Category.CategoryName,
                    CategoryId = pc.Category.Id
                }))
                .GroupBy(dc => new { dc.CategoryName, dc.CategoryId })
                .Select(grouped => new DashboardCommand
                {
                    Amount = grouped.Sum(dc => dc.Amount),
                    CategoryName = grouped.Key.CategoryName,
                    CategoryId = grouped.Key.CategoryId
                });

            return result;



        }


        //Pending,Requested and completed sales

        public IQueryable<GetOrderStatus> GetProductStatus()
        {
            var orderStatus = _context.Orders.Where(o => !o.IsDeleted)
                                        .GroupBy(o => o.OrderStatus)
                                        .Select(x => new GetOrderStatus
                                        {
                                           OrderedQuantity =  x.Count(),
                                           OrderStatus = x.Key
                                        });
            return orderStatus;
        }
        

    }
}

