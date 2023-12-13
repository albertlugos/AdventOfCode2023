using System.Text.RegularExpressions;

var numbers = new Dictionary<string, string>()
{
    { "one", "1" },
    { "two", "2" },
    { "three", "3" },
    { "four", "4" },
    { "five", "5" },
    { "six", "6" },
    { "seven", "7" },
    { "eight", "8" },
    { "nine", "9" }
};

Regex NumbersRegex = new(@"\d{1}|(?=(one|two|three|four|five|six|seven|eight|nine))");

string[] lines = File.ReadAllLines("input.txt");

int result = lines
    .Select(line =>
    {
        IEnumerable<string> matches = NumbersRegex.Matches(line)
            .Select(match => match.Groups.Values.Select(group => group.Value).First(value => !string.IsNullOrWhiteSpace(value)));
            
        return new[] { matches.First(), matches.Last() };
    })
    .Sum(matches =>
            int.Parse(string.Join("", matches.Select(match => match.Length > 1 ? numbers[match] : match))));

Console.WriteLine(result);