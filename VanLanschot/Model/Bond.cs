using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VanLanschot.Model
{
    public class Bond
    {
        const double inflationPercent = 3.0;
        public string BondID { get; set; } 
        public string Issuer { get; set; }

        private string _rateRaw;
        public string RateRaw
        {
            get => _rateRaw;
            set
            {
                _rateRaw = value;
                Rate = ParseRate(value);
            }
        }

        private double _rate;
        public double Rate
        {
            get => _rate;
            private set => _rate = value;
        }

        public double FaceValue { get; set; }

        private string _paymentFrequency;
        public string PaymentFrequency
        {
            get => _paymentFrequency;
            set
            {
                _paymentFrequency = value;
                PaymentsPerYear = ParsePaymentsPerYear(value);
            }
        }

        private double _paymentsPerYear;
        public double PaymentsPerYear
        {
            get => _paymentsPerYear;
            private set => _paymentsPerYear = value;
        }

        public string Rating { get; set; }
        public string Type { get; set; }
        public double YearsToMaturity { get; set; }
        public double DiscountFactor { get; set; }
        public string DeskNotes { get; set; }
        public double PresentValue { get; set; }

        private static double ParsePaymentsPerYear(string freq)
        {
            return freq?.Trim().ToLowerInvariant() switch
            {
                "annual" => 1.0,
                "semi-annual" => 2.0,
                "quarterly" => 4.0,
                "monthly" => 12.0,
                "none" => 1.0,
                _ => 1.0
            };
        }

        private double ParseRate(string rateRaw)
        {
            if (string.IsNullOrWhiteSpace(rateRaw))
                return 0.0;

            string rateLower = rateRaw.Trim().ToLowerInvariant();

            if (rateLower == "inflation")
            {
                return inflationPercent;
            }
            else if (rateLower.StartsWith("inflation+"))
            {
                string extra = rateLower.Substring("inflation+".Length).Replace("%", "");
                if (double.TryParse(extra, NumberStyles.Any, CultureInfo.InvariantCulture, out double extraRate))
                {
                    return inflationPercent + extraRate;
                }
                else
                {
                    // Could not parse, return 0 or throw if preferred
                    return 0.0;
                }
            }
            else
            {
                string numeric = rateLower.Replace("%", "");
                if (double.TryParse(numeric, NumberStyles.Any, CultureInfo.InvariantCulture, out double parsed))
                {
                    return parsed;
                }
                else
                {
                    // Could not parse, return 0 or throw if preferred
                    return 0.0;
                }
            }
        }

        public void CalculatePresentValue()
        {
            double rateDecimal = Rate / 100.0;
            if (this.Type == "Bond" || this.Type == "Inflation-Linked")
            {
                double cpp = rateDecimal / this.PaymentsPerYear;
                double n = this.YearsToMaturity * this.PaymentsPerYear;
                this.PresentValue = (Math.Pow(1 + cpp, n) * this.FaceValue) * this.DiscountFactor;
            }
            else if (this.Type == "Zero-Coupon")
            {
                this.PresentValue = (Math.Pow(1 + rateDecimal, this.YearsToMaturity) * this.FaceValue) * this.DiscountFactor;
            }
        }
    }
}
