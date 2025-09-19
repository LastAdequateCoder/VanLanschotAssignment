using System.Globalization;
using VanLanschot.Model;
using Xunit;

namespace VanLanschot.Tests
{
    public class BondTests
    {
        [Theory]
        [InlineData("annual", 1.0)]
        [InlineData("semi-annual", 2.0)]
        [InlineData("quarterly", 4.0)]
        [InlineData("monthly", 12.0)]
        [InlineData("none", 1.0)]
        [InlineData("unknown", 1.0)]
        public void PaymentFrequency_SetsPaymentsPerYear_Correctly(string freq, double expected)
        {
            var bond = new Bond { PaymentFrequency = freq };
            Assert.Equal(expected, bond.PaymentsPerYear);
        }

        [Theory]
        [InlineData("5%", 5.0)]
        [InlineData("3.5", 3.5)]
        [InlineData("inflation", 3.0)]
        [InlineData("inflation+2", 5.0)]
        [InlineData("inflation+2.5%", 5.5)]
        [InlineData("badvalue", 0.0)]
        public void RateRaw_ParsesRate_Correctly(string rateRaw, double expected)
        {
            var bond = new Bond { RateRaw = rateRaw };
            Assert.Equal(expected, bond.Rate, 2);
        }

        [Fact]
        public void CalculatePresentValue_BondType_ComputesCorrectly()
        {
            var bond = new Bond
            {
                RateRaw = "5",
                FaceValue = 1000,
                PaymentFrequency = "annual",
                Type = "Bond",
                YearsToMaturity = 2,
                DiscountFactor = 0.95
            };
            bond.CalculatePresentValue();
            Assert.True(bond.PresentValue > 0);
        }

        [Fact]
        public void CalculatePresentValue_ZeroCoupon_ComputesCorrectly()
        {
            var bond = new Bond
            {
                RateRaw = "5",
                FaceValue = 1000,
                PaymentFrequency = "none",
                Type = "Zero-Coupon",
                YearsToMaturity = 2,
                DiscountFactor = 0.95
            };
            bond.CalculatePresentValue();
            Assert.True(bond.PresentValue > 0);
        }
    }
}