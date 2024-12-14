namespace AOC;

internal class Data
{
    public string Line { get; }
    private int OrigPositionX { get; }
    private int OrigPositionY { get; }
    public int PositionX { get; set; }
    public int PositionY { get; set; }
    public int VelocityX { get; }
    public int VelocityY { get; }

    public Data(string line)
    {
        Line = line;
        var split = Line.Split(' ');
        var pSplit = split[0].Split(',');
        var vSplit = split[1].Split(',');
        OrigPositionX = PositionX = int.Parse(pSplit[0][2..]);
        OrigPositionY = PositionY = int.Parse(pSplit[1]);
        VelocityX = int.Parse(vSplit[0][2..]);
        VelocityY = int.Parse(vSplit[1]);
    }

    public void Reset()
    {
        PositionX = OrigPositionX;
        PositionY = OrigPositionY;
    }
}

internal static class Program
{
    private const string InputFile = "../../../Data/Data.txt";
    private static readonly int Width = InputFile.Contains("Test") ? 11 : 101;
    private static readonly int Height = InputFile.Contains("Test") ? 7 : 103;
    private const int Seconds = 100;

    private static void Main()
    {
        var input = File.ReadLines(InputFile).Select(l => new Data(l)).ToList();
        var part1 = Part1(input);
        Console.WriteLine($"Part1: {part1}");
        var part2 = Part2(input);
        Console.WriteLine($"Part2: {part2}");
    }

    private static long Part1(List<Data> input)
    {
        foreach (var robot in input) robot.Reset();
        for (var i = 0; i < Seconds; i++)
        {
            foreach (var robot in input) MoveRobot(robot);
        }

        var halfWidth = Width / 2;
        var halfHeight = Height / 2;
        var counts = new int[4];
        foreach (var robot in input)
        {
            if (robot.PositionX < halfWidth && robot.PositionY < halfHeight) ++counts[0];
            else if (robot.PositionX < halfWidth && robot.PositionY > halfHeight) ++counts[1];
            else if (robot.PositionX > halfWidth && robot.PositionY < halfHeight) ++counts[2];
            else if (robot.PositionX > halfWidth && robot.PositionY > halfHeight) ++counts[3];
        }
        return counts[0] * counts[1] * counts[2] * counts[3];
    }

    private static void MoveRobot(Data robot)
    {
        var positionX = robot.PositionX + robot.VelocityX;
        var positionY = robot.PositionY + robot.VelocityY;
        if (positionX < 0) positionX += Width;
        if (positionY < 0) positionY += Height;
        robot.PositionX = positionX % Width;
        robot.PositionY = positionY % Height;
    }

    private static long Part2(List<Data> input)
    {
        foreach (var robot in input) robot.Reset();
        for (var seconds = 0L; ; ++seconds)
        {
            if (IsTree(input))
            {
                Console.WriteLine($"Part2: seconds = {seconds}");
                Display(input);
                return seconds;
            }
            if (seconds < 0)
            {
                Console.WriteLine($"Part 2: Wrapped");
                return -1;
            }
            foreach (var robot in input) MoveRobot(robot);
        }
    }

    public static bool IsTree(List<Data> input)
    {
        var count = 0;
        var xMin = Width / 3;
        var xMax = xMin * 2;
        var yMin = Height / 3;
        var yMax = yMin * 2;
        foreach (var robot in input)
        {
            if (robot.PositionX >= xMin && robot.PositionX <= xMax && robot.PositionY >= yMin && robot.PositionY <= yMax) ++count;
        }
        return count >= input.Count / 2;
    }

    private static void Display(List<Data> robots)
    {
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                var count = robots.Count(data => data.PositionX == x && data.PositionY == y);
                var ch = count switch
                {
                    0 => '.',
                    > 9 => '*',
                    _ => (char)(count + '0')
                };
                Console.Write(ch);
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
}