namespace AOC;

internal static class Program
{
    private const string InputFile = "../../../Data/Data.txt";

    private static void Main()
    {
        var input = File.ReadAllText(InputFile).Trim().Split(' ').Select(long.Parse).ToList();
        var part1 = Part1(input);
        Console.WriteLine($"Part1: {part1}");
        var part2 = Part2(input);
        Console.WriteLine($"Part2: {part2}");
    }

    private static long Part1(IReadOnlyList<long> input) => Blink(input, 25);

    private static long Part2(IReadOnlyList<long> input) => Blink(input, 75);

    private static long Blink(IReadOnlyList<long> input, int times)
    {
        var stonesDict = new Dictionary<long, long>();
        foreach (var b in input) Add(b, 1L);
        for (var i = 0; i < times; i++)
        {
            foreach (var (key, count) in stonesDict.ToList())
            {
                Remove(key, count);
                if (key is 0L) Add(1L, count);
                else
                {
                    var digits = key.ToString();
                    if (digits.Length % 2 is 1) Add(key * 2024, count);
                    else
                    {
                        Add(long.Parse(digits[..(digits.Length / 2)]), count);
                        Add(long.Parse(digits[(digits.Length / 2)..]), count);
                    }
                }
            }
        }
        return stonesDict.Values.Sum();
    
        void Add(long key, long value)
        {
            stonesDict.TryAdd(key, 0L);
            stonesDict[key] += value;
        }
    
        void Remove(long key, long value) => stonesDict[key] -= value;
    }
}