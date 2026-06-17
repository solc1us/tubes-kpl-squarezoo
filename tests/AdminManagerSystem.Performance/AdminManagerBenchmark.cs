using System;
using System.Collections.Generic;
using System.IO;
using BenchmarkDotNet.Attributes;
using tubes_kpl_squarezoo;
using tubes_kpl_squarezoo.Enums;
using tubes_kpl_squarezoo.Models;
using tubes_kpl_squarezoo.Services;

namespace AdminManagerSystem.Performance
{
    // Menampilkan penggunaan memory saat benchmark dijalankan
    [MemoryDiagnoser]
    public class AdminManagerBenchmark
    {
        private ReportService _service;
        private AdminManager _admin;
        private string _filePath;

        // Setup data sebelum benchmark dijalankan
        [GlobalSetup]
        public void Setup()
        {
            // Membuat file JSON sementara untuk benchmark
            _filePath = Path.Combine(
                Path.GetTempPath(),
                $"admin_manager_benchmark_{Guid.NewGuid()}.json"
            );

            // Setup service dan admin manager
            _service = new ReportService(_filePath);
            _admin = new AdminManager(_service);

            // Membuat dummy user
            var user = new User("Budi", "Visitor");

            // Menambahkan 1000 dummy report
            for (int i = 1; i <= 1000; i++)
            {
                var report = new Report(
                    $"Report {i}",
                    $"Description {i}",
                    user
                );

                _service.CreateReport(report);

                // Mengubah sebagian report menjadi Submitted
                if (i % 2 == 0)
                {
                    report.TransitionTo(ReportStatus.Submitted);
                }
            }

            // Menyimpan data benchmark ke file JSON
            _service.SaveToFile();
        }

        // Benchmark untuk method GetAllReports()
        [Benchmark]
        public List<Report> GetAllReports_Benchmark()
        {
            return _admin.GetAllReports();
        }

        // Benchmark untuk method GetByStatus()
        [Benchmark]
        public List<Report> GetByStatus_Benchmark()
        {
            return _admin.GetByStatus(ReportStatus.Submitted);
        }

        // Benchmark untuk method GetSummary()
        [Benchmark]
        public Dictionary<ReportStatus, int> GetSummary_Benchmark()
        {
            return _admin.GetSummary();
        }

        // Benchmark untuk method CloseReport()
        [Benchmark]
        public bool CloseReport_Benchmark()
        {
            var user = new User("Budi", "Visitor");

            var report = new Report(
                "Benchmark Report",
                "Benchmark Description",
                user
            );

            _service.CreateReport(report);

            report.TransitionTo(ReportStatus.Submitted);
            report.TransitionTo(ReportStatus.UnderReview);
            report.TransitionTo(ReportStatus.Resolved);

            return _admin.CloseReport(report.ReportId);
        }

        // Cleanup file benchmark setelah selesai dijalankan
        [GlobalCleanup]
        public void Cleanup()
        {
            if (File.Exists(_filePath))
            {
                File.Delete(_filePath);
            }
        }
    }
}