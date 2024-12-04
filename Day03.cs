    using System.Text.RegularExpressions;

    namespace AOC;

    internal static class Program
    {
        private const string InputFile = "../../../Data/Data.txt";
        private static readonly Regex Regex1 = new(@"mul\(([0-9]{1,3}),([0-9]{1,3})\)", RegexOptions.Multiline);
        private static readonly Regex Regex2 = new(@"(mul\(([0-9]{1,3}),([0-9]{1,3})\))|(do\(\))|(don't\(\))", RegexOptions.Multiline);

        private static void Main()
        {
            var input = File.ReadAllText(InputFile);
            var part1 = Part1(input);
            Console.WriteLine($"Part1: {part1}");
            var part2 = Part2(input);
            Console.WriteLine($"Part2: {part2}");
        }

        private static long Part1(string input)
        {
            var matches = Regex1.Matches(input);
            var sum = 0;
            foreach (Match match in matches)
            {
                var n1 = int.Parse(match.Groups[1].Value);
                var n2 = int.Parse(match.Groups[2].Value);
                sum += n1 * n2;
            }
            return sum;
        }

        private static long Part2(string input)
        {
            var matches = Regex2.Matches(input);
            var sum = 0;
            var enabled = true;
            foreach (Match match in matches)
            {
                if (match.Captures[0].Value.StartsWith("mul"))
                {
                    if (enabled)
                    {
                        var n1 = int.Parse(match.Groups[2].Value);
                        var n2 = int.Parse(match.Groups[3].Value);
                        sum += n1 * n2;
                    }
                }
                else if (match.Captures[0].Value.StartsWith("don't")) enabled = false;
                else if (match.Captures[0].Value.StartsWith("do")) enabled = true;
            }
            return sum;
        }
    }