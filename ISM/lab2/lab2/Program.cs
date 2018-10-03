using System;
using System.Collections.Generic;
using System.Linq;

namespace lab2
{
    public class Program
    {
        #region Constants

        public const double Epsilon = 0.05;
        public const int N = 1000;
        public const int CriteriaItemsAmount = 10;
        public const double BernulliP = 0.5;
        public const int BinomialM = 5;
        public const double BinomialP = 0.25;
        public const double GeometricalP = 0.6;
        public const double PuassonLambda = 2;
        public const int NegativeBinomialR = 5;
        public const double NegativeBinomialP = 0.25;
        public static Dictionary<double, double> Pearson49Quantiles = new Dictionary<double, double>()
        {
            { 0.01, 74.919 },
            { 0.025, 70.222 },
            { 0.05, 66.339 },
            { 0.10, 62.038 },
            { 0.20, 57.059 },
            { 0.30, 53.67 }
        };

        #endregion

        static void Main(string[] args)
        {
            Console.WriteLine($"Epsilon={Epsilon}, N={N}, CriteriaItemsAmount={CriteriaItemsAmount}");
            Console.WriteLine(Environment.NewLine + $"-----Bernulli distribution (P={BernulliP})-----");
            DisplayResults(GetBernulliSequence(BernulliP), 
                BernulliProbabilityFunction, BernulliMathematicalExpectation, BernulliDispersion);
            Console.WriteLine(Environment.NewLine + $"-----Binomial distribution (M={BinomialM}, P={BinomialP})-----");
            DisplayResults(GetBinomialSequence(N, BinomialM, BinomialP), 
                BinomialProbabilityFunction, BinomialMathematicalExpectation, BinomialDispersion);
            Console.WriteLine(Environment.NewLine + $"-----Geometrical distribution (P={GeometricalP})-----");
            DisplayResults(GetGeometricalSequence(GeometricalP), 
                GeometricalProbabilityFunction, GeometricalMathematicalExpectation, GeometricalDispersion);
            Console.WriteLine(Environment.NewLine + $"-----Puasson distribution (Lamdba={PuassonLambda})-----");
            DisplayResults(GetPuassonSequence(N, PuassonLambda), 
                PuassonProbabilityFunction, PuassonMathematicalExpectation, PuassonDispersion);
            Console.WriteLine(Environment.NewLine + $"-----Negative Binomial distribution (R={NegativeBinomialR}, P={NegativeBinomialP})-----");
            DisplayResults(GetNegativeBinomialSequence(N, NegativeBinomialR, NegativeBinomialP), 
                NegativeBinomialProbabilityFunction, NegativeBinomialMathematicalExpectation, NegativeBinomialDispersion);
            Console.ReadKey();
        }

        private static void DisplayResults(IEnumerable<double> results, ProbabilityFunction probabilityFunction, 
            MathematicalExpectation mathematicalExpectation, Dispersion dispersion)
        {
            results.Take(30).ToList().ForEach(x => Console.Write($"{x:F0} "));
            Console.Write(Environment.NewLine + $"{results.Count()} items: ");
            for (int i = 0; i < 8; i++)
            {
                Console.Write($"{i}({results.Count(x => x == i)}) ");
            }

            Console.WriteLine();
            Console.WriteLine($"Practical  : Average={results.Average():F5}, Dispersion={CalculateDispersion(results.ToList()):F5}");
            Console.WriteLine($"Theoretical: Average={mathematicalExpectation():F5}, Dispersion={dispersion():F5}");
            CheckPearsonCriteria(GetSortedList(results), probabilityFunction);
        }

        #region Sequence modeling algorithms

        private static IEnumerable<double> GetBernulliSequence(double p) =>
            GetRandomSequence(N).Select(x => x <= p ? 1.0 : 0.0);

        private static IEnumerable<double> GetBinomialSequence(int amount, int m, double p)
        {
            var randomSequence = GetRandomSequence(amount * m);
            for (int i = 0; i < amount; i++)
            {
                var randomSubsequence = randomSequence.Skip(i * m).Take(m).ToList();
                double value = 0;
                randomSubsequence.ForEach(x => value += p > x ? 1 : 0);
                yield return value;
            }
        }

        private static IEnumerable<double> GetGeometricalSequence(double p) =>
            GetRandomSequence(N).Select(x => Math.Floor(Math.Log(x) / Math.Log(1 - p)));

        private static IEnumerable<double> GetPuassonSequence(int amount, double lamdba)
        {
            var randomSequence = GetRandomSequence((int)(amount * lamdba * 10));
            var eInMinusLambda = Math.Pow(Math.E, -lamdba);
            int randomSequenceCounter = 0;
            for (int i = 0; i < amount; i++)
            {
                double currentMultiplication = randomSequence.ElementAt(randomSequenceCounter);
                randomSequenceCounter++;
                double counter = 0;
                while (currentMultiplication >= eInMinusLambda)
                {
                    counter++;
                    currentMultiplication *= randomSequence.ElementAt(randomSequenceCounter);
                    randomSequenceCounter++;
                }

                yield return counter;
            }
        }

