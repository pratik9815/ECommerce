using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Common
{
    public enum UserType
    {
        //If we do not explicitely set the value then the values would start from 0
        [Display(Name = "SuperAdmin")]
        SuperAdmin = 0,
        [Display(Name = "Admin")]
        Admin = 1,
        [Display(Name = "Customer")]
        Customer = 2,
    }
    public enum RoleType
    {
        [Display(Name ="SuperAdmin")]
        SuperAdmin = 0,
        [Display(Name = "Admin")]
        Admin = 1,
        [Display(Name = "Customer")]
        Customer = 2,
    }
    public enum Gender
    {
        [Display(Name ="Male")]
        Male = 0,
        [Display(Name ="Female")]
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
        [Display(Name = "Limited Stock Available")]
        LimitedStock = 3,
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

    public enum Rating
    {
        VeryBad = 0,
        Bad =1,
        Average = 2,    
        Good = 3,   
        VeryGood = 4
    }


}
