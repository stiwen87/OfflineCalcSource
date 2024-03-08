using System;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;

namespace OfflineCalc3
{
    public class BigNum
    {
        private static readonly Dictionary<string, int> values =
            new(){
                  { "", 0 },{ "K", 3},{ "M", 6},{ "B", 9},{ "T", 12},{ "AA", 15},{ "BB", 18},{ "CC", 21},{ "DD", 24},{ "EE", 27},{ "FF", 30},{
    "GG", 33},{ "HH", 36},{ "II", 39},{ "JJ", 42},{ "KK", 45},{ "LL", 48},{ "MM", 51},{ "NN", 54},{ "OO", 57},{
    "PP", 60},{ "QQ", 63},{ "RR", 66},{ "SS", 69},{ "TT", 72},{ "UU", 75},{ "VV", 78},{ "WW", 81},{ "XX", 84},{
    "YY", 87},{ "ZZ", 90},{ "AAA", 93}, { "BBB", 96}, { "CCC", 99}, {"DDD", 102 },{"EEE",105 },{"FFF", 108 },
                {"GGG",111 },{"HHH",114 }, {"III",117 },{"JJJ",120 },{"KFC",123 },{"LLL",126 },{"MMM",129 },{"NNN",132 },
                {"OOO",135 },{"PPP",138 },{"QQQ",141 }, {"RRR",144 },{"SSS",147 },{"TTT",150 },{"UUU",153 } };
        public double firstPart { get; set; } = 0.0;
        public int secondPart { get; set; } = 0;

        public BigNum(double firstPart, int secondPart)
        {
            this.firstPart = firstPart;
            this.secondPart = secondPart;
        }
        public BigNum(string num)
        {
            firstPart = 0.0;
            secondPart = 0;
            if (num is null || num == "")
            {
                return;
            }
            num = num.Replace(',', '.');
            num = Regex.Replace(num, @"\s+", "");
            for (int i = 0; i < num.Length-1; i++)
            {
                if (Char.IsDigit(num[i]) && Char.IsLetter(num[i+1])) 
                { 
                    num = num.Insert(i+1, " ");
                    break; 
                }
            }
            string[] tmp = num.Split();

            if (tmp.Length > 1)
            {
                secondPart = values[tmp[1].ToUpper()];
            }
            firstPart = float.Parse(tmp[0], CultureInfo.InvariantCulture);
            NormalizeNum();
        }

        public void NormalizeNum()
        {
            if (firstPart == 0) return;

            while (firstPart >= 10)
            {
                secondPart++;
                firstPart /= 10;
            }
            while (firstPart < 1)
            {
                secondPart--;
                firstPart *= 10;
            }
            return;
        }

        public static BigNum AddNums(BigNum numA, BigNum numB)
        {

            if (numB.secondPart > numA.secondPart)
            {
                (numA, numB) = (numB, numA);
            }

            int diff = numA.secondPart - numB.secondPart;
            if (diff > 2) return numA;

            BigNum res = new BigNum(numA.firstPart * Convert.ToSingle(Math.Pow(10, diff)) +
                numB.firstPart, numB.secondPart);
            res.NormalizeNum();
            return res;
        }
        public static BigNum DivideNums(BigNum numA, BigNum numB)
        {
            BigNum res = new BigNum(numA.firstPart / numB.firstPart,
                numA.secondPart - numB.secondPart);
            res.NormalizeNum();
            return res;
        }

        public static BigNum MinNum(BigNum numA, BigNum numB)
        {
            numA.NormalizeNum();
            numB.NormalizeNum();

            if (numA.secondPart == numB.secondPart)
                return numA.firstPart < numB.firstPart ? numA : numB;
            return numA.secondPart < numB.secondPart ? numA : numB;
        }

        public static BigNum MaxNum(BigNum numA, BigNum numB)
        {
            numA.NormalizeNum();
            numB.NormalizeNum();

            if (numA.secondPart == numB.secondPart)
                return numA.firstPart > numB.firstPart ? numA : numB;
            return numA.secondPart > numB.secondPart ? numA : numB;
        }

        public string ReverseTranslate()
        {
            string res = (firstPart *
                Convert.ToSingle(Math.Pow(10, secondPart % 3))).ToString("n2");
            var myKey = values.FirstOrDefault(x => x.Value == secondPart / 3 * 3).Key;
            res += " " + myKey;

            return res;
        }
    }



}
