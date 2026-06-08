namespace tubes_kpl_squarezoo.Models.DTOs;
using tubes_kpl_squarezoo.Enums;

public record CreateUserRequest(string Name, string NoHP, UserRole Role = UserRole.Pelapor, string? Password = null);
public record UpdateUserRequest(string Name, string NoHP, UserRole Role = UserRole.Pelapor, string? Password = null);
public record LoginUserRequest(string NoHP, string Password);
