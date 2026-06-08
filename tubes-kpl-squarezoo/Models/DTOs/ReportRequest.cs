namespace tubes_kpl_squarezoo.Models.DTOs;
using tubes_kpl_squarezoo.Enums;
public record CreateReportRequest(string Title, string Description, Guid UserId);
public record UpdateReportRequest(string Title, string Description);
public record TransitionRequest(ReportStatus NextStatus);
