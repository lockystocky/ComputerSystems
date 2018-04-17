using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryMultiplication
{
    public class BinaryConverter
    {
        public static string FromDecimalToBinary(int number)
        {
            int k = 128;
            string binaryStr = "";
            while (k > 0)
            {
                binaryStr += Convert.ToInt32((k & number) != 0);
                k = k >> 1;
            }
            while (binaryStr.StartsWith("0"))
            {
                binaryStr = binaryStr.Substring(1);
            }
            return binaryStr;
        }

        public static int FromBinaryToDecimal(string number)
        {
            int decimalNumber = 0;
            int powerOfTwo = 0;
            for(int i = number.Length - 1; i >= 0; i--)
            {
                if(number[i] == '1')
                {
                    decimalNumber += (int) Math.Pow(2, powerOfTwo);
                }
                powerOfTwo++;
            }
            return decimalNumber;
        }

        public static string ToRegister(string binNumber, int register)
        {
            while(binNumber.Length < register)
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

        public static string Add(string C, string A)
        {
            string c = "";
            string a = A;
            while(a.Length < C.Length)
            {
                a += "0";
            }
            bool carry = false;
            for(int i = a.Length - 1; i >= 0; i--)
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

        public static string Add2(string C, string A)
        {
            string c = "";
            string a = A;
            while (a.Length < C.Length)
            {
                a = "0" + a;
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

        public static string MultiplyWhithRightShift(string A, string B, int register)
        {
            A = ToRegister(A, register);
            B = ToRegister(B, register);            

            string C = ToRegister("0", register * 2);
            Console.WriteLine("Початок: A = {0},  B = {1}, C = {2}", A, B, C);
            for (int i = B.Length - 1; i >= 0; i--)
            {               
                Console.WriteLine("----------------------");
                if (B[i] == '1')
                {
                    C = Add(C, A);
                    Console.WriteLine("B[i] = 1, додавання A до C");
                }
                Console.WriteLine("Зсуваємо вправо C");
                C = RightShift(C);
                Console.WriteLine("i = {0}, B[i] = {2}:  A = {1}", i, A, B[i]);
                Console.WriteLine("i = {0}, B[i] = {2}:  B = {1}", i, B, B[i]);
                Console.WriteLine("i = {0}, B[i] = {2}:  C = {1}", i, C, B[i]);
            }
            return C;
        }

        public static string ToAdditionalCode(string number, int register)
        {
            string additionalnumber = "";
            while(number.Length < register)
            {
                number = "0" + number;
            }
            for(int i = 0; i < number.Length; i++)
            {
                if (number[i] == '0')
                    additionalnumber += "1";
                else
                    additionalnumber += "0";
            }
            additionalnumber = Add2(additionalnumber, "1");
            return additionalnumber;
        }

        public static string SimpleDivision(string dividend, string divisor, int register)
        {
            string quotient = "";
            dividend = ToRegister(dividend, register);
            divisor = ToRegister(divisor, register);
            
            while(dividend.IndexOf("1") != divisor.IndexOf("1"))
            {
                divisor = LeftShift(divisor);
            }

            string additionalDivisor = ToAdditionalCode(divisor, register);
                        
            Console.WriteLine("\n\n");
           
            Console.WriteLine("START");
            Console.WriteLine("Делимое             = {0}", dividend);
            Console.WriteLine("Делитель            = {0}", additionalDivisor);
            Console.WriteLine("Делитель в доп коде = " + additionalDivisor);
            Console.WriteLine("--------------------");
            int k = 0;
            while (dividend.Contains("1") && k < 15)
            {                
                Console.WriteLine("Частичный остаток = {0}", dividend);
                Console.WriteLine("Делитель          = {0}", additionalDivisor);
                string currentDivident = "";
                if (dividend.StartsWith("0"))
                {
                    Console.WriteLine("Вычитаем из частичного остатка делитель, если остаток положительный");
                    currentDivident = Add(dividend, additionalDivisor);
                }
                else
                {
                    Console.WriteLine("Прибавляем к частичному остатку делитель, если остаток отрицательный");
                    currentDivident = Add(dividend, divisor);
                }
                
                if (currentDivident.Length > dividend.Length && currentDivident[0] == '1')
                {
                    quotient += "1";
                    dividend = currentDivident.Substring(1);
                    Console.WriteLine("Анализируем знак полученного частичного остатка. В регистр результата записываем 1 если остаток положительный");
                }
                else
                {
                    quotient += "0";
                    dividend = currentDivident;
                    Console.WriteLine("Анализируем знак полученного частичного остатка. В регистр результата записываем 0 если остаток отрицательный");
                }
                
                Console.WriteLine("Результат         = {0}", quotient);
                Console.WriteLine("Полученный час.ост= {0}", currentDivident);
                Console.WriteLine("Сдвигаем частичный остаток влево");
                Console.WriteLine("--------------------");

                dividend = LeftShift(dividend);
                k++;                
            }
            return quotient;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Деление 105 на 5");
            string num1 = BinaryConverter.FromDecimalToBinary(105);
            string num2 = BinaryConverter.FromDecimalToBinary(5);
            string result =  BinaryConverter.SimpleDivision(num1, num2, 16);;
            Console.WriteLine();
            Console.WriteLine("Результат в двоичном виде: " + result);
            Console.WriteLine("Результат в десятичном виде: " + BinaryConverter.FromBinaryToDecimal(result));
            

            Console.ReadLine();
        }
    }
}
