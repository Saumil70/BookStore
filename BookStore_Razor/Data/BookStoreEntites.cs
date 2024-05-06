using BookStore_Razor.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace BookStore_Razor.Data
{
    public class BookStoreEntites : DbContext


    {
        public BookStoreEntites(DbContextOptions<BookStoreEntites> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
    }
}
