using Msn.InteropDemo.Dfa.Components;
using System.Collections.Generic;
using System.Linq;

namespace Msn.InteropDemo.Dfa.Base
{
    public abstract class StateBase
    {
        private List<Transition> transitions = new List<Transition>();

        public StateBase() { }

        public StateBase(string stateName, int stateIndex, bool isFinalState = false)
        {
            StateName = stateName;
            StateIndex = stateIndex;
            IsFinalState = isFinalState;
        }

        public string StateName { get; set; }
        public int StateIndex { get; set; }

        public bool IsFinalState { get; set; }

        public virtual void AddTransition(Transition transition)
        {
            transition.FromState = this;
            transitions.Add(transition);
        }

        public virtual StateBase GetNextState(char token)
        {
            var trans = transitions.FirstOrDefault(x => x.Token == token);
            if (trans != null)
            {
                return trans.ToState;
            }

            return null;
        }


    }
}
