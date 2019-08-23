using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Msn.InteropDemo.ViewModel.Attributes.Validators
{
    public class EnsureCollectionItemsAttributeValidator : ValidationAttribute
    {
        private readonly int minElements;
        public EnsureCollectionItemsAttributeValidator(int minElements)
        {
            this.minElements = minElements;
        }

        public override bool IsValid(object value)
        {
            var list = value as IList;
            if (list != null)
            {
                return list.Count >= minElements;
            }
            return false;
        }

    }
}
