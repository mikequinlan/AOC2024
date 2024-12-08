using System.Collections;
using System.Diagnostics;
using System.Numerics;

namespace AOC;

internal class Data
{
    public string Line { get; }
    public long TestValue { get; }
    public IReadOnlyList<long> Numbers { get; }
    
    public Data(string line)
    {
        Line = line;
        var split = line.Split(": ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        TestValue = long.Parse(split[0]);
        Numbers = split[1].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(long.Parse)
            .ToList();
    }
}

internal static class Program
{
    private const string InputFile = "../../../Data/Data.txt";

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
        long sum = 0;
        foreach (var data in input)
        {
            if (IsPossiblyTrue(data)) sum += data.TestValue;
        }
        return sum;
    }

    private static bool IsPossiblyTrue(Data data)
    {
        var bitmapMax = 1L << (data.Numbers.Count - 1);
        for (var bitmap = 0; bitmap < bitmapMax; bitmap++)
        {
            var operators = GetOperators(bitmap, data.Numbers.Count - 1).ToList();
            if (IsTrue(data, operators)) return true;
        }
        return false;
    }

    private static bool IsPossiblyTrue2(Data data)
    {
        if (data.Numbers.Count > 31) throw new InvalidOperationException();
        Console.WriteLine(data.Line);
        var bitmapMax = 1L << ((data.Numbers.Count - 1) * 2);
        for (var bitmap = 0; bitmap < bitmapMax; bitmap++)
        {
            var operators = GetOperators2(bitmap, data.Numbers.Count - 1).ToList();
            if (IsTrue(data, operators)) return true;
        }
        return false;
    }

    private static IEnumerable<char> GetOperators(int number, int count)
    {
        var mask = 1L;
        for (var i = 0; i < count; ++i)
        {
            yield return (number & mask) == 0 ? '+' : '*';
            mask <<= 1;
        }
    }

    private static IEnumerable<char> GetOperators2(int number, int count)
    {
        var mask = 3L;
        for (var i = 0; i < count; ++i)
        {
            var test = number & mask;
            if (test is 0L) yield return '+';
            if (test is 1L) yield return '*';
            if (test is 2L or 3L) yield return '|';
            number >>= 2;
        }
    }

    private static bool IsTrue(Data data, IReadOnlyList<char> operators)
    {
        var total = (BigInteger)data.Numbers[0];
        for (var i = 1; i < data.Numbers.Count; i++)
        {
            switch (operators[i - 1])
            {
                case '+':
                    total += data.Numbers[i];
                    break;
                case '*':
                    total *= data.Numbers[i];
                    break;
                case '|':
                    total = BigInteger.Parse(total.ToString() + data.Numbers[i]);
                    break;
            }
        }
        return data.TestValue == total;
    }

    private static long Part2(List<Data> input)
    {
        long sum = 0;
        foreach (var data in input)
        {
            if (IsPossiblyTrue2(data)) sum += data.TestValue;
        }
        return sum;
    }
}