using LaporU.Tests.Helpers;
using Microsoft.AspNetCore.Mvc;
using tubes_kpl_squarezoo.Controllers;
using tubes_kpl_squarezoo.Enums;
using tubes_kpl_squarezoo.Models.DTOs;
using tubes_kpl_squarezoo.Services;

namespace LaporU.Tests.Controllers
{
    public class ReportControllerTests : IDisposable
    {
        private readonly string _filePath;
        private readonly ReportService _service;
        private readonly ReportController _controller;

        public ReportControllerTests()
        {
            _filePath = TestDataFactory.CreateTempJsonPath("reports.json");
            _service = new ReportService(_filePath);
            _controller = new ReportController(_service);
        }

        public void Dispose()
        {
            TestDataFactory.DeleteTempDirectoryForFile(_filePath);
        }

        [Fact]
        public void GetAll_ShouldReturnOkResult()
        {
            _service.CreateReport(TestDataFactory.CreateSampleReport());

            var result = _controller.GetAll(null, null);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<PaginatedResponse<ReportResponse>>(ok.Value);
        }

        [Fact]
        public void GetAll_WithStatusFilter_ShouldReturnOkResult()
        {
            _service.CreateReport(TestDataFactory.CreateSampleReport());

            var result = _controller.GetAll(ReportStatus.Diterima, null);

            var ok = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<PaginatedResponse<ReportResponse>>(ok.Value);
            Assert.All(response.Items, item => Assert.Equal(ReportStatus.Diterima, item.Status));
        }

        [Fact]
        public void GetAll_WithTitleSearch_ShouldReturnPaginatedResult()
        {
            var matching = TestDataFactory.CreateSampleReport();
            matching.Title = "Kekerasan di kampus";
            _service.CreateReport(matching);

            var result = _controller.GetAll(null, "kekerasan");

            var ok = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<PaginatedResponse<ReportResponse>>(ok.Value);
            Assert.Single(response.Items);
            Assert.Equal(matching.ReportId, response.Items[0].ReportId);
        }

        [Fact]
        public void CreateReport_WithValidRequest_ShouldReturnSuccess()
        {
            var request = TestDataFactory.CreateCreateReportRequest();

            var result = _controller.Create(request);

            var created = Assert.IsType<CreatedAtActionResult>(result);
            var response = Assert.IsType<CreateReportResponse>(created.Value);
            Assert.NotEqual(Guid.Empty, response.ReportId);
            Assert.False(string.IsNullOrWhiteSpace(response.TrackingPin));
        }

        [Fact]
        public void AddEvidence_WithValidPin_ShouldReturnSuccess()
        {
            var report = _service.CreateReport(TestDataFactory.CreateSampleReport());
            var request = TestDataFactory.CreateAddEvidenceRequest(report.TrackingPin);

            var result = _controller.AddEvidence(report.ReportId, request);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(ok.Value);
            Assert.Single(report.Evidences);
        }

        [Fact]
        public void AddEvidence_WithInvalidPin_ShouldReturnFailure()
        {
            var report = _service.CreateReport(TestDataFactory.CreateSampleReport());
            var request = TestDataFactory.CreateAddEvidenceRequest("wrong-pin");

            var result = _controller.AddEvidence(report.ReportId, request);

            Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Empty(report.Evidences);
        }

        [Fact]
        public void TransitionStatus_WithInvalidTransition_ShouldReturnConflictOrExpectedError()
        {
            var report = _service.CreateReport(TestDataFactory.CreateSampleReport());
            var request = new TransitionReportStatusRequest { Status = ReportStatus.Selesai };

            var result = _controller.Transition(report.ReportId, request);

            Assert.IsType<ConflictObjectResult>(result);
        }
    }
}
