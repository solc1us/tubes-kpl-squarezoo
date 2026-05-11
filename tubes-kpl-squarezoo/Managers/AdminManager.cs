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

        // Menggunakan ReportService untuk mengelola data report
        private readonly ReportService _service;

        // Constructor AdminManager
        public AdminManager(ReportService service)
        {
            AdminID = Guid.NewGuid();
            Name = "System Admin";
            _service = service;
        }

        // Mengambil seluruh data report
        public List<Report> GetAllReports()
        {
            return _service.GetAllReports();
        }

        // Mengambil report berdasarkan status tertentu
        public List<Report> GetByStatus(ReportStatus status)
        {
            return _service.GetAllReports()
                .Where(report => report.Status == status)
                .ToList();
        }

        // Membuat summary jumlah report berdasarkan status
        public Dictionary<ReportStatus, int> GetSummary()
        {
            return _service.GetAllReports()
                .GroupBy(report => report.Status)
                .ToDictionary(group => group.Key, group => group.Count());
        }

        // Menutup report jika status sudah valid untuk di-close
        public bool CloseReport(Guid reportId)
        {
            // Mengambil report berdasarkan ID
            Report? report = _service.GetById(reportId);

            // Return false jika report tidak ditemukan
            if (report == null) return false;

            // Mengubah status report menjadi Closed
            bool success = report.TransitionTo(ReportStatus.Closed);

            // Simpan perubahan jika transisi berhasil
            if (success)
            {
                _service.SaveToFile();
            }

            return success;
        }
    }
}