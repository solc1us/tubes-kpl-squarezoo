using tubes_kpl_squarezoo.Enums;
using tubes_kpl_squarezoo.Models;
using tubes_kpl_squarezoo.Models.DTOs;

namespace LaporU.Tests.Helpers
{
    public static class TestDataFactory
    {
        public static Report CreateSampleReport() =>
            new(
                "Budi Santoso",
                "081234567890",
                "Pelanggaran akademik",
                "Deskripsi laporan pelanggaran akademik",
                "Terlapor A",
                "Gedung Rektorat",
                new DateTime(2026, 1, 15));

        public static Report CreateSampleReportWithStatus(ReportStatus status)
        {
            var report = CreateSampleReport();
            report.Status = status;
            return report;
        }

        public static Evidence<string> CreateSampleEvidence(
            EvidenceType type = EvidenceType.Testimoni,
            string content = "Kesaksian valid untuk laporan",
            string description = "Keterangan bukti") =>
            new(type, content, description);

        public static User CreateSampleAdminUser() =>
            new("Admin Satgas", "081111111111", UserRole.Admin, "admin-password");

        public static User CreateSamplePimpinanUser() =>
            new("Pimpinan Kampus", "082222222222", UserRole.Pimpinan, "pimpinan-password");

        public static CreateReportRequest CreateCreateReportRequest() =>
            new()
            {
                ReporterName = "Budi Santoso",
                ReporterNoHP = "081234567890",
                Title = "Pelanggaran akademik",
                Description = "Deskripsi laporan pelanggaran akademik",
                ReportedPerson = "Terlapor A",
                Location = "Gedung Rektorat",
                IncidentDate = new DateTime(2026, 1, 15)
            };

        public static AddEvidenceRequest CreateAddEvidenceRequest(string pin) =>
            new()
            {
                Pin = pin,
                Type = EvidenceType.Testimoni,
                Content = "Kesaksian valid untuk laporan",
                Description = "Keterangan bukti"
            };

        public static string CreateTempJsonPath(string fileName = "data.json")
        {
            var directory = Path.Combine(Path.GetTempPath(), "LaporU.Tests", Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(directory);
            return Path.Combine(directory, fileName);
        }

        public static void DeleteTempDirectoryForFile(string filePath)
        {
            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrWhiteSpace(directory) && Directory.Exists(directory))
            {
                Directory.Delete(directory, recursive: true);
            }
        }
    }
}
