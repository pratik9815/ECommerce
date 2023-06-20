﻿using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.IRepositories
{
    public interface IOrderDetailsRepository
    {
        IQueryable<OrderDetails> GetOrderDetails(Guid customerId);
    }
}
