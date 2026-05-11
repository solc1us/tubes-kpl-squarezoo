using BenchmarkDotNet.Running;

namespace tubes_kpl_squarezoo.Tests
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<PerformancEvidence>();
        }
    }
}