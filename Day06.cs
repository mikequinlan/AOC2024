using System.Data;

namespace AOC;

[Flags]
internal enum Dir
{
    None = 0,
    Up = 0x01, 
    Down = 0x02, 
    Left = 0x04, 
    Right = 0x08,
    All = Up | Down | Left | Right,
}

internal readonly record struct Direction
{
    public static Direction Up => new(Dir.Up,-1, 0, "Up");
    public static Direction Down => new(Dir.Down, 1, 0, "Down");
    public static Direction Left => new(Dir.Left, 0, -1, "Left");
    public static Direction Right => new(Dir.Right, 0, 1, "Right");
    
    public Dir Dir { get; }
    public int RowDelta { get; }
    public int ColDelta { get; }
    public string Name { get; }

    public override string ToString() => Name;

    private Direction(Dir dir, int rowDelta, int colDelta, string name)
    {
        Dir = dir;
        RowDelta = rowDelta;
        ColDelta = colDelta;
        Name = name;
    }

    public Direction Turn()
    {
        if (this == Up) return Right;
        if (this == Down) return Left;
        if (this == Right) return Down;
        if (this == Left) return Up;
        throw new InvalidOperationException();
    }
}

internal static class Program
{
    private const string InputFile = "../../../Data/Data.txt";

    private static void Main()
    {
        var input = File.ReadLines(InputFile).ToList();
        var part1 = Part1(input);
        Console.WriteLine($"Part1: {part1}");
        var part2 = Part2(input);
        Console.WriteLine($"Part2: {part2}");
    }

    private static long Part1(IReadOnlyList<string> input)
    {
        var (startRow, startCol, direction) = FindStart(input);
        var data = GetData(input);

        while (true)
        {
            data[startRow, startCol] = 'X';
            var newRow = startRow + direction.RowDelta;
            var newCol = startCol + direction.ColDelta;
            var nextChar = GetValue(data, newRow, newCol);
            if (nextChar == '*') break;
            if (nextChar == '#')
            {
                direction = direction.Turn();
                continue;
            }
            startRow = newRow;
            startCol = newCol;
        }

        var count = 0;
        for (var row = 0; row < data.GetLength(0); row++)
        {
            for (var col = 0; col < data.GetLength(1); col++)
            {
                if (data[row, col] == 'X') ++count;
            }
        }
        return count;
    }
    
    private static long Part2(IReadOnlyList<string> input)
    {
        var (startRow, startCol, direction) = FindStart(input);
        var possibilities = new List<(int row, int col)>();
        for (var row = 0; row < input.Count; row++)
        {
            for (var col = 0; col < input[0].Length; col++)
            {
                if (input[row][col] is '.') possibilities.Add((row, col));
            }
        }

        var loopCount = 0;
        foreach (var possibility in possibilities)
        {
            var data = GetData(input);
            data[possibility.row, possibility.col] = '#';
            var loop = MarkPath(data, startRow, startCol, direction);
            if (loop) ++loopCount;
        }
        return loopCount;
    }

    private static char[,] GetData(IReadOnlyList<string> input)
    {
        var data = new char[input.Count, input[0].Length];
        for (var row = 0; row < input.Count; row++)
        {
            for (var col = 0; col < input[0].Length; col++) data[row, col] = input[row][col];
        }
        return data;
    }

    private static Dir[,] GetVisited(char[,] data)
    {
        var visited = new Dir[data.GetLength(0), data.GetLength(1)];
        for (var row = 0; row < data.GetLength(0); row++)
        {
            for (var col = 0; col < data.GetLength(1); col++) visited[row, col] = Dir.None;
        }
        return visited;
    }

    private static bool MarkPath(char[,] data, int startRow, int startCol, Direction direction)
    {
        var visited = GetVisited(data);
        var row = startRow;
        var col = startCol;
        while (true)
        {
            var dir = visited[row, col];
            if (dir.HasFlag(direction.Dir)) return true;
            visited[row, col] |= direction.Dir;
            var newRow = row + direction.RowDelta;
            var newCol = col + direction.ColDelta;
            var nextChar = GetValue(data, newRow, newCol);
            if (nextChar == '*') break;
            if (nextChar == '#')
            {
                direction = direction.Turn();
                continue;
            }
            row = newRow;
            col = newCol;
        }
        return false;
    }

    private static (int row, int col, Direction) FindStart(IReadOnlyList<string> input)
    {
        for (var row = 0; row < input.Count; row++)
        {
            for (var col = 0; col < input[0].Length; col++)
            {
                if (input[row][col] is '^') return (row, col, Direction.Up);
                if (input[row][col] is 'v' or 'V') return (row, col, Direction.Down);
                if (input[row][col] is '>') return (row, col, Direction.Right);
                if (input[row][col] is '<') return (row, col, Direction.Left);
            }
        }
        throw new ArgumentException("Invalid input");
    }

    private static char GetValue(char[,] data, int row, int col)
    {
        if (row is < 0 || row >= data.GetLength(0)) return '*';
        if (col is < 0 || col >= data.GetLength(1)) return '*';
        return data[row, col];
    }
}