using System;
using Regulus.Remoting;

namespace Regulus.Project.Lockstep.Game
{
    internal class MatchCondition : Regulus.Utility.Invoker<Party>
    {
        

        public readonly int PlayerAmount;

        public MatchCondition(int player_amount)
        {
            PlayerAmount = player_amount;
            Result = this;
        }

        public readonly Regulus.Utility.Invoker<Party> Result;

        public void Done(Party party)
        {
            base.Invoke(party);
        }
    }
}