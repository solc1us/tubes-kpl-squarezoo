using System;
using System.Collections.Generic;
using System.Linq;
using tubes_kpl_squarezoo.Enums;
using tubes_kpl_squarezoo.Models;
using tubes_kpl_squarezoo.Services;

namespace tubes_kpl_squarezoo
{
    public class AdminManager
    {
        public Guid AdminID { get; set; }
        public string Name { get; set; }

        private readonly ReportService _service;

        public AdminManager(string name, ReportService service)
        {
            AdminID = Guid.NewGuid();
            Name = name;
            _service = service;
        }

        public List<Report> GetAllReports()
        {
            return _service.GetAllReports();
        }

        public List<Report> GetByStatus(ReportStatus status)
        {
            return _service.GetAllReports()
                .Where(report => report.Status == status)
                .ToList();
        }

        public Dictionary<ReportStatus, int> GetSummary()
        {
            return _service.GetAllReports()
                .GroupBy(report => report.Status)
                .ToDictionary(group => group.Key, group => group.Count());
        }

        public bool CloseReport(Guid reportId)
        {
            Report report = _service.GetById(reportId);

            if (report == null)
            {
                return false;
            }

            return report.TransitionTo(ReportStatus.Closed);
        }
    }
}