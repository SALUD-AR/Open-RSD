using Msn.InteropDemo.Dfa.Base;

namespace Msn.InteropDemo.Dfa.Components
{
    public class Transition
    {
        public Transition(char token, StateBase toState)
        {
            Token = token;
            ToState = toState;
        }

        public StateBase FromState { get; set; }
        public char Token { get; }
        public StateBase ToState { get; set; }
    }
}
