using Msn.InteropDemo.Entities.Activity;
using System;

namespace Msn.InteropDemo.Fhir.ConsoleTest.Mock
{
    public class CurrentContext : AppServices.Core.ICurrentContext
    {

        public CurrentContext(Data.Context.DataContext dataContext)
        {
            DataContext = dataContext;
            SessionUserId = Guid.NewGuid().ToString();
        }

        public string GetCurrentUserId => "1";

        public Data.Context.DataContext DataContext { get; }

        public string SessionUserId { get; }


        public void RegisterActivityLog(ActivityLog activityLog)
        {
            activityLog.SessionUserId = SessionUserId;
            activityLog.ContextUserId = GetCurrentUserId;

            DataContext.ActivityLogs.Add(activityLog);
            DataContext.SaveChanges();
        }

        public void RegisterActivityLog(Entities.Activity.ActivityType activityType, string request, string response)
        {
            var activityLog = new ActivityLog
            {
                ActivityTypeDescriptorId = (int)activityType,
                ActivityRequest = request,
                ActivityResponse = response,
            };

            RegisterActivityLog(activityLog);
        }
    }
}
