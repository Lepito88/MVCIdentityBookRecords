using MVCIdentityBookRecords.Data;
using MVCIdentityBookRecords.Models;
using MVCIdentityBookRecords.Interfaces;
using MVCIdentityBookRecords.Responses;
using Microsoft.EntityFrameworkCore;

namespace MVCIdentityBookRecords.Services
{
    public class EntityRelationShipManagerService : IEntityRelationShipManagerService
    {
        private readonly ApplicationDbContext _context;

        public EntityRelationShipManagerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AuthorToBookResponse> AddAuthorToBookAsync(int bookid, int authorid)
        {
            //get specific book and include authors to it
            var book = await _context.Books
                .Where(book => book.Idbook == bookid)
                .Include(_ => _.Authors)
                .FirstOrDefaultAsync();
            if (book == null)
            {
                return new AuthorToBookResponse
                {
                    Success = false,
                    Error = "Unable to find book",
                    ErrorCode = "ATB02"

                };
            }

            // find author
            var author = await _context.Authors.FindAsync(authorid);

            if (author == null)
            {
                return new AuthorToBookResponse
                {
                    Success = false,
                    Error = "Unable to find author",
                    ErrorCode = "ATB03"

                };
            }

            var isAuthorAddedToBook = book.Authors.SingleOrDefault(_ => _.Idauthor == authorid);
            if (isAuthorAddedToBook != null)
            {
                return new AuthorToBookResponse
                {
                    Success = false,
                    Error = "Author is already addded to book",
                    ErrorCode = "ATB07"

                };
            }

            //Add Author to Book
            book.Authors.Add(author);

            //Mark book state to modified
            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(bookid))
                {
                    return new AuthorToBookResponse
                    {
                        Success = false,
                        Error = "Book does not exist.",
                        ErrorCode = "ATB04"
                    };
                }
                else
                {
                    throw;
                }
            }

