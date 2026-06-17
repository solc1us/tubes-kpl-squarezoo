namespace tubes_kpl_squarezoo.Models.DTOs;
using tubes_kpl_squarezoo.Enums;

/// <summary>
/// Request body for creating a user.
/// </summary>
public record CreateUserRequest
{
    /// <summary>
    /// User display name.
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// User phone number used as login identifier.
    /// </summary>
    public string NoHP { get; init; } = string.Empty;

    /// <summary>
    /// User role. 0 = Pelapor, 1 = Admin, 2 = Pimpinan.
    /// </summary>
    public UserRole Role { get; init; } = UserRole.Pelapor;

    /// <summary>
    /// Optional plain password for MVP login.
    /// </summary>
    public string? Password { get; init; }
}

/// <summary>
/// Request body for updating a user.
/// </summary>
public record UpdateUserRequest
{
    /// <summary>
    /// Updated user display name.
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Updated user phone number.
    /// </summary>
    public string NoHP { get; init; } = string.Empty;

    /// <summary>
    /// Updated user role. 0 = Pelapor, 1 = Admin, 2 = Pimpinan.
    /// </summary>
    public UserRole Role { get; init; } = UserRole.Pelapor;

    /// <summary>
    /// Optional updated plain password for MVP login.
    /// </summary>
    public string? Password { get; init; }
}

/// <summary>
/// Request body for simple MVP login.
/// </summary>
public record LoginUserRequest
{
    /// <summary>
    /// User phone number.
    /// </summary>
    public string NoHP { get; init; } = string.Empty;

    /// <summary>
    /// Plain password for MVP login.
    /// </summary>
    public string Password { get; init; } = string.Empty;
}
