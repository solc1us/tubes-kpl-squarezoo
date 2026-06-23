using BenchmarkDotNet.Running;
using LaporU.PerformanceTests.Helpers;
using LaporU.PerformanceTests.Models;

var benchmarkArgs = args.Length == 0 ? new[] { "--filter", "*" } : args;

Console.WriteLine("Lapor-U BenchmarkDotNet Performance Tests");
Console.WriteLine("Running all benchmark classes...");
Console.WriteLine();

var summaries = BenchmarkSwitcher
    .FromAssembly(typeof(ReportModelBenchmarks).Assembly)
    .Run(benchmarkArgs)
    .ToArray();

CombinedBenchmarkSummaryPrinter.Print(summaries);

Console.WriteLine();
Console.WriteLine("Full BenchmarkDotNet reports are available in:");
Console.WriteLine("BenchmarkDotNet.Artifacts/results/");
