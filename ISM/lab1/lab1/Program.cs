using System;
using System.Collections.Generic;
using System.Linq;

namespace lab1
{
    class Program
    {
        public const int K = 64;
        public const uint M = 2147483648; //2^31
        public const int beta = 79507;
        public const double epsilon = 0.05;

        static void Main(string[] args)
        {
            int n = 1000;

            Console.WriteLine("--------------------Multiplicative congruential method--------------------");
            var multiplicativeList = GetMultiplicativeCongruentialSequence(n);
            multiplicativeList.ForEach(x => Console.Write($"{((double)x / M):F5} "));
            Console.WriteLine();
            Console.WriteLine($"Kolmogorov criterion: {Kolmogorov(GetSortedList(multiplicativeList), n)}");
            Console.WriteLine($"Pearson criterion: {Pearson(GetSortedList(multiplicativeList), n)}");

            Console.WriteLine("--------------------MacLaren-Marsaglia method--------------------");
            var maclarenList = GetMacLarenMarsagliaSequence(n);
            maclarenList.ForEach(x => Console.Write($"{((double)x / M):F5} "));
            Console.WriteLine();
            Console.WriteLine($"Kolmogorov criterion: {Kolmogorov(GetSortedList(maclarenList), n)}");
            Console.WriteLine($"Pearson criterion: {Pearson(GetSortedList(maclarenList), n)}");

            Console.ReadKey();
        }

        private static List<int> GetMultiplicativeCongruentialSequence(int amount)
        {
            var multiplicativeList = new List<int>();
            multiplicativeList.Add(beta);
            for (int i = 1; i < amount; i++)
            {
                long x = (long)beta * multiplicativeList[i - 1];
                int value = (int)(x % M);
                multiplicativeList.Add(value);
            }

            return multiplicativeList;
        }

        private static List<int> GetMacLarenMarsagliaSequence(int amount)
        {
            var maclarenList = new List<int>();
            var firstList = GetMultiplicativeCongruentialSequence(amount + K);
            var secondList = GetRandomSequence(amount).ToList();
            var supportingList = firstList.Take(K).ToList();
            for (int i = 0; i < amount; i++)
            {
                int index = (int)((double)secondList[i] / M * K);
                int value = supportingList[index];
                maclarenList.Add(value);
                supportingList[index] = firstList[K + i];
            }

            return maclarenList;
        }

        private static IEnumerable<int> GetRandomSequence(int amount)
        {
            var random = new Random();
            for (int i = 0; i < amount; i++)
            {
                yield return random.Next(0, int.MaxValue);
            }
        }

        private static List<double> GetSortedList(List<int> list) =>
            list.Select(x => (double)x / M).OrderBy(x => x).ToList();

        private static double Kolmogorov(List<double> list, int amount)
        {
            var frequency = new List<double>();
            for (int i = 0; i < amount; i++)
            {
                frequency.Add((double)i / amount);
            }

            var diff = new List<double>();
            for (int i = 0; i < amount; i++)
            {
                diff.Add(Math.Abs(list[i] - frequency[i]));
            }

            double maxDiff = diff.Max();
            return maxDiff / Math.Sqrt(amount);
        }

        private static double Pearson(List<double> list, int amount)
        {
            var frequency = new List<double>();
            for (int i = 0; i < amount; i++)
            {
                frequency.Add((double)i / amount);
            }

            var diff = new List<double>();
            for (int i = 0; i < amount; i++)
            {
                diff.Add(Math.Abs(list[i] - frequency[i]));
            }

            double theor = 1.0 / amount;
            return diff.Sum(x => Math.Pow(x - theor, 2));
        }
    }
}