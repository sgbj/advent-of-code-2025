using System.Text.RegularExpressions;

var parts = Regex.Split(File.ReadAllText("day5.txt"), "\r?\n\r?\n");

var freshRanges = parts[0].Split('\n')
    .Select(line => line.Split('-').Select(long.Parse).ToArray())
    .Select(range => (Start: range[0], End: range[1]))
    .OrderBy(range => range.Start)
    .ToArray();
var ingredientIds = parts[1].Split('\n').Select(long.Parse).ToArray();

var merged = new List<(long Start, long End)>();
var current = freshRanges[0];

foreach (var range in freshRanges.Skip(1))
{
    if (range.Start <= current.End + 1)
    {
        current = (current.Start, Math.Max(current.End, range.End));
    }
    else
    {
        merged.Add(current);
        current = range;
    }
}

merged.Add(current);

Console.WriteLine(ingredientIds.Count(id => merged.Any(range => id >= range.Start && id <= range.End)));
Console.WriteLine(merged.Sum(r => r.End - r.Start + 1));
