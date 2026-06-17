using System;
using tubes_kpl_squarezoo.Enums;

namespace tubes_kpl_squarezoo.Models
{
    public class Evidence<T>
    {
        public Guid EvidenceId { get; set; }
        public EvidenceType Type { get; set; }
        public T Content { get; set; }
        public string Description { get; set; }

        public Evidence(EvidenceType type, T content, string description)
        {
            EvidenceId = Guid.NewGuid();
            Type = type;
            Content = content;
            Description = description;
        }

        public string GetSummary()
        {
            if (Content == null)
                return "No Content";

            string text = Content.ToString();

            switch (Type)
            {
                case EvidenceType.Testimoni:
                    return "[Testimoni] " + (text.Length > 50 ? text.Substring(0, 50) + "..." : text);

                case EvidenceType.KronologiTambahan:
                    return "[Kronologi Tambahan] " + text;

                case EvidenceType.CatatanPendukung:
                    return "[Catatan Pendukung] " + text;

                default:
                    return text;
            }
        }

        public bool Validate()
        {
            if (Content == null)
                return false;

            string text = Content.ToString();

            switch (Type)
            {
                case EvidenceType.Testimoni:
                    return text.Length >= 10;

                case EvidenceType.KronologiTambahan:
                    return !string.IsNullOrWhiteSpace(text);

                case EvidenceType.CatatanPendukung:
                    return !string.IsNullOrWhiteSpace(text);

                default:
                    return false;
            }
        }
    }
}
