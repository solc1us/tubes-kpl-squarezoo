using System.Text.Json;
using tubes_kpl_squarezoo.Enums;
using tubes_kpl_squarezoo.Models;

namespace tubes_kpl_squarezoo.Services
{
    public class ReportService
    {
        private Dictionary<Guid, Report> _reports;
        private string _filePath;

        public ReportService(string filePath)
        {
            _filePath = filePath;
            _reports = new Dictionary<Guid, Report>();
            LoadFromFile();
        }

        public Report CreateReport(Report report)
        {
            if (report == null) throw new ArgumentNullException(nameof(report));

            if (report.Status != ReportStatus.Diterima)
                throw new InvalidOperationException("Laporan baru harus memiliki status Diterima.");

            if (!_reports.ContainsKey(report.ReportId))
            {
                _reports.Add(report.ReportId, report);
                SaveToFile();
            }

            return report;
        }

        public List<Report> GetAllReports()
        {
            return _reports.Values.ToList();
        }

        public List<Report> GetReportsByStatus(ReportStatus status)
        {
            return _reports.Values
                .Where(report => report.Status == status)
                .ToList();
        }

        public object GetSummary()
        {
            return new
            {
                total = _reports.Count,
                diterima = _reports.Values.Count(report => report.Status == ReportStatus.Diterima),
                diproses = _reports.Values.Count(report => report.Status == ReportStatus.Diproses),
                selesai = _reports.Values.Count(report => report.Status == ReportStatus.Selesai),
                ditolak = _reports.Values.Count(report => report.Status == ReportStatus.Ditolak)
            };
        }

        public Report? GetById(Guid reportId)
        {
            return _reports.TryGetValue(reportId, out var report) ? report : null;
        }

        public object? TrackReport(Guid reportId, string pin)
        {
            var report = GetById(reportId);
            if (report == null || report.TrackingPin != pin)
                return null;

            return new
            {
                report.ReportId,
                report.ReporterName,
                report.ReporterNoHP,
                report.Title,
                report.Description,
                report.ReportedPerson,
                report.Location,
                report.IncidentDate,
                report.Status,
                report.Evidences
            };
        }

        public bool UpdateReport(Guid reportId, string title, string desc)
        {
            var report = GetById(reportId);
            if (report == null) return false;

            report.Title = title;
            report.Description = desc;

            SaveToFile();
            return true;
        }

        public bool DeleteReport(Guid reportId)
        {
            if (_reports.Remove(reportId))
            {
                SaveToFile();
                return true;
            }

            return false;
        }

        public bool ExecuteTransition(Guid reportId, ReportStatus nextStatus)
        {
            var report = GetById(reportId);
            if (report == null) return false;

            bool isTransitionSuccessful = report.TransitionTo(nextStatus);
            if (!isTransitionSuccessful) return false;

            SaveToFile();
            return true;
        }

        public Report? CloseReport(Guid reportId)
        {
            var report = GetById(reportId);
            if (report == null) return null;

            report.Status = ReportStatus.Selesai;
            SaveToFile();
            return report;
        }

        public Evidence<string>? AddEvidenceToReport(Guid reportId, string pin, EvidenceType type, string content, string description)
        {
            var report = GetById(reportId);
            if (report == null) return null;
            if (report.TrackingPin != pin) return null;
            if (content == null) throw new ArgumentNullException(nameof(content));

            if (!Enum.IsDefined(typeof(EvidenceType), type))
                throw new ArgumentException("Invalid evidence type.", nameof(type));

            var evidence = new Evidence<string>(type, content, description);
            report.AddEvidence(evidence);
            SaveToFile();

            return evidence;
        }

        public void SaveToFile()
        {
            try
            {
                string? directory = Path.GetDirectoryName(_filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                string jsonString = JsonSerializer.Serialize(_reports, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_filePath, jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Gagal menyimpan data: {ex.Message}");
            }
        }

        public void LoadFromFile()
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    string jsonString = File.ReadAllText(_filePath);
                    var data = JsonSerializer.Deserialize<Dictionary<Guid, Report>>(jsonString);
                    _reports = data ?? new Dictionary<Guid, Report>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Gagal memuat data: {ex.Message}");
                _reports = new Dictionary<Guid, Report>();
            }
        }
    }
}
