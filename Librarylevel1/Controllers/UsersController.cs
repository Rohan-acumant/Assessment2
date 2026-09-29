using Librarylevel1.Data;
using Librarylevel1.Models;
using Microsoft.AspNetCore.Mvc;

namespace Librarylevel1.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController(UserRepository users) : ControllerBase
{
    [HttpPost("register")]
    public ActionResult<User> Register(User newUser)
    {
        newUser.Name = newUser.Name.Trim();
        newUser.Email = newUser.Email.Trim();

        if (string.IsNullOrWhiteSpace(newUser.Name) || string.IsNullOrWhiteSpace(newUser.Email))
        {
            return BadRequest(new { message = "Name and email are required." });
        }

        if (users.GetByEmail(newUser.Email) is not null)
        {
            return Conflict(new { message = "A user with this email already exists." });
        }

        users.Add(newUser);
        return CreatedAtAction(nameof(GetByEmail), new { email = newUser.Email }, newUser);
    }

    [HttpGet]
    public ActionResult<IEnumerable<User>> GetAll()
    {
        return Ok(users.GetAll());
    }

    [HttpGet("{email}")]
    public ActionResult<User> GetByEmail(string email)
    {
        var user = users.GetByEmail(email);
        return user is null ? NotFound(new { message = "User not found." }) : Ok(user);
    }
}