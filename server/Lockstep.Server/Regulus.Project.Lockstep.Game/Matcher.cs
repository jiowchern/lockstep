using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Regulus.Remoting;

namespace Regulus.Project.Lockstep.Game
{
    
    internal class Matcher
    {
        private readonly IPartyProvider _PartyProvider;
        private readonly List<MatchCondition> _Conditions;

        public Matcher(IPartyProvider party_provider)
        {
            _PartyProvider = party_provider;
            _Conditions = new List<MatchCondition>();
        }

        
        public void Unregist(Regulus.Utility.Notifier<Party> token)
        {
            _Conditions.RemoveAll(t => t.Result == token);

        }
        public Regulus.Utility.Notifier<Party> Regist(int player_amount)
        {


            var token = new MatchCondition(player_amount);
            _Conditions.Add(token);

            foreach (var partys in _Match(_Conditions))
            {
                var count = partys.Count();
                var party = _PartyProvider.Spawn(count);
                
                foreach (var matchCondition in partys)
                {                    
                    matchCondition.Done(party);
                }

            }
            return token.Result;
        }

        private IEnumerable<IEnumerable<MatchCondition>> _Match(List<MatchCondition> conditions)
        {
            


            var modes = from c in conditions group c by c.PlayerAmount ; 


            foreach (var mode in modes)
            {
                var playerCount = mode.Key;
                var partyCount = mode.Count() / playerCount;
                for (int i = 0; i < partyCount; i++)
                {
                    var players = mode.Skip(i * playerCount).Take(playerCount);
                    yield return players;
                }
            }
        }
    }
}