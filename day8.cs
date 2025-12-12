var junctionBoxes = File.ReadAllLines("day8.txt")
    .Select(line => line.Split(',').Select(int.Parse).ToArray())
    .Select(coords => new JunctionBox(coords[0], coords[1], coords[2]))
    .ToArray();

var distances = new Dictionary<(JunctionBox, JunctionBox), double>();

foreach (var a in junctionBoxes)
{
    foreach (var b in junctionBoxes)
    {
        if (a == b || distances.ContainsKey((b, a)))
        {
            continue;
        }

        distances[(a, b)] = Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2) + Math.Pow(a.Z - b.Z, 2));
    }
}

var circuits = junctionBoxes.ToDictionary(b => b, b => new List<JunctionBox> { b });
var pairs = distances.OrderBy(kv => kv.Value).Select(kv => kv.Key).ToList();

for (var i = 0; i < pairs.Count; i++)
{
    if (i == 1000)
    {
        Console.WriteLine(circuits.Values.Distinct().OrderByDescending(l => l.Count).Take(3).Aggregate(1, (acc, l) => acc * l.Count));
    }

    var (a, b) = pairs[i];

    if (circuits[a].Contains(b))
    {
        continue;
    }

    circuits[a].AddRange(circuits[b]);

    foreach (var box in circuits[b])
    {
        circuits[box] = circuits[a];
    }

    if (circuits.Values.Distinct().Count() == 1)
    {
        Console.WriteLine(a.X * b.X);
        break;
    }   
}

record JunctionBox(int X, int Y, int Z);
