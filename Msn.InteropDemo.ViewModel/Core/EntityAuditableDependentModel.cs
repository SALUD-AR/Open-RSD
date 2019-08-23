using System;

namespace Msn.InteropDemo.ViewModel.Core
{
    public abstract class EntityAuditableDependentModel : EntityAuditableDependentModel<int>
    { }

    public abstract class EntityAuditableDependentModel<TKey>
    {
        public string ContextUserId { get; set; }

        public string CreatedUserId { get; set; }

        public string CreatedUserName { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public string UpdatedUserId { get; set; }

        public string UpdatedUserName { get; set; }

        public DateTime? UpdatedDateTime { get; set; }

        public string RowVersion { get; set; }
    }
}
