using System;
using System.Collections.Generic;
using System.Text;

namespace Msn.InteropDemo.Dfa.Dfas
{
    public class NumberDfa : Base.Dfa
    {
        public NumberDfa()
        {
            var s0 = new Components.State("Numeros Inicial - Final", 0, true);
            var s1 = new Components.State("Punto Separador decimales", 1, false);
            var s2 = new Components.State("Numeros - Final", 1, false);

            s0.AddTransition(new Components.Transition('0', s0));
            s0.AddTransition(new Components.Transition('1', s0));
            s0.AddTransition(new Components.Transition('2', s0));
            s0.AddTransition(new Components.Transition('3', s0));
            s0.AddTransition(new Components.Transition('4', s0));
            s0.AddTransition(new Components.Transition('5', s0));
            s0.AddTransition(new Components.Transition('6', s0));
            s0.AddTransition(new Components.Transition('7', s0));
            s0.AddTransition(new Components.Transition('8', s0));
            s0.AddTransition(new Components.Transition('9', s0));
            s0.AddTransition(new Components.Transition('.', s1));

            s1.AddTransition(new Components.Transition('0', s2));
            s1.AddTransition(new Components.Transition('1', s2));
            s1.AddTransition(new Components.Transition('2', s2));
            s1.AddTransition(new Components.Transition('3', s2));
            s1.AddTransition(new Components.Transition('4', s2));
            s1.AddTransition(new Components.Transition('5', s2));
            s1.AddTransition(new Components.Transition('6', s2));
            s1.AddTransition(new Components.Transition('7', s2));
            s1.AddTransition(new Components.Transition('8', s2));
            s1.AddTransition(new Components.Transition('9', s2));

            s2.AddTransition(new Components.Transition('0', s2));
            s2.AddTransition(new Components.Transition('1', s2));
            s2.AddTransition(new Components.Transition('2', s2));
            s2.AddTransition(new Components.Transition('3', s2));
            s2.AddTransition(new Components.Transition('4', s2));
            s2.AddTransition(new Components.Transition('5', s2));
            s2.AddTransition(new Components.Transition('6', s2));
            s2.AddTransition(new Components.Transition('7', s2));
            s2.AddTransition(new Components.Transition('8', s2));
            s2.AddTransition(new Components.Transition('9', s2));

            States.Add(s0);
            States.Add(s1);
            States.Add(s2);
        }
    }
}
