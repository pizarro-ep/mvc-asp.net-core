using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using University.Data;
using University.Models;

namespace University.Controllers
{
    public class CourseController : Controller
    {
        private readonly SchoolContext _context;

        public CourseController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Course
        public async Task<IActionResult> Index()
        {
            // incluye cursos relacionados
            var courses = _context.Courses.Include(c => c.Department).AsNoTracking();
            return View(await courses.ToListAsync());
            //return View(await _context.Courses.ToListAsync());
        }

        // GET: Course/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // var course = await _context.Courses.FirstOrDefaultAsync(m => m.CourseID == id);
            var course = await _context.Courses.Include(c => c.Department).AsNoTracking().FirstOrDefaultAsync(m => m.CourseID == id); 
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Course/Create
        public IActionResult Create()
        {
            PopulateDepartmentsDropDownList(); //add
            return View();
        }

        // POST: Course/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseID,Title,Credits,DepartmentID")] Course course)
        //public async Task<IActionResult> Create([Bind("CourseID,Title,Credits")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            /*else{
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Error: {error.ErrorMessage}");
                    ModelState.AddModelError("", error.ErrorMessage);
                }
            } */ 
            PopulateDepartmentsDropDownList(course.DepartmentID); //add
            return View(course);
        }

        // GET: Course/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var course = await _context.Courses.FindAsync(id);
            var course = await _context.Courses.AsNoTracking().FirstOrDefaultAsync(m => m.CourseID == id);
            if (course == null)
            {
                return NotFound();
            }
            PopulateDepartmentsDropDownList(course.DepartmentID); // add
            
            return View(course);
        }

        // POST: Course/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseID,Title,Credits")] Course course)
        {
            if (id != course.CourseID)
            {
                return NotFound();
            }
            
            var courseToUpdate = await _context.Courses.FirstOrDefaultAsync(c => c.CourseID == id); // add

            //if (ModelState.IsValid)
            if (await TryUpdateModelAsync<Course>(courseToUpdate, "", c => c.Credits, c => c.DepartmentID, c => c.Title))
            {
                try
                {
                    //_context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /*DbUpdateConcurrencyException*/)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator."); // add
                    /* if (!CourseExists(course.CourseID)) return NotFound(); 
                    else throw; */
                }
                return RedirectToAction(nameof(Index));
            }
            PopulateDepartmentsDropDownList(courseToUpdate.DepartmentID); // add
            return View(course);
        }

        // GET: Course/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var course = await _context.Courses.FirstOrDefaultAsync(m => m.CourseID == id);
            var course = await _context.Courses.Include(c => c.Department).AsNoTracking().FirstOrDefaultAsync(m => m.CourseID == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CourseID == id);
        }

        // Construir el select para los departamentos
        private void PopulateDepartmentsDropDownList(object selectedDepartment = null){
            var departmensQuery = from d in _context.Departments
                                  orderby d.Name
                                  select d;
            ViewBag.DepartmentID = new SelectList(departmensQuery.AsNoTracking(), "DepartmentID", "Name", selectedDepartment);
        }
    }
}
