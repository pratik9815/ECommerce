using DataAccessLayer.Common.Dashboard;
using DataAccessLayer.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECOMMERCE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardController(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }
        [HttpGet("get-dashboard-data")]
        [AllowAnonymous]
        public async  Task<ActionResult<List<DashboardCommand>>> GetDashboardData()
        {
            var result = await _dashboardRepository.GetDataForDashboard().ToListAsync();
            return Ok(result);
        }
        [HttpGet("get-popular-product")]
        public async Task<ActionResult<List<GetPopularProducts>>> GetPopularProductForDashboard()
        {
            var result = await _dashboardRepository.GetPopularProductForDashboard().ToListAsync();
            if(result != null)
            {
                return Ok(result);
            }
            return NotFound("No records are available");
        }
        [HttpGet("get-dashboard-data-with")]
        [AllowAnonymous]
        public async Task<ActionResult<List<DashboardCommand>>> GetDataForDashboardUsingMethodSyntax()
        {
            var result = await _dashboardRepository.GetDataForDashboardUsingMethodSyntax().ToListAsync();
            return Ok(result);
        }
        [HttpGet("get-order-status-for-dashboard")]
        [AllowAnonymous]
        public async Task<ActionResult<List<GetOrderStatus>>> GetProductStatus()
        {
            var result = await _dashboardRepository.GetProductStatus().ToListAsync();
            return Ok(result);
        }
    }
}
