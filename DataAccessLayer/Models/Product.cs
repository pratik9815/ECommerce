using DataAccessLayer.Common;
using DataAccessLayer.Models.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Product : AuditableEntity
    {
        public Product()
        {
            ProductCategories = new List<ProductCategory>();
            ProductImages = new List<ProductImage>();
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } 
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string Img { get; set; }
        public ProductStatus ProductStatus { get; set; }
        //product category relationship
        public ICollection<ProductCategory> ProductCategories { get; set; }

        //order-details realationship   
        public ICollection<OrderDetails> OrderDetails { get; set; }

        //if we want to set the category while creating the product we should define it inside the product constructor\

        public ICollection<ProductImage> ProductImages { get; set; }

        public void AddProductCategory(Product product , Guid CategoryId)
        {
            var category = new ProductCategory
            {
                ProductId = product.Id,
                CategoryId = CategoryId
            };
            ProductCategories.Add(category);
        }

        public void AddProductCategory(ProductCategory productCategory)
        {

            ProductCategories.Add(productCategory);
        }

    }
}
