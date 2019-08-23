using Msn.InteropDemo.Web.Models;
using System.Collections.Generic;
using System.Linq;

namespace Msn.InteropDemo.Web.Models
{
    public class BreadcrumbViewModel
    {
        public BreadcrumbViewModel()
        {
            Items = new List<BreadcrumbItemViewModel>();
        }

        public BreadcrumbItemViewModel GetActiveBreadCrumb()
        {
            return Items.FirstOrDefault(x => x.IsActive);
        }

        public void AddItem(string activeText, string fromURL, string toURL)
        {
            var exists = Items.FirstOrDefault(x => x.ToURL == toURL);

            //si no existe el item previamente
            if (exists == null)
            {
                var active = Items.FirstOrDefault(x => x.IsActive);
                if (active != null)
                {
                    active.IsActive = false;
                }

                var item = new BreadcrumbItemViewModel
                {
                    FromURL = fromURL,
                    ToURL = toURL,
                    Text = activeText,
                    IsActive = true
                };

                Items.Add(item);
            }
            else
            {
                exists.IsActive = true;
                Items.RemoveAll(x => x.CreationDateTime > exists.CreationDateTime);
            }
        }

        public BreadcrumbItemViewModel AddItem(string text, string action = "", string controller = "", string area = "", bool isActive = false)
        {
            var item = new BreadcrumbItemViewModel
            {
                Text = text,
                Action = action,
                Controller = controller,
                IsActive = isActive
            };

            Items.Add(item);

            return item;

        }

        public List<BreadcrumbItemViewModel> Items { get; set; }

    }
}
