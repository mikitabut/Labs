using System;
using System.Collections.Generic;
using System.Linq;

namespace lab4
{
    public class Program
    {
        public const double FirstIntegralValue = 1.855438667260078401416698965069;
        public const double SecondIntegralValue = Math.PI / 2;

        delegate double Function(double x);

        private static double FirstIntegralFunction(double x) => x/*Math.Exp(-Math.Pow(x, 2))*/;

        public static void Main(string[] args)
        {
            CalculateIntegral(new IntegralInfo("e^(-x^6)", FirstIntegralFunction, FirstIntegralValue, 0, 2, 0, 2));
        }

        private static void CalculateIntegral(IntegralInfo integralInfo)
        {
            int count = 0;
            int[] iterationsAmount = { 10, 100, 1_000, 10_000, 100_000, 1_000_000, 10_000_000 };
            foreach (int n in iterationsAmount)
            {
                var randomX = GetUniformSequence(n, integralInfo.A, integralInfo.B);
                var randomY = GetUniformSequence(n, integralInfo.Min, integralInfo.Max);
                for (int i = 0; i < n; i++)
                {
                    if (randomY[i] < integralInfo.Function(randomX[i]))
                    {
                        count++;
                    }
                }
                double hitPercentage = (double)count / n;
                double monteCarloValue = integralInfo.Area * hitPercentage;
                Console.WriteLine($"n={n}" + Environment.NewLine + 
                    $"Real value        : {integralInfo.Value}" + Environment.NewLine +
                    $"Monte-Carlo value : {monteCarloValue}" + Environment.NewLine);
            }
            Console.ReadKey();
        }

        private static List<double> GetUniformSequence(int amount, double a, double b)
        {
            return GetRandomSequence().Select(x => a + (b - a) * x).ToList();
            IEnumerable<double> GetRandomSequence()
            {
                var random = new Random();
                for (int i = 0; i < amount; i++)
                {
                    yield return random.NextDouble();
                }
            }
        }

        private class IntegralInfo
        {
            public readonly string Info;
            public Function Function;
            public readonly double Value;
            public readonly double Min;
            public readonly double Max;
            public readonly double A;
            public readonly double B;

            public IntegralInfo(string info, Function function, double value, double min, double max, double a, double b)
            {
                Info = info;
                Function = function;
                Value = value;
                Min = min;
                Max = max;
                A = a;
                B = b;
            }

            public double Area => (B - A) * (Max - Min);
        }
    }
}
