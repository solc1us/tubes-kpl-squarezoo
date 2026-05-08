using tubes_kpl_squarezoo.Enums;
using tubes_kpl_squarezoo.Models;
using tubes_kpl_squarezoo.Services;
using Xunit;

namespace ReportingSystem.Tests;

public class ReportServiceTests : IDisposable
{

    private readonly string _testPath = "test_reports.json";
    private readonly ReportService _service;
    public ReportServiceTests()
    {
        // Setiap test dapet service instance baru dan file baru
        _service = new ReportService(_testPath);
    }

    [Fact]
    public void CreateReport_ValidDraft_ShouldSucceed()
    {
        var user = new User("Marcel", "0812");
        var report = new Report("Judul", "Desc", user); // Default status is Draft

        var result = _service.CreateReport(report);

        Assert.NotNull(_service.GetById(result.ReportId));
        Assert.Equal(ReportStatus.Draft, result.Status);
    }

    [Fact]
    public void CreateReport_NonDraftStatus_ShouldThrowException()
    {
        var user = new User("Marcel", "0812");
        var report = new Report("Judul", "Desc", user);

        // Sengaja ngerusak status sebelum di-persist
        report.Status = ReportStatus.Closed;

        // Assert: Harus nge-throw karena bypass Automata dideteksi service
        Assert.Throws<InvalidOperationException>(() => _service.CreateReport(report));
    }

    [Fact]
    public void ExecuteTransition_ValidFlow_ShouldUpdateFile()
    {
        var user = new User("Marcel", "0812");
        var report = _service.CreateReport(new Report("Judul", "Desc", user));

        // Act: Pindah Draft -> Submitted
        bool success = _service.ExecuteTransition(report.ReportId, ReportStatus.Submitted);

        // Assert
        Assert.True(success);
        Assert.Equal(ReportStatus.Submitted, _service.GetById(report.ReportId).Status);
    }

    public void Dispose()
    {
        if (File.Exists(_testPath)) File.Delete(_testPath);
    }
}