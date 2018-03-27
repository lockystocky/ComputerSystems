using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entropy
{
    public class InfoResearch
    {
        public static double GetSymbolFrequency(char symbol, string filePath)
        {
            string text = File.ReadAllText(filePath, Encoding.Default);
            double symbCount = text.Where(s => s == symbol).Count();
            int fileSymbCount = text.Count();
            if (fileSymbCount == 0) return 0;
            return (symbCount / fileSymbCount);
        }

        public static Dictionary<char, double> GetSymbolsFrequencies(string filePath)
        {
            Dictionary<char, double> frequencies = new Dictionary<char, double>();
            string text = File.ReadAllText(filePath, Encoding.Default);

            foreach (var symbol in text)
            {
                if (!frequencies.ContainsKey(symbol))
                {
                    frequencies.Add(symbol, GetSymbolFrequency(symbol, filePath));
                }
            }

            return frequencies;
        }

        public static double GetAvgEntropy(string filePath)
        {
            var symbols = File.ReadAllText(filePath, Encoding.Default).Distinct();
            double entropy = 0;
            foreach (var symbol in symbols)
            {
                double frequency = GetSymbolFrequency(symbol, filePath);
               if(frequency != 0) entropy += frequency * Math.Log(frequency, 2);
            }
            entropy = -entropy;
            return entropy;
        }

        public static double GetInfoAmount(string filePath)
        {
            var symbolsCount = File.ReadAllText(filePath, Encoding.Default).Count();
                
            return symbolsCount * GetAvgEntropy(filePath) / 8;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string filePath = "D:\\entropyPCIbz2.txt";
            var frequencies = InfoResearch.GetSymbolsFrequencies(filePath);
            foreach(var frequency in frequencies)
            {
                Console.WriteLine("{0} : {1}", frequency.Key, frequency.Value);
            }
            Console.WriteLine("--------------------");
            Console.WriteLine("Entropy (bits): {0}", InfoResearch.GetAvgEntropy(filePath));
            Console.WriteLine("--------------------");
            Console.WriteLine("Amount of information (bytes): {0}", InfoResearch.GetInfoAmount(filePath));
            Console.WriteLine("--------------------");
            Console.WriteLine("File size (bytes): {0}", new FileInfo(filePath).Length);

            Console.ReadKey();

        }
    }
}
