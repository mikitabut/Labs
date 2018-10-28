using System;
using System.Collections.Generic;
using System.Linq;

namespace lab4
{
    public class Program
    {
        public const double FirstIntegralValue = 1.855438667260078401416698965069;
        public const double SecondIntegralValue = Math.PI / 2;

        public static void Main(string[] args)
        {
            
        }

        private static List<double> GetUniformSequence(double a, double b, int amount)
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
    }
}
