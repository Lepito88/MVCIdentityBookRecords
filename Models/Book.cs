using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MVCIdentityBookRecords.Models
{
    public partial class Book
    {
        public Book()
        {
            Authors = new HashSet<Author>();
            Categories = new HashSet<Category>();
            Users = new HashSet<ApplicationUser>();
        }
        [Key]
        public int Idbook { get; set; }
        public string BookName { get; set; } = null!;

        [DataType(DataType.Date)]
        public DateTime? ReleaseDate { get; set; }
        public string? Type { get; set; }
        public string? Isbn { get; set; }

        public virtual ICollection<Author> Authors { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        [JsonIgnore]
        public virtual ICollection<ApplicationUser> Users { get; set; }
    }
}
