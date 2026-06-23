using System.Text.Json;
using tubes_kpl_squarezoo.Enums;
using tubes_kpl_squarezoo.Models;
using tubes_kpl_squarezoo.Models.DTOs;
using tubes_kpl_squarezoo.Services;

namespace LaporU.PerformanceTests.Helpers
{
    public static class BenchmarkTestDataFactory
    {
        public static Report CreateSampleReport(int index = 0) =>
            new(
                $"Pelapor {index}",
                $"08123456{index:0000}",
                $"Laporan {index}",
                $"Deskripsi laporan benchmark {index}",
                $"Terlapor {index}",
                "Gedung Rektorat",
                new DateTime(2026, 1, 15).AddDays(index % 30));

        public static Report CreateSampleReportWithStatus(ReportStatus status, int index = 0)
        {
            var report = CreateSampleReport(index);
            report.Status = status;
            return report;
        }

        public static List<Report> CreateManyReports(int count) =>
            Enumerable.Range(0, count).Select(CreateSampleReport).ToList();

        public static Evidence<string> CreateSampleEvidence(
            EvidenceType type = EvidenceType.Testimoni,
            int index = 0) =>
            new(type, $"Isi bukti benchmark yang valid {index}", $"Deskripsi bukti {index}");

        public static List<Evidence<string>> CreateManyEvidences(int count) =>
            Enumerable.Range(0, count).Select(i => CreateSampleEvidence(EvidenceType.Testimoni, i)).ToList();

        public static User CreateSampleAdminUser() =>
            new("Admin Satgas", "081111111111", UserRole.Admin, "admin-password");

        public static User CreateSamplePimpinanUser() =>
            new("Pimpinan Kampus", "082222222222", UserRole.Pimpinan, "pimpinan-password");

        public static List<User> CreateManyUsers(int count) =>
            Enumerable.Range(0, count)
                .Select(i => new User($"User {i}", $"08222222{i:0000}", UserRole.Pelapor, $"password-{i}"))
                .ToList();

        public static CreateReportRequest CreateCreateReportRequest() =>
            new()
            {
                ReporterName = "Pelapor Benchmark",
                ReporterNoHP = "089999999999",
                Title = "Laporan benchmark",
                Description = "Deskripsi laporan benchmark",
                ReportedPerson = "Terlapor Benchmark",
                Location = "Gedung Rektorat",
                IncidentDate = new DateTime(2026, 1, 15)
            };

        public static AddEvidenceRequest CreateAddEvidenceRequest(string pin) =>
            new()
            {
                Pin = pin,
                Type = EvidenceType.Testimoni,
                Content = "Isi bukti benchmark yang valid",
                Description = "Deskripsi bukti benchmark"
            };

        public static ReportService CreateSeededReportService(
            int count,
            out string filePath,
            Func<int, ReportStatus>? statusSelector = null)
        {
            filePath = CreateTempJsonPath("reports.json");
            var reports = Enumerable.Range(0, count)
                .Select(i => CreateSampleReportWithStatus(statusSelector?.Invoke(i) ?? ReportStatus.Diterima, i))
                .ToDictionary(report => report.ReportId);

            File.WriteAllText(filePath, JsonSerializer.Serialize(reports, new JsonSerializerOptions { WriteIndented = true }));
            return new ReportService(filePath);
        }

        public static UserService CreateSeededUserService(int count, out string filePath)
        {
            filePath = CreateTempJsonPath("users.json");
            var users = new List<User>
            {
                CreateSampleAdminUser(),
                CreateSamplePimpinanUser()
            };
            users.AddRange(CreateManyUsers(count));

            var userDictionary = users.ToDictionary(user => user.NoHP);
            File.WriteAllText(filePath, JsonSerializer.Serialize(userDictionary, new JsonSerializerOptions { WriteIndented = true }));
            return new UserService(filePath);
        }

        public static string CreateTempJsonPath(string fileName)
        {
            var directory = Path.Combine(Path.GetTempPath(), "LaporU.PerformanceTests", Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(directory);
            return Path.Combine(directory, fileName);
        }

        public static void DeleteTempDirectoryForFile(string? filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return;
            }

            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrWhiteSpace(directory) && Directory.Exists(directory))
            {
                Directory.Delete(directory, recursive: true);
            }
        }
    }
}
