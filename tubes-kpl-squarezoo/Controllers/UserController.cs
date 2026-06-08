using Microsoft.AspNetCore.Mvc;
using tubes_kpl_squarezoo.Models.DTOs;
using tubes_kpl_squarezoo.Services;

namespace tubes_kpl_squarezoo.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService) => _userService = userService;

    /// <summary>
    /// Gets all users.
    /// </summary>
    /// <returns>A raw array of users.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetAll() => Ok(_userService.GetAll());

    /// <summary>
    /// Gets user by ID.
    /// </summary>
    /// <param name="id">User ID.</param>
    /// <returns>The matching user.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(Guid id)
    {
        var user = _userService.GetById(id);
        return user == null ? NotFound(new { message = $"User dengan ID {id} tidak ditemukan." }) : Ok(user);
    }

    /// <summary>
    /// Creates a user.
    /// </summary>
    /// <param name="request">User creation request.</param>
    /// <returns>The created user.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] CreateUserRequest request)
    {
        try
        {
            var user = _userService.CreateUser(request.Name, request.NoHP, request.Role, request.Password);
            return Ok(user);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Simple MVP login using noHP and password.
    /// </summary>
    /// <remarks>No JWT and no password hashing are used for the MVP. Returns user role and computed permissions.</remarks>
    /// <param name="request">Login request containing noHP and password.</param>
    /// <returns>User identity, role, and permissions.</returns>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Login([FromBody] LoginUserRequest request)
    {
        var user = _userService.Login(request.NoHP, request.Password);
        if (user == null)
        {
            return Unauthorized(new { message = "NoHP atau password tidak valid." });
        }

        return Ok(new
        {
            user.UserId,
            user.Name,
            user.NoHP,
            user.Role,
            permissions = user.GetPermissions()
        });
    }

    /// <summary>
    /// Updates user data.
    /// </summary>
    /// <param name="id">User ID.</param>
    /// <param name="request">User update request.</param>
    /// <returns>The updated user.</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(Guid id, [FromBody] UpdateUserRequest request)
    {
        try
        {
            var updatedUser = _userService.UpdateUser(id, request.Name, request.NoHP, request.Role, request.Password);
            return Ok(updatedUser);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Deletes a user.
    /// </summary>
    /// <param name="id">User ID.</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult DeleteById(Guid id)
    {
        bool deleted = _userService.DeleteUser(id);
        return deleted ? NoContent() : NotFound(new { message = $"User dengan ID {id} tidak ditemukan." });
    }
}
