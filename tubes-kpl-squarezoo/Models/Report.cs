using System;
using System.Collections.Generic;
using tubes_kpl_squarezoo.Enums;

namespace tubes_kpl_squarezoo.Models
{
    public class Report
    {
        public Guid ReportId { get; set; }
        public string ReporterName { get; set; }
        public string ReporterNoHP { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ReportedPerson { get; set; }
        public string Location { get; set; }
        public DateTime IncidentDate { get; set; }
        public List<Evidence<string>> Evidences { get; set; }
        public ReportStatus Status { get; set; }
        public string TrackingPin { get; set; } = string.Empty;

        public Report()
        {
            ReportId = Guid.NewGuid();
            ReporterName = string.Empty;
            ReporterNoHP = string.Empty;
            Title = string.Empty;
            Description = string.Empty;
            ReportedPerson = string.Empty;
            Location = string.Empty;
            IncidentDate = DateTime.MinValue;
            Evidences = new List<Evidence<string>>();
            Status = ReportStatus.Diterima;
            TrackingPin = string.Empty;
        }

        public Report(
            string reporterName,
            string reporterNoHP,
            string title,
            string description,
            string reportedPerson,
            string location,
            DateTime incidentDate)
        {
            if (string.IsNullOrWhiteSpace(reporterName))
                throw new Exception("Nama pelapor tidak boleh kosong");

            if (string.IsNullOrWhiteSpace(reporterNoHP))
                throw new Exception("Nomor HP pelapor tidak boleh kosong");

            if (string.IsNullOrWhiteSpace(title))
                throw new Exception("Title tidak boleh kosong");

            if (string.IsNullOrWhiteSpace(description))
                throw new Exception("Description tidak boleh kosong");

            ReportId = Guid.NewGuid();
            ReporterName = reporterName;
            ReporterNoHP = reporterNoHP;
            Title = title;
            Description = description;
            ReportedPerson = reportedPerson;
            Location = location;
            IncidentDate = incidentDate;
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
