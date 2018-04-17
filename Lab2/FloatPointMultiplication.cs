using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloatPointMultiplication
{
    public class BinaryConverter
    {

        public static string Add(string C, string A)
        {
            string c = "";
            string a = A;
            while (a.Length < C.Length)
            {
                a += "0";
            }
            bool carry = false;
            for (int i = a.Length - 1; i >= 0; i--)
            {

                if (a[i] == C[i])
                {

                    if (carry)
                    {
                        c = "1" + c;
                        if (a[i] == '0')
                            carry = false;
                    }
                    else
                    {
                        c = "0" + c;
                        if (a[i] == '1')
                            carry = true;
                    }
                }
                else
                {
                    if (carry)
                        c = "0" + c;
                    else
                        c = "1" + c;
                }


            }
            if (carry) c = "1" + c;

            return c;
        }

        public static string ToRegister(string binNumber, int register)
        {
            while (binNumber.Length < register)
            {
                binNumber = "0" + binNumber;
            }
            return binNumber;
        }

        public static string RightShift(string number)
        {
            return ("0" + number).Substring(0, number.Length);
        }

        public static string LeftShift(string number)
        {
            return (number + "0").Substring(1);
        }


        public static string MultiplyWhithRightShift(string A, string B, int register)
        {
            A = ToRegister(A, register);
            B = ToRegister(B, register);
            string C = ToRegister("0", register * 2);
            for (int i = B.Length - 1; i >= 0; i--)
            {

                //Console.WriteLine("----------------------");
                if (B[i] == '1')
                    C = Add(C, A);
                C = RightShift(C);
                //Console.WriteLine("i = {0}, B[i] = {2}:  A = {1}", i, A, B[i]);
                //Console.WriteLine("i = {0}, B[i] = {2}:  B = {1}", i, B, B[i]);
                //Console.WriteLine("i = {0}, B[i] = {2}:  C = {1}", i, C, B[i]);
            }
            return C;
        }

        public static int FromBinaryToDecimal(string number)
        {
            int decimalNumber = 0;
            int powerOfTwo = 0;
            for (int i = number.Length - 1; i >= 0; i--)
            {
                if (number[i] == '1')
                {
                    decimalNumber += (int)Math.Pow(2, powerOfTwo);
                }
                powerOfTwo++;
            }
            return decimalNumber;
        }

        public static double FromBinaryToFloat(string sign, string exponenta, string mantissa)
        {
            string bstr = sign + exponenta + mantissa;
            long v = 0;
            for (int i = bstr.Length - 1; i >= 0; i--)
                v = (v << 1) + (bstr[i] - '0');
            double d = BitConverter.ToDouble(BitConverter.GetBytes(v), 0);
            // d = 1.41466386031414E-314
            return d;
           // return BitConverter.Int64BitsToDouble(Convert.ToInt64(bstr, 2));
        }
    }
    class Program
    {
        static string ToBinaryString(float value)
        {

            int bitCount = sizeof(float) * 8; 
            char[] result = new char[bitCount]; 
            int intValue = System.BitConverter.ToInt32(BitConverter.GetBytes(value), 0);

            for (int bit = 0; bit < bitCount; ++bit)
            {
                int maskedValue = intValue & (1 << bit); 
                if (maskedValue > 0)
                    maskedValue = 1;
                result[bitCount - bit - 1] = maskedValue.ToString()[0];
            }

            return new string(result); 
        }

        public static void FloatPointMultiplication(float a, float b)
        {
            string aBinary = ToBinaryString(a);
            string bBinary = ToBinaryString(b);
            string aMantissa = "1" + aBinary.Substring(9);
            string bMantissa = "1" + bBinary.Substring(9);
            string resultMantissa = BinaryConverter.MultiplyWhithRightShift(aMantissa, bMantissa, 24);
            while (resultMantissa.StartsWith("0") && resultMantissa.Length > 48)
            {
                resultMantissa = resultMantissa.Substring(1);
            }
            string exponent = "0";
            if (resultMantissa.StartsWith("1"))
            {
                exponent = "1";
                resultMantissa = resultMantissa.Substring(1, 23);
            }

            exponent = BinaryConverter.ToRegister(exponent, 8);
            string aExp = aBinary.Substring(1, 8);
            string bExp = bBinary.Substring(1, 8);

            string signOfResult = "";
            if (aBinary[0] == bBinary[0])
                signOfResult = "0";
            else
                signOfResult = "1";            
            
            string exponentOfResult = BinaryConverter.Add(aExp, bExp);
            while (exponentOfResult.Length > 8)
            {
                exponentOfResult = exponentOfResult.Substring(1);
            }

            exponentOfResult = BinaryConverter.Add(exponentOfResult, exponent);
            //bias = 2^(e-1) - 1 = 2^(8 - 1) - 1 = 127,   -127 = 10000001
            exponentOfResult = BinaryConverter.Add(exponentOfResult, "10000001");

            Console.WriteLine("aExponent = " + aExp);
            Console.WriteLine("bExponent = " + bExp);
            Console.WriteLine("exponent of result = aExponent + bExponent + bias (10000001)");
            Console.WriteLine("exponent of result = " + exponentOfResult);
            Console.WriteLine();
            Console.WriteLine("aSign = " + aBinary[0]);
            Console.WriteLine("bSign = " + bBinary[0]);
            Console.WriteLine("sign of result = " + signOfResult);
            Console.WriteLine();
            Console.WriteLine("aMantissa = " + aMantissa);
            Console.WriteLine("bMantissa = " + bMantissa);
            Console.WriteLine("mantissa of result = aMantissa * bMantissa");
            Console.WriteLine("mantissa of result = " + resultMantissa);
            Console.WriteLine();
            Console.WriteLine("sign of result = " + signOfResult);
            Console.WriteLine("exponent of result = " + exponentOfResult);
            Console.WriteLine("exponent of result dec = " + BinaryConverter.FromBinaryToFloat(signOfResult, exponentOfResult, resultMantissa));
            Console.WriteLine("mantissa of result = " + resultMantissa);
        }

        static void Main(string[] args)
        {
            float a = 125.125F;
            float b = 12.0625F;
            FloatPointMultiplication(a, b);
           
            Console.ReadKey();
        }
    }
}
