using MVCIdentityBookRecords.Data;
using MVCIdentityBookRecords.Models;
using MVCIdentityBookRecords.Interfaces;
using MVCIdentityBookRecords.Responses.Users;
using Microsoft.EntityFrameworkCore;

namespace MVCIdentityBookRecords.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CreateUserResponse> CreateUserAsync(ApplicationUser user)
        {
            if (user != null)
            {
                //user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                await _context.Users.AddAsync(user);
                var createResponse = await _context.SaveChangesAsync();
                if (createResponse >= 0)
                {
                    return new CreateUserResponse
                    {
                        Success = true,
                        Iduser = user.Id,
                        Email = user.Email,
                        Username = user.UserName
                    };
                }
                return new CreateUserResponse
                {
                    Success = false,
                    Error = "Unable to save user",
                    ErrorCode = "U04"
                };
            }

            return new CreateUserResponse
            {
                Success = false,
                Error = "Unable to create user",
                ErrorCode = "U05"
            };
        }

        public async Task<CreateUserResponse> DeleteUserAsync(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return new CreateUserResponse
                {
                    Success = false,
                    Error = "ApplicationUser Not Found",
                    ErrorCode = "U02"
                };
            }

            _context.Users.Remove(user);
            var deleteResponse = await _context.SaveChangesAsync();
            if (deleteResponse >= 0)
            {
                return new CreateUserResponse
                {
                    Success = true,
                    Email = user.Email,
                    Username = user.UserName
                };
            }

            return new CreateUserResponse
            {
                Success = false,
                Error = "Unable to delete user",
                ErrorCode = "U03"

            };
        }

        public async Task<UserDetailResponse> GetUserByIdAsync(string id)
        {
            var user = await _context.Users
                .Where(u => u.Id == id)
                .Include(_ => _.Books)
                .ThenInclude(_ => _.Authors)
                .ThenInclude(_=> _.Books)
                .ThenInclude(_ => _.Categories)
                .FirstOrDefaultAsync();
            if (user is null)
            {
                return new UserDetailResponse
                {
                    Success = false,
                    Error = "ApplicationUser not found",
                    ErrorCode = "U02",
                };
            }

            return new UserDetailResponse
            {
                Success = true,
                Iduser = user.Id,
                Username = user.UserName,
                Email = user.Email,
                Firstname = user.FirstName,
                Lastname = user.LastName,
                Books = user.Books
            };
        }

        public async Task<GetUsersResponse> GetUsersAsync()
        {
            var users = await _context.Users.ToListAsync();

            if (users.Count == 0)
            {
                return new GetUsersResponse
                {
                    Success = false,
                    Error = "No Users found",
                    ErrorCode = "U01"
                };
            }
            return new GetUsersResponse { Success = true, Users = users };
        }

        public async Task<CreateUserResponse> UpdateUserAsync(string id, ApplicationUser user)
        {
            if (id != user.Id)
            {
                return new CreateUserResponse
                {
                    Success = false,
                    Error = "User ids Don't match",
                    ErrorCode = "U06"

                };
            }
            //if (user.Password != null)
            //{
            //    user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            //}

            //user.UpdatedAt = DateTime.Now;
            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return new CreateUserResponse
                    {
                        Success = false,
                        Error = "Author Not found",
                        ErrorCode = "U02",
                    };
                }
                else
                {
                    throw;
                }
            }

            return new CreateUserResponse
            {
                Success = true,
                Iduser = user.Id,
                Email = user.Email,
                Username = user.UserName,
                
            };
        }
        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

    }
}
//U01 : No users found
//U02 : User not found
//U03 : Unable to delete user
//U04 : Unable to save user
//U05 : Unable to create user
//U96 : User ids Don't match