var rotations = File.ReadAllLines("day1.txt")
    .Select(line => (line[0], int.Parse(line[1..])))
    .ToList();

Console.WriteLine(GetPassword(false));
Console.WriteLine(GetPassword(true));

int GetPassword(bool countClicks)
{
    var dial = 50;
    var password = 0;

    foreach (var (direction, distance) in rotations)
    {
        if (countClicks)
        {
            for (var i = 0; i < distance; i++)
            {
                dial = (dial + (direction == 'L' ? -1 : 1) + 100) % 100;

                if (dial == 0)
                {
                    password++;
                }
            }
        }
        else
        {
            dial = (dial + (direction == 'L' ? -distance : distance) + 100) % 100;

            if (dial == 0)
            {
                password++;
            }
        }
    }

    return password;
}
