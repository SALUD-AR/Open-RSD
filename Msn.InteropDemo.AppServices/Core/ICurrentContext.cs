
namespace Msn.InteropDemo.AppServices.Core
{
    public interface ICurrentContext
    {
        string GetCurrentUserId { get; }

        Data.Context.DataContext DataContext { get; }

        string SessionUserId { get; }

        void RegisterActivityLog(Entities.Activity.ActivityType activityType, string request, string response);
        void RegisterActivityLog(Entities.Activity.ActivityLog activityLog);
    }
}
