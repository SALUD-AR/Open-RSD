using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Msn.InteropDemo.Web.Extensions
{
    public static class ListExtension
    {
        public static SelectList ToSelectList<T>(this IList<T> lst, Func<T, int> dataValueField, Func<T, string> dataTextField, string defaultValue = "", string defaultText = " -- Seleccione -- ", Func<T, bool> selectedValue = null)
        {
            IList<SelectListItem> items;

            if (selectedValue != null)
            {
                items = lst.Select(x => new SelectListItem
                {
                    Value = dataValueField(x).ToString(),
                    Text = dataTextField(x),
                    Selected = selectedValue(x)
                }).ToList();
            }
            else
            {
                items = lst.Select(x => new SelectListItem
                {
                    Value = dataValueField(x).ToString(),
                    Text = dataTextField(x)
                }).ToList();
            }

            var defItem = new SelectListItem();
            defItem.Value = defaultValue;
            defItem.Text = defaultText;

            items.Insert(0, defItem);
            var slist = new SelectList(items, "Value", "Text");

            return slist;
        }

        public static IEnumerable<SelectListItem> ToSelectItems<T>(this IList<T> lst, Func<T, int> dataValueField, Func<T, string> dataTextField, Func<T, bool> selectedValue = null)
        {
            IList<SelectListItem> items;

            if (selectedValue != null)
            {
                items = lst.Select(x => new SelectListItem
                {
                    Value = dataValueField(x).ToString(),
                    Text = dataTextField(x),
                    Selected = selectedValue(x)
                }).ToList();
            }
            else
            {
                items = lst.Select(x => new SelectListItem
                {
                    Value = dataValueField(x).ToString(),
                    Text = dataTextField(x)
                }).ToList();
            }

            return items;
        }

        //public static IEnumerable<SelectListItem> ToSelectListItems<T>(this IEnumerable<T> items, Func<T, string> name, Func<T, string> value, string selectedValue)
        //{
        //    return items.ToSelectListItems(name, value, selectedValue, false);
        //}

        //public static IEnumerable<SelectListItem> ToSelectListItems<T>(this IEnumerable<T> items, Func<T, string> name, Func<T, string> value, string selectedValue, bool includeNotApplicable, string notApplicableValue = "", string notApplicableText = "(Not Applicable)")
        //{
        //    return items.ToSelectListItems(name, value, x => value(x) == selectedValue, includeNotApplicable, notApplicableValue, notApplicableText);
        //}

        //public static IEnumerable<SelectListItem> ToSelectListItems<T>(this IEnumerable<T> items, Func<T, string> name, Func<T, string> value, Func<T, bool> isSelected)
        //{
        //    return items.ToSelectListItems(name, value, isSelected, false);
        //}

        //public static IEnumerable<SelectListItem> ToSelectListItems<T>(this IEnumerable<T> items, Func<T, string> name, Func<T, string> value, Func<T, bool> isSelected, bool includeNotApplicable, string notApplicableValue = "", string notApplicableText = "(Not Applicable)")
        //{
        //    if (includeNotApplicable)
        //    {
        //        var returnItems = new List<SelectListItem>
        //                  {
        //                    new SelectListItem
        //                      {
        //                        Text = notApplicableText,
        //                        Value = notApplicableValue,
        //                        Selected = false
        //                      }
        //                  };

        //        if (items != null)
        //        {
        //            returnItems.AddRange(items.Select(item => new SelectListItem
        //            {
        //                Text = name(item),
        //                Value = value(item),
        //                Selected = isSelected(item)
        //            }));
        //        }
        //        return returnItems;
        //    }

        //    if (items == null)
        //        return new List<SelectListItem>();

        //    return items.Select(item => new SelectListItem
        //    {
        //        Text = name(item),
        //        Value = value(item),
        //        Selected = isSelected(item)
        //    });
        //}

        //public static IEnumerable<SelectListItem> ToMultiSelectListItems<T>(this IEnumerable<T> items, Func<T, string> name, Func<T, string> value, IEnumerable<int> selectedValues)
        //{
        //    if (selectedValues == null)
        //        selectedValues = new List<int>();
        //    return items.ToMultiSelectListItems(name, value, selectedValues.Select(x => x.ToString()));
        //}

        //public static IEnumerable<SelectListItem> ToMultiSelectListItems<T>(this IEnumerable<T> items, Func<T, string> name, Func<T, string> value, IEnumerable<long> selectedValues)
        //{
        //    if (selectedValues == null)
        //        selectedValues = new List<long>();
        //    return items.ToMultiSelectListItems(name, value, selectedValues.Select(x => x.ToString()));
        //}

        //public static IEnumerable<SelectListItem> ToMultiSelectListItems<T>(this IEnumerable<T> items, Func<T, string> name, Func<T, string> value, IEnumerable<double> selectedValues)
        //{
        //    if (selectedValues == null)
        //        selectedValues = new List<double>();
        //    return items.ToMultiSelectListItems(name, value, selectedValues.Select(x => x.ToString()));
        //}

        //public static IEnumerable<SelectListItem> ToMultiSelectListItems<T>(this IEnumerable<T> items, Func<T, string> name, Func<T, string> value, IEnumerable<decimal> selectedValues)
        //{
        //    if (selectedValues == null)
        //        selectedValues = new List<decimal>();
        //    return items.ToMultiSelectListItems(name, value, selectedValues.Select(x => x.ToString()));
        //}

        //public static IEnumerable<SelectListItem> ToMultiSelectListItems<T>(this IEnumerable<T> items, Func<T, string> name, Func<T, string> value, IEnumerable<string> selectedValues)
        //{
        //    if (items == null)
        //        return new List<SelectListItem>();

        //    if (selectedValues == null)
        //        selectedValues = new List<string>();

        //    return items.ToMultiSelectListItems(name, value, x => selectedValues.Contains(value(x)));
        //}

        //public static IEnumerable<SelectListItem> ToMultiSelectListItems<T>(this IEnumerable<T> items, Func<T, string> name, Func<T, string> value, Func<T, bool> isSelected)
        //{
        //    if (items == null)
        //        return new List<SelectListItem>();

        //    return items.Select(item => new SelectListItem
        //    {
        //        Text = name(item),
        //        Value = value(item),
        //        Selected = isSelected(item)
        //    });
        //}
    }

}
