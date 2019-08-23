using Msn.InteropDemo.Snowstorm.Expressions.Operators.Core;
using System.Collections.Generic;
using System.Linq;

namespace Msn.InteropDemo.Snowstorm.Expressions.ExpresionBuilders.Base
{
    public abstract class BaseExpBuilder : Core.IExpression
    {
        private List<HeaderParameter> headerParams;

        public BaseExpBuilder(EclOperatorFactory.EclOperatorType eclOperator,
                              string sctConceptId,
                              string sctConceptTerm = null,
                              string searchTerm = null,
                              bool? activeItems = true,
                              int offset = 0,
                              int limit = 50)
        {
            EclOperator = EclOperatorFactory.CreateAttOperator(eclOperator);
            SctConceptId = sctConceptId;
            SctConceptTerm = sctConceptTerm;
            SearchTerm = searchTerm;
            ActiveItems = activeItems;
            Offset = offset;
            Limit = limit;

            //headerParams = new List<HeaderParameter>
            //{
            //    new HeaderParameter("Accept", "application/json"),
            //    new HeaderParameter("Accept-Language", "es")
            //};

            headerParams = Constants.HeaderDefault.Headers.ToList();

            Attributes = new List<Attributes.Base.EclAttribute>();
        }

        public IEnumerable<HeaderParameter> GetHeaderParameters()
        {
            return headerParams.AsReadOnly();
        }

        public EclOperator EclOperator { get; }
        public string SctConceptId { get; }
        public string SctConceptTerm { get; }
        public string SearchTerm { get; set; }
        public bool? ActiveItems { get; set; }
        public int Offset { get; set; }
        public int Limit { get; set;}
        public List<Attributes.Base.EclAttribute> Attributes { get; set; }
        public bool IsRefset { get; set; }

        protected virtual string GetRenderAttributes(List<Attributes.Base.EclAttribute> attributes)
        {
            if (attributes == null || attributes.Count == 0)
            {
                return string.Empty;
            }

            var attrOperator = ":";
            var attSeparator = string.Empty;
            var beginSequence = string.Empty;
            var endSequence = string.Empty;
            var attrvts = string.Empty;

            if (attributes.Count == 0)
            {
                return string.Empty;
            }
            if (attributes.Count > 1)
            {
                attSeparator = ",";
                beginSequence = "{";
                endSequence = "}";
            }

            foreach (var item in attributes)
            {
                attrvts += $"{item.GetExpression()}{attSeparator}";
            }

            if (attrvts.LastIndexOf(attSeparator) > -1)
            {
                attrvts = attrvts.Remove(attrvts.LastIndexOf(attSeparator));
            }

            attrvts = $"{beginSequence}{attrvts}{endSequence}";

            return attrOperator + attrvts;
        }

        private string GetActiveExpression(bool? isActive)
        {
            if (isActive.HasValue)
            {
                return isActive.Value ? "activeFilter=true" : "activeFilter=false";
            }

            return string.Empty;
        }

        private string GetTermExpression(string term)
        {
            if(string.IsNullOrWhiteSpace(term))
            {
                return string.Empty;
            }

            return $"term={term}";
        }

        private string GetOffsetExpression(int offset)
        {
            return $"offset={offset}";
        }

        private string GetLimitExpression(int limit)
        {
            return $"limit={limit}";
        }



        public string GetExpression()
        {
            var expression = EclOperator.GetOperator;

            if (!string.IsNullOrWhiteSpace(SctConceptId))
            {
                expression += SctConceptId;

                if (!string.IsNullOrWhiteSpace(SctConceptTerm))
                {
                    expression += $"|{SctConceptTerm}|";
                }
            }

            expression += $"{GetRenderAttributes(Attributes)}";

            return expression;
        }

        public string GetQueryString(bool useEncodeUrl = true)
        {
            var eclExpression = GetExpression();

            if(!string.IsNullOrWhiteSpace(eclExpression))
            {
                if (useEncodeUrl)
                {
                    eclExpression = System.Web.HttpUtility.UrlEncode(eclExpression);
                }
                eclExpression = $"ecl={eclExpression}";
            }
            
            var expression = string.Empty;
            AddQueryParameter(ref expression, GetActiveExpression(this.ActiveItems));
            AddQueryParameter(ref expression, GetTermExpression(this.SearchTerm));
            AddQueryParameter(ref expression, eclExpression);
            AddQueryParameter(ref expression, GetOffsetExpression(this.Offset));
            AddQueryParameter(ref expression, GetLimitExpression(this.Limit));

            return expression;
        }


        private void AddQueryParameter(ref string exp, string parameter)
        {
            if(string.IsNullOrWhiteSpace(parameter))
            {
                return;
            }

            if(!string.IsNullOrWhiteSpace(exp))
            {
                exp += "&";
            }

            exp += parameter;
        }
    }
}
