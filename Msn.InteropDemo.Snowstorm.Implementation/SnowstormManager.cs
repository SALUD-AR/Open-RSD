using Microsoft.Extensions.Logging;
using Msn.InteropDemo.AppServices.Core;
using Msn.InteropDemo.Entities.Activity;
using Msn.InteropDemo.Integration.Configuration;
using Msn.InteropDemo.Snowstorm.Expressions.ExpresionBuilders.Base;
using Msn.InteropDemo.Snowstorm.Model.Components;
using Msn.InteropDemo.Snowstorm.Model.Response;
using Msn.InteropDemo.ViewModel.Activity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Msn.InteropDemo.Snowstorm.Implementation
{
    public class SnowstormManager: ISnowstormManager
    {
        private readonly ILogger<SnowstormManager> _logger;
        private readonly IntegrationServicesConfiguration _integrationServicesConfiguration;
        private readonly ICurrentContext _currentContext;

        public SnowstormManager(ILogger<SnowstormManager> logger,
                              IntegrationServicesConfiguration integrationServicesConfiguration,
                              AppServices.Core.ICurrentContext currentContext)
        {
            _logger = logger;
            _integrationServicesConfiguration = integrationServicesConfiguration;
            _currentContext = currentContext;
        }

        
        private QueryResponse ExecuteQuery(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                throw new ArgumentException("message", nameof(query));
            }


            var headerParams = Expressions.Constants.HeaderDefault.Headers;
            var serviceConfig = _integrationServicesConfiguration.GetConfigurationService(IntegrationServicesConfiguration.ConfigurationServicesName.SNOWSTORM);
            var endpoint = serviceConfig.GetEndPoint(IntegrationService.ConfigurationEndPointName.SNOWSTORM_FIND_CONCEPTS);

            var urlFriendly = $"{endpoint.FriendlyURL}?{query}";
            var url = $"{endpoint.URL}?{query}";

            var client = new HttpConsumer.SnowstormRequest(headerParams);
            var ret = client.Get<QueryResponse>(url);
            
            var activityLog = new ActivityLog
            {
                ActivityRequest = $"{url}",
                ActivityRequestUI = $"{urlFriendly}",
                ActivityTypeDescriptorId = (int)Entities.Activity.ActivityType.SNOWSTORM_FIND_CONCEPTS,
                ActivityResponse = $"Conceptos obtenidos Limite:{ret.Limit} sobre un Total de:{ret.Total}",
                RequestIsURL = true
            };

            _currentContext.RegisterActivityLog(activityLog);

            return ret;
        }

        private void FilterQueryReponseByTerms(ref RefsetQueryResponse queryResponse, string term)
        {
            var termArr = term.Split(' ');
            var filterdItems = queryResponse.Items.ToList();

            foreach (var str in termArr)
            {
                filterdItems = filterdItems.Where(x => x.referencedComponent.pt.Term.Contains(str) ||
                                                       x.referencedComponent.fsn.Term.Contains(str))
                                                .ToList();
            }

            queryResponse.Items = filterdItems;
        }

        /// <summary>
        /// Realiza un Query a Snowstorm
        /// </summary>
        /// <param name="builder">Bulder de Snowstorm.Expressions</param>
        /// <returns></returns>
        public QueryResponse RunQuery(BaseExpBuilder builder)
        {
            var query = builder.GetQueryString();

            return ExecuteQuery(query);
        }


        /// <summary>
        /// Realiza un Query a Snowstorm
        /// </summary>
        /// <param name="expression">Espresion de ECL</param>
        /// <param name="term">Termino a buscar</param>
        /// <returns></returns>
        public QueryResponse RunQuery(string expression, string term)
        {
            if (string.IsNullOrWhiteSpace(expression))
            {
                throw new ArgumentException("message", nameof(expression));
            }

            if (string.IsNullOrWhiteSpace(term))
            {
                throw new ArgumentException("message", nameof(term));
            }
            
            var ecl = System.Web.HttpUtility.UrlEncode(expression);
            //var ecl = expression;
            var query = $"activeFilter=true&ecl={ecl}&term={term}&offset={0}&limit={50}";
            var response = ExecuteQuery(query);

            return response;
        }
        

        /// <summary>
        /// Realiza un Query de un determinado Refset
        /// </summary>
        /// <param name="conceptId">El ConcepId del Refset en el cual se busca</param>
        /// <param name="term">Termino por el cual se filtran los datos obtenidos</param>
        /// <returns></returns>
        public RefsetQueryResponse RunQueryRefset(string conceptId, string term)
        {
            if (string.IsNullOrWhiteSpace(conceptId))
            {
                throw new ArgumentException("message", nameof(conceptId));
            }
            if (string.IsNullOrWhiteSpace(term))
            {
                throw new ArgumentException("message", nameof(term));
            }
            
            var query = $"referenceSet={conceptId}&activeFilter=true&offset={0}&limit={50}";
            var serviceConfig = _integrationServicesConfiguration.GetConfigurationService(IntegrationServicesConfiguration.ConfigurationServicesName.SNOWSTORM);
            var endpoint = serviceConfig.GetEndPoint(IntegrationService.ConfigurationEndPointName.SNOWSTORM_REFSET_MEMBERS);
            var url = $"{endpoint.URL}?{query}";

            var client = new HttpConsumer.SnowstormRequest(Expressions.Constants.HeaderDefault.Headers);
            var ret = client.Get<RefsetQueryResponse>(url);

            FilterQueryReponseByTerms(ref ret, term);

            var activityLog = new ActivityLog
            {
                ActivityRequest = $"{url}",
                ActivityTypeDescriptorId = (int)Entities.Activity.ActivityType.SNOWSTORM_FIND_CONCEPTS,
                ActivityResponse = $"Conceptos obtenidos Limite:{ret.Limit} sobre un Total de:{ret.Total}"
            };
            
            _currentContext.RegisterActivityLog(activityLog);

            return ret;
        }
    }
}
