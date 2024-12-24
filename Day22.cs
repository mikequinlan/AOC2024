namespace AOC;

internal static class Program
{
    private const string InputFile = "../../../Data/Data.txt";

    private static void Main()
    {
        var input = File.ReadLines(InputFile).Select(long.Parse).ToList();
        var part1 = Part1(input);
        Console.WriteLine($"Part1: {part1}");
        var part2 = Part2(input);
        Console.WriteLine($"Part2: {part2}");
    }

    private static long Part1(IReadOnlyList<long> input) 
        => input.Select(n => GetSecretNumbers(n, 2000).Last()).Sum();

    private static IEnumerable<long> GetSecretNumbers(long secretNumber, int count)
    {
        yield return secretNumber;
        for (var i = 0; i < count; i++)
        {
            secretNumber = CalcNext(secretNumber);
            yield return secretNumber;
        }

        static long CalcNext(long n)
        {
            var n1 = MixPrune(n, n * 64);
            var n2 = MixPrune(n1, n1 / 32);
            var n3 = MixPrune(n2, n2 * 2048);
            return n3;
            
            long MixPrune(long a, long b) => (a ^ b) % 16_777_216;
        }
    }

    private static IEnumerable<int> GetPriceChanges(IReadOnlyList<int> prices)
    {
        for (var i = 1; i < prices.Count; i++) yield return prices[i] - prices[i - 1];
    }

    private static IEnumerable<int> GetPrices(IEnumerable<long> secretNumbers) =>
        secretNumbers.Select(n => (int)(n % 10));

    private static IEnumerable<(int d1, int d2, int d3, int d4)> GetPossibleTriggers(IReadOnlyList<int> priceChanges)
    {
        for (var i = 0; i < priceChanges.Count - 4; i++)
        {
            yield return (priceChanges[i], priceChanges[i + 1], priceChanges[i + 2], priceChanges[i + 3]);
        }
    }

    private static int BuyBananas(IReadOnlyList<int> prices, (int d1, int d2, int d3, int d4) trigger)
    {
        var priceChanges = GetPriceChanges(prices).ToList();
        for (var i = 0; i < priceChanges.Count - 4; i++)
        {
            if (priceChanges[i] == trigger.d1 && priceChanges[i + 1] == trigger.d2 &&
                priceChanges[i + 2] == trigger.d3 && priceChanges[i + 3] == trigger.d4)
            {
                return prices[i + 4];
            }
        }
        return 0;
    }
    
    private static long Part2(IReadOnlyList<long> input)
    {
        var lockObj = new object();
        var pricesLists = input.Select(n => GetSecretNumbers(n, 1999))
            .Select(longs => GetPrices(longs).ToList())
            .ToList();
        var possibleTriggers = pricesLists.Select(prices => GetPriceChanges(prices).ToList())
            .SelectMany(GetPossibleTriggers)
            .Distinct()
            .ToList();
        var mostBananas = int.MinValue;
        Parallel.ForEach(possibleTriggers, () => int.MinValue, (trigger, _, max) =>
            {
                var bananasBought = pricesLists.Select(p => BuyBananas(p, trigger)).Sum();
                return Math.Max(bananasBought, max);
            },
            max =>
            {
                lock (lockObj)
                {
                    mostBananas = Math.Max(mostBananas, max);
                }
            });
        return mostBananas;
    }
}