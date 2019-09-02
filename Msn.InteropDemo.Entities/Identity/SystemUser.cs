using Microsoft.AspNetCore.Identity;

namespace Msn.InteropDemo.Entities.Identity
{
    public class SystemUser: IdentityUser
    {
        public SystemUser() : base()
        {
        }

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public long? CUIT { get; set; }

    }
}