        private static IEnumerable<double> GetNegativeBinomialSequence(int amount, int r, double p)
        {
            var randomSequence = GetRandomSequence((int)(amount * 2 * r * (1 - p) / p));
            int randomSequenceCounter = 0;
            for (int i = 0; i < amount; i++)
            {
                var smallerThanPCounter = 0;
                var biggerThanPCounter = 0;
                while (smallerThanPCounter < r)
                {
                    var randomElement = randomSequence.ElementAt(randomSequenceCounter);
                    randomSequenceCounter++;
                    if (randomElement < p)
                    {
                        smallerThanPCounter++;
                    }
                    else
                    {
                        biggerThanPCounter++;
                    }
                }

                yield return biggerThanPCounter;
            }
        }

        #endregion

        #region Probability functions

        delegate double ProbabilityFunction(int x);

        private static double BernulliProbabilityFunction(int x)
        {
            if (x == 1)
            {
                return BernulliP;
            }
            else if (x == 0)
            {
                return 1 - BernulliP;
            }
            else
            {
                return 0;
            }
        }

        private static double BinomialProbabilityFunction(int x) =>
            Math.Pow(BinomialP, x) * Math.Pow(1 - BinomialP, BinomialM - x) *
                Factorial(BinomialM) / (Factorial(BinomialM - x) * Factorial(x));

        private static double GeometricalProbabilityFunction(int x) => GeometricalP * Math.Pow(1 - GeometricalP, x);

        private static double PuassonProbabilityFunction(int x) =>
            Math.Pow(Math.E, -PuassonLambda) * Math.Pow(PuassonLambda, x) / Factorial(x);

        private static double NegativeBinomialProbabilityFunction(int x) =>
            Math.Pow(NegativeBinomialP, NegativeBinomialR) * Math.Pow(1 - NegativeBinomialP, x) *
                Factorial(x + NegativeBinomialR - 1) / (Factorial(NegativeBinomialR - 1) * Factorial(x));

        #endregion

        #region Mathematical expectations

        delegate double MathematicalExpectation();

        private static double BernulliMathematicalExpectation() => BernulliP;

        private static double BinomialMathematicalExpectation() => BinomialM * BinomialP;

        private static double GeometricalMathematicalExpectation() => (1 - GeometricalP) / GeometricalP;

        private static double PuassonMathematicalExpectation() => PuassonLambda;

        private static double NegativeBinomialMathematicalExpectation() =>
            NegativeBinomialR * (1 - NegativeBinomialP) / NegativeBinomialP;

        #endregion

        #region Dispersions

        delegate double Dispersion();

        private static double BernulliDispersion() => BernulliP * (1 - BernulliP);

        private static double BinomialDispersion() => BinomialM * BinomialP * (1 - BinomialP);

        private static double GeometricalDispersion() => (1 - GeometricalP) / Math.Pow(GeometricalP, 2);

        private static double PuassonDispersion() => PuassonLambda;

        private static double NegativeBinomialDispersion() =>
            NegativeBinomialR * (1 - NegativeBinomialP) / Math.Pow(NegativeBinomialP, 2);

        #endregion

        #region Pearson criteria

        private static void CheckPearsonCriteria(List<double> list, ProbabilityFunction function)
        {
            var probabilityFunctionValues = GetProbabilityFunctionValues(function, CriteriaItemsAmount).ToList();
            var valuesAmount = GetAmountByValues(list).ToList();
            double value = 0;
            for (int i = 0; i < CriteriaItemsAmount; i++)
            {
                if (probabilityFunctionValues[i] != 0)
                {
                    value += Math.Pow(valuesAmount[i] - N * probabilityFunctionValues[i], 2) / (N * probabilityFunctionValues[i]);
                }
            }

            Console.WriteLine($"Pearson criterion: {DisplayCriteriaResults(value, Pearson49Quantiles[Epsilon])}");
        }

        //For Puasson: 0.220, 0.333, 0.252, 0.127, 0.048, 0.015, 0.005
        private static IEnumerable<double> GetProbabilityFunctionValues(ProbabilityFunction function, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                yield return function(i);
            }
        }

        private static IEnumerable<int> GetAmountByValues(List<double> list)
        {
            for (int i = 0; i < CriteriaItemsAmount; i++)
            {
                yield return list.Count(x => x == i);
            }
        }

        private static string DisplayCriteriaResults(double value, double quantile) => value < quantile ?
            $"{value:F5} < {quantile:F3}, so it's passed" : $"{value:F5} > {quantile:F3}, so it isn't passed";

        #endregion

        #region Supporting functions

        private static double CalculateDispersion(List<double> list) =>
            list.Sum(x => Math.Pow(x - list.Average(), 2)) / list.Count;

        private static long Factorial(long x) => x <= 1 ? 1 : x * Factorial(x - 1);

        private static IEnumerable<double> GetRandomSequence(int amount)
        {
            var random = new Random();
            for (int i = 0; i < amount; i++)
            {
                yield return random.NextDouble();
            }
        }

        private static List<double> GetSortedList(IEnumerable<double> list) => list.OrderBy(x => x).ToList();

        #endregion
    }
}
