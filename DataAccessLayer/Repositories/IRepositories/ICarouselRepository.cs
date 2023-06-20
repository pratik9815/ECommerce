using DataAccessLayer.Query.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.IRepositories
{
    public interface ICarouselRepository
    {
        Task<List<ImageList>> GetCarouselImages();
    }
}
