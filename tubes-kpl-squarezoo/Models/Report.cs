using System;
using System.Collections.Generic;
using tubes_kpl_squarezoo.Enums;

namespace tubes_kpl_squarezoo.Models
{
    public class Report
    {
        public Guid ReportId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public User ReportedBy { get; set; }
        public List<Evidence<string>> Evidences { get; set; }
        public ReportStatus Status { get; set; }
        public string TrackingPin { get; set; } = string.Empty;

        public Report(string title, string description, User reportedBy)
        {
            if (title == "")
                throw new Exception("Title tidak boleh kosong");

            if (description == "")
                throw new Exception("Description tidak boleh kosong");

            if (reportedBy == null)
                throw new Exception("User pelapor tidak boleh null");

            ReportId = Guid.NewGuid();
            Title = title;
            Description = description;
            ReportedBy = reportedBy;
            Evidences = new List<Evidence<string>>();
            Status = ReportStatus.Diterima;
            TrackingPin = Random.Shared.Next(100000, 1000000).ToString();
        }

        public bool TransitionTo(ReportStatus newStatus)
        {
            if (!Enum.IsDefined(typeof(ReportStatus), newStatus))
                return false;

            if (!GetAllowedTransitions().Contains(newStatus))
                return false;

            Status = newStatus;
            return true;
        }

        public List<ReportStatus> GetAllowedTransitions()
        {
            return Status switch
            {
                ReportStatus.Diterima => new List<ReportStatus> { ReportStatus.Diproses, ReportStatus.Ditolak },
                ReportStatus.Diproses => new List<ReportStatus> { ReportStatus.Selesai, ReportStatus.Ditolak },
                _ => new List<ReportStatus>()
            };
        }

        public void AddEvidence(Evidence<string> evidence)
        {
            if (evidence == null)
                throw new Exception("Evidence tidak boleh null");

            Evidences.Add(evidence);
        }
    }
}
