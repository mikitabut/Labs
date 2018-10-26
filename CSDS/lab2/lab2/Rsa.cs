using NLog;
using System.Numerics;

namespace lab2
{
    public class Rsa
    {
        public static Logger Logger = LogManager.GetCurrentClassLogger();

        public static string Generate()
        {
            Logger.Info("Starting RSA key generation...");
            var p = BigInteger.Parse("3557");
            Logger.Info("First prime number has been generated: " + p);
            var q = BigInteger.Parse("2579");
            Logger.Info("Second prime number has been generated: " + q);
            var openExponent = BigInteger.Parse("3");
            Logger.Info("Open exponent has been generated: " + openExponent);
            var eulerFunctionValue = GetEulerFunctionValue(p, q);
            Logger.Info("Euler function value has been calculated: " + eulerFunctionValue);
            var privateExponent = GetPrivateExponent(openExponent, eulerFunctionValue);
            Logger.Info("Private exponent has been calculated: " + privateExponent);
            Logger.Info("Finishing RSA key generation...");
            return $"{openExponent},{p*q}";
        }

        private static BigInteger GetEulerFunctionValue(BigInteger p, BigInteger q) => BigInteger.Multiply(p - 1, q - 1);

        private static BigInteger GetPrivateExponent(BigInteger openExponent, BigInteger eulerFunctionValue)
        {
            int i = 1;
            while (true)
            {
                var multipliedEulerFunctionValue = eulerFunctionValue * i;
                var quotient = BigInteger.Divide(multipliedEulerFunctionValue, openExponent);
                if (multipliedEulerFunctionValue - quotient * openExponent == openExponent - 1)
                {
                    return quotient + 1;
                }
                i++;
            }
        }

        public static BigInteger ModPow(string text, string openExponent, string rsaKey) =>
            ModPow(BigInteger.Parse(text), BigInteger.Parse(openExponent), BigInteger.Parse(rsaKey));

        public static BigInteger ModPow(BigInteger text, BigInteger openExponent, BigInteger rsaKey) 
            => BigInteger.ModPow(text, openExponent, rsaKey);
    }
}
