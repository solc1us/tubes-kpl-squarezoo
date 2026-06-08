using tubes_kpl_squarezoo.Enums;

namespace tubes_kpl_squarezoo.Models.DTOs;

public record AddEvidenceRequest(EvidenceType Type, string Content, string Description);
