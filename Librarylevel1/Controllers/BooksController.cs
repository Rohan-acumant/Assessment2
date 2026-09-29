using Librarylevel1.Models;
using Librarylevel1.Data;
using Microsoft.AspNetCore.Mvc;

namespace Librarylevel1.Controllers;

[ApiController]
[Route("api/books")]
public class BooksController(BookRepository books, UserRepository users) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Book>> GetAll()
    {
        return Ok(books.GetAll());
    }

    [HttpGet("{id:int}")]
    public ActionResult<Book> GetById(int id)
    {
        var book = books.GetById(id);
        return book is null ? NotFound(new { message = "Book not found." }) : Ok(book);
    }

    [HttpPost]
    public ActionResult<Book> Add(Book newBook)
    {
        newBook.IsAvailable = true;
        books.Add(newBook);
        return CreatedAtAction(nameof(GetById), new { id = newBook.Id }, newBook);
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, Book updatedBook)
    {
        var book = books.GetById(id);
        if (book is null)
        {
            return NotFound(new { message = "Book not found." });
        }

        book.Title = updatedBook.Title;
        book.Author = updatedBook.Author;
        book.Year = updatedBook.Year;
        books.SaveChanges();
        return Ok(new { message = "Book updated.", book });
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var book = books.GetById(id);
        if (book is null)
        {
            return NotFound(new { message = "Book not found." });
        }

        if (!book.IsAvailable)
        {
            return Conflict(new { message = "A borrowed book cannot be deleted." });
        }

        books.Delete(id);
        return Ok(new { message = "Book deleted." });
    }

    [HttpPost("{id:int}/borrow")]
    public IActionResult Borrow(int id, [FromQuery] int userId)
    {
        var book = books.GetById(id);
        if (book is null)
        {
            return NotFound(new { message = "Book not found." });
        }

        var user = users.GetById(userId);
        if (user is null)
        {
            return NotFound(new { message = "User not found." });
        }

        if (!book.IsAvailable)
        {
            return Conflict(new { message = "Book is already borrowed." });
        }

        if (user.BorrowedBookIds.Count >= 3)
        {
            return Conflict(new { message = "A user can borrow no more than 3 books." });
        }

        user.BorrowedBookIds.Add(book.Id);
        book.IsAvailable = false;
        users.SaveChanges();
        books.SaveChanges();
        return Ok(new { message = "Book borrowed.", book, user });
    }

    [HttpPost("{id:int}/return")]
    public IActionResult Return(int id, [FromQuery] int userId)
    {
        var book = books.GetById(id);
        if (book is null)
        {
            return NotFound(new { message = "Book not found." });
        }

        var user = users.GetById(userId);
        if (user is null)
        {
            return NotFound(new { message = "User not found." });
        }

        if (!user.BorrowedBookIds.Remove(book.Id))
        {
            return Conflict(new { message = "This user has not borrowed this book." });
        }

        book.IsAvailable = true;
        users.SaveChanges();
        books.SaveChanges();
        return Ok(new { message = "Book returned.", book, user });
    }
}