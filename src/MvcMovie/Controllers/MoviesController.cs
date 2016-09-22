using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Data;
using MvcMovie.Models;

namespace MvcMovie.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;

        //The constructor uses Dependency Injection to inject the database context into the controller.
        //The database context is used in each of the CRUD methods in the controller.
        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Movies
        //A request to the Movies controller returns all the entries in the Movies table and
        //then passes the data to the Index view.
        public async Task<IActionResult> Index(string selectedGenre, string searchString)
        {
            //Without Search : Returning all Movies
            //return View(await _context.Movie.ToListAsync());

            //The following code is a LINQ query that retrieves all the genres from the database.
            //Use LINQ to get list of genres.
            IQueryable<string> genreQuery = from m in _context.Movie
                                            orderby m.Genre
                                            select m.Genre;

            //LINQ query to select the movies:
            //The query is only defined at this point, it has not been run against the database.
            //{Microsoft.EntityFrameworkCore.Query.Internal.EntityQueryable`1[MvcMovie.Models.Movie]}
            var movies = from m in _context.Movie
                         select m;

            //If the searchString parameter contains a string, the movies query is modified to filter on the value of the search string, using the following code:
            //
            //The s => s.Title.Contains() code above is a Lambda Expression. 
            //Lambdas are used in method-based LINQ queries as arguments to standard query operator methods such as the Where method or Contains used in the code above. 
            //LINQ queries are not executed when they are defined or when they are modified by calling a method such as Where, Contains or OrderBy. 
            //Instead, query execution is deferred, which means that the evaluation of an expression is delayed 
            //until its realized value is actually iterated over or the ToListAsync method is called.
            if (!String.IsNullOrEmpty(searchString))
            {
                //The Contains method is run on the database, not the c# code above. 
                //On the database, Contains maps to SQL LIKE, which is case insensitive.
                movies = movies.Where(s => s.Title.Contains(searchString));
            }

            if (!String.IsNullOrEmpty(selectedGenre))
            {
                movies = movies.Where(x => x.Genre == selectedGenre);
            }

            var movieGenreViewModel = new MovieGenreViewModel();
            //Execute LINQ Queries NOW in both ToListAsync
            //The SelectList of genres is created by projecting the distinct genres(we don’t want our select list to have duplicate genres).
            movieGenreViewModel.selectGenre = new SelectList(await genreQuery.Distinct().ToListAsync());
            movieGenreViewModel.movies = await movies.ToListAsync();
            
            return View(movieGenreViewModel);
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.SingleOrDefaultAsync(m => m.ID == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Genre,Price,ReleaseDate,Title,Rating")] Movie movie)
        {
            //The ModelState.IsValid method verifies that the data submitted in the form can be used to modify(edit or update) a Movie object
            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(movie);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.SingleOrDefaultAsync(m => m.ID == id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //The ValidateAntiForgeryTokenAttribute attribute is used to prevent forgery of a request and is paired up with an anti-forgery token generated in the edit view file(Views/Movies/Edit.cshtml).
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Genre,Price,ReleaseDate,Title,Rating")] Movie movie)
        {
            if (id != movie.ID)
            {
                return NotFound();
            }

            //The ModelState.IsValid method verifies that the data submitted in the form can be used to modify(edit or update) a Movie object
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.SingleOrDefaultAsync(m => m.ID == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movie.SingleOrDefaultAsync(m => m.ID == id);
            _context.Movie.Remove(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool MovieExists(int id)
        {
            return _context.Movie.Any(e => e.ID == id);
        }
    }
}
