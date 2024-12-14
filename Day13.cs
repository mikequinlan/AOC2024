namespace AOC;

internal readonly record struct Coord(long X, long Y)
{
    public Coord Add(Coord other) => new(X + other.X, Y + other.Y);
    public Coord Times(long n) => new(X * n, Y * n);
}

internal class Data(Coord buttonA, Coord buttonB, Coord prize)
{
    public static long Adjustment { get; set; } = 0;
    public Coord ButtonA { get; } = buttonA;
    public Coord ButtonB { get; } = buttonB;
    public Coord Prize => prize.Add(new Coord(Adjustment, Adjustment));
}

internal static class Program
{
    private const string InputFile = "../../../Data/Data.txt";

    private static void Main()
    {
        var input = File.ReadLines(InputFile);
        var data = ParseInput(input).ToList();
        var part1 = Part1(data);
        Console.WriteLine($"Part1: {part1}");
        var part2 = Part2(data);
        Console.WriteLine($"Part2: {part2}");
    }

    private static IEnumerable<Data> ParseInput(IEnumerable<string> input)
    {
        var lines = input.ToList();
        for (var i = 0; i < lines.Count; i += 4)
        {
            var buttonA = GetCoord(lines[i]);
            var buttonB = GetCoord(lines[i + 1]);
            var prize = GetCoord(lines[i + 2]);
            yield return new Data(buttonA, buttonB, prize);
        }

        static Coord GetCoord(string line)
        {
            var split = line.Split(':', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            split = split[1].Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var x = int.Parse(new string(GetDigits(split[0]).ToArray()));
            var y = int.Parse(new string(GetDigits(split[1]).ToArray()));
            return new Coord(x, y);

            static IEnumerable<char> GetDigits(string line)
            {
                foreach (var ch in line)
                {
                    if (ch is >= '0' and <= '9') yield return ch;
                }
            }
        }
    }

    private static long Part1(List<Data> input)
    {
        var sum = 0L;
        foreach (var data in input)
        {
            var solution = Solve(data.ButtonA, data.ButtonB, data.Prize);
            if (solution is not null)
            {
                sum += solution.Value.buttonACount * 3 + solution.Value.buttonBCount;
            }
        }
        return sum;
    }

    private static long Part2(List<Data> input)
    {
        Data.Adjustment = 10_000_000_000_000;
        var sum = 0L;
        foreach (var data in input)
        {
            var solution = Solve(data.ButtonA, data.ButtonB, data.Prize);
            if (solution is not null)
            {
                sum += solution.Value.buttonACount * 3 + solution.Value.buttonBCount;
            }
        }
        return sum;
    }

    private static (long buttonACount, long buttonBCount)? Solve(Coord buttonA, Coord buttonB, Coord prize)
    {
        var determinant = buttonA.X * buttonB.Y - buttonA.Y * buttonB.X;
        if (determinant is 0) return null;
        var acNumerator = prize.X * buttonB.Y - prize.Y * buttonB.X;
        var bcNumerator = prize.Y * buttonA.X - prize.X * buttonA.Y;
        if (acNumerator % determinant is not 0 || bcNumerator % determinant is not 0) return null;
        var ac = acNumerator / determinant;
        var bc = bcNumerator / determinant;
        return ac < 0 || bc < 0 ? null : (ac, bc);
    }
}