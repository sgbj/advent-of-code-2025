using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

var sections = Regex.Split(File.ReadAllText("day12.txt"), @"\r?\n\r?\n");
var presents = sections[..^1].Select(s => Regex.Split(s, @"\r?\n")[1..].Select(s => s.ToCharArray()).ToArray()).ToArray();
var presentRotations = presents.Select(present => GetRotations(present).ToArray()).ToArray();
var regions = Regex.Split(sections[^1], @"\r?\n").Select(s =>
{
    var match = Regex.Match(s, @"(\d+)x(\d+):(?: (\d+))+");
    return (int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value),
            match.Groups[3].Captures.Select(c => int.Parse(c.Value)).ToArray());
}).ToArray();

Console.WriteLine(regions.Count(CanFit));

bool CanFit((int width, int height, int[] counts) region)
{
    var totalPresentArea = region.counts.Select((c, i) => c * GetArea(presents[i])).Sum();

    if (totalPresentArea > region.width * region.height)
    {
        return false;
    }

    var space = Enumerable.Range(0, region.height).Select(_ => new string('.', region.width).ToCharArray()).ToArray();

    return CanFit(space, 0, 0);

    bool CanFit(char[][] space, int presentIndex, int presentCount)
    {
        if (presentIndex == region.counts.Length)
        {
            return true;
        }

        if (presentCount == region.counts[presentIndex])
        {
            return CanFit(space, presentIndex + 1, 0);
        }

        foreach (var rotation in presentRotations[presentIndex])
        {
            for (var y = 0; y < region.height - rotation.Length + 1; y++)
            {
                for (var x = 0; x < region.width - rotation[0].Length + 1; x++)
                {
                    if (TryPlace(space, rotation, x, y, out var newSpace) && CanFit(newSpace, presentIndex, presentCount + 1))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
}

bool TryPlace(char[][] space, char[][] present, int x, int y, [NotNullWhen(true)] out char[][]? result)
{
    for (var i = 0; i < present.Length; i++)
    {
        for (var j = 0; j < present[i].Length; j++)
        {
            if (present[i][j] == '#' && space[y + i][x + j] != '.')
            {
                result = null;
                return false;
            }
        }
    }

    result = [.. space.Select(row => new string(row).ToCharArray())];

    for (var i = 0; i < present.Length; i++)
    {
        for (var j = 0; j < present[i].Length; j++)
        {
            if (present[i][j] == '#')
            {
                result[y + i][x + j] = '#';
            }
        }
    }

    return true;
}

int GetArea(char[][] present) => present.Sum(row => row.Count(c => c == '#'));

IEnumerable<char[][]> GetRotations(char[][] present)
{
    for (var i = 0; i < 4; i++)
    {
        present = [.. Enumerable.Range(0, present[0].Length).Select(y =>
            Enumerable.Range(0, present.Length).Select(x => present[^(x + 1)][y]).ToArray())];
        yield return present;
    }
}
