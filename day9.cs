var tiles = File.ReadAllLines("day9.txt")
    .Select(line => line.Split(',').Select(long.Parse).ToArray())
    .Select(p => (x: p[0], y: p[1]))
    .ToArray();

var rectangles = 
    (from i in Enumerable.Range(0, tiles.Length)
     from j in Enumerable.Range(i + 1, tiles.Length - i - 1)
     let area = (Math.Abs(tiles[j].x - tiles[i].x) + 1) * (Math.Abs(tiles[i].y - tiles[j].y) + 1)
     orderby area descending
     select (tiles[i], tiles[j], area)).ToList();

Console.WriteLine(rectangles.First().area);
Console.WriteLine(rectangles.First(IsRectangleInPolygon).area);

bool IsRectangleInPolygon(((long x, long y) p1, (long x, long y) p2, long area) rect)
{
    var (minX, maxX) = (Math.Min(rect.p1.x, rect.p2.x), Math.Max(rect.p1.x, rect.p2.x));
    var (minY, maxY) = (Math.Min(rect.p1.y, rect.p2.y), Math.Max(rect.p1.y, rect.p2.y));

    // Corners
    if (!IsPointInPolygon(minX, minY) || !IsPointInPolygon(maxX, minY) || !IsPointInPolygon(maxX, maxY) || !IsPointInPolygon(minX, maxY))
    {
        return false;
    }

    // Edges
    for (int i = 0; i < tiles.Length; i++)
    {
        var (px1, py1) = tiles[i];
        var (px2, py2) = tiles[(i + 1) % tiles.Length];

        if ((py1 == py2 && py1 > minY && py1 < maxY && Math.Min(px1, px2) < maxX && Math.Max(px1, px2) > minX) ||
            (px1 == px2 && px1 > minX && px1 < maxX && Math.Min(py1, py2) < maxY && Math.Max(py1, py2) > minY))
        {
            return false;
        }
    }

    return true;
}

bool IsPointInPolygon(long x, long y)
{
    // Border
    for (int i = 0; i < tiles.Length; i++)
    {
        var (x1, y1) = tiles[i];
        var (x2, y2) = tiles[(i + 1) % tiles.Length];
        
        if ((x1 == x2 && x == x1 && y >= Math.Min(y1, y2) && y <= Math.Max(y1, y2)) ||
            (y1 == y2 && y == y1 && x >= Math.Min(x1, x2) && x <= Math.Max(x1, x2)))
        {
            return true;
        }
    }

    var inside = false;
    
    for (int i = 0, j = tiles.Length - 1; i < tiles.Length; j = i++)
    {
        if (((tiles[i].y > y) != (tiles[j].y > y)) && 
            (x < (tiles[j].x - tiles[i].x) * (y - tiles[i].y) / (tiles[j].y - tiles[i].y) + tiles[i].x))
        {
            inside = !inside;
        }
    }

    return inside;
}
