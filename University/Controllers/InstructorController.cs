using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using University.Data;
using University.Models;
using University.Models.SchoolViewModels;

namespace University.Controllers
{
    public class InstructorController : Controller
    {
        private readonly SchoolContext _context;

        public InstructorController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Instructor
        public async Task<IActionResult> Index(int? id, int? courseID)
        //public async Task<IActionResult> Index()
        {
            var viewModel = new InstructorIndexData();
            viewModel.Instructors = await _context.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.CourseAssignments).ThenInclude(i => i.Course).ThenInclude(i => i.Enrollment).ThenInclude(i => i.Student)
                .Include(i => i.CourseAssignments).ThenInclude(i => i.Course).ThenInclude(i => i.Department)
                .AsNoTracking().OrderBy(i => i.LastName).ToListAsync();

            if (id != null)
            {
                ViewData["InstructorID"] = id.Value;
                Instructor instructor = viewModel.Instructors.Where(i => i.ID == id.Value).Single();
                viewModel.Courses = instructor.CourseAssignments.Select(s => s.Course);
            }

            if (courseID != null)
            {
                ViewData["CourseID"] = courseID.Value;
                //viewModel.Enrollments = viewModel.Courses.Where(x => x.CourseID == courseID).Single().Enrollment;
                var selectedCourse = viewModel.Courses.Where(x => x.CourseID == courseID).Single();
                await _context.Entry(selectedCourse).Collection(x => x.Enrollment).LoadAsync();
                foreach (Enrollment enrollment in selectedCourse.Enrollment) {
                    await _context.Entry(enrollment).Reference(x => x.Student).LoadAsync();
                }
                viewModel.Enrollments = selectedCourse.Enrollment;
            }

            return View(viewModel);
            //return View(await _context.Instructors.ToListAsync());
        }

        // GET: Instructor/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .FirstOrDefaultAsync(m => m.ID == id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // GET: Instructor/Create
        public IActionResult Create()
        {
            var instructor = new Instructor();//add
            instructor.CourseAssignments = new List<CourseAssignment>(); //add
            PopulateAssignedCourseData(instructor); // add
            return View();
        }

        // POST: Instructor/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,LastName,Surname,HireDate")] Instructor instructor, string[] selectedCourses)
        //public async Task<IActionResult> Create([Bind("ID,LastName,Surname,HireDate")] Instructor instructor)
        {
            // add ->
            if (selectedCourses != null)
            {
                instructor.CourseAssignments = new List<CourseAssignment>();
                foreach (var course in selectedCourses)
                {
                    var courseToAdd = new CourseAssignment { InstructorID = instructor.ID, CourseID = int.Parse(course) };
                    instructor.CourseAssignments.Add(courseToAdd);
                }
            } // <- add
            if (ModelState.IsValid)
            {
                _context.Add(instructor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateAssignedCourseData(instructor); // add
            return View(instructor);
        }

        // GET: Instructor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var instructor = await _context.Instructors.FindAsync(id);
            var instructor = await _context.Instructors
                            .Include(i => i.OfficeAssignment)
                            .Include(i => i.CourseAssignments).ThenInclude(i => i.Course)// add
                            .AsNoTracking().FirstOrDefaultAsync(m => m.ID == id); 
            if (instructor == null)
            {
                return NotFound();
            }
            PopulateAssignedCourseData(instructor); // add
            return View(instructor);
        }

        // POST: Instructor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] selectedCourses)
        //public async Task<IActionResult> Edit(int? id, bool falseParam)
        //public async Task<IActionResult> Edit(int id, [Bind("ID,LastName,Surname,HireDate")] Instructor instructor)
        {
            //if (id != instructor.ID)
            if (id != id)
            {
                return NotFound();
            }

            var instructorToUpdate = await _context.Instructors
                                    .Include(i => i.OfficeAssignment)
                                    .Include(i => i.CourseAssignments).ThenInclude(i => i.Course) // add -> add
                                    .FirstOrDefaultAsync(s => s.ID == id); // add

            //if (ModelState.IsValid)
            if (await TryUpdateModelAsync<Instructor>(instructorToUpdate,"",i => i.Surname, i => i.LastName, i => i.HireDate, i => i.OfficeAssignment))
            {
                if (String.IsNullOrWhiteSpace(instructorToUpdate.OfficeAssignment?.Location)) {//new
                    instructorToUpdate.OfficeAssignment = null; //new
                } // new
                UpdateInstructorCourses(selectedCourses, instructorToUpdate); // add -> add
                try
                {
                    //_context.Update(instructor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /*DbUpdateConcurrencyException*/)
                {
                    /*if (!InstructorExists(instructor.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }*/
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
                return RedirectToAction(nameof(Index));
            }
            //return View(instructor);
            UpdateInstructorCourses(selectedCourses, instructorToUpdate); // add
            PopulateAssignedCourseData(instructorToUpdate); // add
            return View(instructorToUpdate);
        }

        // GET: Instructor/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .FirstOrDefaultAsync(m => m.ID == id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // POST: Instructor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var instructor = await _context.Instructors.FindAsync(id);
            Instructor instructor = await _context.Instructors.Include(i => i.CourseAssignments).SingleAsync(i => i.ID == id);// add
            var departments = await _context.Departments.Where(d => d.InstructorID == id).ToListAsync(); //add
            
            departments.ForEach(d => d.InstructorID = null); // add

            /*if (instructor != null)
            {*/
                _context.Instructors.Remove(instructor);
            //}

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InstructorExists(int id)
        {
            return _context.Instructors.Any(e => e.ID == id);
        }

        private void PopulateAssignedCourseData(Instructor instructor)
        {
            var allCourses = _context.Courses;
            var instructorCourses = new HashSet<int>(instructor.CourseAssignments.Select(c => c.CourseID));
            var viewModel = new List<AssignedCourseData>();
            foreach (var course in allCourses)
            {
                viewModel.Add(new AssignedCourseData
                {
                    CourseID = course.CourseID,
                    Title = course.Title,
                    Assigned = instructorCourses.Contains(course.CourseID)
                });
            }
            ViewData["Courses"] = viewModel;
        }
        private void UpdateInstructorCourses(string[] selectedCourses, Instructor instructorToUpdate)
        {
            if (selectedCourses == null)
            {
                instructorToUpdate.CourseAssignments = new List<CourseAssignment>();
                return;
            }

            var selectedCoursesHS = new HashSet<string>(selectedCourses);
            var instructorCourses = new HashSet<int>
                (instructorToUpdate.CourseAssignments.Select(c => c.Course.CourseID));
            foreach (var course in _context.Courses)
            {
                if (selectedCoursesHS.Contains(course.CourseID.ToString()))
                {
                    if (!instructorCourses.Contains(course.CourseID))
                    {
                        instructorToUpdate.CourseAssignments.Add(new CourseAssignment { InstructorID = instructorToUpdate.ID, CourseID = course.CourseID });
                    }
                }
                else
                {

                    if (instructorCourses.Contains(course.CourseID))
                    {
                        CourseAssignment courseToRemove = instructorToUpdate.CourseAssignments.FirstOrDefault(i => i.CourseID == course.CourseID);
                        _context.Remove(courseToRemove);
                    }
                }
            }
        }
    }
}
