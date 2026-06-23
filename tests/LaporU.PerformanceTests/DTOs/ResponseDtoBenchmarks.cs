using BenchmarkDotNet.Attributes;
using LaporU.PerformanceTests.Helpers;
using tubes_kpl_squarezoo.Models;
using tubes_kpl_squarezoo.Models.DTOs;

namespace LaporU.PerformanceTests.DTOs
{
    [MemoryDiagnoser]
    [ShortRunJob]
    public class ResponseDtoBenchmarks
    {
        private Report _report = null!;
        private Report _reportWithEvidences = null!;
        private List<Report> _reports = [];
        private List<ReportResponse> _reportResponses = [];
        private User _user = null!;

        [GlobalSetup]
        public void Setup()
        {
            _report = BenchmarkTestDataFactory.CreateSampleReport();
            _reportWithEvidences = BenchmarkTestDataFactory.CreateSampleReport();
            foreach (var evidence in BenchmarkTestDataFactory.CreateManyEvidences(25))
            {
                _reportWithEvidences.AddEvidence(evidence);
            }

            _reports = BenchmarkTestDataFactory.CreateManyReports(1000);
            _reportResponses = _reports.Take(10).Select(ReportResponse.FromReport).ToList();
            _user = BenchmarkTestDataFactory.CreateSampleAdminUser();
        }

        [Benchmark]
        public ReportResponse MapSingleReport() =>
            ReportResponse.FromReport(_report);

        [Benchmark]
        public ReportResponse MapReportWithEvidences() =>
            ReportResponse.FromReport(_reportWithEvidences);

        [Benchmark]
        public List<ReportResponse> MapManyReports() =>
            _reports.Select(ReportResponse.FromReport).ToList();

        [Benchmark]
        public PaginatedResponse<ReportResponse> CreatePaginatedResponse() =>
            new()
            {
                Items = _reportResponses,
                Page = 1,
                PageSize = 10,
                TotalItems = 1000,
                TotalPages = 100,
                HasPreviousPage = false,
                HasNextPage = true
            };

        [Benchmark]
        public UserResponse MapUserResponse() =>
            UserResponse.FromUser(_user);
    }
}
