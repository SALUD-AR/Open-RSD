
namespace Msn.InteropDemo.ViewModel.Activity
{
    public class ActivityLogViewModel : Core.EntityAuditableModel
    {
        public string ActivityRequestUI { get; set; }

        public string ActivityRequest { get; set; }

        public string ActivityRequestBody { get; set; }

        public string ActivityResponse { get; set; }

        public string ActivityResponseBody { get; set; }

        public bool ResponseIsURL { get; set; }

        public bool ResponseIsJson { get; set; }

        public bool RequestIsURL { get; set; }

        public bool RequestIsJson { get; set; }

    }
}
