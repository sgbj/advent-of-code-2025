var devices = File.ReadAllLines("day11.txt")
    .ToDictionary(line => line[..3], line => line[5..].Split(' '));

Console.WriteLine(GetPaths("you", "out").Count);
//Console.WriteLine(GetPaths("svr", "out").Count);

List<string> GetPaths(string start, string end)
{
    var queue = new Queue<string>([start]);
    var paths = new List<string>();

    while (queue.TryDequeue(out var path))
    {
        var device = path.Split('-')[^1];
    
        if (device == end)
        {
            paths.Add(path);
            continue;
        }

        foreach (var output in devices[device])
        {
            queue.Enqueue($"{path}-{output}");
        }
    }

    return paths;
}
