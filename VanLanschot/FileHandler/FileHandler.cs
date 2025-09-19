using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VanLanschot.Model;

namespace VanLanschot.FileHandler
{
    public class FileHandler
    {
        public List<Bond> ParseBonds(string inputPath)
        {
            if (!File.Exists(inputPath))
            {
                throw new Exception($"Input file not found: {inputPath}");
            }
            var bonds = new List<Bond>();
            using (var reader = new StreamReader(inputPath))
            {
                string header = reader.ReadLine();
                if (header == null)
                {
                    throw new Exception("The file is empty!");
                }
                var columns = header.Split(';');
                for (int i = 0; i < columns.Length; i++)
                    columns[i] = columns[i].Trim();

                int idxBondID = Array.IndexOf(columns, Header.BondID);
                int idxIssuer = Array.IndexOf(columns, Header.Issuer);
                int idxRate = Array.IndexOf(columns, Header.Rate);
                int idxFaceValue = Array.IndexOf(columns, Header.FaceValue);
                int idxPaymentFrequency = Array.IndexOf(columns, Header.PaymentFrequency);
                int idxRating = Array.IndexOf(columns, Header.Rating);
                int idxType = Array.IndexOf(columns, Header.Type);
                int idxYearsToMaturity = Array.IndexOf(columns, Header.YearsToMaturity);
                int idxDF = Array.IndexOf(columns, Header.DiscountFactor);
                int idxDeskNotes = Array.IndexOf(columns, Header.DeskNotes);

                if (idxBondID < 0 || idxIssuer < 0 || idxRate < 0 || idxFaceValue < 0 ||
                    idxPaymentFrequency < 0 || idxRating < 0 || idxType < 0 ||
                    idxYearsToMaturity < 0 || idxDF < 0 || idxDeskNotes < 0)
                {
                    throw new Exception("One or more required columns are missing in the header.");
                }

                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    var fields = line.Split(';');
                    if (fields.Length < columns.Length)
                    {
                        Console.WriteLine($"Skipping malformed line: {line}");
                        continue;
                    }

                    string paymentFreqRaw = fields[idxPaymentFrequency].Trim();
                    string rateRaw = fields[idxRate].Trim();

                    var bond = new Bond
                    {
                        BondID = fields[idxBondID],
                        Issuer = fields[idxIssuer],
                        RateRaw = rateRaw,
                        FaceValue = double.Parse(fields[idxFaceValue], CultureInfo.InvariantCulture),
                        PaymentFrequency = paymentFreqRaw,
                        Rating = fields[idxRating],
                        Type = fields[idxType],
                        YearsToMaturity = double.Parse(fields[idxYearsToMaturity], CultureInfo.InvariantCulture),
                        DiscountFactor = double.Parse(fields[idxDF], CultureInfo.InvariantCulture),
                        DeskNotes = fields[idxDeskNotes]
                    };

                    bonds.Add(bond);
                }
            }

            return bonds;
        }

        public void WriteBonds(string outputPath, List<Bond> bonds)
        {
            using (var writer = new StreamWriter(outputPath, false))
            {
                writer.WriteLine(string.Join(";", new string[]
                {
                    Header.BondID,
                    Header.Issuer,
                    Header.Rate,
                    Header.FaceValue,
                    Header.PaymentFrequency,
                    Header.Rating,
                    Header.Type,
                    Header.YearsToMaturity,
                    Header.DiscountFactor,
                    Header.DeskNotes,
                    Header.PresentValue
                }));
                foreach (var bond in bonds)
                {
                    writer.WriteLine(string.Join(";", new string[]
                    {
                        bond.BondID,
                        bond.Issuer,
                        bond.RateRaw,
                        bond.FaceValue.ToString(CultureInfo.InvariantCulture),
                        bond.PaymentFrequency,
                        bond.Rating,
                        bond.Type,
                        bond.YearsToMaturity.ToString(CultureInfo.InvariantCulture),
                        bond.DiscountFactor.ToString(CultureInfo.InvariantCulture),
                        bond.DeskNotes,
                        bond.PresentValue.ToString("F2", CultureInfo.InvariantCulture)
                    }));
                }
            }
        }
    }
}
