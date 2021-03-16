using System;
using System.Collections.Generic;
using System.Text;

// OpSoftware.OpTools.StringHelper
// OpSoftware.OpLib
namespace KaJourHelper
{
    public static class StringHelper
    {
        /// <summary>
        /// (110, 4) -> "0110"
        /// </summary>
        /// <param name="number">Nummer</param>
        /// <param name="length">Laenge</param>
        /// <returns></returns>
        public static string StrZero(int number, int length)
        {
            string ret;

            // (110, 4) -> "0110"

            //ret = "";
            ret = number.ToString();
            string n = new string('0', length);
           //ret = "0000000000000000000000000000000000000000000000000000" + ret;
            ret = n + ret;
            //ret = string.right(ret, length);
            ret = ret.Substring(ret.Length - length, length);

            return ret;
        }

        /// <summary>
        /// ("110", 4) -> "0110" - .Net: "".PadLeft(4,'0')
        /// </summary>
        /// <param name="number">Nummer</param>
        /// <param name="length">Laenge</param>
        /// <returns></returns>
        public static string StrZero(string number, int length)
        {
            string ret;

            // "".PadLeft(4,'0')

            if (number == null) { number = ""; }

            ret = number.Trim().ToString();
            string n = new string('0', length);
            //ret = "0000000000000000000000000000000000000000000000000000" + ret;
            ret = n + ret;
            ret = ret.Substring(ret.Length - length, length);

            return ret;
        }

