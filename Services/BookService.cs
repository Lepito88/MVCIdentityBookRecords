using MVCIdentityBookRecords.Data;
using MVCIdentityBookRecords.Models;
using MVCIdentityBookRecords.Interfaces;
using MVCIdentityBookRecords.Responses.Authors;
using MVCIdentityBookRecords.Responses.Books;
using Microsoft.EntityFrameworkCore;

namespace MVCIdentityBookRecords.Services
{
    public class BookService : IBookService
    {
        private readonly ApplicationDbContext _context;

        public BookService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetBooksResponse> GetBooksAsync()
        {
            var booksResponse = await _context.Books.ToListAsync();

            if (booksResponse.Count == 0)
            {
                return new GetBooksResponse
                    {
                        Success = false,
                        Error = "No Books found",
                        ErrorCode = "B01"
                    };
            }
            return new GetBooksResponse { Success = true, Books = booksResponse };

        }
        public async Task<BookResponse> GetBookByIdAsync(int id)
        {
            var bookResponse = await _context.Books
                .Where(book => book.Idbook == id)
                .Include(_ => _.Authors)
                .Include(_ => _.Categories)
                .FirstOrDefaultAsync();
            if (bookResponse is null)
            {
                return new BookResponse 
                {
                    Success = false,
                    Error = "Book not found",
                    ErrorCode = "B02"
                };
            }


            return new BookResponse {
                Success = true,
                Idbook = bookResponse.Idbook,
                BookName = bookResponse.BookName,
                Type = bookResponse.Type,
                Isbn = bookResponse.Isbn,
                ReleaseYear = (DateTime)bookResponse.ReleaseDate,
                Authors = bookResponse.Authors,
                Categories = bookResponse.Categories
            };

        }


        public async Task<BookResponse> CreateBookAsync(Book book)
        {
            if (book != null)
            {
                await _context.Books.AddAsync(book);

                var createResponse = await _context.SaveChangesAsync();

                if (createResponse >= 0)
                {
                    return new BookResponse
                    {
                        Success = true,
                        Idbook = book.Idbook,
                        BookName = book.BookName,
                        Type = book.Type,
                        Isbn = book.Isbn,
                        ReleaseYear = (DateTime)book.ReleaseDate
                    };
                }
                return new BookResponse
                {
                    Success = false,
                    Error = "Unable to save Book",
                    ErrorCode = "B05"
                };
            }
            return new BookResponse
            {
                Success = false,
                Error = "Unable to create book",
                ErrorCode = "B04"
            };

        }

        public async Task<BookResponse> DeleteBookAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return new BookResponse
                {
                    Success = false,
                    Error = "Book Not Found",
                    ErrorCode = "B02"
                };
            }

            _context.Books.Remove(book);
            var deleteResponse = await _context.SaveChangesAsync();
            if (deleteResponse >= 0)
            {
                return new BookResponse
                {
                    Success = true,
                    Idbook = book.Idbook
                };
            }

            return new BookResponse
            {
                Success = false,
                Error = "Unable to delete book",
                ErrorCode = "B03"

            };
        }



        public async Task<BookResponse> UpdateBookAsync(int id, Book book)
        {
            if (id != book.Idbook)
            {
                return new BookResponse
                {
                    Success = false,
                    Error = "Unable to update book",
                    ErrorCode = "B04"
                };
            }

             _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return new BookResponse
                    {
                        Success = false,
                        Error = "Book Not found",
                        ErrorCode = "B02",
                    };
                }
                else
                {
                    throw;
                }
            }

            return new BookResponse
            {
                Success = true,
                Idbook = book.Idbook,
                BookName = book.BookName,
                Type = book.Type,
                Isbn = book.Isbn,
                ReleaseYear = (DateTime)book.ReleaseDate

            };
        }
        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Idbook == id);
        }
    }
}
/*
B01 : No Books found
B02 : Book not found
B03 : Unable to delete book
B04 : Unable to update book
B05 : Unadle to save book

 */