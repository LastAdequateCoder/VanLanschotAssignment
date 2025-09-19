using VanLanschot.FileHandler;
using VanLanschot.Model;

class Program
{
    static void Main(string[] args)
    {
        string inputName = args.Length > 0 ? args[0] : "bond_positions_sample.csv";
        string outputName = args.Length > 1 ? args[1] : "output.csv";

        string inputPath = Path.Combine(ProjectRoot(), inputName);
        string outputPath = Path.Combine(ProjectRoot(), outputName);
        FileHandler fileHandler = new FileHandler();
        List<Bond> bonds = fileHandler.ParseBonds(inputPath);
        foreach (var bond in bonds)
        {
            bond.CalculatePresentValue();
        }
        fileHandler.WriteBonds(outputPath, bonds);
        Console.WriteLine("Wrote data successfully!");
    }

    static string ProjectRoot()
    {
        return Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", ".."));
    }
}