using Microsoft.AspNetCore.Identity;
using static System.Reflection.Metadata.BlobBuilder;

namespace MVCIdentityBookRecords.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Books = new HashSet<Book>();
            //RefreshTokens = new HashSet<RefreshToken>();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int UsernameChangeLimit { get; set; } = 10;
        public byte[]? ProfilePicture { get; set; }

        //public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
        public virtual ICollection<Book> Books { get; set; }
    }
}
