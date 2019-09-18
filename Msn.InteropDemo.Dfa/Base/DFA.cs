using Msn.InteropDemo.Dfa.Notifications;
using System.Collections.Generic;

namespace Msn.InteropDemo.Dfa.Base
{
    /// <summary>
    /// DFA = Deterministic Finite Automata
    /// </summary>
    public abstract class Dfa
    {
        public Dfa() {
            States = new List<StateBase>();
        }

        public Dfa(List<StateBase> states)
        {
            States = states;
        }

        public List<StateBase> States { get; protected set; }

        /// <summary>
        /// Recolecta los Tokens validos 
        /// </summary>
        /// <param name="caracters">caracteres a evaluar por el automata</param>
        /// <param name="matchNotificator">Objeto al cual se llamara en cada coincidencia de la Sintaxis</param>
        /// <returns>True si cumplio con la sintaxis, caso contrario False</returns>
        public virtual bool CollectTokens(char[] caracters, IMatchNotificator matchNotificator)
        {
            var currentState = States[0];
            StateBase nextState = null;
            foreach (var c in caracters)
            {
                nextState = CollectToken(currentState, c, matchNotificator);
                if (nextState != null)
                {
                    currentState = nextState;
                }

                //nextState = currentState.GetNextState(c);
                //if (nextState != null)
                //{
                //    matchNotificator.Notificate(c);
                //    currentState = nextState;
                //}
            }

            return currentState != null && currentState.IsFinalState;
            //return nextState != null && nextState.IsFinalState;
        }

        public virtual StateBase CollectToken(StateBase currentState, char c, IMatchNotificator matchNotificator)
        {
            var nextState = currentState.GetNextState(c);
            if (nextState != null)
            {
                matchNotificator.Notificate(c);
            }

            return nextState;
        }

        /// <summary>
        /// Valida si se cumple con la Sintaxis 
        /// </summary>
        /// <param name="caracters">caracteres a evaluar por el automata</param>
        /// <returns>True si cumplio con la sintaxis, caso contrario False</returns>
        public virtual bool Validate(char[] caracters)
        {
            var nextState = States[0];
            foreach (var c in caracters)
            {
                nextState = nextState.GetNextState(c);
                if (nextState == null)
                {
                    return false;
                }
            }

            return nextState != null && nextState.IsFinalState;
        }
    }
}
