using BenchmarkDotNet.Running;
using Report.UnitTest.PerformanceTest;

var summary = BenchmarkRunner.Run<ReportPerformance>();
