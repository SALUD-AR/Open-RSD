using Msn.InteropDemo.Dfa.Base;

namespace Msn.InteropDemo.Dfa.Components
{
    public class ErrorState : StateBase
    {
        public override StateBase GetNextState(char token) => this;
    }
}
