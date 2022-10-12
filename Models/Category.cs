using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MVCIdentityBookRecords.Models
{
    public partial class Category
    {
        public Category()
        {
            Books = new HashSet<Book>();
        }
        [Key]
        public int Idcategory { get; set; }
        public string CategoryName { get; set; } = null!;
        [JsonIgnore]
        public virtual ICollection<Book> Books { get; set; }
    }
}
