using System.Reflection;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace LaporU.PerformanceTests.Helpers
{
    public static class CombinedBenchmarkSummaryPrinter
    {
        private static readonly string[] Headers =
        [
            "No",
            "Class",
            "Method",
            "Mean",
            "Error",
            "StdDev",
            "Allocated"
        ];

        public static void Print(IReadOnlyList<Summary> summaries)
        {
            var rows = BuildRows(summaries);

            Console.WriteLine();
            Console.WriteLine("Lapor-U Combined Benchmark Summary");
            Console.WriteLine();

            if (rows.Count == 0)
            {
                Console.WriteLine("No benchmark results were available.");
                return;
            }

            var widths = CalculateWidths(rows);
            PrintRow(Headers, widths, numericColumns: new HashSet<int>());
            PrintSeparator(widths);

            var numericColumns = new HashSet<int> { 0 };
            foreach (var row in rows)
            {
                PrintRow(row, widths, numericColumns);
            }
        }

        private static List<string[]> BuildRows(IReadOnlyList<Summary> summaries)
        {
            var resultRows = new List<string[]>();
            var number = 1;

            foreach (var summary in summaries)
            {
                var artifactRows = BuildRowsFromArtifact(summary);
                if (artifactRows.Count > 0)
                {
                    foreach (var artifactRow in artifactRows)
                    {
                        resultRows.Add(
                        [
                            number.ToString(),
                            artifactRow[0],
                            artifactRow[1],
                            artifactRow[2],
                            artifactRow[3],
                            artifactRow[4],
                            artifactRow[5]
                        ]);
                        number++;
                    }

                    continue;
                }

                var columns = GetSummaryColumns(summary);
                foreach (var report in GetReports(summary))
                {
                    var benchmarkCase = GetBenchmarkCase(report);
                    if (benchmarkCase == null)
                    {
                        continue;
                    }

                    resultRows.Add(
                    [
                        number.ToString(),
                        GetClassName(benchmarkCase),
                        GetMethodName(benchmarkCase),
                        GetColumnValue(columns, summary, benchmarkCase, report, "Mean"),
                        GetColumnValue(columns, summary, benchmarkCase, report, "Error"),
                        GetColumnValue(columns, summary, benchmarkCase, report, "StdDev"),
                        GetColumnValue(columns, summary, benchmarkCase, report, "Allocated")
                    ]);
                    number++;
                }
            }

            return resultRows;
        }

        private static List<string[]> BuildRowsFromArtifact(Summary summary)
        {
            var benchmarkCase = GetReports(summary)
                .Select(GetBenchmarkCase)
                .FirstOrDefault(item => item != null);
            if (benchmarkCase == null)
            {
                return [];
            }

            var className = GetClassName(benchmarkCase);
            var fullClassName = benchmarkCase.Descriptor.Type.FullName;
            if (string.IsNullOrWhiteSpace(fullClassName))
            {
                return [];
            }

            var reportPath = Path.Combine(
                "BenchmarkDotNet.Artifacts",
                "results",
                $"{fullClassName}-report-github.md");
            if (!File.Exists(reportPath))
            {
                return [];
            }

            var tableLines = File.ReadLines(reportPath)
                .Where(line => line.StartsWith("|", StringComparison.Ordinal))
                .ToList();
            var headerIndex = tableLines.FindIndex(line =>
                line.Contains("Method", StringComparison.Ordinal)
                && line.Contains("Mean", StringComparison.Ordinal)
                && line.Contains("Allocated", StringComparison.Ordinal));
            if (headerIndex < 0 || headerIndex + 2 >= tableLines.Count)
            {
                return [];
            }

            var headers = SplitMarkdownRow(tableLines[headerIndex]);
            var methodIndex = headers.IndexOf("Method");
            var meanIndex = headers.IndexOf("Mean");
            var errorIndex = headers.IndexOf("Error");
            var stdDevIndex = headers.IndexOf("StdDev");
            var allocatedIndex = headers.IndexOf("Allocated");

            if (methodIndex < 0 || meanIndex < 0 || errorIndex < 0 || stdDevIndex < 0 || allocatedIndex < 0)
            {
                return [];
            }

            var rows = new List<string[]>();
            foreach (var line in tableLines.Skip(headerIndex + 2))
            {
                var cells = SplitMarkdownRow(line);
                if (cells.Count != headers.Count)
                {
                    continue;
                }

                var method = cells[methodIndex];
                var parameterSuffix = BuildParameterSuffix(headers, cells);
                rows.Add(
                [
                    className,
                    string.IsNullOrWhiteSpace(parameterSuffix) ? method : $"{method} {parameterSuffix}",
                    NormalizeMissing(cells[meanIndex]),
                    NormalizeMissing(cells[errorIndex]),
                    NormalizeMissing(cells[stdDevIndex]),
                    NormalizeMissing(cells[allocatedIndex])
                ]);
            }

            return rows;
        }

        private static List<string> SplitMarkdownRow(string line) =>
            line.Trim()
                .Trim('|')
                .Split('|')
                .Select(cell => cell.Trim())
                .ToList();

        private static string BuildParameterSuffix(IReadOnlyList<string> headers, IReadOnlyList<string> cells)
        {
            var parameters = new List<string>();
            for (var i = 0; i < headers.Count; i++)
            {
                var header = headers[i];
                if (header is "Method" or "Mean" or "Error" or "StdDev" or "Median" or "Gen0" or "Gen1" or "Gen2" or "Allocated")
                {
                    continue;
                }

                var value = NormalizeMissing(cells[i]);
                if (value != "-")
                {
                    parameters.Add($"{header}={value}");
                }
            }

            return parameters.Count == 0 ? string.Empty : $"({string.Join(", ", parameters)})";
        }

        private static string NormalizeMissing(string value) =>
            string.IsNullOrWhiteSpace(value) || value == "NA"
                ? "-"
                : value.Replace("**", string.Empty, StringComparison.Ordinal);

        private static IReadOnlyList<IColumn> GetSummaryColumns(Summary summary)
        {
            var method = summary.GetType().GetMethod("GetColumns", BindingFlags.Instance | BindingFlags.Public);
            if (method?.Invoke(summary, null) is IColumn[] columns)
            {
                return columns;
            }

            return [];
        }

        private static IEnumerable<object> GetReports(Summary summary)
        {
            var property = summary.GetType().GetProperty("Reports", BindingFlags.Instance | BindingFlags.Public);
            if (property?.GetValue(summary) is IEnumerable<object> reports)
            {
                return reports;
            }

            return [];
        }

        private static BenchmarkCase? GetBenchmarkCase(object report)
        {
            var property = report.GetType().GetProperty("BenchmarkCase", BindingFlags.Instance | BindingFlags.Public);
            return property?.GetValue(report) as BenchmarkCase;
        }

        private static string GetClassName(BenchmarkCase benchmarkCase)
        {
            var descriptor = benchmarkCase.Descriptor;
            return descriptor.Type.Name;
        }

        private static string GetMethodName(BenchmarkCase benchmarkCase)
        {
            var descriptor = benchmarkCase.Descriptor;
            return descriptor.WorkloadMethod.Name;
        }

        private static string GetColumnValue(
            IReadOnlyList<IColumn> columns,
            Summary summary,
            BenchmarkCase benchmarkCase,
            object report,
            string columnName)
        {
            var column = columns.FirstOrDefault(column =>
                string.Equals(column.ColumnName, columnName, StringComparison.OrdinalIgnoreCase)
                || string.Equals(column.Id, columnName, StringComparison.OrdinalIgnoreCase));

            if (column == null)
            {
                return GetFallbackValue(report, columnName);
            }

            var value = column.GetValue(summary, benchmarkCase);
            return NormalizeMissing(value);
        }

        private static string GetFallbackValue(object report, string columnName)
        {
            var statistics = report.GetType()
                .GetProperty("ResultStatistics", BindingFlags.Instance | BindingFlags.Public)
                ?.GetValue(report);
            if (statistics == null)
            {
                return "-";
            }

            return columnName switch
            {
                "Mean" => FormatNanoseconds(GetDoubleProperty(statistics, "Mean")),
                "Error" => FormatNanoseconds(GetConfidenceIntervalMargin(statistics)),
                "StdDev" => FormatNanoseconds(GetDoubleProperty(statistics, "StandardDeviation")),
                _ => "-"
            };
        }

        private static double? GetDoubleProperty(object source, string propertyName)
        {
            var value = source.GetType()
                .GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public)
                ?.GetValue(source);

            return value is double number ? number : null;
        }

        private static double? GetConfidenceIntervalMargin(object statistics)
        {
            var confidenceInterval = statistics.GetType()
                .GetProperty("LegacyConfidenceInterval", BindingFlags.Instance | BindingFlags.Public)
                ?.GetValue(statistics);

            return confidenceInterval == null ? null : GetDoubleProperty(confidenceInterval, "Margin");
        }

        private static string FormatNanoseconds(double? nanoseconds)
        {
            if (!nanoseconds.HasValue)
            {
                return "-";
            }

            var value = nanoseconds.Value;
            if (Math.Abs(value) >= 1_000_000)
            {
                return $"{value / 1_000_000:0.###} ms";
            }

            if (Math.Abs(value) >= 1_000)
            {
                return $"{value / 1_000:0.###} us";
            }

            return $"{value:0.###} ns";
        }

        private static int[] CalculateWidths(IReadOnlyList<string[]> rows)
        {
            var widths = Headers.Select(header => header.Length).ToArray();

            foreach (var row in rows)
            {
                for (var i = 0; i < row.Length; i++)
                {
                    widths[i] = Math.Max(widths[i], row[i].Length);
                }
            }

            return widths;
        }

        private static void PrintRow(string[] cells, int[] widths, IReadOnlySet<int> numericColumns)
        {
            Console.Write("|");
            for (var i = 0; i < cells.Length; i++)
            {
                var value = numericColumns.Contains(i)
                    ? cells[i].PadLeft(widths[i])
                    : cells[i].PadRight(widths[i]);

                Console.Write($" {value} |");
            }

            Console.WriteLine();
        }

        private static void PrintSeparator(int[] widths)
        {
            Console.Write("|");
            foreach (var width in widths)
            {
                Console.Write($"{new string('-', width + 2)}|");
            }

            Console.WriteLine();
        }
    }
}
