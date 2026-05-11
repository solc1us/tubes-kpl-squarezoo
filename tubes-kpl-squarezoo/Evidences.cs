namespace tubes_kpl_squarezoo
{
    public enum EvidenceType
    {
        Testimony,
        Document,
        MediaLink
    }

    public class Evidences<T>
    {
        public Guid EvidenceId { get; set; } = Guid.NewGuid();
        public EvidenceType Type { get; set; }
        public T Content { get; set; }

        public string GetSummary()
        {
            if (Content == null)
                return "No Content";

            string text = Content.ToString();

            switch (Type)
            {
                case EvidenceType.Testimony:
                    return "[Testimony] " + (text.Length > 50 ? text.Substring(0, 50) + "..." : text);

                case EvidenceType.Document:
                    return "[Document] " + text;

                case EvidenceType.MediaLink:
                    return "[Media] " + text;

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
                case EvidenceType.Testimony:
                    return text.Length >= 10;

                case EvidenceType.Document:
                    return !string.IsNullOrWhiteSpace(text);

                case EvidenceType.MediaLink:
                    return Uri.IsWellFormedUriString(text, UriKind.Absolute);

                default:
                    return false;
            }
        }
    }
}