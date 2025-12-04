var banks = File.ReadAllLines("day3.txt");

Console.WriteLine(GetTotalJoltage(2));
Console.WriteLine(GetTotalJoltage(12));

long GetTotalJoltage(int batteries)
{
    var total = 0L;

    foreach (var bank in banks)
    {
        var b = "";
        var index = 0;

        while (b.Length < batteries)
        {
            var max = bank[index..^(batteries - b.Length - 1)].Max();
            index = bank.IndexOf(max, index) + 1;
            b += max;
        }

        total += long.Parse(b);
    }

    return total;
}
