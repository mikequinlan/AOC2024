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
    {
        var uniqueAntiNodes = new HashSet<(int row, int col)>();
        var antennas = GetAntennas(input);
        var antennaToRowCol = GetAntennaToRowCol(antennas);
        foreach (var potentialAntiNode in 
                 from antenna in antennas 
                 let sameFrequencyAntennas = antennaToRowCol[antenna.Freq]
                     .Where(rowCol => rowCol.row != antenna.row || rowCol.col != antenna.col)
                     .ToList() 
                 from rowCol in sameFrequencyAntennas 
                 let delta = (row: antenna.row - rowCol.row, col: antenna.col - rowCol.col) 
                 select (row: antenna.row + delta.row, col: antenna.col + delta.col) 
                 into potentialAntiNode 
                 where InGrid(potentialAntiNode) 
                 select potentialAntiNode)
        {
            uniqueAntiNodes.Add(potentialAntiNode);
        }
        return uniqueAntiNodes.Count;
    }

    private static Dictionary<char, List<(int row, int col)>> GetAntennaToRowCol(List<(char Freq, int row, int col)> antennas)
    {
        var antennaToRowCol = antennas.GroupBy(t => t.Freq)
            .ToDictionary(g => g.Key, 
                g => g.Select(t => (t.row, t.col)).ToList());
        return antennaToRowCol;
    }

    private static List<(char Freq, int row, int col)> GetAntennas(IReadOnlyList<string> input)
    {
        var antennas = input.SelectMany((line, row) => line.Select((ch, col) => (Freq: ch, row, col)))
            .Where(t => t.Freq != '.')
            .ToList();
        return antennas;
    }

    private static long Part2(IReadOnlyList<string> input)
    {
        var uniqueAntiNodes = new HashSet<(int row, int col)>();
        var antennas = GetAntennas(input);
        var antennaToRowCol = GetAntennaToRowCol(antennas);
        foreach (var antenna in antennas)
        {
            var sameFrequencyAntennas = antennaToRowCol[antenna.Freq]
                .Where(rowCol => rowCol.row != antenna.row || rowCol.col != antenna.col)
                .ToList();
            foreach (var rowCol in sameFrequencyAntennas)
            {
                var delta = (row: antenna.row - rowCol.row, col: antenna.col - rowCol.col);
                var potentialAntiNode = (row: antenna.row + delta.row, col: antenna.col + delta.col);
                while (InGrid(potentialAntiNode))
                {
                    uniqueAntiNodes.Add(potentialAntiNode);
                    potentialAntiNode = (potentialAntiNode.row + delta.row, potentialAntiNode.col + delta.col);
                }
                potentialAntiNode = (row: antenna.row - delta.row, col: antenna.col - delta.col);
                while (InGrid(potentialAntiNode))
                {
                    uniqueAntiNodes.Add(potentialAntiNode);
                    potentialAntiNode = (potentialAntiNode.row - delta.row, potentialAntiNode.col - delta.col);
                }
            }
        }
        return uniqueAntiNodes.Count;
    }

    private static bool InGrid((int row, int col) rowCol) 
        => rowCol.row >= 0 && rowCol.row < RowCount && rowCol.col >= 0 && rowCol.col < ColCount;
}