using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MVCIdentityBookRecords.Models
{
    public partial class Author
    {
        public Author()
        {
            Books = new HashSet<Book>();
        }
        [Key]
        public int Idauthor { get; set; }
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;

        [JsonIgnore]
        public virtual ICollection<Book> Books { get; set; }

    }
}
