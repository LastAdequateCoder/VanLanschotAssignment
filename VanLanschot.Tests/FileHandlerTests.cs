using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using VanLanschot.FileHandler;
using VanLanschot.Model;
using Xunit;

namespace VanLanschot.Tests
{
    public class FileHandlerTests
    {
        [Fact]
        public void ParseBonds_ThrowsOnMissingFile()
        {
            var handler = new FileHandler.FileHandler(); // Use the fully qualified name to reference the class
            Assert.Throws<Exception>(() => handler.ParseBonds("nonexistent.csv"));
        }

        [Fact]
        public void WriteBonds_And_ParseBonds_RoundTrip()
        {
            var handler = new FileHandler.FileHandler(); // Use the fully qualified name to reference the class
            var bonds = new List<Bond>
                {
                    new Bond
                    {
                        BondID = "B1",
                        Issuer = "Issuer1",
                        RateRaw = "5%",
                        FaceValue = 1000,
                        PaymentFrequency = "annual",
                        Rating = "AAA",
                        Type = "Fixed",
                        YearsToMaturity = 5,
                        DiscountFactor = 0.95,
                        DeskNotes = "Test",
                        PresentValue = 950
                    }
                };

            string tempFile = Path.GetTempFileName();
            try
            {
                handler.WriteBonds(tempFile, bonds);
                var parsed = handler.ParseBonds(tempFile);
                Assert.Single(parsed);
                Assert.Equal("B1", parsed[0].BondID);
                Assert.Equal(5.0, parsed[0].Rate, 2);
                Assert.Equal(1.0, parsed[0].PaymentsPerYear);
            }
            finally
            {
                File.Delete(tempFile);
            }
        }
    }
}