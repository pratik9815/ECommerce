using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class SubCategory
    {
        public Guid Id { get; set; }
        public string SubCategoryName { get; set; }
        public string SubCategoryDescription { get; set;}
        public Category Category { get; set; }
        public Guid CategoryId { get; set; }
    }
}