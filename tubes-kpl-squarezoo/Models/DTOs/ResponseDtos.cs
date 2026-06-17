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
    string ReporterName,
    string ReporterNoHP,
    string Title,
    string Description,
    string ReportedPerson,
    string Location,
    DateTime IncidentDate,
    IEnumerable<Evidence<string>> Evidences,
    ReportStatus Status)
{
    public static ReportResponse FromReport(Report report) =>
        new(
            report.ReportId,
            report.ReporterName,
            report.ReporterNoHP,
            report.Title,
            report.Description,
            report.ReportedPerson,
            report.Location,
            report.IncidentDate,
            report.Evidences,
            report.Status);
}

public record CreateReportResponse(
    Guid ReportId,
    string TrackingPin,
    string Message);
