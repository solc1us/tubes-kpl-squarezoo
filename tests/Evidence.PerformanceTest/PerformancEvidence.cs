using BenchmarkDotNet.Attributes;
using tubes_kpl_squarezoo;

namespace tubes_kpl_squarezoo.Tests
{
    public class PerformancEvidence
    {
        private readonly Evidences<string> evidence;

        public PerformancEvidence()
        {
            evidence = new Evidences<string>
            {
                Type = EvidenceType.Testimony,
                Content = "Ini adalah contoh kesaksian valid untuk benchmark testing"
            };
        }

        [Benchmark]
        public bool ValidateBenchmark()
        {
            return evidence.Validate();
        }

        [Benchmark]
        public string GetSummaryBenchmark()
        {
            return evidence.GetSummary();
        }
    }
}