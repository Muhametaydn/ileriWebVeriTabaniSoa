using Microsoft.EntityFrameworkCore;
using ileriWebVeriTabaniSoa.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace ileriWebVeriTabaniSoa.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Veritabanındaki tablolar için DbSet'ler tanımlanır
        public DbSet<User> Users { get; set; } // User tablosu

        // Diğer tabloları buraya ekleyin
         public DbSet<Post> Posts { get; set; }
        // sonradan eklediklerim
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Category> Categories { get; set; }


        public DbSet<Like> Likes { get; set; }
        // Model kurallarını burada tanımlayabilirsiniz
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User Email zorunlu ve benzersiz
            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            

            base.OnModelCreating(modelBuilder);

            // Post ile Category arasındaki ilişki
            modelBuilder.Entity<Post>()
                .HasOne(p => p.Category) // Bir Post'un bir Category'si var
                .WithMany(c => c.Posts)  // Bir Category'nin birden fazla Post'u var
                .HasForeignKey(p => p.CategoryID) // Foreign key: Post.CategoryID
                .OnDelete(DeleteBehavior.Cascade); // Silme davranışı: Category silinirse, ilgili Post'lar da silinir.

            // Comment tablosu ilişkileri
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostID);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserID);

            // Like tablosu ilişkileri
            modelBuilder.Entity<Like>()
                .HasOne(l => l.Post)
                .WithMany(p => p.Likes)
                .HasForeignKey(l => l.PostID);

            modelBuilder.Entity<Like>()
                .HasOne(l => l.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.UserID);
        }
    }
}
