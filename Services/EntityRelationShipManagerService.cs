using MVCIdentityBookRecords.Data;
using MVCIdentityBookRecords.Models;
using MVCIdentityBookRecords.Interfaces;
using MVCIdentityBookRecords.Responses;
using Microsoft.EntityFrameworkCore;
using MVCIdentityBookRecords.Requests;

namespace MVCIdentityBookRecords.Services
{
    public class EntityRelationShipManagerService : IEntityRelationShipManagerService
    {
        private readonly ApplicationDbContext _context;

        public EntityRelationShipManagerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AuthorToBookResponse> AddAuthorToBookAsync(RelationshipRequest request)
        {
            //get specific book and include authors to it
            var book = await _context.Books
                .Where(book => book.BookId == request.BookId)
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
            var author = await _context.Authors.FindAsync(request.AuthorId);

            if (author == null)
            {
                return new AuthorToBookResponse
                {
                    Success = false,
                    Error = "Unable to find author",
                    ErrorCode = "ATB03"

                };
            }

            var isAuthorAddedToBook = book.Authors.SingleOrDefault(_ => _.AuthorId == request.AuthorId);
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
                if (!BookExists((int)request.BookId))
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
                BookId = request.BookId,
                AuthorId = request.AuthorId,
                BookName = book.BookName

            };
        }

        public async Task<BookToUserResponse> AddBookToUserAsync(RelationshipRequest request)
        {
            //get user
            var user = await _context.Users.Where(_ => _.Id == request.UserId)
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
            var book = await _context.Books.FindAsync(request.BookId);
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
            var isBookOwned = user.Books.SingleOrDefault(_ => _.BookId == request.BookId);

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
                if (!UserExists(request.UserId))
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
                UserId = request.UserId,
                UserName =user.UserName,
                BookId = request.BookId,
                BookName = book.BookName
            };
        }

        public async Task<CategoryToBookResponse> AddCategoryToBookAsync(RelationshipRequest request)
        {
            var book = await _context.Books
                 .Where(book => book.BookId == request.BookId)
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
            var category = await _context.Categories.FindAsync(request.CategoryId);

            if (category == null)
            {
                return new CategoryToBookResponse
                {
                    Success = false,
                    Error = "Unable to find Category.",
                    ErrorCode = "ATB10"
                };
            }
            var isCategoryAdded = book.Categories.SingleOrDefault(_ => _.CategoryId == request.CategoryId);

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
                if (!BookExists(request.BookId))
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
                CatecoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                Bookid = book.BookId,
                BookName = book.BookName
            };
        }

        public async Task<AuthorToBookResponse> RemoveAuthorFromBookAsync(RelationshipRequest request)
        {
            //find book
            var book = await _context.Books
                 .Where(book => book.BookId == request.BookId)
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
            var author = await _context.Authors.FindAsync(request.AuthorId);

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
            var bookHasAuthor = book.Authors.SingleOrDefault(_ => _.AuthorId == request.AuthorId);
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
                if (!BookExists(request.BookId))
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
                BookId = request.BookId,
                AuthorId = request.AuthorId,
                BookName = book.BookName

            };
        }

        public async Task<BookToUserResponse> RemoveBookFromUserAsync(RelationshipRequest request)
        {
            //get user
            var user = await _context.Users
                .Where(_ => _.Id == request.UserId)
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
            var book = await _context.Books.FindAsync(request.BookId);
            if (book == null) {
                return new BookToUserResponse
                {
                    Success = false,
                    Error = "Unable to find book",
                    ErrorCode = "ATB02"

                };
            }

            var isBookAddedToUser = user.Books.SingleOrDefault(_ => _.BookId == request.BookId);

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
                if (!UserExists(request.UserId))
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
                UserId = request.UserId,
                UserName = user.UserName,
                BookId = request.BookId,
                BookName = book.BookName
            };
        }

        public async Task<CategoryToBookResponse> RemoveCategoryFromBookAsync(RelationshipRequest request)
        {
            var book = await _context.Books
                 .Where(book => book.BookId == request.BookId)
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
            var category = await _context.Categories.FindAsync(request.CategoryId);

            if (category == null)
            {
                return new CategoryToBookResponse
                {
                    Success = false,
                    Error = "Unable to find Category.",
                    ErrorCode = "ATB10"
                };
            }
            var isCategoryAdded = book.Categories.SingleOrDefault(_ => _.CategoryId == request.CategoryId);

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
                if (!BookExists(request.BookId))
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
                CatecoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                Bookid = book.BookId,
                BookName = book.BookName
            };
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.BookId == id);
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