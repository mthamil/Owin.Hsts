using NUnit.Framework;

namespace Owin.Hsts.Tests
{
    public class HstsSettingsTests
    {

        [Test]
        public void WhenIncludesSubDomains_GeneratesCorrectValue()
        {
            // Arrange
            var settings = new HstsSettings();

            // Act
            var result = settings.GenerateResponseValue();

            // Assert
            Assert.AreEqual(result, "max-age=2419200 ; includeSubDomains", result);

            // Verify

        }

        [Test]
        public void WhenNotIncludingSubDomains_GeneratesCorrectValue()
        {
            // Arrange
            var settings = new HstsSettings
                           {
                               IncludeSubDomains = false
                           };

            // Act
            var result = settings.GenerateResponseValue();

            // Assert
            Assert.AreEqual(result, "max-age=2419200", result);

            // Verify

        }

    }
}
