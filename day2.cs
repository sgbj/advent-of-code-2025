var ranges = File.ReadAllText("day2.txt").Split(',')
    .Select(ranges => ranges.Split('-').Select(long.Parse).ToArray())
    .ToList();

Console.WriteLine(GetInvalidIds(2).Sum());
Console.WriteLine(GetInvalidIds(null).Sum());

IEnumerable<long> GetInvalidIds(int? repeats)
{
    foreach (var range in ranges)
    {
        for (var i = range[0]; i <= range[1]; i++)
        {
            if (IsInvalid(i, repeats))
            {
                yield return i;
            }
        }
    }
}

bool IsInvalid(long id, int? repeats)
{
    var str = $"{id}";

    for (var i = 1; i <= str.Length; i++)
    {
        var sub = str[0..i];

        if (str.Length % sub.Length != 0 || str.Length / sub.Length < 2)
        {
            break;
        }

        var subs = string.Concat(Enumerable.Repeat(sub, repeats ?? (str.Length / sub.Length)));
        
        if (str == subs)
        {
            return true;
        }
    }

    return false;
}
