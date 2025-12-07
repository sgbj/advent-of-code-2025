using System.Text.RegularExpressions;

var lines = File.ReadAllLines("day6.txt");
var operators = Regex.Matches(lines[^1], @"([+*]\s+)(\s|$)").Select(m => m.Groups[1].Value).ToArray();
var numbers = lines[..^1].Select(line =>
{
    var pos = 0;
    return operators.Select(op => {
        var num = line.Substring(pos, op.Length);
        pos += op.Length + 1;
        return num;
    }).ToArray();
}).ToArray();

Console.WriteLine(GetTotal(ParseNumbers));
Console.WriteLine(GetTotal(ParseCephalopodNumbers));

long GetTotal(Func<int, string[][], IEnumerable<long>> parseNumbers)
{
    var total = 0L;

    for (var col = 0; col < numbers[0].Length; col++)
    {
        var result = parseNumbers(col, numbers).Aggregate((a, b) =>
            operators[col][0] == '+' ? a + b : a * b);

        total += result;
    }

    return total;
}

IEnumerable<long> ParseNumbers(int col, string[][] numbers) => 
    numbers.Select(row => long.Parse(row[col]));

IEnumerable<long> ParseCephalopodNumbers(int col, string[][] numbers) =>
    Enumerable.Range(0, operators[col].Length).Reverse()
        .Select(i => long.Parse(string.Concat(numbers.Select(row => row[col][i]))));
