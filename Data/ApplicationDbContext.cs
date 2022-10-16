using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MVCIdentityBookRecords.Models;
using System.Reflection.Emit;

namespace MVCIdentityBookRecords.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> 
    { 

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Author> Authors { get; set; } = null!;
        public virtual DbSet<Book> Books { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        //public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //builder.Entity<RefreshToken>(entity =>
            //{
            //    entity.HasKey(e => e.IdRefreshToken)
            //        .HasName("PRIMARY");

            //    entity.Property(e => e.ExpiryDate).HasColumnType("datetime");

            //    entity.Property(e => e.TokenHash)
            //        .IsRequired()
            //        .HasMaxLength(1000);

            //    entity.Property(e => e.TokenSalt)
            //        .IsRequired()
            //        .HasMaxLength(1000);

            //    entity.Property(e => e.Timestamp)
            //        .HasColumnType("datetime")
            //        .HasColumnName("timestamp");

            //    entity.HasOne(d => d.User)
            //        .WithMany(p => p.RefreshTokens)
            //        .HasForeignKey(d => d.Iduser)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_RefreshToken_User");

            //    entity.ToTable("RefreshToken");
            //});

            builder.Entity<Author>(entity =>
            {
                entity.HasKey(e => e.Idauthor)
                    .HasName("PRIMARY");

                entity.ToTable("Author");

                entity.Property(e => e.Idauthor).HasColumnName("Idauthor");

                entity.Property(e => e.Firstname)
                    .HasMaxLength(255)
                    .HasColumnName("Firstname");

                entity.Property(e => e.Lastname)
                    .HasMaxLength(255)
                    .HasColumnName("Lastname");

                entity.HasMany(d => d.Books)
                    .WithMany(p => p.Authors)
                    .UsingEntity<Dictionary<string, object>>(
                        "AuthorBook",
                        l => l.HasOne<Book>().WithMany().HasForeignKey("Idbook").OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_book_authorbooks"),
                        r => r.HasOne<Author>().WithMany().HasForeignKey("Idauthor").OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_author_authorbooks"),
                        j =>
                        {
                            j.HasKey("Idauthor", "Idbook").HasName("PRIMARY").HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                            j.ToTable("AuthorBooks");

                            j.HasIndex(new[] { "Idauthor" }, "fk_author_authorbooks_idx");

                            j.HasIndex(new[] { "Idbook" }, "fk_book_authorbooks_idx");

                            j.IndexerProperty<int>("Idauthor").HasColumnName("Idauthor");

                            j.IndexerProperty<int>("Idbook").HasColumnName("Idbook");
                        });
            });

            builder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.Idbook)
                    .HasName("PRIMARY");

                entity.ToTable("Book");

                entity.Property(e => e.Idbook).HasColumnName("Idbook");

                entity.Property(e => e.BookName)
                    .HasMaxLength(255)
                    .HasColumnName("BookName");

                entity.Property(e => e.Isbn)
                    .HasMaxLength(20)
                    .HasColumnName("Isbn");

                entity.Property(e => e.ReleaseDate).HasColumnName("ReleaseDate");

                entity.Property(e => e.Type)
                    .HasColumnType("enum('Hardcover','Paperback','Digital','Comicbook')")
                    .HasColumnName("Type");

                entity.HasMany(d => d.Categories)
                    .WithMany(p => p.Books)
                    .UsingEntity<Dictionary<string, object>>(
                        "BookCategory",
                        l => l.HasOne<Category>().WithMany().HasForeignKey("Idcategory").OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_category_bookcategories"),
                        r => r.HasOne<Book>().WithMany().HasForeignKey("Idbook").OnDelete(DeleteBehavior.Restrict).HasConstraintName("fk_book_bookcategories"),
                        j =>
                        {
                            j.HasKey("Idbook", "Idcategory").HasName("PRIMARY").HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                            j.ToTable("BookCategories");

                            j.HasIndex(new[] { "Idbook" }, "fk_book_bookcategories_idx");

                            j.HasIndex(new[] { "Idcategory" }, "fk_category_bookcategories_idx");

                            j.IndexerProperty<int>("Idbook").HasColumnName("Idbook");

                            j.IndexerProperty<int>("Idcategory").HasColumnName("Idcategory");
                        });
            });

            builder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Idcategory)
                    .HasName("PRIMARY");

                entity.ToTable("Category");

                entity.Property(e => e.Idcategory)
                    .ValueGeneratedNever()
                    .HasColumnName("Idcategory");

                entity.Property(e => e.CategoryName)
                    .HasMaxLength(255)
                    .HasColumnName("CategoryName");
            });




            builder.HasDefaultSchema("Identity");
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable(name: "User");
            });
            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Role");
            });
            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });
            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });
            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });
            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });
            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });

        }
    }
}