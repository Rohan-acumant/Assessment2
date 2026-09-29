using System.Text.Json;
using Librarylevel1.Models;

namespace Librarylevel1.Data;

public class UserRepository
{
    private readonly string _filePath;
    private readonly List<User> _users;

    public UserRepository(string filePath)
    {
        _filePath = filePath;

        if (File.Exists(_filePath))
        {
            _users = JsonSerializer.Deserialize<List<User>>(File.ReadAllText(_filePath)) ?? new();
        }
        else
        {
            _users = new();
            SaveChanges();
        }
    }

    public List<User> GetAll()
    {
        return _users;
    }

    public User? GetById(int id)
    {
        return _users.FirstOrDefault(user => user.Id == id);
    }

    public User? GetByEmail(string email)
    {
        return _users.FirstOrDefault(user => string.Equals(user.Email, email, StringComparison.OrdinalIgnoreCase));
    }

    public User Add(User user)
    {
        user.Id = _users.Count == 0 ? 1 : _users.Max(existingUser => existingUser.Id) + 1;
        user.BorrowedBookIds = new();
        _users.Add(user);
        SaveChanges();
        return user;
    }

    public void SaveChanges()
    {
        File.WriteAllText(_filePath, JsonSerializer.Serialize(_users, new JsonSerializerOptions
        {
            WriteIndented = true
        }));
    }
}