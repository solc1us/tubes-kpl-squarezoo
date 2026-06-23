using LaporU.Tests.Helpers;
using tubes_kpl_squarezoo.Enums;
using tubes_kpl_squarezoo.Models;

namespace LaporU.Tests.Models
{
    public class ReportTests
    {
        [Fact]
        public void Report_WithValidData_ShouldStoreReportProperties()
        {
            var incidentDate = new DateTime(2026, 1, 15);

            var report = new Report(
                "Budi Santoso",
                "081234567890",
                "Pelanggaran akademik",
                "Deskripsi laporan",
                "Terlapor A",
                "Gedung Rektorat",
                incidentDate);

            Assert.Equal("Budi Santoso", report.ReporterName);
            Assert.Equal("081234567890", report.ReporterNoHP);
            Assert.Equal("Pelanggaran akademik", report.Title);
            Assert.Equal("Deskripsi laporan", report.Description);
            Assert.Equal("Terlapor A", report.ReportedPerson);
            Assert.Equal("Gedung Rektorat", report.Location);
            Assert.Equal(incidentDate, report.IncidentDate);
        }

        [Fact]
        public void Report_WhenCreated_ShouldContainReportId()
        {
            var report = TestDataFactory.CreateSampleReport();

            Assert.NotEqual(Guid.Empty, report.ReportId);
        }

        [Fact]
        public void Report_WhenCreated_ShouldContainTrackingPin()
        {
            var report = TestDataFactory.CreateSampleReport();

            Assert.False(string.IsNullOrWhiteSpace(report.TrackingPin));
        }

        [Fact]
        public void Report_WhenCreated_ShouldHaveDefaultStatusDiterima()
        {
            var report = TestDataFactory.CreateSampleReport();

            Assert.Equal(ReportStatus.Diterima, report.Status);
        }

        [Fact]
        public void Report_WhenCreated_ShouldInitializeEvidenceList()
        {
            var report = TestDataFactory.CreateSampleReport();

            Assert.NotNull(report.Evidences);
            Assert.Empty(report.Evidences);
        }

        [Fact]
        public void Report_WithEvidence_ShouldStoreEvidenceInEvidenceList()
        {
            var report = TestDataFactory.CreateSampleReport();
            var evidence = TestDataFactory.CreateSampleEvidence();

            report.AddEvidence(evidence);

            Assert.Single(report.Evidences);
            Assert.Same(evidence, report.Evidences[0]);
        }

        [Fact]
        public void Report_StatusCanBeUpdated()
        {
            var report = TestDataFactory.CreateSampleReport();

            var updated = report.TransitionTo(ReportStatus.Diproses);

            Assert.True(updated);
            Assert.Equal(ReportStatus.Diproses, report.Status);
        }
    }
}
