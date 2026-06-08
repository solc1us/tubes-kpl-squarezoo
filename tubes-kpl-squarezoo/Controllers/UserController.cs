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

    [HttpGet]
    public IActionResult GetAll() => Ok(_userService.GetAll());

    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        var user = _userService.GetById(id);
        return user == null ? NotFound(new { message = $"User dengan ID {id} tidak ditemukan." }) : Ok(user);
    }

    [HttpPost]
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

    [HttpPost("login")]
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

    [HttpPut("{id}")]
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

    [HttpDelete("{id}")]
    public IActionResult DeleteById(Guid id)
    {
        bool deleted = _userService.DeleteUser(id);
        return deleted ? NoContent() : NotFound(new { message = $"User dengan ID {id} tidak ditemukan." });
    }
}
