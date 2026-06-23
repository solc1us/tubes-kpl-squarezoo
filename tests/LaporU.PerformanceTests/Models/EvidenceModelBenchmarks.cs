using BenchmarkDotNet.Attributes;
using LaporU.PerformanceTests.Helpers;
using tubes_kpl_squarezoo.Enums;
using tubes_kpl_squarezoo.Models;

namespace LaporU.PerformanceTests.Models
{
    [MemoryDiagnoser]
    [ShortRunJob]
    public class EvidenceModelBenchmarks
    {
        [Benchmark]
        public Evidence<string> CreateEvidenceObject() =>
            BenchmarkTestDataFactory.CreateSampleEvidence();

        [Benchmark]
        public EvidenceType AssignEvidenceType()
        {
            var evidence = new Evidence<string>(
                EvidenceType.KronologiTambahan,
                "Kronologi tambahan benchmark",
                "Deskripsi benchmark");

            return evidence.Type;
        }

        [Benchmark]
        public List<Evidence<string>> CreateManyEvidences() =>
            BenchmarkTestDataFactory.CreateManyEvidences(100);
    }
}
