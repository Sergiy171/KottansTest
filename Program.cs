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
            string creditCardNumber = "35301113333000001";
            Program p = new Program();

            Console.WriteLine(p.CreditCardNumbersCount(creditCardNumber));
            Console.WriteLine();

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
                if (CreditCardNumbersCount(cardNumber) == 15)
                {
                    return "American Express";
                }
                else
                {
                    return "Unknown";
                }
            }
            else if ((tempSubstringFirstTwoNumbers == 50) || ((tempSubstringFirstTwoNumbers >= 56) && (tempSubstringFirstTwoNumbers <= 69)))
            {
                if ((CreditCardNumbersCount(cardNumber) >= 12) && (CreditCardNumbersCount(cardNumber) <= 19))
                {
                    return "Maestro";
                }
                else
                {
                    return "Unknown";
                }
            }
            else if (((tempSubstringFirstFourNumbers >= 2221) && (tempSubstringFirstFourNumbers <= 2720)) || ((tempSubstringFirstTwoNumbers >= 51) && (tempSubstringFirstTwoNumbers <= 55)))
            {
                if (CreditCardNumbersCount(cardNumber) == 16)
                {
                    return "MasterCard";
                }
                else
                {
                    return "Unknown";
                }
            }
            else if (Convert.ToInt32(Convert.ToString(cardNumber[0])) == 4)
            {
                if ((CreditCardNumbersCount(cardNumber) == 13) || (CreditCardNumbersCount(cardNumber) == 16) || (CreditCardNumbersCount(cardNumber) == 19))
                {
                    return "Visa";
                }
                else
                {
                    return "Unknown";
                }
            }
            else if ((tempSubstringFirstFourNumbers >= 3528) && (tempSubstringFirstFourNumbers <= 3589))
            {
                if (CreditCardNumbersCount(cardNumber) == 16)
                {
                    return "JCB";
                }
                else
                {
                    return "Unknown";
                }
            }
            else
            {
                return "Unknown";
            }
        }

        private int CreditCardNumbersCount(string cardNumber)
        {
            return ConvertedCardNumber(cardNumber).Count();
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
            int j = 0;
            for (int i = convertedCardNumber.Count - 1; i >= 0; i--)
            {
                j += 1;
                if ((j % 2) == 0)
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
            string oldVendor = GetCreditCardVendor(cardNumber);
            List<int> convCardNumber = ConvertedCardNumber(cardNumber);

            string strCardNumber = convCardNumber.ToString();
            do
            {
                if ((convCardNumber[(convCardNumber.Count - 1)]) != 9)
                {
                    convCardNumber[(convCardNumber.Count - 1)] += 1;
                }
                else
                {
                    (convCardNumber[(convCardNumber.Count - 1)]) = 0;

                    if ((convCardNumber[(convCardNumber.Count - 2)]) != 9)
                    {
                        convCardNumber[(convCardNumber.Count - 2)] += 1;
                    }
                    else
                    {
                        convCardNumber[(convCardNumber.Count - 2)] = 0;
                        convCardNumber[(convCardNumber.Count - 3)] += 1;
                    }
                }
                                
                strCardNumber = ConvertNumberToString(convCardNumber);
            }
            while (!(IsCreditCardNumberValid(strCardNumber)));

            string newVendor = GetCreditCardVendor(strCardNumber);

            if ((oldVendor) == (newVendor))
            {
                return strCardNumber;
            }
            else
            {
                return "No more card numbers available for this vendor.";
            }
        }

    }
}
