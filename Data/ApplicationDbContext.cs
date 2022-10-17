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
        public virtual DbSet<ApplicationUser> AppUsers { get; set; } = null!;

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