using Msn.InteropDemo.Dfa.Base;

namespace Msn.InteropDemo.Dfa.Components
{
    public class State : StateBase
    {


        public State(string stateName, int stateIndex, bool isFinalState = false) :
                    base(stateName, stateIndex, isFinalState)
        {
        }

        //override public StateBase GetNextState(char token)
        //{
        //    var trans = transitions.FirstOrDefault(x => x.Token == token);
        //    if (trans != null)
        //    {
        //        return trans.ToState;
        //    }

        //    return this;

        //}





    }
}
