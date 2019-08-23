using System;

namespace Msn.InteropDemo.Web.Models
{
    public class BreadcrumbItemViewModel
    {
        public BreadcrumbItemViewModel()
        {
            CreationDateTime = DateTime.Now;
        }

        public string Text { get; set; }

        public string Action { get; set; }

        public string Controller { get; set; }

        public string FromURL { get; set; }

        public string ToURL { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreationDateTime { get; }
    }
}
