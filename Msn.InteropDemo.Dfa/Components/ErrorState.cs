using Msn.InteropDemo.Dfa.Base;

namespace Msn.InteropDemo.Dfa.Components
{
    public class ErrorState : StateBase
    {
        public ErrorState(string stateName, int stateIndex) : base(stateName, stateIndex, false)
        {
            IsErrorState = true;
        }

        public override StateBase GetNextState(char token) => this;
    }
}
