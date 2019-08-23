using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Msn.InteropDemo.Entities.Core
{
    public abstract class EntityAuditable : EntityAuditable<int>
    { }

    public abstract class EntityAuditable<TKey> : EntityBase<TKey>
    {
        public EntityAuditable()
        {
            CreatedDateTime = DateTime.Now;
        }

        [Required]
        public string CreatedUserId { get; set; }

        [ForeignKey("CreatedUserId")]
        public Entities.Identity.SystemUser CreatedUser { get; set; }

        [Column(TypeName = "SmallDateTime")]
        public DateTime CreatedDateTime { get; set; }

        public string UpdatedUserId { get; set; }

        [ForeignKey("UpdatedUserId")]
        public Entities.Identity.SystemUser UpdatedUser { get; set; }

        [Column(TypeName = "SmallDateTime")]
        public DateTime? UpdatedDateTime { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
