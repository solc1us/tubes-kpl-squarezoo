using BenchmarkDotNet.Attributes;
using LaporU.PerformanceTests.Helpers;
using tubes_kpl_squarezoo.Enums;
using tubes_kpl_squarezoo.Models;

namespace LaporU.PerformanceTests.Models
{
    [MemoryDiagnoser]
    [ShortRunJob]
    public class ReportModelBenchmarks
    {
        [Benchmark]
        public Report CreateReportObject() =>
            BenchmarkTestDataFactory.CreateSampleReport();

        [Benchmark]
        public void AddEvidenceToReport()
        {
            var report = BenchmarkTestDataFactory.CreateSampleReport();
            var evidence = BenchmarkTestDataFactory.CreateSampleEvidence();

            report.AddEvidence(evidence);
        }

        [Benchmark]
        public bool UpdateReportStatus()
        {
            var report = BenchmarkTestDataFactory.CreateSampleReport();

            return report.TransitionTo(ReportStatus.Diproses);
        }

        [Benchmark]
        public List<Report> CreateManyReports() =>
            BenchmarkTestDataFactory.CreateManyReports(100);
    }
}
