using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base64
{

    public class Base32Converter
    {
        public static string ToBase32(string input)
        {
            string base64arr = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
            string base64Str = "";
            int i;
           
            string binaryStr = input.Select(s => BinaryConverter.ToBinary(s)).Aggregate((k, l) => k + l);
            
            for (i = 0; i < (binaryStr.Length - 5); i += 6)
            {
                int symbNumb = Convert.ToInt32(binaryStr.Substring(i, 6), 2);
                base64Str += base64arr[symbNumb];
            }

            if(i < (binaryStr.Length - 1))
            {
                string s = binaryStr.Substring(i);
                s += Enumerable.Repeat("0", 6 - s.Length).Aggregate((k, l) => k + l);
                int ss = Convert.ToInt32(s, 2);
                base64Str += base64arr[ss];
            }

            base64Str += Enumerable.Repeat("=", binaryStr.Length % 3).Aggregate((k, l) => k + l);

            return base64Str;

        }

        public static void ToBase32(string inputPath, string outputPath)
        {
            string simpleText = File.ReadAllText(inputPath, Encoding.Default);
            var simpleTextBytes = Encoding.UTF8.GetBytes(simpleText);
            string encodedText = Convert.ToBase64String(simpleTextBytes);
            File.WriteAllText(outputPath, encodedText);
        }
    }

    public class BinaryConverter
    {
        public static string ToBinary(int symbol)
        {
            int k = 128;
            string binaryStr = "";
            while (k > 0)
            {
                binaryStr += Convert.ToInt32((k & symbol) != 0);
                k = k >> 1;
            }
            return binaryStr;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            
            Base32Converter.ToBase32("D:\\entropyPCI.txt.bz2", "D:\\entropyPCIbz2.txt");

            
            Console.ReadKey();
        }
    }
}
