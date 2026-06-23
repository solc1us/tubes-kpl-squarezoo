using LaporU.Tests.Helpers;
using tubes_kpl_squarezoo.Enums;
using tubes_kpl_squarezoo.Models.DTOs;

namespace LaporU.Tests.DTOs
{
    public class ResponseDtoTests
    {
        [Fact]
        public void ReportResponse_FromReport_ShouldMapReportFields()
        {
            var report = TestDataFactory.CreateSampleReport();

            var response = ReportResponse.FromReport(report);

            Assert.Equal(report.ReportId, response.ReportId);
            Assert.Equal(report.ReporterName, response.ReporterName);
            Assert.Equal(report.ReporterNoHP, response.ReporterNoHP);
            Assert.Equal(report.Title, response.Title);
            Assert.Equal(report.Description, response.Description);
            Assert.Equal(report.ReportedPerson, response.ReportedPerson);
            Assert.Equal(report.Location, response.Location);
            Assert.Equal(report.IncidentDate, response.IncidentDate);
        }

        [Fact]
        public void ReportResponse_FromReport_ShouldMapEvidenceList()
        {
            var report = TestDataFactory.CreateSampleReport();
            var evidence = TestDataFactory.CreateSampleEvidence();
            report.AddEvidence(evidence);

            var response = ReportResponse.FromReport(report);

            Assert.Single(response.Evidences);
            Assert.Same(evidence, response.Evidences.Single());
        }

        [Fact]
        public void ReportResponse_FromReport_ShouldMapStatus()
        {
            var report = TestDataFactory.CreateSampleReportWithStatus(ReportStatus.Diproses);

            var response = ReportResponse.FromReport(report);

            Assert.Equal(ReportStatus.Diproses, response.Status);
        }

        [Fact]
        public void UserResponse_FromUser_ShouldMapUserFields()
        {
            var user = TestDataFactory.CreateSampleAdminUser();

            var response = UserResponse.FromUser(user);

            Assert.Equal(user.UserId, response.UserId);
            Assert.Equal(user.Name, response.Name);
            Assert.Equal(user.NoHP, response.NoHP);
            Assert.Equal(user.Role, response.Role);
            Assert.Contains("ViewReports", response.Permissions);
        }

        [Fact]
        public void UserResponse_FromUser_ShouldNotExposePassword()
        {
            var user = TestDataFactory.CreateSampleAdminUser();

            var response = UserResponse.FromUser(user);

            Assert.Null(response.GetType().GetProperty("Password"));
        }

        [Fact]
        public void PaginatedResponse_WithItems_ShouldStorePaginationMetadata()
        {
            var report = ReportResponse.FromReport(TestDataFactory.CreateSampleReport());

            var response = new PaginatedResponse<ReportResponse>
            {
                Items = new[] { report },
                Page = 2,
                PageSize = 1,
                TotalItems = 3,
                TotalPages = 3,
                HasPreviousPage = true,
                HasNextPage = true
            };

            Assert.Single(response.Items);
            Assert.Equal(2, response.Page);
            Assert.Equal(1, response.PageSize);
            Assert.Equal(3, response.TotalItems);
            Assert.Equal(3, response.TotalPages);
            Assert.True(response.HasPreviousPage);
            Assert.True(response.HasNextPage);
        }
    }
}
