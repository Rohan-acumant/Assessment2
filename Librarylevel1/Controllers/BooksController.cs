using Librarylevel1.Models;
using Microsoft.AspNetCore.Mvc;

namespace Librarylevel1.Controllers;

[ApiController]
[Route("api/books")]
public class BooksController(List<Book> books) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Book>> GetAll()
    {
        return Ok(books);
    }

    [HttpGet("{id:int}")]
    public ActionResult<Book> GetById(int id)
    {
        var book = books.FirstOrDefault(book => book.Id == id);
        return book is null ? NotFound(new { message = "Book not found." }) : Ok(book);
    }

    [HttpPost]
    public ActionResult<Book> Add(Book newBook)
    {
        newBook.Id = books.Count == 0 ? 1 : books.Max(book => book.Id) + 1;
        books.Add(newBook);
        return CreatedAtAction(nameof(GetById), new { id = newBook.Id }, newBook);
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, Book updatedBook)
    {
        var book = books.FirstOrDefault(book => book.Id == id);
        if (book is null)
        {
            return NotFound(new { message = "Book not found." });
        }

        book.Title = updatedBook.Title;
        book.Author = updatedBook.Author;
        book.Year = updatedBook.Year;
        return Ok(new { message = "Book updated.", book });
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var book = books.FirstOrDefault(book => book.Id == id);
        if (book is null)
        {
            return NotFound(new { message = "Book not found." });
        }

        books.Remove(book);
        return Ok(new { message = "Book deleted." });
    }

    [HttpPost("{id:int}/borrow")]
    public IActionResult Borrow(int id)
    {
        return SetAvailability(id, isAvailable: false);
    }

    [HttpPost("{id:int}/return")]
    public IActionResult Return(int id)
    {
        return SetAvailability(id, isAvailable: true);
    }

    private IActionResult SetAvailability(int id, bool isAvailable)
    {
        var book = books.FirstOrDefault(book => book.Id == id);
        if (book is null)
        {
            return NotFound(new { message = "Book not found." });
        }

        if (book.IsAvailable == isAvailable)
        {
            return Conflict(new { message = isAvailable ? "Book is already available." : "Book is already borrowed." });
        }

        book.IsAvailable = isAvailable;
        return Ok(new { message = isAvailable ? "Book returned." : "Book borrowed.", book });
    }
}