using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchneiderDashboard.Models;

namespace SchneiderDashboard.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Read Function
        public async Task<IActionResult> Index()
        {
            var employees = await _context.Employees
                .Include(e => e.GenderCodeNavigation)
                .Include(e => e.DepartmentCodeNavigation)
                .ToListAsync();
            return View(employees);
        }

        public IActionResult Create()
        {
            ViewBag.Genders = _context.Genders.ToList();
            ViewBag.Departments = _context.Departments.ToList();
            return View();
        }

        // Create Function
        [HttpPost]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Genders = _context.Genders.ToList();
            ViewBag.Departments = _context.Departments.ToList();
            return View(employee);
        }

        // Edit Function 
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound();

            ViewBag.Genders = _context.Genders.ToList();
            ViewBag.Departments = _context.Departments.ToList();

            return View(employee);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Employee employee)
        {
            if (id != employee.SesaId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Employees.Any(e => e.SesaId == id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Genders = _context.Genders.ToList();
            ViewBag.Departments = _context.Departments.ToList();

            return View(employee);
        }

        // Delete Function based on ID
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();

            var employee = await _context.Employees
                .Include(e => e.DepartmentCodeNavigation)
                .Include(e => e.GenderCodeNavigation)
                .FirstOrDefaultAsync(e => e.SesaId == id);

            if (employee == null) return NotFound();

            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }




    }
}
