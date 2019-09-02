using System;
using System.Text.RegularExpressions;

namespace Msn.InteropDemo.Common.Utils.Helpers
{
    public static class Cuit
    {
        public static bool Validate(string cuit)
        {
            if (string.IsNullOrWhiteSpace(cuit))
            {
                return false;
            }

            cuit = cuit.Trim();
            if (cuit.Length > 13) //no puede ser mayor que 13 con los guiones incluidos.
            {
                return false;
            }

            cuit = cuit.Replace("-", ""); // Le quito el guión
            if (cuit.Length != 11)
            {
                return false;
            }

            var rg = new Regex("[A-Z_a-z]"); // Expresión regular para caracteres no válidos
            if (rg.IsMatch(cuit))
            {
                // Tiene caracteres no válidos
                return false;
            }

            var calculado = GetCuitVerificationDigit(cuit);
            var digito = int.Parse(cuit.Substring(10));
            return calculado == digito;
        }

        public static int GetCuitVerificationDigit(string cuit)
        {
            var mult = new[] { 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 };
            var nums = cuit.ToCharArray();
            var total = 0;
            for (var i = 0; i < mult.Length; i++)
            {
                total += int.Parse(nums[i].ToString()) * mult[i];
            }
            var resto = total % 11;
            return resto == 0 ? 0 : resto == 1 ? 9 : 11 - resto;
        }

        public static string ToCleanFormat(string cuit)
        {
            if (cuit != null)
            {
                cuit = cuit.Trim(); //quito espacios
                cuit = cuit.Replace("-", ""); // Le quito el guión
            }
            return cuit;
        }

        public static string ToUIFormat(long? cuit, bool mustValidate = false)
        {
            if(!cuit.HasValue)
            {
                return string.Empty;
            }
            if(cuit.Value == 0)
            {
                return string.Empty;
            }

            return ToUIFormat(cuit.ToString(), mustValidate);
        }

        public static string ToUIFormat(string cuit, bool mustValidate = false)
        {
            if (mustValidate && !Validate(cuit))
            {
                throw new ArgumentException("Número de CUIT incorrecto");
            }

            cuit = cuit.Trim(); //quito espacios
            cuit = cuit.Replace("-", ""); // Le quito el guión si lo hubiera, posiblemente tenga solo uno.

            //cuit = 20172874127

            if (cuit.Length != 11)
            {
                return cuit;
            }

            var preCuit = cuit.Substring(0, 2) + "-";
            //preCuit = 20-

            preCuit += cuit.Substring(2, cuit.Length - 3) + "-";
            //preCuit = 20-17287412-

            preCuit += cuit.Substring(cuit.Length - 1);
            //preCuit = 20-17287412-7

            cuit = preCuit;

            return cuit;
        }

    }
}
