namespace AOC;

internal static class Program
{
    private const string InputFile = "../../../Data/Data.txt";

    private static void Main()
    {
        var input = File.ReadAllLines(InputFile);
        var part1 = Part1(input);
        Console.WriteLine($"Part1: {part1}");
        var part2 = Part2(input);
        Console.WriteLine($"Part2: {part2}");
    }

    private static long Part1(IReadOnlyList<string> input)
    {
        var count = 0;
        for (int row = 0; row < input.Count; row++)
        {
            var line = input[row];
            for (var col = 0; col < line.Length; col++)
            {
                if (line[col] == 'X') count += FindAllXMAS(input, row, col);
            }
        }
        return count;
    }

    private static int FindAllXMAS(IReadOnlyList<string> input, int row, int col)
    {
        return MatchXMAS(input, row, col, -1, -1)
               + MatchXMAS(input, row, col, -1, 0)
               + MatchXMAS(input, row, col, -1, 1)
               + MatchXMAS(input, row, col, 0, -1)
               + MatchXMAS(input, row, col, 0, 1)
               + MatchXMAS(input, row, col, 1, -1)
               + MatchXMAS(input, row, col, 1, 0)
               + MatchXMAS(input, row, col, 1, 1);
    }
    
    private static int MatchXMAS(IReadOnlyList<string> input, int row, int col, int rowDelta, int colDelta)
    {
        return (MatchLetter(input, row + rowDelta, col + colDelta, 'M')
                && MatchLetter(input, row + rowDelta * 2, col + colDelta * 2, 'A')
                && MatchLetter(input, row + rowDelta * 3, col + colDelta * 3, 'S')) ? 1 : 0;
    }

    private static long Part2(IReadOnlyList<string> input)
    {
        var count = 0;
        for (int row = 0; row < input.Count; row++)
        {
            var line = input[row];
            for (var col = 0; col < line.Length; col++)
            {
                if (line[col] == 'A') count += MatchMAS(input, row, col);
            }
        }
        return count;
    }
    
    private static int MatchMAS(IReadOnlyList<string> input, int row, int col)
    {
        var match1 = (MatchLetter(input, row - 1, col - 1, 'M') && MatchLetter(input, row + 1, col + 1, 'S'))
                     || (MatchLetter(input, row - 1, col - 1, 'S') && MatchLetter(input, row + 1, col + 1, 'M'));
        var match2 = (MatchLetter(input, row + 1, col - 1, 'M') && MatchLetter(input, row - 1, col + 1, 'S'))
                     || (MatchLetter(input, row + 1, col - 1, 'S') && MatchLetter(input, row - 1, col + 1, 'M'));
        return match1 && match2 ? 1 : 0;
    }

    private static bool MatchLetter(IReadOnlyList<string> input, int row, int col, char letter)
    {
        if (row < 0 || row >= input.Count) return false;
        if (col < 0 || col >= input[0].Length) return false;
        return input[row][col] == letter;
    }
}