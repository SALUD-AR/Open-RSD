namespace Msn.InteropDemo.Web.Models
{
    public class AutocompleteViewModel : AutocompleteViewModel<int>
    { }

    public class AutocompleteViewModel<TKey>
    {
        public TKey Id { get; set; }

        public string Name { get; set; }
    }
}
