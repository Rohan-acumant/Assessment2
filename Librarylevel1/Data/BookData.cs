using System.Collections.Generic;
using Librarylevel1.Models;

namespace Librarylevel1.Data
{
    internal class BookData
    {
       public static List<Book> GetBooks()
       {
            return new List<Book>
            {
            new() { Id = 1, Title = "The Hobbit", Author = "J.R.R. Tolkien", Year = 1937, IsAvailable = true },
            new() { Id = 2, Title = "Pride and Prejudice", Author = "Jane Austen", Year = 1813, IsAvailable = true },
            new() { Id = 3, Title = "1984", Author = "George Orwell", Year = 1949, IsAvailable = true },
            new() { Id = 4, Title = "To Kill a Mockingbird", Author = "Harper Lee", Year = 1960, IsAvailable = true },
            new() { Id = 5, Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", Year = 1925, IsAvailable = true }
            };
       }
    }
}

