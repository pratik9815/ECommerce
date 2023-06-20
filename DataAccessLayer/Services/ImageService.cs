using DataAccessLayer.Models;
using DataAccessLayer.Query.Product;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace DataAccessLayer.Services
{
    public class ImageService 
    {
        public static string GetByteImage(ImageList image, string path)
        {
            //string path = _webHostEnvironment.WebRootPath + @"/images/" + image.ImageName;
            using var img = Image.Load(path);
            using var m = new MemoryStream();
            img.Save(m, new JpegEncoder());
            byte[] imageBytes = m.ToArray();
            return ("data:image/jpeg;base64," + Convert.ToBase64String(imageBytes));
        }

        public static async Task<ProductImage> StoreImage(IFormFile image, string fileWithPath)
        {
            var ext = Path.GetExtension(image.FileName);
            var uniqueString = Guid.NewGuid().ToString();
            var fileName = uniqueString + ext;
            var filePath = Path.Combine(fileWithPath, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            var productImage = new ProductImage
            {
                ImageName = fileName
            };
            return productImage;
        }
    }
}
