using Msn.InteropDemo.ViewModel.Snomed;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Msn.InteropDemo.AppServices.Implementation.Internal
{
    internal class SnomedToCie10Mapper
    {
        Lazy<Dfa.Dfas.ComparizonDfa>  dfaComparizon;
        Lazy<Dfa.Dfas.IntegerNumberDfa> dfaNumber;
        Lazy<Dfa.Notifications.MatchCollector> matchCollector;

        public SnomedToCie10Mapper()
        {
            dfaComparizon = new Lazy<Dfa.Dfas.ComparizonDfa>();
            dfaNumber = new Lazy<Dfa.Dfas.IntegerNumberDfa>();
            matchCollector = new Lazy<Dfa.Notifications.MatchCollector>();
        }

        public void SetMapeoPreferido(IEnumerable<Cie10MapResultViewModel> items,
                                       string sexo,
                                       int edad)
        {
            var groups = from i in items
                         group i by i.MapGroup into newGroup
                         orderby newGroup.Key
                         select newGroup;

            foreach (var g in groups)
            {
                foreach (var item in g)
                {
                    //Rancking por Sexo
                    SetRankingSexo(item, sexo);

                    //Rancking  por Edad
                    SetRankingEdad(item, edad);
                }

                var maxRank = g.Max(x => x.RankingPreferido);
                if((maxRank == 0) && (g.Count() > 1)) //ninguno item tiene Ranking en el Grupo
                {
                    var ot = g.Where(x => x.MapRule.Contains("OTHERWISE TRUE"));
                    foreach (var item in ot)
                    {
                        item.RankingPreferido++;
                    }

                    maxRank = 1;
                }

                var preferidos = g.Where(x => x.RankingPreferido == maxRank);

                foreach (var item in preferidos)
                {
                    item.EsMapeoPreferido = true;
                }
            }
        }

        private void SetRankingSexo(Cie10MapResultViewModel item, string sexo)
        {
            if (((item.MapRule.Contains("248153007") && sexo == "M") || (item.MapRule.Contains("248152002") && sexo == "F")))
            {
                item.RankingPreferido++;
            }
        }

        private void SetRankingEdad(Cie10MapResultViewModel item, int edad)
        {
            string ruleSimbol;
            string ruleAge;

            if (item.MapRule.Contains("424144002"))
            {
                var textToParse = item.MapRule.Split('|')[2].ToCharArray();

                matchCollector.Value.CollectorResult = string.Empty;
                dfaComparizon.Value.CollectTokens(textToParse, matchCollector.Value);
                ruleSimbol = matchCollector.Value.CollectorResult;

                matchCollector.Value.CollectorResult = string.Empty;
                dfaNumber.Value.CollectTokens(textToParse, matchCollector.Value);
                ruleAge = matchCollector.Value.CollectorResult;

                if(CompareEdad(ruleSimbol, ruleAge, edad))
                {
                    item.RankingPreferido++;
                }
            }
        }

        private bool CompareEdad(string ruleSimbol, string ruleAge, int edad)
        {
            if (ruleSimbol is null)
            {
                throw new ArgumentNullException(nameof(ruleSimbol));
            }

            if (ruleAge is null)
            {
                throw new ArgumentNullException(nameof(ruleAge));
            }

            if (!int.TryParse(ruleAge, out var ruleEdad))
            {
                throw new Exception($"No se pudo parsear la Edad del la Rule:{ruleAge}");
            }
            if(ruleEdad >= 100)
            {
                ruleEdad = ruleEdad / 10;
            }

            switch (ruleSimbol)
            {
                case "<":
                    return edad < ruleEdad;
                case "<=":
                    return edad <= ruleEdad;
                case "=":
                    return edad == ruleEdad;
                case ">=":
                    return edad >= ruleEdad;
                case ">":
                    return edad > ruleEdad;
                case "!":
                    return edad != ruleEdad;
                case "<>":
                    return edad != ruleEdad;
            }

            return false;
        }
    }
}
