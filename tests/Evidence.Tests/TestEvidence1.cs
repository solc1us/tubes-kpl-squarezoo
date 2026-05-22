using System;
using tubes_kpl_squarezoo.Enums;
using Xunit;

namespace tubes_kpl_squarezoo.Tests
{
    public class TestEvidence1
    {
        // =========================
        // TEST GetSummary()
        // =========================

        [Fact]
        public void GetSummary_ContentNull_ReturnsNoContent()
        {
            // Arrange
            var evidence = new Evidences<string>
            {
                Type = EvidenceType.Testimony,
                Content = null
            };

            // Act
            var result = evidence.GetSummary();

            // Assert
            Assert.Equal("No Content", result);
        }

        [Fact]
        public void GetSummary_TestimonyShort_ReturnsFormattedText()
        {
            // Arrange
            var evidence = new Evidences<string>
            {
                Type = EvidenceType.Testimony,
                Content = "Kesaksian korban"
            };

            // Act
            var result = evidence.GetSummary();

            // Assert
            Assert.Equal("[Testimony] Kesaksian korban", result);
        }

        [Fact]
        public void GetSummary_TestimonyLong_ReturnsTrimmedText()
        {
            // Arrange
            string longText = new string('A', 60);

            var evidence = new Evidences<string>
            {
                Type = EvidenceType.Testimony,
                Content = longText
            };

            // Act
            var result = evidence.GetSummary();

            // Assert
            Assert.Equal("[Testimony] " + new string('A', 50) + "...", result);
        }

        [Fact]
        public void GetSummary_Document_ReturnsDocumentFormat()
        {
            // Arrange
            var evidence = new Evidences<string>
            {
                Type = EvidenceType.Document,
                Content = "laporan.pdf"
            };

            // Act
            var result = evidence.GetSummary();

            // Assert
            Assert.Equal("[Document] laporan.pdf", result);
        }

        [Fact]
        public void GetSummary_MediaLink_ReturnsMediaFormat()
        {
            // Arrange
            var evidence = new Evidences<string>
            {
                Type = EvidenceType.MediaLink,
                Content = "https://youtube.com"
            };

            // Act
            var result = evidence.GetSummary();

            // Assert
            Assert.Equal("[Media] https://youtube.com", result);
        }

        // =========================
        // TEST Validate()
        // =========================

        [Fact]
        public void Validate_ContentNull_ReturnsFalse()
        {
            // Arrange
            var evidence = new Evidences<string>
            {
                Type = EvidenceType.Document,
                Content = null
            };

            // Act
            var result = evidence.Validate();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Validate_TestimonyValid_ReturnsTrue()
        {
            // Arrange
            var evidence = new Evidences<string>
            {
                Type = EvidenceType.Testimony,
                Content = "Kesaksian lengkap"
            };

            // Act
            var result = evidence.Validate();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Validate_TestimonyTooShort_ReturnsFalse()
        {
            // Arrange
            var evidence = new Evidences<string>
            {
                Type = EvidenceType.Testimony,
                Content = "Pendek"
            };

            // Act
            var result = evidence.Validate();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Validate_DocumentValid_ReturnsTrue()
        {
            // Arrange
            var evidence = new Evidences<string>
            {
                Type = EvidenceType.Document,
                Content = "dokumen.docx"
            };

            // Act
            var result = evidence.Validate();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Validate_DocumentEmpty_ReturnsFalse()
        {
            // Arrange
            var evidence = new Evidences<string>
            {
                Type = EvidenceType.Document,
                Content = "   "
            };

            // Act
            var result = evidence.Validate();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Validate_MediaLinkValid_ReturnsTrue()
        {
            // Arrange
            var evidence = new Evidences<string>
            {
                Type = EvidenceType.MediaLink,
                Content = "https://google.com"
            };

            // Act
            var result = evidence.Validate();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Validate_MediaLinkInvalid_ReturnsFalse()
        {
            // Arrange
            var evidence = new Evidences<string>
            {
                Type = EvidenceType.MediaLink,
                Content = "invalid_link"
            };

            // Act
            var result = evidence.Validate();

            // Assert
            Assert.False(result);
        }
    }
}