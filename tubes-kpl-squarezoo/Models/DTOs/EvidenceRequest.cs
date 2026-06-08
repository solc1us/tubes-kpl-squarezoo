using tubes_kpl_squarezoo.Enums;

namespace tubes_kpl_squarezoo.Models.DTOs;

/// <summary>
/// Request body for adding text-based evidence to a report.
/// </summary>
public record AddEvidenceRequest
{
    /// <summary>
    /// Evidence type. 0 = Testimoni, 1 = KronologiTambahan, 2 = CatatanPendukung.
    /// </summary>
    public EvidenceType Type { get; init; }

    /// <summary>
    /// Text content of the evidence.
    /// </summary>
    public string Content { get; init; } = string.Empty;

    /// <summary>
    /// Short description of the evidence.
    /// </summary>
    public string Description { get; init; } = string.Empty;
}
