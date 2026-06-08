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

        private static Dictionary<ReportStatus, List<ReportStatus>> transitionTable =
            new Dictionary<ReportStatus, List<ReportStatus>>()
            {
                { ReportStatus.Draft, new List<ReportStatus>() { ReportStatus.Submitted } },
                { ReportStatus.Submitted, new List<ReportStatus>() { ReportStatus.UnderReview } },
                { ReportStatus.UnderReview, new List<ReportStatus>() { ReportStatus.Resolved } },
                { ReportStatus.Resolved, new List<ReportStatus>() { ReportStatus.Closed } },
                { ReportStatus.Closed, new List<ReportStatus>() }
            };

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
            Status = ReportStatus.Draft;
        }

        public bool TransitionTo(ReportStatus newStatus)
        {
            List<ReportStatus> allowedStatus = GetAllowedTransitions();

            if (allowedStatus.Contains(newStatus))
            {
                Status = newStatus;
                return true;
            }

            return false;
        }

        public List<ReportStatus> GetAllowedTransitions()
        {
            return transitionTable[Status];
        }

        public void AddEvidence(Evidence<string> evidence)
        {
            if (evidence == null)
                throw new Exception("Evidence tidak boleh null");

            Evidences.Add(evidence);
        }
    }
}
