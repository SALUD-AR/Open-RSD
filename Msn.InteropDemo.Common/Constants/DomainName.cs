using System;
using System.Collections.Generic;
using System.Text;

namespace Msn.InteropDemo.Common.Constants
{
    public sealed class DomainName
    {
        public static readonly DomainName LocalDomain = new DomainName { Value = "http://www.msal.gov.ar" };
        public static readonly DomainName FederadorPatientDomain = new DomainName { Value = "https://federador.msal.gob.ar/patient-id" };
        public static readonly DomainName RenaperDniDomain = new DomainName { Value = "http://www.renaper.gob.ar/dni" };
        public static readonly DomainName MinInteriorPassportDomain = new DomainName { Value = "http://www.mininterior.gob.ar/pas" };

        public string Value { get; private set; }

    }
}
