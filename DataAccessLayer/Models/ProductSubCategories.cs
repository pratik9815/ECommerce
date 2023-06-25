using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class ProductSubCategories
    {
        //product category ko satta yo table use garxam hami

        public Guid Id { get; set; }
        public Category Category { get; set; }
        public Guid CategoryId { get; set; }   
        public SubCategory SubCategory { get; set; }
        public Guid SubCategoryId { get; set; } 
        public Product Product { get; set; }
        public Guid ProductId { get; set; }     
    }
}
