using System.Collections.Generic;
using System.Linq;

namespace Msn.InteropDemo.Common.OperationResults
{
    public class OperationResult : OperationResult<int>
    { }

    public class OperationResult<TKey>
    {
        List<OperationError> errorList;

        public OperationResult()
        {
            errorList = new List<OperationError>();
        }

        public TKey Id { get; set; }

        public bool OK { get; set; } = true;

        public bool NOK { get { return !OK; } }

        public void AddErrorRange(IReadOnlyList<OperationError> errors)
        {
            foreach (var item in errors)
            {
                AddError(new OperationError { ErrorCode = item.ErrorCode, ErrorDescription = item.ErrorDescription });
            }
        }

        public void AddError(string errorDescription, string errorCode = "")
        {
            var err = new OperationError { ErrorCode = errorCode, ErrorDescription = errorDescription };
            AddError(err);
            OK = false;
        }

        public void AddError(OperationError opError)
        {
            errorList.Add(opError);
            OK = false;
        }

        public List<OperationError> GetErrorlist()
        {
            return errorList;
        }

        public override string ToString()
        {
            if (!GetErrorlist().Any())
            {
                return "OK";
            }

            var str = string.Empty;
            foreach (var item in GetErrorlist())
            {
                if (str != string.Empty)
                {
                    str += " | ";
                }
                str += item.GetError();
            }
            return str;
        }
    }
}
