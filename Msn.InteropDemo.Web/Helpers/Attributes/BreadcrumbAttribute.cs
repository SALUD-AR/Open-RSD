using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Msn.InteropDemo.Web.Extensions;
using System;
using System.Linq;


namespace Msn.InteropDemo.Web.Helpers.Attributes
{
    public class BreadcrumbAttribute : Attribute, IActionFilter
    {
        private readonly string breadCrumbName;

        public BreadcrumbAttribute()
        {
            breadCrumbName = null;
        }

        public BreadcrumbAttribute(string name)
        {
            breadCrumbName = name;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            Models.BreadcrumbViewModel model;
            if (!context.HttpContext.Session.Keys.Contains("_BREAD_CRUMB_"))
            {
                model = new Models.BreadcrumbViewModel();
                context.HttpContext.Session.Set("_BREAD_CRUMB_", model);
            }
            else
            {
                model = context.HttpContext.Session.Get<Models.BreadcrumbViewModel>("_BREAD_CRUMB_");
            }

            if (!string.IsNullOrWhiteSpace(breadCrumbName))
            {
                var strFromUrl = context.HttpContext.Request.Headers["Referer"].ToString();
                var strToRul = context.HttpContext.Request.Path.Value;
                model.AddItem(breadCrumbName, strFromUrl, strToRul);
            }

            context.HttpContext.Session.Set("_BREAD_CRUMB_", model);

            var ctrlr = (Controller)context.Controller;
            ctrlr.ViewBag.Breadcrumb = model;
        }
    }
}
