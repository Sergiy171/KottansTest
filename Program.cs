using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditCardMethods
{
    class Program
    {
        static void Main(string[] args)
        {
            string creditCardNumber = "5211 5273 0672 4150";
            Program p = new Program();

            if (p.IsCreditNumberGrouped(creditCardNumber) == 1)
            {
                Console.WriteLine("Цифри згруповані");
            }
            else if (p.IsCreditNumberGrouped(creditCardNumber) == 0)
            {
                Console.WriteLine("Цифри незгруповані");
            }
            else
            {
                Console.WriteLine("Неправильно введений номер картки");
            }

            Console.WriteLine(p.GetCreditCardVendor(creditCardNumber));

            foreach (int item in p.ConvertedCardNumber(creditCardNumber))
            {
                Console.Write(Convert.ToString(item) + " ");
            }

            Console.WriteLine();
            Console.WriteLine("Luhn:");

            foreach (int item in p.SimpleLuhnAlgorithm(p.ConvertedCardNumber(creditCardNumber)))
            {
                Console.Write(Convert.ToString(item) + " ");
            }

            Console.WriteLine();
            Console.WriteLine("Сума по Луну: " + Convert.ToString(p.LuhnSummary(p.SimpleLuhnAlgorithm(p.ConvertedCardNumber(creditCardNumber)))));

            Console.WriteLine();
            Console.WriteLine("Контрольне число: " + Convert.ToString(p.CheckDigit(p.SimpleLuhnAlgorithm(p.ConvertedCardNumber(creditCardNumber)))));

            Console.WriteLine();
            Console.WriteLine("Наступний номер: " + Convert.ToString(p.GenerateNextCreditCardNumber(creditCardNumber)));

            Console.ReadKey();
        }

        public string GetCreditCardVendor(string cardNumber)
        {
            int tempSubstringFirstTwoNumbers = Convert.ToInt32(cardNumber.Substring(0,2));
            int tempSubstringFirstFourNumbers = Convert.ToInt32(cardNumber.Substring(0,4));

            if (tempSubstringFirstTwoNumbers == 34 || tempSubstringFirstTwoNumbers == 37)
            {
                return "American Express";
            }
            else if ((tempSubstringFirstTwoNumbers == 50) || ((tempSubstringFirstTwoNumbers >= 56) && (tempSubstringFirstTwoNumbers <= 69)))
            {
                return "Maestro";
            }
            else if (((tempSubstringFirstFourNumbers >= 2221) && (tempSubstringFirstFourNumbers <= 2720)) || ((tempSubstringFirstTwoNumbers >= 51) && (tempSubstringFirstTwoNumbers <= 55)))
            {
                return "MasterCard";
            }
            else if (Convert.ToInt32(cardNumber[0]) == 4)
            {
                return "Visa";
            }
            else if ((tempSubstringFirstFourNumbers >= 3528) && (tempSubstringFirstFourNumbers <= 3589))
            {
                return "JCB";
            }
            else
            {
                return "Unknown";
            }
        }

        private int CreditCardNumbersCount(string cardNumber)
        {
            return cardNumber.Length;
        }

        private int IsCreditNumberGrouped(string cardNumber)
        {
            int numbersCount = CreditCardNumbersCount(cardNumber);

            if (numbersCount == 16)
            {
                return 0;
            }
            else if (numbersCount == 19)
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }

        private List<int> ConvertedCardNumber(string cardNumber)
        {
            List<int> convertedCardNumber = new List<int>();

            for (int i = 0; i < cardNumber.Length; i++ )
            {
                if (!(Convert.ToString(cardNumber[i]).Equals(" ")))
                    convertedCardNumber.Add(int.Parse(Convert.ToString(cardNumber[i])));
            }

            return convertedCardNumber;
        }

        private List<int> SimpleLuhnAlgorithm(List<int> convertedCardNumber)
        {
            for (int i = 0; i < convertedCardNumber.Count; i++ )
            {
                if (((i % 2) + 1) == 1)
                {
                    convertedCardNumber[i] *= 2;

                    if ((convertedCardNumber[i]) > 9)
                    {
                        convertedCardNumber[i] -= 9;
                    }
                }
            }

            return convertedCardNumber;
        }

        private int LuhnSummary(List<int> LuhnCardNumber)
        {
            int summ = 0;
            foreach (int item in LuhnCardNumber)
            {
                summ += item;
            }

            return summ;
        }

        private int CheckDigit(List<int> LuhnCardNumber)
        {
            int summ = LuhnSummary(LuhnCardNumber);

            return ((summ * 9) % 10);
        }

        public bool IsCreditCardNumberValid(string cardNumber)
        {
            if ((LuhnSummary(SimpleLuhnAlgorithm(ConvertedCardNumber(cardNumber))) % 10) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private string ConvertNumberToString(List<int> cardNumber)
        {
            StringBuilder strcardNumber = new StringBuilder();
            foreach (int item in cardNumber)
            {
                strcardNumber.Append(Convert.ToString(item));
            }

            return Convert.ToString(strcardNumber);
        }

        public string GenerateNextCreditCardNumber(string cardNumber)
        {
            List<int> convCardNumber = ConvertedCardNumber(cardNumber);

            string strCardNumber = convCardNumber.ToString();
            do
            {
                if ((convCardNumber[15]) != 9)
                {
                    convCardNumber[15] += 1;
                }
                else
                {
                    (convCardNumber[15]) = 0;

                    if ((convCardNumber[14]) != 9)
                    {
                        convCardNumber[14] += 1;
                    }
                    else
                    {
                        convCardNumber[14] = 0;
                        convCardNumber[13] += 1;
                    }
                }
                                
                strCardNumber = ConvertNumberToString(convCardNumber);
            }
            while (!(IsCreditCardNumberValid(strCardNumber)));

            return strCardNumber;
        }

    }
}
