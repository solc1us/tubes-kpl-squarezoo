using LaporU.Tests.Helpers;
using tubes_kpl_squarezoo.Enums;
using tubes_kpl_squarezoo.Services;

namespace LaporU.Tests.Services
{
    public class ReportServiceTests : IDisposable
    {
        private readonly string _filePath;
        private readonly ReportService _service;

        public ReportServiceTests()
        {
            _filePath = TestDataFactory.CreateTempJsonPath("reports.json");
            _service = new ReportService(_filePath);
        }

        public void Dispose()
        {
            TestDataFactory.DeleteTempDirectoryForFile(_filePath);
        }

        [Fact]
        public void CreateReport_WithValidRequest_ShouldCreateReport()
        {
            var report = TestDataFactory.CreateSampleReport();

            var created = _service.CreateReport(report);

            Assert.Same(report, created);
            Assert.Same(report, _service.GetById(report.ReportId));
        }

        [Fact]
        public void CreateReport_WithValidRequest_ShouldGenerateReportId()
        {
            var report = TestDataFactory.CreateSampleReport();

            var created = _service.CreateReport(report);

            Assert.NotEqual(Guid.Empty, created.ReportId);
        }

        [Fact]
        public void CreateReport_WithValidRequest_ShouldGenerateTrackingPin()
        {
            var report = TestDataFactory.CreateSampleReport();

            var created = _service.CreateReport(report);

            Assert.False(string.IsNullOrWhiteSpace(created.TrackingPin));
        }

        [Fact]
        public void CreateReport_WithValidRequest_ShouldSetStatusToDiterima()
        {
            var report = TestDataFactory.CreateSampleReport();

            var created = _service.CreateReport(report);

            Assert.Equal(ReportStatus.Diterima, created.Status);
        }

        [Fact]
        public void GetAllReports_WithExistingReports_ShouldReturnReports()
        {
            var first = _service.CreateReport(TestDataFactory.CreateSampleReport());
            var second = _service.CreateReport(TestDataFactory.CreateSampleReport());

            var reports = _service.GetAllReports();

            Assert.Contains(first, reports);
            Assert.Contains(second, reports);
        }

        [Fact]
        public void GetReportsByStatus_WithDiterimaStatus_ShouldReturnOnlyDiterimaReports()
        {
            var diterima = _service.CreateReport(TestDataFactory.CreateSampleReport());
            var diproses = _service.CreateReport(TestDataFactory.CreateSampleReport());
            diproses.Status = ReportStatus.Diproses;

            var reports = _service.GetReportsByStatus(ReportStatus.Diterima);

            Assert.Single(reports);
            Assert.Equal(diterima.ReportId, reports[0].ReportId);
        }

        [Fact]
        public void GetReports_WithTitleSearch_ShouldReturnMatchingReports()
        {
            var matching = TestDataFactory.CreateSampleReport();
            matching.Title = "Kekerasan di kampus";
            var other = TestDataFactory.CreateSampleReport();
            other.Title = "Kehilangan barang";
            _service.CreateReport(matching);
            _service.CreateReport(other);

            var result = _service.GetReports(null, "Kekerasan", 1, 10);

            Assert.Single(result.Items);
            Assert.Equal(matching.ReportId, result.Items[0].ReportId);
        }

        [Fact]
        public void GetReports_WithTitleSearch_ShouldBeCaseInsensitive()
        {
            var report = TestDataFactory.CreateSampleReport();
            report.Title = "Kekerasan di kampus";
            _service.CreateReport(report);

            var result = _service.GetReports(null, "kEkErAsAn", 1, 10);

            Assert.Single(result.Items);
            Assert.Equal(report.ReportId, result.Items[0].ReportId);
        }

        [Fact]
        public void GetReports_WithPagination_ShouldReturnCorrectPage()
        {
            var first = TestDataFactory.CreateSampleReport();
            first.Title = "Laporan 1";
            var second = TestDataFactory.CreateSampleReport();
            second.Title = "Laporan 2";
            var third = TestDataFactory.CreateSampleReport();
            third.Title = "Laporan 3";
            _service.CreateReport(first);
            _service.CreateReport(second);
            _service.CreateReport(third);

            var result = _service.GetReports(null, null, 2, 2);

            Assert.Single(result.Items);
            Assert.Equal(third.ReportId, result.Items[0].ReportId);
        }

        [Fact]
        public void GetReports_WithPagination_ShouldReturnCorrectMetadata()
        {
            _service.CreateReport(TestDataFactory.CreateSampleReport());
            _service.CreateReport(TestDataFactory.CreateSampleReport());
            _service.CreateReport(TestDataFactory.CreateSampleReport());

            var result = _service.GetReports(null, null, 2, 2);

            Assert.Equal(2, result.Page);
            Assert.Equal(2, result.PageSize);
            Assert.Equal(3, result.TotalItems);
            Assert.Equal(2, result.TotalPages);
            Assert.True(result.HasPreviousPage);
            Assert.False(result.HasNextPage);
        }

        [Fact]
        public void GetReports_WithInvalidPage_ShouldNormalizePage()
        {
            _service.CreateReport(TestDataFactory.CreateSampleReport());

            var result = _service.GetReports(null, null, 0, 10);

            Assert.Equal(1, result.Page);
        }

