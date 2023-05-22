using DataAccessLayer.Query.Product;
using DataAccessLayer.Repositories.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECOMMERCE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarouselController : ControllerBase
    {
        private readonly ICarouselRepository _carouselRepository;

        public CarouselController(ICarouselRepository carouselRepository)
        {
            _carouselRepository = carouselRepository;
        }

        [HttpGet("carousel-images")]
        public async Task<ActionResult<List<ImageList>>> GetImageForCarousel()
        {
            return await _carouselRepository.GetCarouselImages();
        }
    }
}
