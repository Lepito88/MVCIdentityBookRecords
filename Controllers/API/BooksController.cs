using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCIdentityBookRecords.Data;
using MVCIdentityBookRecords.Models;
using MVCIdentityBookRecords.Interfaces;
using MVCIdentityBookRecords.Responses.Authors;
using MVCIdentityBookRecords.Responses.Books;
using MVCIdentityBookRecords.Services;
using Microsoft.AspNetCore.Authorization;

namespace MVCIdentityBookRecords.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Basic,Admin")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            var getBooksResponse = await _bookService.GetBooksAsync();

            if (!getBooksResponse.Success)
            {
                return BadRequest();
            }

            var booksResponse = getBooksResponse.Books.ConvertAll(o => new BookResponse
            {
                BookId = o.BookId,
                BookName = o.BookName,
                Type = o.Type,
                Isbn = o.Isbn,
                ReleaseYear = (DateTime)o.ReleaseDate
                
            });

            return Ok(booksResponse);
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(int id)
        {
            var bookResponse = await _bookService.GetBookByIdAsync(id);

            if (!bookResponse.Success)
            {
                return NotFound();
            }

            return Ok(new BookResponse
            {
                BookId=bookResponse.BookId,
                BookName=bookResponse.BookName,
                Type=bookResponse.Type,
                Isbn=bookResponse.Isbn,
                ReleaseYear=bookResponse.ReleaseYear,
                Authors = bookResponse.Authors,
                Categories = bookResponse.Categories
            });
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            if (id != book.BookId)
            {
                return BadRequest();
            }

            var bookResponse = await _bookService.UpdateBookAsync(id, book);

            if (!bookResponse.Success)
            {
                return BadRequest(book);
            }

            return NoContent();
        }

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            var bookResponse = await _bookService.CreateBookAsync(book);

            if (!bookResponse.Success)
            {
                return UnprocessableEntity(book);
            }

            return CreatedAtAction("GetBook", new { id = book.BookId }, book);
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var bookResponse = await _bookService.DeleteBookAsync(id);
            if (!bookResponse.Success)
            {
                return BadRequest();
            }

            return NoContent();
        }

    }
}
