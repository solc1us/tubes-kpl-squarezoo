using BenchmarkDotNet.Running;

namespace UserPerformence
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<UserPerformence>();
        }
    }
}