        /// <summary>
        /// "a", "b", " " -> "a b"
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="delimiter"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        public static string StrAdd(string str1, string delimiter, string str2) {
            string ret = "";

            if (str1 == null) { str1 = ""; }
            if (str2 == null) { str2 = ""; }
            if (delimiter == null) { delimiter = ""; }

            if (str1 != "")
            {
                // A
                if (str2 != "")
                {
                    // B -> A B
                    ret = str1 + delimiter + str2;
                }
                else {
                    //"" -> A
                    ret = str1;
                }
            }
            else {
                // "" -> B
                ret = str2;
            }

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static int Asc(string txt){
            int ret = 0;

            // https://www.itlnet.net/programming/program/Reference/c53g01c/ngd56f.html
            // ASC()
            // Convert a character to its ASCII value
            // ASC(<cExp>) --> nCode
            // ? ASC("A")                     // Result: 65
            // ? ASC("Apple")                 // Result: 65
            // ? ASC("a")                     // Result: 97
            // ? ASC("Z") - ASC("A")          // Result: 25
            // ? ASC("")                      // Result: 0

            byte[] asciiBytes = Encoding.ASCII.GetBytes(txt);
            ret = asciiBytes[0];

            return ret;
        }

        /// <summary>
        /// Chr(code), CHR(nCode) --> cChar, CHR(72) -> H, CHR(65) -> A, CHR(97) -> a
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string Chr(int code)
        {
            string ret = "";

            // https://www.itlnet.net/programming/program/Reference/c53g01c/ng194af.html
            // CHR()
            // Convert an ASCII code to a character value
            // CHR(<nCode>) --> cChar
            // ? CHR(72)                    // Result: H
            // ? CHR(ASC("A") + 32)         // Result: a
            // ? CHR(7)                     // Result: bell sounds
            // These lines of code show the difference between a null string and the null character:
            // ? LEN("")                   // Result: 0
            // ? LEN(CHR(0))               // Result: 1

            var chr = Convert.ToChar(code);
            ret = chr.ToString();

            return ret;
        }

        // https://www.itlnet.net/programming/program/Reference/c53g01c/nga0e79.html
        // STR()
        // Convert a numeric expression to a character string
        // STR(<nNumber>, [<nLength>], [<nDecimals>]) --> cNumber
        // nNumber:= 123.45
        // ? STR(nNumber)                   // Result:  123.45
        // ? STR(nNumber, 4)                // Result:  123
        // ? STR(nNumber, 2)                // Result:  **
        // ? STR(nNumber * 10, 7, 2)        // Result:  1234.50
        // ? STR(nNumber * 10, 12, 4)       // Result:  1234.5000
        // ? STR(nNumber, 10, 1)            // Result:  1234.5

        // https://www.itlnet.net/programming/program/Reference/c53g01c/ng1acdd.html
        // CTOD()
        // Convert a date string to a date value
        // CTOD(<cDate>) --> dDate

        // https://www.itlnet.net/programming/program/Reference/c53g01c/ng3e298.html
        // DTOC()
        // Convert a date value to a character string
        // DTOC(<dDate>) --> cDate
        // ? DATE()                  // Result: 09/01/90
        // ? DTOC(DATE())            // Result: 09/01/90
        // ? "Today is " + DTOC(DATE()) // Result: Today is 09/01/90

        // https://www.itlnet.net/programming/program/Reference/c53g01c/ng3e7ae.html
        // DTOS()
        // Convert a date value to a character string formatted as yyyymmdd
        // DTOS(<dDate>) --> cDate
        // ? DATE()                        // Result: 09/01/90
        // ? DTOS(DATE())                  // Result: 19900901
        // ? LEN(DTOS(CTOD("")))           // Result: 8

        /// <summary>
        /// #01.01.2012# -> '20120101'
        /// </summary>
        /// <param name="date">Datum</param>
        /// <returns></returns>
        public static string DateToString(DateTime date)
        {
            string ret;

            // #01.01.2012# -> '20120101'
            // #31.12.2012# -> '20121231'

            ret = date.ToString("yyyyMMdd");

            return ret;
        }

        /// <summary>
        /// #01.01.1994# -> '01.01.1994 00:00:00'
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string DateToString_DE_DE(DateTime date){
            string ret;

            var culture = System.Globalization.CultureInfo.GetCultureInfo("de-DE");
            ret = date.ToString(culture);

            return ret;
        }

        /// <summary>
        /// #01.01.2012# -> '2012.01.01'
        /// </summary>
        /// <param name="date">Datum</param>
        /// <returns></returns>
        public static string DateToYMD(DateTime date)
        {
            string ret;

            // #01.01.2012# -> '2012.01.01'
            // #31.12.2012# -> '2012.12.31'

            ret = date.ToString("yyyy.MM.dd");

            return ret;
        }

        /// <summary>
        /// #01.01.1994# -> 'Sa'
        /// </summary>
        /// <param name="date">Datum</param>
        /// <returns></returns>
        public static string DateGetWeekDayString(DateTime date){
            string ret;

            var culture = System.Globalization.CultureInfo.GetCultureInfo("de-DE");
            ret = date.ToString("ddd", culture);
            // Assert.AreEqual failed. Expected:<Sa>. Actual:<Sat>. 30.BuWTag, Rec: 0

            return ret;
        }

        /// <summary>
        /// Month 1-3 -> 1, 4-6 -> 2, 7-9 -> 3, 10-12 -> 4
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static int DateGetQuarter(DateTime date){
            int ret;

            ret = (date.Month + 2) / 3;

            return ret;
        }

        /// <summary>
        /// Mi.01.01.2020 -> 1, Mo.04.01.2021 -> 1
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static int DateGetWeekOfYear(DateTime date){
            int ret;

            var culture = System.Globalization.CultureInfo.GetCultureInfo("de-DE");
            ret = culture.Calendar.GetWeekOfYear(date, System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            return ret;
        }

        /// <summary>
        /// '20120101' -> #01.01.2012#
        /// </summary>
        /// <param name="date">Datums-String (yyyymmdd)</param>
        /// <returns></returns>
        public static DateTime StringToDate(string date)
        {
            DateTime ret;
            int nyear;
            int nmonth;
            int nday;

            // '20120101' -> #01.01.2012#
            // '20121231' -> #31.12.2012#

            nyear = Convert.ToInt32(date.Substring(0, 4));
            nmonth = Convert.ToInt32(date.Substring(4, 2));
            nday = Convert.ToInt32(date.Substring(6, 2));
            ret = new DateTime(nyear, nmonth, nday);

            return ret;
        }

        /// <summary>
        /// 12.34m -> "12.34"
        /// </summary>
        /// <param name="number">Nummer</param>
        /// <returns></returns>
        public static string NumberToString(decimal number)
        {
            string ret;
            //ret = Convert.ToString(number);
            //ret = ret.Replace(',', '.');
            ret = Convert.ToString(number, System.Globalization.CultureInfo.InvariantCulture);
            return ret;
        }

        /// <summary>
        /// "12.34" | "12,34" -> 12.34m, "" -> 0
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static decimal StringToNumber(string number)
        {
            decimal ret = 0;

            if (number!=null) {
                //var culture = System.Globalization.CultureInfo.GetCultureInfo("de-DE");
                //number = number.Replace('.', ',');
                number = number.Replace(',', '.');
                if (!string.IsNullOrEmpty(number))
                {
                    try
                    {
                        ret = Convert.ToDecimal(number, System.Globalization.CultureInfo.InvariantCulture);
                    }
                    catch (Exception ex)
                    {
                        //
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// "0" -> 0, "1" -> 1, "" -> 0
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static int StringToInt(string number)
        {
            int ret = 0;

            if (!string.IsNullOrEmpty(number))
            {
                ret = Convert.ToInt32(number);
            }

            return ret;
        }

        /// <summary>
        /// #10.12.2012# -> #01.01.2012#
        /// </summary>
        /// <param name="date">Datum</param>
        /// <returns></returns>
        public static DateTime DateFirstDayInYear(DateTime date)
        {
            DateTime ret;

            ret = new DateTime(date.Year, 1, 1);

            return ret;
        }

        /// <summary>
        /// #10.01.2012# -> #31.21.2012#
        /// </summary>
        /// <param name="date">Daum</param>
        /// <returns></returns>
        public static DateTime DateLastDayInYear(DateTime date)
        {
            DateTime ret;

            ret = new DateTime(date.Year, 12, 31);

            return ret;
        }

        /// <summary>
        /// #10.12.2012# -> #01.12.2012#
        /// </summary>
        /// <param name="date">Daum</param>
        /// <returns></returns>
        public static DateTime DateFirstDayInMonth(DateTime date)
        {
            DateTime ret;

            ret = new DateTime(date.Year, date.Month, 1);

            return ret;
        }

        /// <summary>
        /// #10.10.2012# -> #31.10.2012#
        /// </summary>
        /// <param name="date">Daum</param>
        /// <returns></returns>
        public static DateTime DateLastDayInMonth(DateTime date)
        {
            DateTime ret;

            ret = new DateTime(date.Year, date.Month, 1);
            ret = ret.AddMonths(1);
            ret = ret.AddDays(-1);

            return ret;
        }

        /// <summary>
        /// 1 ... "1" , ... , 10 ... "A" , ... , 15 ... "F" , 16 ... "G" , ... , 31 ... "V" , 32 ... "10"
        /// </summary>
        /// <param name="number">Nummer</param>
        /// <returns></returns>
        public static string IntToHexHex(int number)
        {
            string ret = "";

            // 1 ... 1
            // 9 ... 9
            // 10 ... A
            // 15 ... F
            // 16 ... G
            // 31 ... V
            // 32 ... 10

            int v1 = number / 32;
            int vRest= number % 32;

            string s0 = IntToBase32(vRest);
            string s1 = IntToBase32(v1);

            ret = s1 + s0;

            return ret;
        }

        /// <summary>
        /// 0->"0", 1->"1",...,9->"9",10->"A",...,15->"F",16->"G",...,31->"V"
        /// </summary>
        /// <param name="number">Nummer</param>
        /// <returns></returns>
        public static string IntToBase32(int number){
            string ret = "0";

            if (number == 0) { ret = "0"; }
            else if(number == 1){ ret = "1"; }
            else if (number == 2) { ret = "2"; }
            else if (number == 3) { ret = "3"; }
            else if (number == 4) { ret = "4"; }
            else if (number == 5) { ret = "5"; }
            else if (number == 6) { ret = "6"; }
            else if (number == 7) { ret = "7"; }
            else if (number == 8) { ret = "8"; }
            else if (number == 9) { ret = "9"; }
            else if (number == 10) { ret = "A"; }
            else if (number == 11) { ret = "B"; }
            else if (number == 12) { ret = "C"; }
            else if (number == 13) { ret = "D"; }
            else if (number == 14) { ret = "E"; }
            else if (number == 15) { ret = "F"; }
            else if (number == 16) { ret = "G"; }
            else if (number == 17) { ret = "H"; }
            else if (number == 18) { ret = "I"; }
            else if (number == 19) { ret = "J"; }
            else if (number == 20) { ret = "K"; }
            else if (number == 21) { ret = "L"; }
            else if (number == 22) { ret = "M"; }
            else if (number == 23) { ret = "N"; }
            else if (number == 24) { ret = "O"; }
            else if (number == 25) { ret = "P"; }
            else if (number == 26) { ret = "Q"; }
            else if (number == 27) { ret = "R"; }
            else if (number == 28) { ret = "S"; }
            else if (number == 29) { ret = "T"; }
            else if (number == 30) { ret = "U"; }
            else if (number == 31) { ret = "V"; }

            return ret;
        }

        /// <summary>
        /// Rundet kaufmännisch auf die Anzahl der übergebenen Nachkommastellen
        /// 
        /// / Achtung: Seit .NET 2.0 gibt es folgende Überladung die dieses Snippet hinfällig machen: Math.Round(3.65m,1,MidpointRounding.AwayFromZero)
        /// </summary>
        /// <param name="value">Zu rundender Wert</param>
        /// <param name="dec">Anzahl der Nachkommastellen</param>
        /// <returns>Gerundeter Wert</returns>
        public static decimal CommercialRound(decimal value, int dec){
            // https://dotnet-snippets.de/snippet/kaufmaennisches-runden-in-decimal/303

            //3,6 = Math.Round(Convert.ToDecimal("3,64"),1,MidpointRounding.AwayFromZero));
            //3,7 = Math.Round(Convert.ToDecimal("3,65"),1,MidpointRounding.AwayFromZero));

            decimal ret = Math.Round(value, dec, MidpointRounding.AwayFromZero);

            return ret;
        }
    }
}
