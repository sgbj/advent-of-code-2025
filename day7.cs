var grid = File.ReadAllLines("day7.txt");
var start = grid.SelectMany((row, y) => row.Select((c, x) => (c, x, y))).First(p => p.c == 'S');

var splits = new HashSet<(int, int)>();
var timelines = new Dictionary<(int, int), long>();

GetTimelines(start.x, start.y);

Console.WriteLine(splits.Count);
Console.WriteLine(timelines[(start.x, start.y)]);

long GetTimelines(int x, int y)
{
    if (y >= grid.Length)
    {
        return 1;
    }
    
    if (timelines.TryGetValue((x, y), out var t))
    {
        return t;
    }
    
    var c = grid[y][x];

    if (c == '^')
    {
        splits.Add((x, y));
        t = GetTimelines(x - 1, y) + GetTimelines(x + 1, y);
    }
    else if (c is 'S' or '.')
    {
        t = GetTimelines(x, y + 1);
    }

    timelines[(x, y)] = t;
    return t;
}
