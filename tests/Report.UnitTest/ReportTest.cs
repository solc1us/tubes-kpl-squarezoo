using System;
using Xunit;
using tubes_kpl_squarezoo.Models;
using tubes_kpl_squarezoo.Enums;

namespace tubes_kpl_squarezoo.Tests
{
    public class ReportTest
    {
        private User CreateUser()
        {
            return new User("Budi", "Pelapor");
        }

        [Fact]
        public void Report_Baru_StatusAwalDraft()
        {
            Report report = new Report(
                "Judul Laporan",
                "Deskripsi laporan",
                CreateUser()
            );

            Assert.Equal(ReportStatus.Draft, report.Status);
        }

        [Fact]
        public void Report_Baru_ReportIdOtomatisTerisi()
        {
            Report report = new Report(
                "Judul Laporan",
                "Deskripsi laporan",
                CreateUser()
            );

            Assert.NotEqual(Guid.Empty, report.ReportId);
        }

        [Fact]
        public void TransitionTo_DraftKeSubmitted_Berhasil()
        {
            Report report = new Report(
                "Judul Laporan",
                "Deskripsi laporan",
                CreateUser()
            );

            bool hasil = report.TransitionTo(ReportStatus.Submitted);

            Assert.True(hasil);
            Assert.Equal(ReportStatus.Submitted, report.Status);
        }

        [Fact]
        public void TransitionTo_DraftKeClosed_Gagal()
        {
            Report report = new Report(
                "Judul Laporan",
                "Deskripsi laporan",
                CreateUser()
            );

            bool hasil = report.TransitionTo(ReportStatus.Closed);

            Assert.False(hasil);
            Assert.Equal(ReportStatus.Draft, report.Status);
        }

        [Fact]
        public void GetAllowedTransitions_StatusDraft_MengembalikanSubmitted()
        {
            Report report = new Report(
                "Judul Laporan",
                "Deskripsi laporan",
                CreateUser()
            );

            var allowed = report.GetAllowedTransitions();

            Assert.Single(allowed);
            Assert.Contains(ReportStatus.Submitted, allowed);
        }

        [Fact]
        public void AddEvidence_EvidenceValid_BerhasilDitambahkan()
        {
            Report report = new Report(
                "Judul Laporan",
                "Deskripsi laporan",
                CreateUser()
            );

            Evidence<string> evidence = new Evidence<string>(
                EvidenceType.Document,
                "Isi bukti laporan",
                "Bukti dari pelapor"
            );

            report.AddEvidence(evidence);

            Assert.Equal(1, report.Evidences.Count);
            Assert.Equal(EvidenceType.Document, report.Evidences[0].Type);
        }

        [Fact]
        public void Constructor_TitleKosong_ThrowException()
        {
            Assert.Throws<Exception>(() =>
                new Report("", "Deskripsi laporan", CreateUser())
            );
        }

        [Fact]
        public void Constructor_DescriptionKosong_ThrowException()
        {
            Assert.Throws<Exception>(() =>
                new Report("Judul Laporan", "", CreateUser())
            );
        }

        [Fact]
        public void Constructor_UserNull_ThrowException()
        {
            Assert.Throws<Exception>(() =>
                new Report("Judul Laporan", "Deskripsi laporan", null)
            );
        }
    }
}