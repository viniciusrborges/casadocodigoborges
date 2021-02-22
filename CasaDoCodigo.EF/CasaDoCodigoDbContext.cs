using CasaDoCodigo.Model;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CasaDoCodigo.EF
{
    public class CasaDoCodigoDbContext : DbContext
    {
        public CasaDoCodigoDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var book = modelBuilder.Entity<Book>();
            var author = modelBuilder.Entity<Author>();
            var category = modelBuilder.Entity<Category>();
            var bookAuthorJoin = modelBuilder.Entity<BookAuthorJoin>();

            bookAuthorJoin
                .HasKey(bc => new { bc.BookId, bc.AuthorId });

            bookAuthorJoin
                .HasOne(bc => bc.Book)
                .WithMany(c => c.BookAuthors)
                .HasForeignKey(bc => bc.BookId);

            bookAuthorJoin
                .HasOne(bc => bc.Author)
                .WithMany(c => c.BookAuthors)
                .HasForeignKey(bc => bc.AuthorId);

            category.HasKey(i => i.Id);
            category.Property(e => e.Name).IsRequired();

            author.HasKey(i => i.Id);
            author.Property(e => e.Name).IsRequired();

            book.HasKey(i => i.Id);
            book.Property(e => e.Title).IsRequired();
            book.Property(e => e.SubTitle).IsRequired();
            book.Property(e => e.Summary).IsRequired();
            book.Property(e => e.DisplayName).IsRequired();
            book.Property(e => e.Price).IsRequired();
            book.Property(e => e.Price).HasColumnType("decimal(5,2)");
            book.HasOne(e => e.Category);
        }

        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Author> Authors { get; set; }
    }
}
