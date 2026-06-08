namespace tubes_kpl_squarezoo.Models.DTOs;
using tubes_kpl_squarezoo.Enums;

/// <summary>
/// Request body for creating a report.
/// </summary>
public record CreateReportRequest
{
    /// <summary>
    /// Report title.
    /// </summary>
    public string Title { get; init; } = string.Empty;

    /// <summary>
    /// Report chronology or description.
    /// </summary>
    public string Description { get; init; } = string.Empty;

    /// <summary>
    /// ID of the Pelapor submitting the report.
    /// </summary>
    public Guid UserId { get; init; }
}

/// <summary>
/// Request body for updating report title and description.
/// </summary>
public record UpdateReportRequest
{
    /// <summary>
    /// Updated report title.
    /// </summary>
    public string Title { get; init; } = string.Empty;

    /// <summary>
    /// Updated report description.
    /// </summary>
    public string Description { get; init; } = string.Empty;
}

/// <summary>
/// Request body for updating report status.
/// </summary>
public record TransitionReportStatusRequest
{
    /// <summary>
    /// Target report status. 0 = Diterima, 1 = Diproses, 2 = Selesai, 3 = Ditolak.
    /// </summary>
    public ReportStatus Status { get; init; }
}

/// <summary>
/// Request body for public report tracking.
/// </summary>
public record TrackReportRequest
{
    /// <summary>
    /// Report ID returned when the report was created.
    /// </summary>
    public Guid ReportId { get; init; }

    /// <summary>
    /// Six-digit tracking PIN returned when the report was created.
    /// </summary>
    public string Pin { get; init; } = string.Empty;
}
