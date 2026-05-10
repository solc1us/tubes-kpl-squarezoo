using BenchmarkDotNet.Attributes;
using tubes_kpl_squarezoo.Models;
using tubes_kpl_squarezoo.Enums;
using ReportClass = tubes_kpl_squarezoo.Models.Report;

namespace Report.UnitTest.PerformanceTest
{
    [MemoryDiagnoser]
    public class ReportPerformance
    {
        [Params(1000, 10000, 50000)]
        public int JumlahData;

        private User CreateUser()
        {
            return new User("Budi", "Pelapor");
        }

        [Benchmark]
        public void CreateManyReports()
        {
            for (int i = 0; i < JumlahData; i++)
            {
                ReportClass report = new ReportClass(
                    "Judul Laporan " + i,
                    "Deskripsi laporan " + i,
                    CreateUser()
                );
            }
        }

        [Benchmark]
        public void TransitionStatusManyTimes()
        {
            for (int i = 0; i < JumlahData; i++)
            {
                ReportClass report = new ReportClass(
                    "Judul Laporan " + i,
                    "Deskripsi laporan " + i,
                    CreateUser()
                );

                report.TransitionTo(ReportStatus.Submitted);
                report.TransitionTo(ReportStatus.UnderReview);
                report.TransitionTo(ReportStatus.Resolved);
                report.TransitionTo(ReportStatus.Closed);
            }
        }

        [Benchmark]
        public void AddManyEvidences()
        {
            ReportClass report = new ReportClass(
                "Judul Laporan",
                "Deskripsi laporan",
                CreateUser()
            );

            for (int i = 0; i < JumlahData; i++)
            {
                Evidence<string> evidence = new Evidence<string>(
                    EvidenceType.Document,
                    "Isi bukti ke-" + i,
                    "Deskripsi bukti"
                );

                report.AddEvidence(evidence);
            }
        }
    }
}