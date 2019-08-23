using Microsoft.AspNetCore.Http;
using Msn.InteropDemo.Entities.Activity;
using Msn.InteropDemo.ViewModel.Activity;
using Msn.InteropDemo.Web.Extensions;

namespace Msn.InteropDemo.Web.Security
{
    public class CurrentContext : AppServices.Core.ICurrentContext
    {
        private readonly HttpContext _httpContext;
        private readonly Data.Context.DataContext _dataContext;

        public CurrentContext(IHttpContextAccessor httpContextAccessor, Data.Context.DataContext dataContext)
        {
            _httpContext = httpContextAccessor.HttpContext;
            _dataContext = dataContext;
        }


        public string GetCurrentUserId
        {
            get
            {
                #if (DEBUG)
                    if(string.IsNullOrWhiteSpace(_httpContext.User.GetUserId()))
                    {
                        return "1";
                    }
                #endif

                return _httpContext.User.GetUserId();
            }
        }

        public Data.Context.DataContext DataContext => _dataContext;

        public string SessionUserId => _httpContext.Session.Id;


        public void RegisterActivityLog(ActivityLog activityLog)
        {
            activityLog.SessionUserId = SessionUserId;
            activityLog.CreatedUserId = GetCurrentUserId;

            _dataContext.ActivityLogs.Add(activityLog);
            _dataContext.SaveChanges();
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