        [Theory]
        [InlineData(0, 10)]
        [InlineData(100, 50)]
        public void GetReports_WithInvalidPageSize_ShouldNormalizeOrCapPageSize(int pageSize, int expected)
        {
            _service.CreateReport(TestDataFactory.CreateSampleReport());

            var result = _service.GetReports(null, null, 1, pageSize);

            Assert.Equal(expected, result.PageSize);
        }

        [Fact]
        public void TrackReport_WithValidReportIdAndPin_ShouldReturnReport()
        {
            var report = _service.CreateReport(TestDataFactory.CreateSampleReport());

            var tracked = _service.TrackReport(report.ReportId, report.TrackingPin);

            Assert.NotNull(tracked);
        }

        [Fact]
        public void TrackReport_WithInvalidPin_ShouldFail()
        {
            var report = _service.CreateReport(TestDataFactory.CreateSampleReport());

            var tracked = _service.TrackReport(report.ReportId, "wrong-pin");

            Assert.Null(tracked);
        }

        [Fact]
        public void TrackReport_WithInvalidReportId_ShouldFail()
        {
            var tracked = _service.TrackReport(Guid.NewGuid(), "123456");

            Assert.Null(tracked);
        }

        [Fact]
        public void AddEvidence_WithValidReportIdAndPin_ShouldAddEvidence()
        {
            var report = _service.CreateReport(TestDataFactory.CreateSampleReport());

            var evidence = _service.AddEvidenceToReport(
                report.ReportId,
                report.TrackingPin,
                EvidenceType.Testimoni,
                "Kesaksian valid untuk laporan",
                "Keterangan bukti");

            Assert.NotNull(evidence);
            Assert.Single(report.Evidences);
            Assert.Same(evidence, report.Evidences[0]);
        }

        [Fact]
        public void AddEvidence_WithInvalidPin_ShouldRejectEvidence()
        {
            var report = _service.CreateReport(TestDataFactory.CreateSampleReport());

            var evidence = _service.AddEvidenceToReport(
                report.ReportId,
                "wrong-pin",
                EvidenceType.Testimoni,
                "Kesaksian valid untuk laporan",
                "Keterangan bukti");

            Assert.Null(evidence);
            Assert.Empty(report.Evidences);
        }

        [Fact]
        public void AddEvidence_WithInvalidReportId_ShouldFail()
        {
            var evidence = _service.AddEvidenceToReport(
                Guid.NewGuid(),
                "123456",
                EvidenceType.Testimoni,
                "Kesaksian valid untuk laporan",
                "Keterangan bukti");

            Assert.Null(evidence);
        }

        [Fact]
        public void AddEvidence_ShouldStoreCorrectEvidenceType()
        {
            var report = _service.CreateReport(TestDataFactory.CreateSampleReport());

            var evidence = _service.AddEvidenceToReport(
                report.ReportId,
                report.TrackingPin,
                EvidenceType.CatatanPendukung,
                "Catatan pendukung laporan",
                "Keterangan bukti");

            Assert.NotNull(evidence);
            Assert.Equal(EvidenceType.CatatanPendukung, evidence.Type);
            Assert.Equal("Catatan pendukung laporan", evidence.Content);
            Assert.Equal("Keterangan bukti", evidence.Description);
        }

        [Theory]
        [InlineData(ReportStatus.Diterima, ReportStatus.Diproses)]
        [InlineData(ReportStatus.Diterima, ReportStatus.Ditolak)]
        [InlineData(ReportStatus.Diproses, ReportStatus.Selesai)]
        [InlineData(ReportStatus.Diproses, ReportStatus.Ditolak)]
        public void TransitionStatus_WithValidTransition_ShouldSucceed(ReportStatus currentStatus, ReportStatus nextStatus)
        {
            var report = _service.CreateReport(TestDataFactory.CreateSampleReport());
            report.Status = currentStatus;

            var success = _service.ExecuteTransition(report.ReportId, nextStatus);

            Assert.True(success);
            Assert.Equal(nextStatus, report.Status);
        }

        [Theory]
        [InlineData(ReportStatus.Diterima, ReportStatus.Selesai)]
        [InlineData(ReportStatus.Selesai, ReportStatus.Diproses)]
        [InlineData(ReportStatus.Selesai, ReportStatus.Ditolak)]
        [InlineData(ReportStatus.Ditolak, ReportStatus.Diproses)]
        [InlineData(ReportStatus.Ditolak, ReportStatus.Selesai)]
        public void TransitionStatus_WithInvalidTransition_ShouldFail(ReportStatus currentStatus, ReportStatus nextStatus)
        {
            var report = _service.CreateReport(TestDataFactory.CreateSampleReport());
            report.Status = currentStatus;

            var success = _service.ExecuteTransition(report.ReportId, nextStatus);

            Assert.False(success);
            Assert.Equal(currentStatus, report.Status);
        }

        [Fact]
        public void CloseReport_WithValidReport_ShouldSetStatusToSelesai()
        {
            var report = _service.CreateReport(TestDataFactory.CreateSampleReport());

            var closed = _service.CloseReport(report.ReportId);

            Assert.NotNull(closed);
            Assert.Equal(ReportStatus.Selesai, closed.Status);
        }
    }
}
