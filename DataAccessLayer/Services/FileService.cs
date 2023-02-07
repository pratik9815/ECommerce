using DataAccessLayer.Common.Product;
using DataAccessLayer.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public Tuple<int, string> SaveImage(CreateProductCommand product)
        {
            try
            {
                //.content root path
                var contentPath = _webHostEnvironment.WebRootPath;
                var path = Path.Combine(contentPath, "Images", product.Name);

                //var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images", product);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //Check allowed extensions
                var ext = Path.GetExtension(product.Img.FileName);
                var allowedExtension = new string[] { ".jpg", ".png", ".jpeg", ".pdf" };

                if (!allowedExtension.Contains(ext))
                {
                    string msg = string.Format("Only {0} extension are allowed", string.Join(",", allowedExtension));
                    return new Tuple<int, string>(0, msg);
                }
                string uniqueString = Guid.NewGuid().ToString();
                //we are trying to create a unique file name here
                var newFileName = uniqueString + ext;

                var fileWithPath = Path.Combine(path, newFileName);

                var stream = new FileStream(fileWithPath, FileMode.Create);
                product.Img.CopyTo(stream);
                stream.Close();
                return new Tuple<int, string>(1, newFileName);
            }
            catch (Exception ex)
            {
                return new Tuple<int, string>(0, "Error has occured");
            }
        }

        public bool DeleteImage(string imageFileName)
        {
            try
            {
                var wwwPath = _webHostEnvironment.WebRootPath;
                var path = Path.Combine(wwwPath, "Uploads\\", imageFileName);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}

