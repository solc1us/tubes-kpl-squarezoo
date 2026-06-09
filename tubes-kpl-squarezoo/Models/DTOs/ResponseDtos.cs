namespace tubes_kpl_squarezoo.Models.DTOs;

using tubes_kpl_squarezoo.Enums;
using tubes_kpl_squarezoo.Models;

public record UserResponse(
    Guid UserId,
    string Name,
    string NoHP,
    UserRole Role,
    IEnumerable<string> Permissions)
{
    public static UserResponse FromUser(User user) =>
        new(user.UserId, user.Name, user.NoHP, user.Role, user.GetPermissions());
}

public record ReportResponse(
    Guid ReportId,
    string Title,
    string Description,
    UserResponse ReportedBy,
    IEnumerable<Evidence<string>> Evidences,
    ReportStatus Status,
    string TrackingPin)
{
    public static ReportResponse FromReport(Report report) =>
        new(
            report.ReportId,
            report.Title,
            report.Description,
            UserResponse.FromUser(report.ReportedBy),
            report.Evidences,
            report.Status,
            report.TrackingPin);
}
