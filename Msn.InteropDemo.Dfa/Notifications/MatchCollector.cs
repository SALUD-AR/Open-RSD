using System;
using System.Collections.Generic;
using System.Text;

namespace Msn.InteropDemo.Dfa.Notifications
{
    public class MatchCollector : IMatchNotificator
    {
        public void Notificate(char c)
        {
            CollectorResult += c;
        }

        public string CollectorResult { get; private set; }


    }
}
