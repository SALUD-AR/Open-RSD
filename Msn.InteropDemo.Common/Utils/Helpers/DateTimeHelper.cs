using System;
using System.Globalization;

namespace Msn.InteropDemo.Common.Utils.Helpers
{
    public static class DateTimeHelper
    {
        public static string[] Meses
        {
            get
            {
                return new[] { "", "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };
            }
        }

        public static bool TryParseFromAR(string s, out DateTime dt)
        {
            var culture = CultureInfo.CreateSpecificCulture("es-AR");
            return DateTime.TryParse(s, culture, DateTimeStyles.None, out dt);
        }

        public static DateTime? FromDateTimeAR(string dateTime)
        {
            if(TryParseFromAR(dateTime, out var dt))
            {
                return dt;
            }

            return null;
        }

        public static int CalculateAge(DateTime birthdate)
        {
            var today = DateTime.Today;

            // Calculate the age.
            var age = today.Year - birthdate.Year;
            // Go back to the year the person was born in case of a leap year
            if (birthdate.Date > today.AddYears(-age))
            {
                age--;
            }

            return age;
        }

        public static string FriendlyDateAR(DateTime dt)
        {
            var mes = Meses[dt.Month];
            var ret = $"{dt.Day} de {mes}, {dt.Year}";
            return ret;
        }
    }
}
