using System;
using System.Collections.Generic;
using System.IO;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using tubes_kpl_squarezoo.Models;
using tubes_kpl_squarezoo.Services;

namespace ReportingSystem.Performance
{
    // [MemoryDiagnoser] wajib buat liat seberapa boros RAM pas serialisasi JSON
    [MemoryDiagnoser]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class ReportServiceBenchmark
    {
        private ReportService _service;
        private string _tempFilePath;
        private User _testUser;
        private List<Guid> _existingIds;

        // Params buat ngetes scaling: 10 (kecil), 100 (sedang), 1000 (mulai berat)
        [Params(10, 100, 1000)]
        public int N;

        [GlobalSetup]
        public void Setup()
        {
            _tempFilePath = Path.Combine(Path.GetTempPath(), $"benchmark_reports_{Guid.NewGuid()}.json");
            _service = new ReportService(_tempFilePath);
            _testUser = new User("BenchmarkUser", "08123456789");
            _existingIds = new List<Guid>();

            // Pre-populate data sesuai N buat ngetes Read/Update/Delete
            for (int i = 0; i < N; i++)
            {
                var r = new Report($"Initial Title {i}", $"Desc {i}", _testUser);
                _service.CreateReport(r);
                _existingIds.Add(r.ReportId);
            }
        }

        [Benchmark]
        public void Benchmark_CreateReport()
        {
            // Ngetes overhead instansiasi + Dictionary Add + SaveToFile (Full Write)
            var report = new Report("New Report", "New Description", _testUser);
            _service.CreateReport(report);
        }

        [Benchmark]
        public void Benchmark_GetById()
        {
            // O(1) di memori, harusnya paling kenceng karena gak ada Disk I/O
            if (_existingIds.Count > 0)
            {
                var targetId = _existingIds[0];
                _service.GetById(targetId);
            }
        }

        [Benchmark]
        public void Benchmark_UpdateReport()
        {
            // Update atribut + SaveToFile (Full Rewrite)
            if (_existingIds.Count > 0)
            {
                var targetId = _existingIds[0];
                _service.UpdateReport(targetId, "Updated Title", "Updated Description");
            }
        }

        [Benchmark]
        public void Benchmark_GetAll()
        {
            // Ngetes cost buat convert Dictionary values ke List
            _service.GetAllReports();
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            // Hapus file sampah biar gak menuhin Temp folder
            if (File.Exists(_tempFilePath))
            {
                File.Delete(_tempFilePath);
            }
        }
    }
}