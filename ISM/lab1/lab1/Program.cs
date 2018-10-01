using System;
using System.Collections.Generic;
using System.Linq;

namespace lab1
{
    public class Program
    {
        public static Dictionary<double, double> KolmogorovQuantiles = new Dictionary<double, double>()
        {
            { 0.005, 1.73 },
            { 0.01, 1.63 },
            { 0.025, 1.48 },
            { 0.05, 1.36 },
            { 0.10, 1.22 },
            { 0.15, 1.14 },
            { 0.20, 1.07 },
            { 0.25, 1.02 }
        };

        public static Dictionary<double, double> Pearson50Quantiles = new Dictionary<double, double>()
        {
            { 0.01, 76.154 },
            { 0.025, 71.42 },
            { 0.05, 67.50 },
            { 0.10, 63.167 },
            { 0.20, 58.164 },
            { 0.30, 54.723 }
        };

        public const int K = 64;
        public const uint M = 2147483648; //2^31
        public const int beta = 79507;
        public const double epsilon = 0.05;
        public const int n = 10;

        delegate double DistributionFunction(double x);

        private static double Uniform(double x) => x;

        readonly static DistributionFunction function = new DistributionFunction(Uniform);

        static void Main(string[] args)
        {

            Console.WriteLine("--------------------Multiplicative congruential method--------------------");
            DisplayResults(GetMultiplicativeCongruentialSequence(n));
            Console.WriteLine("--------------------MacLaren-Marsaglia method--------------------");
            DisplayResults(GetMacLarenMarsagliaSequence(n));
            Console.ReadKey();
        }

        private static void DisplayResults(List<double> results)
        {
            results.Take(10).ToList().ForEach(x => Console.Write($"{x:F5} "));
            Console.WriteLine();
            CheckKolmogorovCriteria(GetSortedList(results), function);
            CheckPearsonCriteria(GetSortedList(results), function);
        }

        private static List<double> GetMultiplicativeCongruentialSequence(int amount)
        {
            var multiplicativeList = new List<double>();
            multiplicativeList.Add((double)beta / M);
            for (int i = 1; i < amount; i++)
            {
                long x = (long)(beta * multiplicativeList[i - 1] * M);
                double value = (double)(x % M) / M;
                multiplicativeList.Add(value);
            }

            return multiplicativeList;
        }

        private static List<double> GetMacLarenMarsagliaSequence(int amount)
        {
            var maclarenList = new List<double>();
            var firstList = GetMultiplicativeCongruentialSequence(amount + K);
            var secondList = GetRandomSequence(amount).ToList();
            var supportingList = firstList.Take(K).ToList();
            for (int i = 0; i < amount; i++)
            {
                int index = (int)(secondList[i] / M * K);
                double value = supportingList[index];
                maclarenList.Add(value);
                supportingList[index] = firstList[K + i];
            }

            return maclarenList;
        }

        private static IEnumerable<double> GetRandomSequence(int amount)
        {
            var random = new Random();
            for (int i = 0; i < amount; i++)
            {
                yield return random.NextDouble();
            }
        }

        private static List<double> GetSortedList(List<double> list) => list.OrderBy(x => x).ToList();

        private static void CheckKolmogorovCriteria(List<double> list, DistributionFunction function)
        {
            var empiricalDistributionFunction = GetEmpiricalDistributionFunction(function, list.Count).ToList();
            double maxDiff = 0;
            for (int i = 0; i < list.Count; i++)
            {
                if (Math.Abs(list[i] - empiricalDistributionFunction[i]) > maxDiff)
                {
                    maxDiff = Math.Abs(list[i] - empiricalDistributionFunction[i]);
                }
                if (Math.Abs(list[i] - empiricalDistributionFunction[i + 1]) > maxDiff)
                {
                    maxDiff = Math.Abs(list[i] - empiricalDistributionFunction[i + 1]);
                }
            }

            var value = maxDiff * Math.Sqrt(list.Count);
            Console.WriteLine($"Kolmogorov criterion: {DisplayCriteriaResults(value, KolmogorovQuantiles[epsilon])}");
        }

        private static void CheckPearsonCriteria(List<double> list, DistributionFunction function)
        {
            var empiricalDistributionFunction = GetEmpiricalDistributionFunction(function, list.Count).ToList();
            var diff = new List<double>();
            for (int i = 0; i < list.Count; i++)
            {
                diff.Add(Math.Abs(list[i] - empiricalDistributionFunction[i]));
            }

            double theor = 1.0 / list.Count;
            var value = diff.Sum(x => Math.Pow(x - theor, 2));
            Console.WriteLine($"Pearson criterion: {DisplayCriteriaResults(value, Pearson50Quantiles[epsilon])}");
        }

        private static IEnumerable<double> GetEmpiricalDistributionFunction(DistributionFunction function, int amount)
        {
            for (int i = 0; i < amount + 1; i++)
            {
                yield return function((double)i / amount);
            }
        }

        private static string DisplayCriteriaResults(double value, double quantile) => value < quantile ?
            $"{value:F5} < {quantile:F3}, so it's passed" : $"{value:F5} > {quantile:F3}, so it isn't passed";
    }
}
