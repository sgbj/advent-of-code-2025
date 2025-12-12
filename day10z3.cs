#:package Microsoft.Z3@4.12.2
using Microsoft.Z3;
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

Console.WriteLine(machines.Sum(m => GetFewestPresses(m.Lights, m.Buttons, true)));
Console.WriteLine(machines.Sum(m => GetFewestPresses(m.Joltages, m.Buttons, false)));

int GetFewestPresses(int[] target, int[][] buttons, bool isLights)
{
    using var ctx = new Context();
    var solver = ctx.MkOptimize();

    var buttonVariables = buttons.Select((_, i) => ctx.MkIntConst($"b{i}")).ToArray();

    foreach (var v in buttonVariables)
    {
        solver.Assert(ctx.MkGe(v, ctx.MkInt(0)));
    }

    for (var i = 0; i < target.Length; i++)
    {
        var coefficients = new List<IntExpr>();

        for (var j = 0; j < buttons.Length; j++)
        {
            if (buttons[j].Contains(i))
            {
                coefficients.Add(buttonVariables[j]);
            }
        }

        if (isLights)
        {
            solver.Assert(ctx.MkEq(ctx.MkMod((IntExpr)ctx.MkAdd(coefficients), ctx.MkInt(2)), ctx.MkInt(target[i])));
        }
        else
        {
            solver.Assert(ctx.MkEq(ctx.MkAdd(coefficients), ctx.MkInt(target[i])));
        }
    }

    solver.MkMinimize(ctx.MkAdd(buttonVariables));

    return solver.Check() == Status.SATISFIABLE ? buttonVariables.Sum(v => ((IntNum)solver.Model.Evaluate(v)).Int) : -1;
}

record Machine(int[] Lights, int[][] Buttons, int[] Joltages);
