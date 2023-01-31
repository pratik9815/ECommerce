using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Common.Category
{
    public class GetCategoryCommand
    {
        public string CategoryName { get; set; }
        public string Description { get; set; }
    }
    public class CreateCategoryCommand
    {
        public string CategoryName { get; set; }
        public string Description { get; set; }
    }
    public class UpdateCategoryCommand
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
    }

}
