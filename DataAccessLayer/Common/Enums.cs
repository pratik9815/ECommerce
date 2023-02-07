using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Common
{
    public enum UserType
    {
        SuperAdmin = 0,
        Admin = 1,
        User = 2,
    }
    public enum RoleType
    {
        SuperAdmin = 0,
        Admin = 1,
        User = 2,
    }
    public enum Gender
    {
        Male = 0,
        Female = 1,
    }

    public enum ProductStatus
    {
        [Display(Name = "InStock")]
        InStock = 0,
        [Display(Name = "OutOfStock")]
        OutOfStock = 1,
        [Display(Name = "Damaged")]
        Damaged = 2,
    }
    public enum OrderStatus
    {
        [Display(Name = "OrderRequested")]
        Pending = 0,
        [Display(Name = "Rejected")]
        OrderRejected = 1,
        [Display(Name = "Processing")]
        OrderProcessing = 2,
        [Display(Name = "Delivered")]
        OrderDelivered = 3

    }


}
