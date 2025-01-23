using Clinic.DAL;
using Clinic.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            HomeVm homeVm = new HomeVm()
            {
                Employees = await _context.Employees
                .Where(x => x.IsDeleted == false)
                .OrderBy(x => x.Order)
                .Take(4)
                .ToListAsync()
            };
            return View(homeVm);
        }
    }
}
