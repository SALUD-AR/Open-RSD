﻿using System;
using System.Text;

namespace Msn.InteropDemo.Common.Utils.Helpers
{
    public static class StringHelper
    {
        public static string Soundex(string data)
        {
            var result = new StringBuilder();
            if (data != null && data.Length > 0)
            {
                string previousCode, currentCode;
                result.Append(char.ToUpper(data[0]));
                previousCode = string.Empty;
                for (var i = 1; i < data.Length; i++)
                {
                    currentCode = EncodeChar(data[i]);
                    if (currentCode != previousCode)
                    {
                        result.Append(currentCode);
                    }

                    if (result.Length == 4)
                    {
                        break;
                    }

                    if (!currentCode.Equals(string.Empty))
                    {
                        previousCode = currentCode;
                    }
                }
            }
            if (result.Length < 4)
            {
                result.Append(new string('0', 4 - result.Length));
            }

            return result.ToString();
        }
        private static string EncodeChar(char c)
        {
            switch (char.ToLower(c))
            {
                case 'b':
                case 'f':
                case 'p':
                case 'v':
                    return "1";
                case 'c':
                case 'g':
                case 'j':
                case 'k':
                case 'q':
                case 's':
                case 'x':
                case 'z':
                    return "2";
                case 'd':
                case 't':
                    return "3";
                case 'l':
                    return "4";
                case 'm':
                case 'n':
                    return "5";
                case 'r':
                    return "6";
                default:
                    return string.Empty;
            }
        }

        //The difference function will match the two Soundex strings and return 0 to 4.
        //0 means Not Matched
        //4 means Strongly Matched

        /// <summary>
        /// Calcula la diferencia entre dos Textoz planos dados (No deben ser Soundex)
        /// </summary>
        /// <param name="text1">Primer texto plano a comparar</param>
        /// <param name="text2">Segundo texto plano a comparar</param>
        /// <returns>un valor entre [0, 4] 0 si no coinciden y si coinciden plemanente</returns>
        public static int DifferenceSoundexPlainText(string text1, string text2)
        {
            if (text1.Equals(string.Empty) || text2.Equals(string.Empty))
            {
                return 0;
            }

            var soundex1 = Soundex(text1);
            var soundex2 = Soundex(text2);
            return DifferenceSoundex(soundex1, soundex2);
        }

        /// <summary>
        /// Calcula la diferencia entre dos Soundex dados
        /// </summary>
        /// <param name="soundex1">Primer Soundex a comparar</param>
        /// <param name="soundex2">Segundo Soundex a comparar</param>
        /// <returns>un valor entre [0, 4] 0 si no coinciden y si coinciden plemanente</returns>
        public static int DifferenceSoundex(string soundex1, string soundex2)
        {
            var result = 0;
            if (soundex1.Equals(soundex2))
            {
                result = 4;
            }
            else
            {
                if (soundex1[0] == soundex2[0])
                {
                    result = 1;
                }

                var sub1 = soundex1.Substring(1, 3); //characters 2, 3, 4
                if (soundex2.IndexOf(sub1) > -1)
                {
                    result += 3;
                    return result;
                }

                var sub2 = soundex1.Substring(2, 2); //characters 3, 4
                if (soundex2.IndexOf(sub2) > -1)
                {
                    result += 2;
                    return result;
                }

                var sub3 = soundex1.Substring(1, 2); //characters 2, 3
                if (soundex2.IndexOf(sub3) > -1)
                {
                    result += 2;
                    return result;
                }

                var sub4 = soundex1[1];
                if (soundex2.IndexOf(sub4) > -1)
                {
                    result++;
                }

                var sub5 = soundex1[2];
                if (soundex2.IndexOf(sub5) > -1)
                {
                    result++;
                }

                var sub6 = soundex1[3];
                if (soundex2.IndexOf(sub6) > -1)
                {
                    result++;
                }
            }
            return result;
        }

        public static int LevenshteinDistance(string s, string t)
        {
            if (s is null)
            {
                throw new System.ArgumentNullException(nameof(s));
            }

            if (t is null)
            {
                throw new System.ArgumentNullException(nameof(t));
            }

            var n = s.Length;
            var m = t.Length;
            var d = new int[n + 1, m + 1];

            // Step 1
            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            // Step 2
            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; d[0, j] = j++)
            {
            }

            // Step 3
            for (int i = 1; i <= n; i++)
            {
                //Step 4
                for (int j = 1; j <= m; j++)
                {
                    // Step 5
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                    // Step 6
                    d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + cost);
                }
            }
            // Step 7
            return d[n, m];
        }
    }
}

