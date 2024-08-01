using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.Models;

namespace App.Controllers
{
    public class PersonController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PersonController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Person
        public async Task<IActionResult> Index(string personGenre, string search)
        {
            if (_context.PersonModel == null)
            {
                return Problem("Entity set 'AppContext.PersonModel'  is null.");
            }

            // Use LINQ to get list of genres
            IQueryable<string> genreQuery = from m in _context.PersonModel
                                            orderby m.Genre
                                            select m.Genre;

            var personModel = from m in _context.PersonModel
                              select m;

            if (!string.IsNullOrEmpty(search))
            {
                personModel = personModel.Where(s => s.Title!.ToUpper().Contains(search.ToUpper()));
            }
            if(!string.IsNullOrEmpty(personGenre)){
                personModel = personModel.Where(x => x.Genre == personGenre);
            }

            var personGerneVM = new PersonGenreViewModel{
                Genres = new SelectList(await genreQuery.Distinct().ToListAsync()),
                PersonModel = await personModel.ToListAsync()
            };

            return View(personGerneVM);
        }

        [HttpPost]
        public string Index(string search, bool notUsed)
        {
            return "From [HttpPost]Index: filter on " + search;
        }

        // GET: Person/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personModel = await _context.PersonModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (personModel == null)
            {
                return NotFound();
            }

            return View(personModel);
        }

        // GET: Person/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Person/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,ReleaseDate,Genre,Rating")] PersonModel personModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(personModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(personModel);
        }

        // GET: Person/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personModel = await _context.PersonModel.FindAsync(id);
            if (personModel == null)
            {
                return NotFound();
            }
            return View(personModel);
        }

        // POST: Person/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ReleaseDate,Genre,Rating")] PersonModel personModel)
        {
            if (id != personModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(personModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonModelExists(personModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(personModel);
        }

        // GET: Person/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personModel = await _context.PersonModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (personModel == null)
            {
                return NotFound();
            }

            return View(personModel);
        }

        // POST: Person/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) 
        {   // public async Task<IActionResult> Delete(int id, bool notUsed)
            var personModel = await _context.PersonModel.FindAsync(id);
            if (personModel != null)
            {
                _context.PersonModel.Remove(personModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonModelExists(int id)
        {
            return _context.PersonModel.Any(e => e.Id == id);
        }

        public IActionResult Welcome(string name, int age = 18)
        {
            ViewData["Message"] = "Hello " + name;
            ViewData["age"] = age;
            return View();
        }
    }
}
