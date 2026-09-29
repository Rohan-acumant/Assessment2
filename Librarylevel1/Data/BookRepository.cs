using System.Text.Json;
using Librarylevel1.Models;

namespace Librarylevel1.Data;

public class BookRepository
{
    private readonly string _filePath;
    private readonly List<Book> _books;

    public BookRepository(string filePath)
    {
        _filePath = filePath;

        if (File.Exists(_filePath))
        {
            _books = JsonSerializer.Deserialize<List<Book>>(File.ReadAllText(_filePath)) ?? new();
        }
        else
        {
            _books = BookData.GetBooks();
            SaveChanges();
        }
    }

    public List<Book> GetAll()
    {
        return _books;
    }

    public Book? GetById(int id)
    {
        return _books.FirstOrDefault(book => book.Id == id);
    }

    public Book Add(Book book)
    {
        book.Id = _books.Count == 0 ? 1 : _books.Max(existingBook => existingBook.Id) + 1;
        _books.Add(book);
        SaveChanges();
        return book;
    }

    public bool Delete(int id)
    {
        var book = GetById(id);
        if (book is null)
        {
            return false;
        }

        _books.Remove(book);
        SaveChanges();
        return true;
    }

    public void SaveChanges()
    {
        File.WriteAllText(_filePath, JsonSerializer.Serialize(_books, new JsonSerializerOptions
        {
            WriteIndented = true
        }));
    }
}