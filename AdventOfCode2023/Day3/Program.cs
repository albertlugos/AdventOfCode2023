using System.Text.RegularExpressions;

public class Program
{
    private static readonly Regex NumberRegex = new Regex(@"\d+");
    private static readonly Regex SymbolRegex = new Regex(@"[\%\$\*#\+/@&=-]");
    private static readonly Regex GearRegex = new Regex(@"\*");

    public static void Main(string[] args)
    {
        string[] lines = File.ReadAllLines("input.txt");

        int partSums = 0;
        int gearRatios = 0;

        partSums += SumParts(lines[0..3], lines[0]);
        gearRatios += SumGears(lines[0..3], lines[0]);

        for (int i = 1; i < lines.Length - 1; i++)
        {
            partSums += SumParts(lines[(i - 1)..(i + 2)], lines[i]);
            gearRatios += SumGears(lines[(i - 1)..(i + 2)], lines[i]);
        }

        partSums += SumParts(lines[^3..], lines[^1]);
        gearRatios += SumGears(lines[^3..], lines[^1]);

        // Part 1
        Console.WriteLine($"Sum of the part numbers: {partSums}");

        // Part 2
        Console.WriteLine($"Sum of the gear ratios: {gearRatios}");
    }

    private static int SumParts(string[] symbolLines, string currentLine)
    {
        IEnumerable<Match> symbols = symbolLines.SelectMany(x => SymbolRegex.Matches(x));
        IEnumerable<Match> numbers = NumberRegex.Matches(currentLine);

        return numbers.Where(num => symbols.Any(num.IsAdjacent))
                      .Sum(num => int.Parse(num.Value));
    }

    private static int SumGears(string[] numberLines, string currentLine)
    {
        IEnumerable<Match> gears = GearRegex.Matches(currentLine);
        IEnumerable<Match> numbers = numberLines.SelectMany(x => NumberRegex.Matches(x));

        int result = 0;

        foreach (Match gear in gears)
        {
            var gearNumbers = numbers
                .Where(number => number.IsAdjacent(gear))
                .Select(gearNumber => int.Parse(gearNumber.Value));

            if (gearNumbers.Count() == 2)
            {
                result += gearNumbers.Aggregate((x, y) => x * y);
            }
        }

        return result;
    }
}

public static class Extensions
{
    public static bool IsAdjacent(this Match number, Match symbol)
    {
        return Enumerable.Range(number.Index - 1, number.Length + 2)
                         .Contains(symbol.Index);
    }
}