using Librarylevel1.Data;
using Librarylevel1.Models;

var books = BookData.GetBooks();

while(true)
{
    Console.WriteLine("\nLibrary menu");
    Console.WriteLine("1. List all books");
    Console.WriteLine("2. Add a new book");
    Console.WriteLine("3. Update a book (by Id)");
    Console.WriteLine("4. Delete a book (by Id)");
    Console.WriteLine("5. Borrow a book");
    Console.WriteLine("6. Return a book");
    Console.WriteLine("7. Exit");
    Console.Write("Choose an option: ");

    var choice = Console.ReadLine();
    switch(choice)
    {
        case "1":
            ListBooks();
            break;
        case "2":
            AddBook();
            break;
        case "3":
            UpdateBook();
            break;
        case "4":
            DeleteBook();
            break;
        case "5":
            BorrowBook();
            break;
        case "6":
            ReturnBook();
            break;
        case "7":
            return;
        default:
            Console.WriteLine("Invalid option. Please try again.");
            break;
    }

    void ListBooks()
    {
        if(books.Count == 0)
        {
            Console.WriteLine("No books available.");
            return;
        }
        foreach (var book in books)
        {
            var availability = book.IsAvailable ? "Available" : "Borrowed";
            Console.WriteLine($"Id: {book.Id} | {book.Title} by {book.Author} ({book.Year}) | {availability}");
        }
    }

    void AddBook()
    {
        var book = new Book
        {
            Id = books.Count == 0 ? 1 : books.Max(book => book.Id) + 1,
            Title = ReadRequiredText("Enter the book title: "),
            Author = ReadRequiredText("Enter the book author: "),
            Year = int.Parse(ReadRequiredText("Enter the book year: "))
        };


        books.Add(book);
        Console.WriteLine($"Book added with Id {book.Id}.");
    }

    void UpdateBook()
    {
        var book = FindBookById();
        if (book == null) return;

        book.Title = ReadRequiredText("New title: ");
        book.Author = ReadRequiredText("New author: ");
        book.Year = ReadPositiveNumber("New year: ");
        Console.WriteLine("Book updated.");
    }

    void DeleteBook()
    {
        var book = FindBookById();

        if(book is not null)
        {
            books.Remove(book);
            Console.WriteLine("Book deleted.");
        }

    }

    void ChangeAvailability(bool isAvailable)
    {
        var book = FindBookById();
        if (book is null) return;

        if (book.IsAvailable == isAvailable)
        {
            Console.WriteLine(isAvailable ? "Book is already returned." : "Book is already borrowed.");
        }
        else
        {
            book.IsAvailable = isAvailable;
            Console.WriteLine(isAvailable ? "Book returned." : "Book borrowed.");
        }
    }

    void BorrowBook()
    {
        ChangeAvailability(false);
    }

    void ReturnBook()
    {
        ChangeAvailability(true);
    }

    Book ? FindBookById()
    {
        var id = ReadPositiveNumber("Enter the book Id: ");
        var book = books.FirstOrDefault(b => b.Id == id);
        if (book is null)
        {
            Console.WriteLine("Book not found.");
        }
        return book;
    }

    string ReadRequiredText(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var value = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            Console.WriteLine("Input cannot be empty. Please try again.");
        }
    }

    int ReadPositiveNumber(String prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if(int.TryParse(Console.ReadLine(), out int number) && number > 0)
            {
                return number;
            }

            Console.WriteLine("Enter a valid number greater than zero.");
        }
    }
}