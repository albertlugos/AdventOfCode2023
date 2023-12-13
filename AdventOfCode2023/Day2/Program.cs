using System.Text.RegularExpressions;

var gameRegex = new Regex(@"Game (\d+): (.+)");
var setRegex = new Regex(@"(\d+) (\w+)");

var rules = new Dictionary<string, int>()
{
    { "red", 12 },
    { "green", 13 },
    { "blue", 14 }
};

string[] lines = File.ReadAllLines("input.txt");

var games = lines
    .Select(line => gameRegex.Match(line))
    .Select(matches => new
    {
        Id = int.Parse(matches.Groups[1].Value),
        Sets = matches.Groups[2].Value.Split(";")
                                      .SelectMany(set => setRegex.Matches(set))
    });

// Part 1
int idSum = games
    .Where(game => game.Sets.All(set => int.Parse(set.Groups[1].Value) <= rules[set.Groups[2].Value]))
    .Sum(game => game.Id);

Console.WriteLine(idSum);

// Part 2
var powerSums = games
    .Sum(game => game.Sets.GroupBy(set => set.Groups[2].Value, set => int.Parse(set.Groups[1].Value), (color, numbers) => new { Color = color, Numbers = numbers })
                             .Select(set => set.Numbers.Max())
                             .Aggregate((max1, max2) => max1 * max2));

Console.WriteLine(powerSums);