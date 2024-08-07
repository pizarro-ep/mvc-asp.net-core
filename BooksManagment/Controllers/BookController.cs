using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BooksManagment.Data;
using BooksManagment.Models;
using Microsoft.AspNetCore.Authorization;

namespace BooksManagment.Controllers
{
    //[Authorize]
    public class BookController : Controller
    {
        private readonly AppDbContext _context;

        public BookController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Book 
        /*public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Books.Include(b => b.Author).Include(b => b.Genre);
            return View(await appDbContext.ToListAsync());
        }*/

        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string search, int? pageNumber)
        {
            // Ordenamiento, busqueda y filtro de datos par mostrar en la tabla
            ViewData["CurrentSort"] = sortOrder;
            ViewData["TitleSortParm"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["PublicationSortParm"] = sortOrder == "Publication" ? "publication_desc" : "Publication";
            ViewData["GenreSortParm"] = sortOrder == "Genre" ? "genre_desc" : "Genre";
            ViewData["AuthorSortParm"] = sortOrder == "Author" ? "author_desc" : "Author";
            
            if(search != null){ pageNumber = 1; } else { search = currentFilter; }

            // para filtros de busqueda
            ViewData["CurrentFilter"] = search;

            // Consulta de datos
            var books = from s in _context.Books.Include(b => b.Author).Include(b => b.Genre)
                           select s;

            if (!string.IsNullOrEmpty(search)){ // Si hay un parametro realizar busqueda
                books = books.Where(w => w.Title.Contains(search) ||
                        (w.Description != null && w.Description.Contains(search)) ||
                        (w.Genre != null && w.Genre.Name != null && w.Genre.Name.Contains(search)) ||
                        (w.Author != null && w.Author.Names != null && w.Author.Names.Contains(search)));
            }

            // casos de ordenamiento
            switch(sortOrder){
                case "title_desc": books = books.OrderByDescending(s => s.Title); break;
                case "Publication": books = books.OrderBy(s => s.PublicationDate); break;
                case "publication_desc": books = books.OrderByDescending(s => s.PublicationDate); break;
                case "Genre": books = books.OrderBy(s => (s.Genre != null ? s.Genre.Name : string.Empty)); break;
                case "genre_desc": books = books.OrderByDescending(s => s.Genre != null ? s.Genre.Name : string.Empty); break;
                case "Author": books = books.OrderBy(s => s.Author != null ? s.Author.Names : string.Empty); break;
                case "author_desc": books = books.OrderByDescending(s => s.Author != null ? s.Author.Names :string.Empty); break;
                default: books = books.OrderBy(s => s.Title); break;
            }
            int pageSize = 5;       // Cantidad de items en la pagina
            return View(await PaginatedList<Book>.CreateAsync(books.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Book/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Book/Create
        public IActionResult Create()
        {
            ViewData["AuthorID"] = new SelectList(_context.Authors, "Id", "Names");
            ViewData["GenreID"] = new SelectList(_context.Genres, "Id", "Name");
            return View();
        }

        // POST: Book/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,PublicationDate,GenreID,AuthorID")] Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors)) {
                ModelState.AddModelError("", $"Error: {error.ErrorMessage}");
            }
            ViewData["AuthorID"] = new SelectList(_context.Authors, "Id", "Names", book.AuthorID);
            ViewData["GenreID"] = new SelectList(_context.Genres, "Id", "Name", book.GenreID);
            return View(book);
        }

        // GET: Book/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["AuthorID"] = new SelectList(_context.Authors, "Id", "Names", book.AuthorID);
            ViewData["GenreID"] = new SelectList(_context.Genres, "Id", "Name", book.GenreID);
            return View(book);
        }

        // POST: Book/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,PublicationDate,GenreID,AuthorID")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
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
            ViewData["AuthorID"] = new SelectList(_context.Authors, "Id", "Names", book.AuthorID);
            ViewData["GenreID"] = new SelectList(_context.Genres, "Id", "Name", book.GenreID);
            return View(book);
        }

        // GET: Book/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Book/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
