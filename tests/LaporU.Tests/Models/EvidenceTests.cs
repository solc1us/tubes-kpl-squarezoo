using tubes_kpl_squarezoo.Enums;
using tubes_kpl_squarezoo.Models;

namespace LaporU.Tests.Models
{
    public class EvidenceTests
    {
        [Fact]
        public void Evidence_WithValidData_ShouldStoreEvidenceProperties()
        {
            var evidence = new Evidence<string>(
                EvidenceType.Testimoni,
                "Kesaksian valid untuk laporan",
                "Keterangan bukti");

            Assert.Equal(EvidenceType.Testimoni, evidence.Type);
            Assert.Equal("Kesaksian valid untuk laporan", evidence.Content);
            Assert.Equal("Keterangan bukti", evidence.Description);
        }

        [Fact]
        public void Evidence_WhenCreated_ShouldContainEvidenceId()
        {
            var evidence = new Evidence<string>(
                EvidenceType.Testimoni,
                "Kesaksian valid untuk laporan",
                "Keterangan bukti");

            Assert.NotEqual(Guid.Empty, evidence.EvidenceId);
        }

        [Fact]
        public void Evidence_TypeTestimoni_ShouldStoreCorrectValue()
        {
            var evidence = new Evidence<string>(EvidenceType.Testimoni, "Kesaksian valid", "Deskripsi");

            Assert.Equal(EvidenceType.Testimoni, evidence.Type);
        }

        [Fact]
        public void Evidence_TypeKronologiTambahan_ShouldStoreCorrectValue()
        {
            var evidence = new Evidence<string>(EvidenceType.KronologiTambahan, "Kronologi tambahan", "Deskripsi");

            Assert.Equal(EvidenceType.KronologiTambahan, evidence.Type);
        }

        [Fact]
        public void Evidence_TypeCatatanPendukung_ShouldStoreCorrectValue()
        {
            var evidence = new Evidence<string>(EvidenceType.CatatanPendukung, "Catatan pendukung", "Deskripsi");

            Assert.Equal(EvidenceType.CatatanPendukung, evidence.Type);
        }

        [Fact]
        public void Evidence_TestimoniWithEnoughContent_ShouldValidate()
        {
            var evidence = new Evidence<string>(EvidenceType.Testimoni, "Isi testimoni valid", "Deskripsi");

            Assert.True(evidence.Validate());
        }

        [Fact]
        public void Evidence_TestimoniWithShortContent_ShouldFailValidation()
        {
            var evidence = new Evidence<string>(EvidenceType.Testimoni, "pendek", "Deskripsi");

            Assert.False(evidence.Validate());
        }
    }
}
