var grid = File.ReadAllLines("day4.txt")
    .Select(line => line.ToCharArray())
    .ToArray();

var rolls = GetRollsOfPaper().ToArray();

Console.WriteLine(rolls.Length);

var total = 0;

for (; rolls.Length > 0; rolls = [.. GetRollsOfPaper()])
{
    foreach (var (x, y) in rolls)
    {
        grid[y][x] = 'x';
    }

    total += rolls.Length;
}

Console.WriteLine(total);

IEnumerable<(int x, int y)> GetRollsOfPaper() =>
    from y in Enumerable.Range(0, grid.Length)
    from x in Enumerable.Range(0, grid[y].Length)
    where grid[y][x] == '@' && CountAdjacent(x, y) < 4
    select (x, y);

int CountAdjacent(int x, int y)
{
    var count = 0;

    for (var dy = -1; dy <= 1; dy++)
    {
        for (var dx = -1; dx <= 1; dx++)
        {
            if ((dx != 0 || dy != 0) && y + dy >= 0 && y + dy < grid.Length && x + dx >= 0 && x + dx < grid[y + dy].Length && grid[y + dy][x + dx] == '@')
            {
                count++;
            }
        }
    }

    return count;
}
