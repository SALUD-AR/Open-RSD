using System;
using System.Collections.Generic;
using System.Text;

namespace Msn.InteropDemo.Dfa.Dfas
{
    public class IntegerNumberDfa : Base.Dfa
    {
        public IntegerNumberDfa()
        {
            var s0 = new Components.State("Numeros Inicial - Final", 0, true);
            //var s1 = new Components.ErrorState("Punto", 1);
            //var s2 = new Components.ErrorState("Comma", 2);
            //var s3 = new Components.ErrorState("Espacio", 2);

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

            //s0.AddTransition(new Components.Transition('.', s1));
            //s0.AddTransition(new Components.Transition(',', s2));
            //s0.AddTransition(new Components.Transition(' ', s3));

            States.Add(s0);
            //States.Add(s1);
            //States.Add(s2);
            //States.Add(s3);
        }
    }
}
