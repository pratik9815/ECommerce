using DataAccessLayer.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Category : AuditableEntity
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; }    
        public string Description { get; set; }

        public ICollection<ProductCategory> ProductCategories { get; set; }

        //for multiple category
        public ICollection<SubCategory> SubCategories { get; set; }

    }
}
