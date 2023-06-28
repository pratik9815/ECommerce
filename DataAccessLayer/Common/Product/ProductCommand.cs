using DataAccessLayer.Models;
using DataAccessLayer.Query.Product;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Common.Product
{
    public class GetProductCommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public ImageList Img { get; set; }
        public ProductStatus ProductStatus { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

    }
    public class CreateProductCommand
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public Guid CategoryId { get; set; }
    }

    public class CreateProductWithImageCommand
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public IFormFile Img { get; set; }
        public ProductStatus ProductStatus { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
    public class CreateProductWithImagesCommand
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public ProductStatus ProductStatus { get; set; }
        public IEnumerable<IFormFile> Img { get; set; }
        public DateTime? CreatedDate { get; set; }
        public IEnumerable<Guid> CategoryId  { get; set; }
    }

    //This is to add subcategory and images
    public class CreateCommand
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public ProductStatus ProductStatus { get; set; }
        public IEnumerable<IFormFile> Img { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid CategoryId { get; set; } 
        public IEnumerable<Guid> SubCategoryId { get; set; } 
    }
    public class UpdateProductCommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public List<Guid> Categories { get; set; }  //Gives id of the respective product
        public ProductStatus ProductStatus { get; set; }
        public string Img { get; set; }

    }

    public class GetProductWithCategory
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string CategoryName { get; set; }
        public ImageList Img { get; set; }
    }


    public class GetProductListCommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public ImageList ImageLists { get; set; }
        public ProductStatus ProductStatus { get; set;}
    }

    public class GetProductWithSubCategory
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string SubCategoryName { get; set; }
        public ImageList Img { get; set; }
    }
    //public Guid Id { get; set; }
    //public string Name { get; set; }
    //public string Description { get; set; }
    //public double Price { get; set; }
    //public int Quantity { get; set; }
    //public string ImgThumbnail { get; set; }
    //public ProductStatus ProductStatus { get; set; }
    //public string CreatedBy { get; set; }
    //public string UpdatedBy { get; set; }
    //public DateTime? CreatedDate { get; set; }
    //public List<ImageList> ImageLists { get; set; }
    //public List<CategoryList> Categories { get; set; } //Gives the category of the respective product
}
