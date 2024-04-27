using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Common.Category
{
    public class GetCategoryCommand
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
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


    public class CreateCategoryWithSubCategoryCommand
    {
        public string CategoryName { get; set; }
        public string Description { get; set; }

        public IEnumerable<CreateSubCategory> subCategory { get; set; }
    }
    public class UpdateCategoryWithSubCategoryCommand
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public IEnumerable<CreateSubCategory> subCategory { get; set; }
    }
    public class CreateSubCategory
    {
        public string SubCategoryName { get; set; }
        public string SubCategoryDescription { get; set; }
    }
    public class GetCategoryWithSubCategory
    {
        public Guid id { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<GetSubCategory> subCategories { get; set; }
    }
    public class GetSubCategory
    {
        public Guid Id { get; set; }
        public string SubCategoryName { get; set; }
        public string SubCategoryDescription { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
