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

            // User ile Post arasındaki ilişki
            modelBuilder.Entity<Post>()
                .HasOne(p => p.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Kullanıcı silindiğinde postları da sil

            // Post ile Category arasındaki ilişki
            modelBuilder.Entity<Post>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Posts)
                .HasForeignKey(p => p.CategoryID)
                .OnDelete(DeleteBehavior.SetNull); // Kategori silindiğinde Post'un CategoryID'si null olur

            // Post ile Comment arasındaki ilişki
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostID)
                .OnDelete(DeleteBehavior.Cascade); // Post silindiğinde yorumlar da silinir

            // User ile Comment arasındaki ilişki
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserID)
                .OnDelete(DeleteBehavior.Cascade); // Kullanıcı silindiğinde yorumlar da silinir

            // Post ile Like arasındaki ilişki
            modelBuilder.Entity<Like>()
                .HasOne(l => l.Post)
                .WithMany(p => p.Likes)
                .HasForeignKey(l => l.PostID)
                .OnDelete(DeleteBehavior.Cascade); // Post silindiğinde beğeniler de silinir

            // User ile Like arasındaki ilişki
            modelBuilder.Entity<Like>()
                .HasOne(l => l.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.UserID)
                .OnDelete(DeleteBehavior.Cascade); // Kullanıcı silindiğinde beğeniler de silinir
        }
    }
}
