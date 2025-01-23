using Clinic.Areas.Admin.ViewModels;
using Clinic.DAL;
using Clinic.Models;
using Clinic.Utilities.Enums;
using Clinic.Utilities.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Clinic.Areas.Admin.Controlers
{
    [Area("Admin")]
    //[Authorize("Admin, Moderator")]
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public EmployeeController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        public async Task<ActionResult> Index()
        {
            List<GetEmployeeVm> employeeVms = await _context.Employees
                .Where(x => x.IsDeleted == false)
                .OrderBy(x => x.Order)
                .Select(x => new GetEmployeeVm
                {
                    Id = x.Id,
                    Image = x.Image,
                    Name = x.Name,
                    Surname = x.Surname,
                    Profession = x.Profession,
                    Order = x.Order,
                }).ToListAsync();
            return View(employeeVms);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeVm employeeVm)
        {
            if (!ModelState.IsValid)
            {
                return View(employeeVm);
            }
            bool twResult = await _context.Employees.AnyAsync(x => x.Twitter == employeeVm.Twitter);
            if (twResult)
            {
                ModelState.AddModelError(nameof(CreateEmployeeVm.Twitter), "account belongs to someone else");
                return View(employeeVm);
            }
            bool insResult = await _context.Employees.AnyAsync(x => x.Instagram == employeeVm.Instagram);
            if (insResult)
            {
                ModelState.AddModelError(nameof(CreateEmployeeVm.Instagram), "account belongs to someone else");
                return View(employeeVm);
            }
            bool fbResult = await _context.Employees.AnyAsync(x => x.FaceBook == employeeVm.FaceBook);
            if (twResult)
            {
                ModelState.AddModelError(nameof(CreateEmployeeVm.FaceBook), "account belongs to someone else");
                return View(employeeVm);
            }
            bool orderResult = await _context.Employees.AnyAsync(x => x.Order == employeeVm.Order);
            if (orderResult)
            {
                ModelState.AddModelError(nameof(CreateEmployeeVm.Order), "is taken");
                return View(employeeVm);
            }
            if (!employeeVm.Image.ValidateType("image/"))
            {
                ModelState.AddModelError(nameof(CreateEmployeeVm.Image), "type is incorrect");
                return View(employeeVm);
            }
            if (!employeeVm.Image.ValidateSize(FileSize.MB, 2))
            {
                ModelState.AddModelError(nameof(CreateEmployeeVm.Image), "size is incorrect");
                return View(employeeVm);
            }
            Employee employee = new Employee()
            {
                Image = await employeeVm.Image.CreateFileAsync(_environment.WebRootPath, "assets", "img"),
                Name = employeeVm.Name,
                Surname = employeeVm.Surname,
                Profession = employeeVm.Profession,
                Twitter = employeeVm.Twitter,
                FaceBook = employeeVm.FaceBook,
                Instagram = employeeVm.Instagram,
                Order = employeeVm.Order,
                Created = DateTime.Now,
                Updated = DateTime.Now,
                IsDeleted = false
            };
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id < 1) return BadRequest();
            Employee? employee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);
            if (employee == null) return NotFound();
            UpdateEmployeeVm employeeVm = new UpdateEmployeeVm()
            {
                Photo = employee.Image,
                Name = employee.Name,
                Surname = employee.Surname,
                Profession = employee.Profession,
                Twitter = employee.Twitter,
                FaceBook = employee.FaceBook,
                Instagram = employee.Instagram,
                Order = employee.Order,
            };
            return View(employeeVm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, UpdateEmployeeVm employeeVm)
        {
            if (id == null || id < 1) return BadRequest();
            Employee? employee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);
            if (employee == null) return NotFound();
            if (!ModelState.IsValid)
            {
                return View(employeeVm);
            }
            bool twResult = await _context.Employees.AnyAsync(x => x.Twitter == employeeVm.Twitter && x.Id != employee.Id);
            if (twResult)
            {
                ModelState.AddModelError(nameof(UpdateEmployeeVm.Twitter), "account belongs to someone else");
                return View(employeeVm);
            }
            bool insResult = await _context.Employees.AnyAsync(x => x.Instagram == employeeVm.Instagram && x.Id != employee.Id);
            if (insResult)
            {
                ModelState.AddModelError(nameof(UpdateEmployeeVm.Instagram), "account belongs to someone else");
                return View(employeeVm);
            }
            bool fbResult = await _context.Employees.AnyAsync(x => x.FaceBook == employeeVm.FaceBook && x.Id != employee.Id);
            if (twResult)
            {
                ModelState.AddModelError(nameof(UpdateEmployeeVm.FaceBook), "account belongs to someone else");
                return View(employeeVm);
            }
            bool orderResult = await _context.Employees.AnyAsync(x => x.Order == employeeVm.Order && x.Id != employee.Id);
            if (orderResult)
            {
                ModelState.AddModelError(nameof(UpdateEmployeeVm.Order), "is taken");
                return View(employeeVm);
            }
            if (employeeVm.Image != null)
            {
                if (!employeeVm.Image.ValidateType("image/"))
                {
                    ModelState.AddModelError(nameof(UpdateEmployeeVm.Image), "type is incorrect");
                    return View(employeeVm);
                }
                if (!employeeVm.Image.ValidateSize(FileSize.MB, 2))
                {
                    ModelState.AddModelError(nameof(UpdateEmployeeVm.Image), "size is incorrect");
                    return View(employeeVm);
                }
                string fileName = await employeeVm.Image.CreateFileAsync(_environment.WebRootPath, "assets", "img");
                employee.Image.DeleteFile(_environment.WebRootPath, "assets", "img");
                employee.Image = fileName;
            }
            employee.Name = employeeVm.Name;
            employee.Surname = employeeVm.Surname;
            employee.Profession = employeeVm.Profession;
            employee.Twitter = employeeVm.Twitter;
            employee.FaceBook = employeeVm.FaceBook;
            employee.Instagram = employeeVm.Instagram;
            employee.Order = employeeVm.Order;
            employee.Updated = DateTime.Now;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id < 1) return BadRequest();
            Employee? employee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);
            if (employee == null) return NotFound();
            if (employee.IsDeleted)
            {
                employee.Image.DeleteFile(_environment.WebRootPath, "assets", "img");
                _context.Employees.Remove(employee);
            }
            if (!employee.IsDeleted)
            {
                employee.IsDeleted = true;
            }
            await _context.SaveChangesAsync();  
            return RedirectToAction(nameof(Index));
        }
    }
}
