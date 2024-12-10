namespace AOC;

internal static class Program
{
    private const string InputFile = "../../../Data/Data.txt";

    private static int RowCount;
    private static int ColCount;

    private static void Main()
    {
        var input = File.ReadLines(InputFile).ToList();
        RowCount = input.Count;
        ColCount = input[0].Length;
        var part1 = Part1(input);
        Console.WriteLine($"Part1: {part1}");
        var part2 = Part2(input);
        Console.WriteLine($"Part2: {part2}");
    }

    private static long Part1(IReadOnlyList<string> input) 
        => GetTrailheads(input)
            .Select(trailhead => FindTrails(input, trailhead.row, trailhead.col, '0'))
            .Select(h => h.Count)
            .Sum();

    private static HashSet<(int row, int col)> FindTrails(IReadOnlyList<string> input, int row, int col, char height)
    {
        if (row < 0 || row >= RowCount || col < 0 || col >= ColCount || input[row][col] != height) return [];
        if (height is '9') return [(row, col)];
        var nextHeight = (char)(height + 1);
        return FindTrails(input, row + 1, col, nextHeight)
            .Concat(FindTrails(input, row - 1, col, nextHeight))
            .Concat(FindTrails(input, row, col + 1, nextHeight))
            .Concat(FindTrails(input, row, col - 1, nextHeight))
            .ToHashSet();
    }

    private static IEnumerable<(int row, int col)> GetTrailheads(IReadOnlyList<string> input)
    {
        for (var row = 0; row < input.Count; row++)
        {
            for (var col = 0; col < input[0].Length; col++)
            {
                if (input[row][col] is '0') yield return (row, col);
            }
        }
    }

    private static long Part2(IReadOnlyList<string> input) 
        => GetTrailheads(input)
            .Sum(trailhead => CountTrails(input, trailhead.row, trailhead.col, '0'));

    private static long CountTrails(IReadOnlyList<string> input, int row, int col, char height)
    {
        if (row < 0 || row >= RowCount || col < 0 || col >= ColCount || input[row][col] != height) return 0;
        if (height is '9') return 1;
        var nextHeight = (char)(height + 1);
        return CountTrails(input, row + 1, col, nextHeight)
               + CountTrails(input, row - 1, col, nextHeight)
               + CountTrails(input, row, col + 1, nextHeight)
               + CountTrails(input, row, col - 1, nextHeight);
    }
}