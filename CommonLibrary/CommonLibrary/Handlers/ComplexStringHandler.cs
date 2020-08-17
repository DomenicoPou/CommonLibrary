using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.Handlers
{
    public static class ComplexStringHandler
    {
        public static string StringCheckToWords(string orginalString)
        {
            string result = "";
            List<string> splitString = SplitNumberAndStrings(orginalString);
            foreach (string str in splitString)
            {
                if (int.TryParse(str, out int digit))
                {
                    result += DigitToWords(digit);
                }
                else
                {
                    result += str;
                }
            }
            return result;
        }

        public static List<string> SplitNumberAndStrings(string original)
        {
            List<string> results = new List<string>();
            bool? isDigit = null;
            string current = "";
            foreach (char c in original)
            {
                if (isDigit != null && isDigit != Char.IsDigit(c))
                {
                    results.Add(current);
                    current = "";
                }
                current += c;
                isDigit = Char.IsDigit(original.First());
            }
            results.Add(current);
            return results;
        }

        /// <summary>
        /// https://stackoverflow.com/a/2730393
        /// </summary>
        /// <param name="digit"></param>
        /// <returns></returns>
        public static string DigitToWords(int number)
        {
                if (number == 0)
                    return "zero";

                if (number < 0)
                    return "minus " + DigitToWords(Math.Abs(number));

                string words = "";

                if ((number / 1000000) > 0)
                {
                    words += DigitToWords(number / 1000000) + " million ";
                    number %= 1000000;
                }

                if ((number / 1000) > 0)
                {
                    words += DigitToWords(number / 1000) + " thousand ";
                    number %= 1000;
                }

                if ((number / 100) > 0)
                {
                    words += DigitToWords(number / 100) + " hundred ";
                    number %= 100;
                }

                if (number > 0)
                {
                    if (words != "")
                        words += "and ";

                    var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                    var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                    if (number < 20)
                        words += unitsMap[number];
                    else
                    {
                        words += tensMap[number / 10];
                        if ((number % 10) > 0)
                            words += "-" + unitsMap[number % 10];
                    }
                }

                return words;
        }
    }
}
