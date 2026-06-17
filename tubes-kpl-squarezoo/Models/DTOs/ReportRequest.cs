namespace tubes_kpl_squarezoo.Models.DTOs;
using tubes_kpl_squarezoo.Enums;

/// <summary>
/// Request body for creating a report.
/// </summary>
public record CreateReportRequest
{
    /// <summary>
    /// Reporter name.
    /// </summary>
    public string ReporterName { get; init; } = string.Empty;

    /// <summary>
    /// Reporter phone number used for follow-up.
    /// </summary>
    public string ReporterNoHP { get; init; } = string.Empty;

    /// <summary>
    /// Report title.
    /// </summary>
    public string Title { get; init; } = string.Empty;

    /// <summary>
    /// Report chronology or description.
    /// </summary>
    public string Description { get; init; } = string.Empty;

    /// <summary>
    /// Reported person's name.
    /// </summary>
    public string ReportedPerson { get; init; } = string.Empty;

    /// <summary>
    /// Incident location.
    /// </summary>
    public string Location { get; init; } = string.Empty;

    /// <summary>
    /// Incident date and time.
    /// </summary>
    public DateTime IncidentDate { get; init; }
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
