using BenchmarkDotNet.Attributes;
using LaporU.PerformanceTests.Helpers;
using tubes_kpl_squarezoo.Enums;
using tubes_kpl_squarezoo.Models;
using tubes_kpl_squarezoo.Services;

namespace LaporU.PerformanceTests.Services
{
    [MemoryDiagnoser]
    [ShortRunJob]
    public class ReportServiceBenchmarks
    {
        private ReportService _reportService = null!;
        private List<Report> _reports = [];
        private string? _filePath;
        private int _addEvidenceIndex;
        private int _transitionIndex;
        private int _closeIndex;

        [Params(10, 100, 1000)]
        public int ReportCount { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            _reportService = BenchmarkTestDataFactory.CreateSeededReportService(
                ReportCount,
                out _filePath,
                i => i % 2 == 0 ? ReportStatus.Diterima : ReportStatus.Diproses);
            _reports = _reportService.GetAllReports();
            _addEvidenceIndex = 0;
            _transitionIndex = 0;
            _closeIndex = 0;
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            BenchmarkTestDataFactory.DeleteTempDirectoryForFile(_filePath);
        }

        [Benchmark]
        public List<Report> GetAllReports() =>
            _reportService.GetAllReports();

        [Benchmark]
        public List<Report> GetReportsByStatus() =>
            _reportService.GetReportsByStatus(ReportStatus.Diterima);

        [Benchmark]
        public object SearchReportsByTitle() =>
            _reportService.GetReports(null, "Laporan 9", 1, 10);

        [Benchmark]
        public object GetReportsWithPagination() =>
            _reportService.GetReports(null, null, 2, 10);

        [Benchmark]
        public object? TrackReport()
        {
            var report = _reports[_reports.Count / 2];

            return _reportService.TrackReport(report.ReportId, report.TrackingPin);
        }

        [Benchmark]
        public Evidence<string>? AddEvidence()
        {
            var report = _reports[_addEvidenceIndex++ % _reports.Count];

            return _reportService.AddEvidenceToReport(
                report.ReportId,
                report.TrackingPin,
                EvidenceType.Testimoni,
                "Isi bukti benchmark yang valid",
                "Deskripsi bukti benchmark");
        }

        [Benchmark]
        public bool TransitionStatus()
        {
            var report = _reports[_transitionIndex++ % _reports.Count];
            report.Status = ReportStatus.Diterima;

            return _reportService.ExecuteTransition(report.ReportId, ReportStatus.Diproses);
        }

        [Benchmark]
        public Report? CloseReport()
        {
            var report = _reports[_closeIndex++ % _reports.Count];

            return _reportService.CloseReport(report.ReportId);
        }

        [Benchmark]
        public object GetSummary() =>
            _reportService.GetSummary();
    }
}
