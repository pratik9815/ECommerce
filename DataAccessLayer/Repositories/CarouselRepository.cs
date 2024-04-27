using DataAccessLayer.DataContext;
using DataAccessLayer.Query.Product;
using DataAccessLayer.Repositories.IRepositories;
using DataAccessLayer.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
    public class CarouselRepository : ICarouselRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;   
        public CarouselRepository(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }


        public async Task<List<ImageList>> GetCarouselImages()
        {
            var images = await _context.ProductImages.Select(x =>new ImageList
            {
                ImageName = x.ImageName
            }).ToListAsync();
            // Randomize the order of images
            var random = new Random();
            var selectedImages = images.OrderBy(x => random.Next()).Take(3).ToList();

            foreach(var image in selectedImages)
            {
                if(image.ImageName is not null)
                {
                    string path = _webHostEnvironment.WebRootPath + @"/images/" + image.ImageName;
                    image.ImageUrl = ImageService.GetByteImage(image,path);
                }
            }
            return selectedImages;      
        }


    }
}
