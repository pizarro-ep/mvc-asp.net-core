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
    public class StudentController : Controller
    {
        private readonly SchoolContext _context;

        public StudentController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Student
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string search, int? pageNumber)
        //public async Task<IActionResult> Index(string sortOrder, string search)
        //public async Task<IActionResult> Index(string sortOrder)
        {
            // Ordenamiento, busqueda y filtro de datos par mostrar en la tabla
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            
            if(search !=null){ pageNumber = 1; } else { search = currentFilter; }

            // para filtros de busqueda
            ViewData["CurrentFilter"] = search;

            var students = from s in _context.Students
                           select s;
            
            if (!string.IsNullOrEmpty(search)){
                students = students.Where(w => w.LastName.Contains(search) || w.Surname.Contains(search));
            }

            switch(sortOrder){
                case "name_desc": students = students.OrderByDescending(s => s.LastName); break;
                case "Date": students = students.OrderBy(s => s.EnrollmentDate); break;
                case "date_desc": students = students.OrderByDescending(s => s.EnrollmentDate); break;
                default: students = students.OrderBy(s => s.LastName); break;
            }
            int pageSize = 3;
            return View(await PaginatedList<Student>.CreateAsync(students.AsNoTracking(), pageNumber ?? 1, pageSize));
            //return View(await students.AsNoTracking().ToListAsync());
            //return View(await _context.Students.ToListAsync());
        }

        // GET: Student/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var student = await _context.Students.FirstOrDefaultAsync(m => m.Id == id);
            // Agrega las inscripciones y los cursos
            var student = await _context.Students.Include(s => s.Enrollments) // Incluir las incripciones
                          .ThenInclude(e => e.Course) // Luego incluir los cursos
                          .AsNoTracking()   // Mejora el rendimiento si no se actualizan las entidades
                          .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Student/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Student/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LastName,Surname,EnrollmentDate")] Student student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(student);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }  catch (DbUpdateException /* ex */) {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(student);
        }

        // GET: Student/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Student/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LastName,Surname,EnrollmentDate")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /*DbUpdateConcurrencyException*/)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Student/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        //public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .AsNoTracking() // new
                .FirstOrDefaultAsync(m => m.Id == id);
            /*if (student == null)
            {
                return NotFound();
            }*/
             if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists see your system administrator.";
            }

            return View(student);
        }

        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            //if (student != null)
            if (student == null)
            {
                // _context.Students.Remove(student);
                return RedirectToAction(nameof(Index));
            }
            try{
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }catch (DbUpdateException){
                return RedirectToAction(nameof(Delete), new {id = id, saveChangesError = true});
            }

        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
