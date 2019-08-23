using System;
using System.Collections.Generic;
using System.Linq;
using Msn.InteropDemo.Data.Context;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Msn.InteropDemo.Web.Helpers
{
    public class SelectListHelper : ISelectListHelper, IDisposable
    {
        private DataContext _dataContext;

        public SelectListHelper(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        // PRIVATE METHODS ***************************************************************************
        private SelectList GenerateList<TEntity>(string selectedValue = "") where TEntity : Entities.Core.EntityDescriptor, new()
        {
            var lst = _dataContext.Set<TEntity>()
                      .OrderBy(o => o.Nombre)
                      .Select(x =>
                      new SelectListItem
                      {
                          Text = x.Nombre,
                          Value = x.Id.ToString()
                      }
            ).ToList();

            lst.Insert(0, GetDefaultItem());

            return new SelectList(lst, "Value", "Text", selectedValue);
        }

        private SelectListItem GetDefaultItem(string text = "-- Seleccione --")
        {
            return new SelectListItem { Value = null, Text = text };
        }
        //*******************************************************************************************
        
        public SelectList GetTipoDocumentoList(string selectedValue = "")
        {
            var lst = GenerateList<Entities.Pacientes.TipoDocumento>(selectedValue);
            return lst;
        }

        public SelectList GetSexoList(string selectedValue = "")
        {
            var lst = new List<SelectListItem>
            {
                GetDefaultItem(),
                new SelectListItem { Value = "F", Text = "Femenino"},
                new SelectListItem { Value = "M", Text = "Masculino"}
            };

            return new SelectList(lst, "Value", "Text", selectedValue);
        }

        public void Dispose()
        {
            _dataContext.Dispose();
            _dataContext = null;
        }
    }

}
