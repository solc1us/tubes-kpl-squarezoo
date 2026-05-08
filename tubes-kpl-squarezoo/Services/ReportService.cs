using System.Net.NetworkInformation;
using System.Text.Json;
using tubes_kpl_squarezoo.Enums;
using tubes_kpl_squarezoo.Models;

namespace tubes_kpl_squarezoo.Services
{
    public class ReportService
    {
        // Menggunakan Dictionary untuk O(1) lookup berdasarkan Guid
        private Dictionary<Guid, Report> _reports;

        // Runtime configuration untuk path file
        private string _filePath;

        public ReportService(string filePath)
        {
            _filePath = filePath;
            _reports = new Dictionary<Guid, Report>();
            LoadFromFile(); // Inisialisasi data saat service dibuat 
        }

        // Business Logic: Menghasilkan instance Report baru
        public Report CreateReport(User user, string title, string desc)
        {
            var newReport = new Report(title, desc, user);

            _reports.Add(newReport.ReportId, newReport);
            SaveToFile(); // Persistence otomatis 
            return newReport;
        }

        public List<Report> GetAllReports()
        {
            // Mengambil semua value dari dictionary dan mengubahnya jadi List
            return _reports.Values.ToList();
        }

        public Report? GetById(Guid reportId)
        {
            // Nullable return untuk menangani record yang tidak ditemukan
            return _reports.TryGetValue(reportId, out var report) ? report : null;
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

        // File I/O langsung ke JSON
        public void SaveToFile()
        {
            try
            {
                string directory = Path.GetDirectoryName(_filePath);
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
