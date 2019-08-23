
namespace Msn.InteropDemo.ViewModel.Core
{
    public abstract class EntityBaseModel : EntityBaseModel<int>
    {
    }

    public abstract class EntityBaseModel<TKey>
    {
        public EntityBaseModel()
        {
            Enabled = true;
        }

        public TKey Id { get; set; }

        public bool Enabled { get; set; }

        public string ContextUserId { get; set; }
    }
}
