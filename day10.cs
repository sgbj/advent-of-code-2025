using System.Text.RegularExpressions;

var machines = File.ReadAllLines("day10.txt")
    .Select(line =>
    {
        var match = Regex.Match(line, @"\[(.+)\] (?:\((.+?)\) )+{(.+)}");
        return new Machine(
            [.. match.Groups[1].Value.Select(c => c == '.' ? 0 : 1)],
            [.. match.Groups[2].Captures.Select(c => c.Value.Split(',').Select(int.Parse).ToArray())],
            [.. match.Groups[3].Value.Split(',').Select(int.Parse)]);
    })
    .ToArray();

Console.WriteLine(machines.Sum(m => GetFewestTotalPresses(m.Lights, m.Buttons)));
Console.WriteLine(machines.Sum(m => GetFewestTotalPresses(m.Joltages, m.Buttons)));

int GetFewestTotalPresses(int[] goal, int[][] buttons)
{
    var patterns = new List<(int[], int)>();
    GenerateButtonCombinations([], 0);
    return FindMinimumPresses(goal, []);

    void GenerateButtonCombinations(List<int> pressed, int index)
    {
        if (index == buttons.Length)
        {
            var increments = new int[goal.Length];
            foreach (var btn in pressed)
            {
                foreach (var pos in buttons[btn])
                {
                    increments[pos]++;
                }
            }
            patterns.Add((increments, pressed.Count));
            return;
        }
        GenerateButtonCombinations(pressed, index + 1);
        pressed.Add(index);
        GenerateButtonCombinations(pressed, index + 1);
        pressed.RemoveAt(pressed.Count - 1);
    }

    int FindMinimumPresses(int[] current, Dictionary<string, int> memo)
    {
        if (current.All(x => x <= 1))
        {
            var minCost = 1000000;
            foreach (var (increments, cost) in patterns)
            {
                if (increments.Zip(current, (inc, cur) => inc % 2 == cur).All(x => x))
                {
                    minCost = Math.Min(minCost, cost);
                }
            }
            return minCost;
        }

        var key = string.Join(",", current);
        if (memo.TryGetValue(key, out var cached))
        {
            return cached;
        }

        var result = 1000000;
        foreach (var (increments, cost) in patterns)
        {
            var valid = true;
            for (var i = 0; i < current.Length; i++)
            {
                if (increments[i] > current[i] || increments[i] % 2 != current[i] % 2)
                {
                    valid = false;
                    break;
                }
            }

            if (valid)
            {
                var next = current.Zip(increments, (g, p) => (g - p) / 2).ToArray();
                var subResult = FindMinimumPresses(next, memo);
                if (subResult < 1000000)
                {
                    result = Math.Min(result, cost + 2 * subResult);
                }
            }
        }

        return memo[key] = result;
    }
}

record Machine(int[] Lights, int[][] Buttons, int[] Joltages);
