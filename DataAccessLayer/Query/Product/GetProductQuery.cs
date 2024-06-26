﻿using DataAccessLayer.Common;

namespace DataAccessLayer.Query.Product
{
    public class GetProductQuery
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string ImgThumbnail { get; set; }
        public ProductStatus ProductStatus { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public List<ImageList> ImageLists { get; set; }
        public List<CategoryList> Categories { get; set; } //Gives the category of the respective product
        public List<SubCategoryList> SubCategories { get; set; }
    }
    public class ImageList
    {
        public Guid ImageId { get; set; }
        public string ImageName { get; set; }
        public string ImageUrl { get; set; }
    }
    public class CategoryList
    {
        public Guid CategoryId { get; set;}
        public string CategoryName { get; set; }
    }
    public class SubCategoryList
    {
        public Guid SubCategoryId { get; set; }
        public string SubCategoryName { get; set;}
    }

}
