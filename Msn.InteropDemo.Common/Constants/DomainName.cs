using System;
using System.Collections.Generic;
using System.Text;

namespace Msn.InteropDemo.Common.Constants
{
    public sealed class DomainName
    {
        public static readonly DomainName LocalDomain = new DomainName();
        public static readonly DomainName FederadorPatientDomain = new DomainName { Value = "https://federador.msal.gob.ar/patient-id" };
        public static readonly DomainName RenaperDniDomain = new DomainName { Value = "http://www.renaper.gob.ar/dni" };
        public static readonly DomainName MinInteriorPassportDomain = new DomainName { Value = "http://www.mininterior.gob.ar/pas" };

        public void SetValue(string domainName)
        {
            Value = domainName;
        }

        public string Value { get; private set; }

    }
}
