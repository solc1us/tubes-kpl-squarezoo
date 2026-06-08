using Microsoft.AspNetCore.Mvc;
using tubes_kpl_squarezoo.Services;
using tubes_kpl_squarezoo.Models.DTOs;

namespace tubes_kpl_squarezoo.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    public UserController(UserService userService) => _userService = userService;

    [HttpPost]
    public IActionResult Create([FromBody] CreateUserRequest request)
    {
        var user = _userService.CreateUser(request.Name, request.PhoneNumber);
        return Ok(user);
    }

    [HttpGet]
    public IActionResult GetAll() => Ok(_userService.GetAll());

    [HttpPut("{id}")]
    public IActionResult Update(Guid id, [FromBody] UpdateUserRequest request)
    {
        try
        {
            var updatedUser = _userService.UpdateUser(id, request.Name, request.PhoneNumber);
            return Ok(updatedUser);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        var user = _userService.GetById(id);
        return user == null ? NotFound($"User dengan ID {id} tidak ditemukan.") : Ok(user);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteById(Guid id)
    {
        var user = _userService.GetById(id);
        if (user == null)
        {
            return NotFound($"User dengan ID {id} tidak ditemukan.");
        }

        bool deleted = _userService.DeleteUser(id);
        return deleted ? NoContent() : NotFound();
    }

}
