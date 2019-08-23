using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Msn.InteropDemo.Common.OperationResults
{
    public class OperationError
    {
        public string ErrorCode { get; set; }

        public string ErrorDescription { get; set; }

        public string GetError()
        {
            var str = string.Empty;
            if (!string.IsNullOrWhiteSpace(ErrorCode))
            {
                return ErrorCode + ": " + ErrorDescription;
            }
            else
            {
                return ErrorDescription;
            }
        }

        public override string ToString()
        {
            return GetError();
        }

    }
}
