using Microsoft.Extensions.Logging;
using Msn.InteropDemo.AppServices.Core;
using Msn.InteropDemo.Entities.Activity;
using Msn.InteropDemo.ViewModel.Activity;
using System.Collections.Generic;
using System.Linq;

namespace Msn.InteropDemo.AppServices.Implementation.AppServices
{
    public class LogActivityAppService : Core.GenericService<ActivityLog>, ILogActivityAppService
    {
        public LogActivityAppService(ICurrentContext currentContext, ILogger<LogActivityAppService> logger) : base(currentContext, logger)
        {
        }

        public IEnumerable<ActivityLogListItemViewModel> GetActivityBySessionId(string sessionId)
        {
            var model = Get<ActivityLogListItemViewModel>(filter: x => x.SessionUserId == sessionId, 
                                                                             includeProperties: "ActivityTypeDescriptor", 
                                                                             orderBy: o=>o.OrderByDescending(x=>x.CreatedDateTime));
            return model;

        }
    }
}
