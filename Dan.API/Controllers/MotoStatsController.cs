using Dan.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dan.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MotoStatsController : ControllerBase
    {
        private readonly DanDbContext _context;

        public MotoStatsController(DanDbContext context)
        {
            _context = context;
        }

        // GET: api/motostats
        [HttpGet]
        public IActionResult GetMotoStats()
        {
            // Number of available motos
            var availableMotos = _context.Motos.Sum(m => m.Total);

            // Calculate the start of the current month
            var startOfMonth = DateTime.Now.Date.AddDays(1 - DateTime.Now.Day);

            // Users registered this month
            var usersRegisteredThisMonth = _context.Users
                .Where(u => u.DateJoined >= startOfMonth)
                .ToList();

            // Number of motos bought this month
            var motosBoughtThisMonth = usersRegisteredThisMonth.Sum(u => u.AccountMoto);

            // Total price of motos bought this month
            var motoCount = _context.Motos.Count();
            decimal totalMotoPriceThisMonth = 0;
            if (motoCount > 0)
            {
                decimal totalMotoPrice = _context.Motos.Sum(m => m.Price * m.Total);
                decimal motoPrice = totalMotoPrice / motoCount;

                totalMotoPriceThisMonth = motosBoughtThisMonth * motoPrice ?? 0;
            }

            var data = new
            {
                available_motos = availableMotos,
                motos_bought_this_month = motosBoughtThisMonth,
                total_price_this_month = totalMotoPriceThisMonth
            };

            return Ok(data);
        }
    }
}

