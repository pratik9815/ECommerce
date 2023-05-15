﻿using DataAccessLayer.Models;
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
        public string Img { get; set; }
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
        public DateTime? CreatedDate { get; set; }

    }
    public class CreateProductWithImagesCommand
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public IEnumerable<IFormFile> Img { get; set; }
        public DateTime? CreatedDate { get; set; }
        public IEnumerable<Guid> CategoryId  { get; set; }

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

   

}
