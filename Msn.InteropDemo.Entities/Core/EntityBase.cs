using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Msn.InteropDemo.Entities.Core
{
    public abstract class EntityBase : EntityBase<int>
    {
    }

    public abstract class EntityBase<TKey>
    {
        public EntityBase()
        {
            Enabled = true;
        }

        public TKey Id { get; set; }

        public bool Enabled { get; set; }

        [NotMapped]
        public string ContextUserId { get; set; }
    }
}
