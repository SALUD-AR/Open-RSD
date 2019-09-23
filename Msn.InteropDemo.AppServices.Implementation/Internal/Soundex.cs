using System;
using System.Text;

namespace Msn.InteropDemo.AppServices.Implementation.Internal
{
    public class Soundex
    {
        public string GetSoundex(string data)
        {
            var result = new StringBuilder();
            if (data != null && data.Length > 0)
            {
                string previousCode, currentCode;
                result.Append(Char.ToUpper(data[0]));
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
                result.Append(new String('0', 4 - result.Length));
            }

            return result.ToString();
        }
        private string EncodeChar(char c)
        {
            switch (Char.ToLower(c))
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
        public int Difference(string data1, string data2)
        {
            var result = 0;
            if (data1.Equals(string.Empty) || data2.Equals(string.Empty))
            {
                return 0;
            }

            var soundex1 = GetSoundex(data1);
            var soundex2 = GetSoundex(data2);
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
    }
}
