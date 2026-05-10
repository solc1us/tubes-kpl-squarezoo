using Xunit;
using tubes_kpl_squarezoo;

namespace tubes_kpl_squarezoo.Tests
{
    public class TestEvidence1
    {
        [Fact]
        public void GetSummary_ContentNull_ReturnsNoContent()
        {
            var evidence = new Evidences<string>
            {
                Type = EvidenceType.Testimony,
                Content = null
            };

            var result = evidence.GetSummary();

            Assert.Equal("No Content", result);
        }

        [Fact]
        public void GetSummary_TestimonyShort_ReturnsFormattedText()
        {
            var evidence = new Evidences<string>
            {
                Type = EvidenceType.Testimony,
                Content = "Kesaksian korban"
            };

            var result = evidence.GetSummary();

            Assert.Equal("[Testimony] Kesaksian korban", result);
        }

        [Fact]
        public void GetSummary_Document_ReturnsDocumentFormat()
        {
            var evidence = new Evidences<string>
            {
                Type = EvidenceType.Document,
                Content = "laporan.pdf"
            };

            var result = evidence.GetSummary();

            Assert.Equal("[Document] laporan.pdf", result);
        }

        [Fact]
        public void Validate_TestimonyValid_ReturnsTrue()
        {
            var evidence = new Evidences<string>
            {
                Type = EvidenceType.Testimony,
                Content = "Kesaksian lengkap"
            };

            var result = evidence.Validate();

            Assert.True(result);
        }

        [Fact]
        public void Validate_MediaLinkInvalid_ReturnsFalse()
        {
            var evidence = new Evidences<string>
            {
                Type = EvidenceType.MediaLink,
                Content = "invalid_link"
            };

            var result = evidence.Validate();

            Assert.False(result);
        }
    }
}