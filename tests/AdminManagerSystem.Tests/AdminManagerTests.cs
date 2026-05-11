using System;
using System.IO;
using Xunit;
using tubes_kpl_squarezoo;
using tubes_kpl_squarezoo.Enums;
using tubes_kpl_squarezoo.Models;
using tubes_kpl_squarezoo.Services;

namespace AdminManagerSystem.Tests
{
    public class AdminManagerTests
    {
        // Membuat service dengan file JSON khusus testing
        private ReportService CreateReportService()
        {
            string filePath = Path.Combine(
                Path.GetTempPath(),
                $"reports_test_{Guid.NewGuid()}.json"
            );

            return new ReportService(filePath);
        }

        // Membuat data report dummy
        private Report CreateDummyReport(string title)
        {
            var user = new User("Budi", "Visitor");

            return new Report(
                title,
                $"Description for {title}",
                user
            );
        }

        [Fact]
        public void GetAllReports_ShouldReturnAllReports()
        {
            // Arrange : Menyiapkan service, admin, dan data report
            var service = CreateReportService();
            var admin = new AdminManager(service);

            service.CreateReport(CreateDummyReport("Report 1"));
            service.CreateReport(CreateDummyReport("Report 2"));

            // Act : Memanggil method GetAllReports()
            var result = admin.GetAllReports();

            // Assert : Memastikan semua report berhasil diambil
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void GetByStatus_ShouldReturnSubmittedReports()
        {
            // Arrange : Menyiapkan report dengan status berbeda
            var service = CreateReportService();
            var admin = new AdminManager(service);

            var report1 = CreateDummyReport("Report 1");
            var report2 = CreateDummyReport("Report 2");

            service.CreateReport(report1);
            service.CreateReport(report2);

            report1.TransitionTo(ReportStatus.Submitted);

            // Act : Memanggil method GetByStatus()
            var result = admin.GetByStatus(ReportStatus.Submitted);

            // Assert : Memastikan hanya report Submitted yang dikembalikan
            Assert.Single(result);
            Assert.Equal(ReportStatus.Submitted, result[0].Status);
        }

        [Fact]
        public void GetSummary_ShouldReturnCorrectSummary()
        {
            // Arrange : Menyiapkan report Draft dan Submitted
            var service = CreateReportService();
            var admin = new AdminManager(service);

            var report1 = CreateDummyReport("Draft");
            var report2 = CreateDummyReport("Submitted");

            service.CreateReport(report1);
            service.CreateReport(report2);

            report2.TransitionTo(ReportStatus.Submitted);

            // Act : Memanggil method GetSummary()
            var result = admin.GetSummary();

            // Assert : Memastikan jumlah report per status sesuai
            Assert.Equal(1, result[ReportStatus.Draft]);
            Assert.Equal(1, result[ReportStatus.Submitted]);
        }

        [Fact]
        public void CloseReport_ShouldReturnTrue_WhenResolved()
        {
            // Arrange : Menyiapkan report dengan status Resolved
            var service = CreateReportService();
            var admin = new AdminManager(service);

            var report = CreateDummyReport("Resolved Report");

            service.CreateReport(report);

            report.TransitionTo(ReportStatus.Submitted);
            report.TransitionTo(ReportStatus.UnderReview);
            report.TransitionTo(ReportStatus.Resolved);

            // Act : Memanggil method CloseReport()
            bool result = admin.CloseReport(report.ReportId);

            // Assert : Memastikan report berhasil ditutup
            Assert.True(result);
            Assert.Equal(ReportStatus.Closed, report.Status);
        }

        [Fact]
        public void CloseReport_ShouldReturnFalse_WhenNotResolved()
        {
            // Arrange : Menyiapkan report yang belum Resolved
            var service = CreateReportService();
            var admin = new AdminManager(service);

            var report = CreateDummyReport("Submitted Report");

            service.CreateReport(report);

            report.TransitionTo(ReportStatus.Submitted);

            // Act : Memanggil method CloseReport()
            bool result = admin.CloseReport(report.ReportId);

            // Assert : Memastikan report gagal ditutup
            Assert.False(result);
            Assert.Equal(ReportStatus.Submitted, report.Status);
        }
    }
}