using Msn.InteropDemo.Dfa.Base;
using Msn.InteropDemo.Dfa.Components;
using System.Collections.Generic;

namespace Msn.InteropDemo.Dfa.Dfas
{
    /// <summary>
    /// Autómata finito para los simbolos de comparación
    /// </summary>
    public class ComparizonDfa : Base.Dfa
    {
        private List<StateBase> states;

        public ComparizonDfa()
        {
            var s0 = new State("State 0 Inicio", 0, true);

            states = new List<StateBase>
            {
                s0
            };

            s0.AddTransition(new Transition('<', s0));
            s0.AddTransition(new Transition('=', s0));
            s0.AddTransition(new Transition('>', s0));
            s0.AddTransition(new Transition('!', s0));

            States = states;
        }
    }
}
