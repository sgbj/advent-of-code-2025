var devices = File.ReadAllLines("day11.txt")
    .ToDictionary(line => line[..3], line => line[5..].Split(' '));

Console.WriteLine(GetPaths("you", [], []));
Console.WriteLine(GetPaths("svr", ["dac", "fft"], []));

long GetPaths(string start, HashSet<string> required, Dictionary<string, long> cache)
{
    if (start == "out")
    {
        return required.Count == 0 ? 1 : 0;
    }

    var key = $"{start}:{string.Join(",", required)}";

    if (cache.TryGetValue(key, out var result))
    {
        return result;
    }

    result = devices[start].Sum(d => GetPaths(d, [.. required.Except([d])], cache));
    cache[key] = result;

    return result;
}
