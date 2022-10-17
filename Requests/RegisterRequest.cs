using System.ComponentModel.DataAnnotations;

namespace MVCIdentityBookRecords.Requests
{
    public class RegisterRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public String Lastname { get; set; }
        //[Required]
        //public DateTime Created { get; set; } = DateTime.Now;

    }
}
