namespace AOC;

internal enum ItemType { Lock, Key }

internal abstract class Item(ItemType itemType, IReadOnlyList<int> heights)
{
    public ItemType ItemType => itemType;
    public IReadOnlyList<int> Heights => heights;

    public bool Fits(Item item) 
        => !heights.Where((_, i) => Heights[i] + item.Heights[i] > heights.Count).Any();
}

internal class KeyItem(IReadOnlyList<int> heights) : Item(ItemType.Key, heights);

internal class LockItem(IReadOnlyList<int> heights) : Item(ItemType.Lock, heights);

internal static class Program
{
    private const string InputFile = "../../../Data/Data.txt";

    private static void Main()
    {
        var items = Parse(File.ReadAllLines(InputFile)).ToList();
        var part1 = Part1(items);
        Console.WriteLine($"Part1: {part1}");
    }

    private static IEnumerable<Item> Parse(IReadOnlyList<string> input)
    {
        for (var i = 0; i < input.Count; i += 8)
        {
            if (input[i].Equals("#####"))
            {
                yield return GetLock(input[i + 1], input[i + 2], input[i + 3], input[i + 4], input[i + 5]);
            }
            else
            {
                yield return GetKey(input[i + 5], input[i + 4], input[i + 3], input[i + 2], input[i + 1]);
            }
        }

        static LockItem GetLock(string s1, string s2, string s3, string s4, string s5) 
            => new(GetHeights(s1, s2, s3, s4, s5));

        static KeyItem GetKey(string s1, string s2, string s3, string s4, string s5)
            => new(GetHeights(s1, s2, s3, s4, s5));

        static int[] GetHeights(string s1, string s2, string s3, string s4, string s5) 
            => 
            [
                GetHeight(s1[0], s2[0], s3[0], s4[0], s5[0]),
                GetHeight(s1[1], s2[1], s3[1], s4[1], s5[1]),
                GetHeight(s1[2], s2[2], s3[2], s4[2], s5[2]),
                GetHeight(s1[3], s2[3], s3[3], s4[3], s5[3]),
                GetHeight(s1[4], s2[4], s3[4], s4[4], s5[4])
            ];

        static int GetHeight(char c1, char c2, char c3, char c4, char c5) 
            => c5 is '#' ? 5 : c4 is '#' ? 4 : c3 is '#' ? 3 : c2 is '#' ? 2 : c1 is '#' ? 1 : 0;
    }

    private static long Part1(IReadOnlyList<Item> items) 
        => items.Where(i => i is LockItem)
            .Sum(lockItem => items.Where(i => i is KeyItem).Count(keyItem => keyItem.Fits(lockItem)));
}