            return new AuthorToBookResponse
            {
                Success = true,
                BookId = bookid,
                AuthorId = authorid,
                BookName = book.BookName

            };
        }

        public async Task<BookToUserResponse> AddBookToUserAsync(string userid, int bookid)
        {
            //get user
            var user = await _context.Users.Where(_ => _.Id == userid)
                .Include(_ => _.Books)
                .FirstOrDefaultAsync();

            if (user == null) {
                return new BookToUserResponse
                {
                    Success = false,
                    Error = "Unable to find user",
                    ErrorCode = "ATB01"
                };
            }

            //find book
            var book = await _context.Books.FindAsync(bookid);
            if (book == null) 
            {
                return new BookToUserResponse
                {
                    Success = false,
                    Error = "Unable to find book",
                    ErrorCode = "ATB02"
                };
            }
            //TODO: ADD check if the user already has the book
            var isBookOwned = user.Books.SingleOrDefault(_ => _.Idbook == bookid);

            if (isBookOwned != null)
            {
                return new BookToUserResponse
                {
                    Success = false,
                    Error = "Book is already added to user",
                    ErrorCode = "ATB06"
                };
            }

            //Add book to user
            user.Books.Add(book);

            //mark user state to be changed
            _context.Entry(user).State = EntityState.Modified;

            try
            {

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(userid))
                {
                    return new BookToUserResponse
                    {
                        Success = false,
                        Error = "User does not exist",
                        ErrorCode = "ATB05"
                    };
                }
                else
                {
                    throw;
                }
            }

            return new BookToUserResponse
            {
                Success = true,
                UserId = userid,
                UserName =user.UserName,
                BookId = bookid,
                BookName = book.BookName
            };
        }

        public async Task<CategoryToBookResponse> AddCategoryToBookAsync(int bookid, int categoryid)
        {
            var book = await _context.Books
                 .Where(book => book.Idbook == bookid)
                 .Include(_ => _.Categories)
                 .FirstOrDefaultAsync();
            if (book == null)
            {
                return new CategoryToBookResponse {
                    Success = false,
                    Error = "Unable to find book.",
                    ErrorCode = "ATB02"
                };
            }
            // find Category
            var category = await _context.Categories.FindAsync(categoryid);

            if (category == null)
            {
                return new CategoryToBookResponse
                {
                    Success = false,
                    Error = "Unable to find Category.",
                    ErrorCode = "ATB10"
                };
            }
            var isCategoryAdded = book.Categories.SingleOrDefault(_ => _.Idcategory == categoryid);

            if (isCategoryAdded != null)
            {
                return new CategoryToBookResponse
                {
                    Success = false,
                    Error = "Category is already added to book",
                    ErrorCode = "ATB11"
                };
            }

            //Add Author to Book
            book.Categories.Add(category);

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(bookid))
                {
                    return new CategoryToBookResponse 
                    {
                        Success = false,
                        Error = "Book does not exist",
                        ErrorCode = "ATB04"
                    };
                }
                else
                {
                    throw;
                }
            }

            return new CategoryToBookResponse
            {
                Success = true,
                CatecoryId = category.Idcategory,
                CategoryName = category.CategoryName,
                Bookid = book.Idbook,
                BookName = book.BookName
            };
        }

        public async Task<AuthorToBookResponse> RemoveAuthorFromBookAsync(int bookid, int authorid)
        {
            //find book
            var book = await _context.Books
                 .Where(book => book.Idbook == bookid)
                 .Include(_ => _.Authors)
                 .FirstOrDefaultAsync();
            if (book == null)
            {
                return new AuthorToBookResponse
                {
                    Success = false,
                    Error = "Unable to find book",
                    ErrorCode = "ATB02"

                };
            }
            // find author
            var author = await _context.Authors.FindAsync(authorid);

            if (author == null)
            {
                return new AuthorToBookResponse
                {
                    Success = false,
                    Error = "Unable to find author",
                    ErrorCode = "ATB03"
                };
            }
            //check if book has author
            var bookHasAuthor = book.Authors.SingleOrDefault(_ => _.Idauthor == authorid);
            if (bookHasAuthor == null)
            {
                return new AuthorToBookResponse
                {
                    Success = false,
                    Error = "Book does not have author added",
                    ErrorCode = "ATB08"
                };
            }

            //remove Author from Book
            book.Authors.Remove(author);

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(bookid))
                {
                    return new AuthorToBookResponse
                    {
                        Success = false,
                        Error = "Book does not exist.",
                        ErrorCode = "ATB04"
                    };
                }
                else
                {
                    throw;
                }
            }

            return new AuthorToBookResponse
            {
                Success = true,
                BookId = bookid,
                AuthorId = authorid,
                BookName = book.BookName

            };
        }

        public async Task<BookToUserResponse> RemoveBookFromUserAsync(string userid, int bookid)
        {
            //get user
            var user = await _context.Users
                .Where(_ => _.Id == userid)
                .Include(_ => _.Books)
                .FirstOrDefaultAsync();
            if (user == null) 
            {
                return new BookToUserResponse
                {
                    Success = false,
                    Error = "Unable to find user",
                    ErrorCode = "ATB01"

                };
            }

            //find book
            var book = await _context.Books.FindAsync(bookid);
            if (book == null) {
                return new BookToUserResponse
                {
                    Success = false,
                    Error = "Unable to find book",
                    ErrorCode = "ATB02"

                };
            }

            var isBookAddedToUser = user.Books.SingleOrDefault(_ => _.Idbook == bookid);

            if (isBookAddedToUser == null)
            { 
                return new BookToUserResponse
                {
                    Success = false,
                    Error = "Book has not been added to user",
                    ErrorCode = "ATB09"

                };
            }

            //Remove book from user
            user.Books.Remove(book);

            //mark user state to be changed
            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(userid))
                {
                    return new BookToUserResponse
                    {
                        Success = false,
                        Error = "User does not exist",
                        ErrorCode = "ATB05"
                    };
                }
                else
                {
                    throw;
                }
            }

            return new BookToUserResponse
            {
                Success = true,
                UserId = userid,
                UserName = user.UserName,
                BookId = bookid,
                BookName = book.BookName
            };
        }

        public async Task<CategoryToBookResponse> RemoveCategoryFromBookAsync(int bookid, int categoryid)
        {
            var book = await _context.Books
                 .Where(book => book.Idbook == bookid)
                 .Include(_ => _.Categories)
                 .FirstOrDefaultAsync();
            if (book == null)
            {
                return new CategoryToBookResponse
                {
                    Success = false,
                    Error = "Unable to find book.",
                    ErrorCode = "ATB02"
                };
            }
            // find Category
            var category = await _context.Categories.FindAsync(categoryid);

            if (category == null)
            {
                return new CategoryToBookResponse
                {
                    Success = false,
                    Error = "Unable to find Category.",
                    ErrorCode = "ATB10"
                };
            }
            var isCategoryAdded = book.Categories.SingleOrDefault(_ => _.Idcategory == categoryid);

            if (isCategoryAdded == null)
            {
                return new CategoryToBookResponse
                {
                    Success = false,
                    Error = "Category has not been added to book. Unable to remove.",
                    ErrorCode = "ATB12"
                };
            }

            //Add Author to Book
            book.Categories.Remove(category);

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(bookid))
                {
                    return new CategoryToBookResponse
                    {
                        Success = false,
                        Error = "Book does not exist",
                        ErrorCode = "ATB04"
                    };
                }
                else
                {
                    throw;
                }
            }

            return new CategoryToBookResponse
            {
                Success = true,
                CatecoryId = category.Idcategory,
                CategoryName = category.CategoryName,
                Bookid = book.Idbook,
                BookName = book.BookName
            };
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Idbook == id);
        }
        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
/*
 * ATB01 : Unable to find user
 * ATB02 : Unable to find book
 * ATB03 : Unable to find author
 * ATB04 : Book does not Exist
 * ATB05 : User does not exist
 * ATB06 : Book is already added to user
 * ATB07 : Author is already addded to book
 * ATB08 : Book does not have author added
 * ATB09 : Book has not been added to user
 * ATB10 : Unable to find category
 * ATB11 : Category is already added to book
 * Atb12 : Category has not been added to book. Unable to remove.
 
 */