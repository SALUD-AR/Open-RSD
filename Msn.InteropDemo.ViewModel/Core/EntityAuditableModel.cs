using System;

namespace Msn.InteropDemo.ViewModel.Core
{
    public abstract class EntityAuditableModel : EntityAuditableModel<int>
    { }

    public abstract class EntityAuditableModel<TKey> : EntityBaseModel<TKey>
    {
        public EntityAuditableModel()
        {
            Enabled = true;
            CreatedDateTime = DateTime.Now;
        }

        public string CreatedUserId { get; set; }

        public string CreatedUserName { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public string UpdatedUserId { get; set; }

        public string UpdatedUserName { get; set; }

        public DateTime? UpdatedDateTime { get; set; }

        public byte[] RowVersion { get; set; }
    }
}
