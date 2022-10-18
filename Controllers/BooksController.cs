using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCIdentityBookRecords.Data;
using MVCIdentityBookRecords.Interfaces;
using MVCIdentityBookRecords.Models;
using MVCIdentityBookRecords.Requests;

namespace MVCIdentityBookRecords.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEntityRelationShipManagerService _entityRelationShipManagerService;

        public BooksController(ApplicationDbContext context, IEntityRelationShipManagerService entityRelationShipManagerService)
        {
            _context = context;
            _entityRelationShipManagerService = entityRelationShipManagerService;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            var signedInUserIdFromCookie = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            if (signedInUserIdFromCookie.Value == null)
                    return View();
            //}

            //Get user
            //var userBooks = await _context.Users
            //    .Where(_ => _.Id == signedInUserIdFromCookie.Value)
            //    .Include(_ => _.Books)
            //    .FirstOrDefaultAsync();
            //if (userBooks == null)
            //    return NotFound();


            var books = await _context.Books
                .Include(_=> _.Authors)
                .Include(_ => _.Categories)
                .Include(_=> _.Users.Where(_ => _.Id == signedInUserIdFromCookie.Value))
                .ToListAsync();


              return View(books);
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(_ => _.Authors)
                .Include(_ => _.Categories)
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookId,BookName,ReleaseDate,Type,Isbn")] Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookId,BookName,ReleaseDate,Type,Isbn")] Book book)
        {
            if (id != book.BookId)
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
                    if (!BookExists(book.BookId))
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
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Books == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Books'  is null.");
            }
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
          return _context.Books.Any(e => e.BookId == id);
    }
    
       

        [HttpPost, ActionName("ManageBookRelationship")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageBookRelationship(string UserId,int AuthorId, int BookId, int CategoryId, string ActionType)
        {
            if (ActionType == null)
            {
                return Problem("Something unexpected happened");
            }
            RelationshipRequest rs = new RelationshipRequest
            {
                UserId=UserId,
                CategoryId=CategoryId,
                AuthorId = AuthorId,
                BookId = BookId,
            };
            if (ActionType == "AddAuthor")
            {
                try
                {
                    var resp = await _entityRelationShipManagerService.AddAuthorToBookAsync(rs);
                    if (!resp.Success)
                    {
                        return BadRequest(new { resp.Error });
                    }
                    return RedirectToAction("ManageBookRelationships", "Books");
                    //return View(resp);
                }
                catch (Exception)
                {

                    throw;
                }
            }
            if (ActionType == "RemoveAuthor")
            {
                try
                {
                    var resp = await _entityRelationShipManagerService.RemoveAuthorFromBookAsync(rs);
                    if (!resp.Success)
                    {
                        return BadRequest(new { resp.Error });
                    }
                    return RedirectToAction("ManageBookRelationships", "Books");

                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                    throw;
                }
            }
            if (ActionType == "AddCategory")
            {
                try
                {
                    var resp = await _entityRelationShipManagerService.AddCategoryToBookAsync(rs);
                    if (!resp.Success)
                    {
                        return BadRequest(new { resp.Error });
                    }
                    return RedirectToAction("ManageBookRelationships", "Books");
                    //return View(resp);
                }
                catch (Exception)
                {

                    throw;
                }
            }
            if (ActionType == "RemoveCategory")
            {
                try
                {
                    var resp = await _entityRelationShipManagerService.RemoveCategoryFromBookAsync(rs);
                    if (!resp.Success)
                    {
                        return BadRequest(new { resp.Error });
                    }
                    return RedirectToAction("ManageBookRelationships", "Books");
                    //return View(resp);
                }
                catch (Exception)
                {

                    throw;
                }
            }
            if (ActionType == "AddBook")
            {
                try
                {
                    var resp = await _entityRelationShipManagerService.AddBookToUserAsync(rs);
                    if (!resp.Success)
                    {
                        return BadRequest(new { resp.Error });
                    }
                    return RedirectToAction(nameof(Index));
                    //return View(resp);
                }
                catch (Exception)
                {

                    throw;
                }
            }
            if (ActionType == "RemoveBook")
            {
                try
                {
                    
                    var resp = await _entityRelationShipManagerService.RemoveBookFromUserAsync(rs);
                    if (!resp.Success)
                    {
                        return BadRequest(new { resp.Error });
                    }
                    return RedirectToAction("mybooks","Home");
                    //return View(resp);
                }
                catch (Exception)
                {

                    throw;
                }
            }

            return RedirectToAction(nameof(Index));
        }
        public async Task<ActionResult> ManageBookRelationships()
        {
            var authors = await _context.Authors.ToListAsync();
            if (authors.Count == 0)
                return View("No books");
            var books = await _context.Books.ToListAsync();
            if (books.Count == 0)
            {
                return View("No Authors");
            }
            var categories = _context.Categories.ToList();
            if (categories.Count == 0)
            {
                return View("No Categories");
            }
            var data = new AddAuthorsData
            {
                Authors = authors,
                Books = books,
                Categories = categories

            };
            return View(data);
        }
    }
}
