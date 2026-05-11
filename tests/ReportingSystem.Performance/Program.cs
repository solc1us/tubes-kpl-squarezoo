using BenchmarkDotNet.Running;
using ReportingSystem.Performance;

// Ini instruksi buat jalanin benchmark-nya
var summary = BenchmarkRunner.Run<ReportServiceBenchmark>();