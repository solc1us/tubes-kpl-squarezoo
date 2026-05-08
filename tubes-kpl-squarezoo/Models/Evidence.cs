using System;

namespace tubes_kpl_squarezoo.Models
{
    public class Evidence<T>
    {
        public Guid EvidenceId { get; set; }
        public string Type { get; set; }
        public T Content { get; set; }
        public string Description { get; set; }

        public Evidence(string type, T content, string description)
        {
            EvidenceId = Guid.NewGuid();
            Type = type;
            Content = content;
            Description = description;
        }
    }
}