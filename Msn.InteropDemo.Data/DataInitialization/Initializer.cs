using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Msn.InteropDemo.Data.Context;
using Msn.InteropDemo.Entities.Activity;
using Msn.InteropDemo.Entities.Pacientes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Msn.InteropDemo.Data.DataInitialization
{
    public class Initializer
    {
        private readonly ILogger<Initializer> _logger;
        private readonly DataContext _dataContext;
        private readonly UserManager<Entities.Identity.SystemUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public Initializer(DataContext dataContext,
                           UserManager<Entities.Identity.SystemUser> userManager,
                           RoleManager<IdentityRole> roleManager,
                           ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Initializer>();
            _dataContext = dataContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            try
            {
                await SeedTiposDocumentosAsync();
                await SeedRoles();
                await SeedDefaultAdminUser();
                await SeedActivityTypeDescriptorsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error inicializando datos", ex);
            }
        }

        private async Task SeedRoles()
        {
            if (!_dataContext.Roles.Any())
            {
                var roles = new List<IdentityRole>
                {
                    new IdentityRole { Id = "ADMINISTRADOR", Name = "ADMINISTRADOR", NormalizedName = "ADMINISTRADOR" },
                    new IdentityRole { Id = "OPERADOR", Name = "OPERADOR", NormalizedName = "OPERADOR" }
                };

                foreach (var item in roles)
                {
                    await _roleManager.CreateAsync(item);
                }

                await _dataContext.SaveChangesAsync();
            }
        }

        private async Task SeedDefaultAdminUser()
        {
            if (!_dataContext.Users.Any(x => x.UserName.ToUpper() == "ADMIN@MSNINTEROPDEMO.MSAL.GOB.AR"))
            {
                _logger.LogInformation("Incializando usuario Admin");

                var user = new Entities.Identity.SystemUser
                {
                    Id = "1",
                    UserName = "admin",
                    Email = "admin@msninteropdemo.msal.gob.ar",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = false,
                    Nombre = "admin",
                    Apellido = "interopdemo"
                };

                var result = _userManager.CreateAsync(user, "123456").Result;
                var roleResult = _userManager.AddToRoleAsync(user, "ADMINISTRADOR").Result;

                _logger.LogInformation($"Usuario registrado: IdentityResult IsSucceeded:{result.Succeeded}");

                await _dataContext.SaveChangesAsync();
            }
        }

        private async Task SeedTiposDocumentosAsync()
        {
            try
            {
                if (!_dataContext.TiposDocumento.Any())
                {
                    var lst = new List<TipoDocumento>
                {
                    new TipoDocumento { Id = 1, Nombre = "DNI", Enabled = true, Orden = 1 },
                    new TipoDocumento { Id = 2, Nombre = "DNI de la Madre", Enabled = true, Orden = 2 },
                    new TipoDocumento { Id = 3, Nombre = "Libreta Enrolamiento", Enabled = true, Orden = 3 },
                    new TipoDocumento { Id = 4, Nombre = "Libreta Cívica", Enabled = true, Orden = 4 },
                    new TipoDocumento { Id = 5, Nombre = "Pasaporte", Enabled = true, Orden = 5 }
                };

                    await _dataContext.TiposDocumento.AddRangeAsync(lst);
                    await _dataContext.SaveChangesAsync();
                    _logger.LogInformation("Datos de Tipos de Documento Inicializados");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inicializando Tipos de Documento");
            }
        }

        private async Task SeedActivityTypeDescriptorsAsync()
        {
            try
            {
                if (!_dataContext.ActivityTypeDescriptors.Any())
                {
                    var lst = new List<Entities.Activity.ActivityTypeDescriptor>
                {
                    new ActivityTypeDescriptor { Id = 1, Nombre = "OBTENER PACIENTE: DB LOCAL", Enabled = true, Orden = 1 },
                    new ActivityTypeDescriptor { Id = 2, Nombre = "BUSQUEDA PACIENTES COINCIDENTES: DB LOCAL", Enabled = true, Orden = 2 },
                    new ActivityTypeDescriptor { Id = 3, Nombre = "ALTA PACIENTE: DB LOCAL", Enabled = true, Orden = 3 },
                    new ActivityTypeDescriptor { Id = 4, Nombre = "OBTENER PACIENTE: BUS BY IDENTIFIER", Enabled = true, Orden = 4 },
                    new ActivityTypeDescriptor { Id = 5, Nombre = "BUSQUEDA PACIENTES: BUS BY MATCH", Enabled = true, Orden = 5 },
                    new ActivityTypeDescriptor { Id = 6, Nombre = "FEDERAR PACIENTE: BUS", Enabled = true, Orden = 6 },
                    new ActivityTypeDescriptor { Id = 7, Nombre = "BUSQUEDA DE CONCEPTOS EN SNOWSTORM", Enabled = true, Orden = 7}
                };

                    await _dataContext.ActivityTypeDescriptors.AddRangeAsync(lst);
                    await _dataContext.SaveChangesAsync();
                    _logger.LogInformation("Datos de Tipos de Actividad Inicializados");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inicializando Tipos de Actividad ");
            }
        }


    }
}
