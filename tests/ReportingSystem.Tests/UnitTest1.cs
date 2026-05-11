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
        // Arrange Global: Menyiapkan service dengan path file khusus test
        // Setiap test dapet service instance baru dan file baru
        _service = new ReportService(_testPath);
    }

    [Fact]
    public void CreateReport_ValidDraft_ShouldSucceed()
    {
        // Arrange: Buat report dengan status default (Draft)
        var user = new User("Marcel", "0812");
        var report = new Report("Laporan 1", "Pelaku menggunakan metode X", user);

        // Act: Simpan report
        var result = _service.CreateReport(report);

        // Assert: Report berhasil disimpan dengan status Draft
        Assert.NotNull(_service.GetById(result.ReportId));
        Assert.Equal(ReportStatus.Draft, result.Status);
    }

    [Fact]
    public void CreateReport_NonDraftStatus_ShouldThrowException()
    {
        // Arrange: Buat report dengan status yang sengaja diubah (bukan Draft)
        var user = new User("Marcel", "0812");
        var report = new Report("Judul", "Desc", user);

        // Sengaja memaksa status berubah
        report.Status = ReportStatus.Closed;

        // Assert: Harus nge-throw karena bypass Automata dideteksi service
        Assert.Throws<InvalidOperationException>(() => _service.CreateReport(report));
    }

    [Fact]
    public void ExecuteTransition_ValidFlow_ShouldUpdateFile()
    {
        // Arrange: Buat report baru
        var user = new User("Marcel", "0812");
        var report = _service.CreateReport(new Report("Laporan 2", "Pelaku menggunakan metode Y", user));

        // Act: Pindah Draft -> Submitted
        bool success = _service.ExecuteTransition(report.ReportId, ReportStatus.Submitted);

        // Assert
        Assert.True(success);
        Assert.Equal(ReportStatus.Submitted, _service.GetById(report.ReportId).Status);
    }

    [Fact]
    public void ExecuteTransition_IllegalFlow_ShouldReturnFalse()
    {
        // Arrange
        var user = new User("Marcel", "0812");
        var report = _service.CreateReport(new Report("Laporan 3", "Pelaku menggunakan metode Z", user));

        // Act: Mencoba lompat dari Draft langsung ke Resolved (Illegal menurut transitionTable)
        bool success = _service.ExecuteTransition(report.ReportId, ReportStatus.Resolved);

        // Assert
        Assert.False(success);
        Assert.Equal(ReportStatus.Draft, _service.GetById(report.ReportId)!.Status);
    }

    public void Dispose()
    {
        if (File.Exists(_testPath)) File.Delete(_testPath);
    }
}