namespace tubes_kpl_squarezoo.Models.DTOs;

using tubes_kpl_squarezoo.Enums;

/// <summary>
/// Request body for adding text-based evidence to a report.
/// </summary>
public record AddEvidenceRequest
{
    /// <summary>
    /// Six-digit tracking PIN for the report.
    /// </summary>
    public string Pin { get; init; } = string.Empty;

    /// <summary>
    /// Evidence type. 0 = Testimoni, 1 = KronologiTambahan, 2 = CatatanPendukung.
    /// </summary>
    public EvidenceType Type { get; init; }

    /// <summary>
    /// Evidence content.
    /// </summary>
    public string Content { get; init; } = string.Empty;

    /// <summary>
    /// Evidence description.
    /// </summary>
    public string Description { get; init; } = string.Empty;
}
