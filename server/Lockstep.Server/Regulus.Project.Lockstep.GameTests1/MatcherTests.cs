using NUnit.Framework;
using Regulus.Project.Lockstep.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NSubstitute.Core;

namespace Regulus.Project.Lockstep.Game.Tests
{
    [TestFixture()]
    public class MatcherTests
    {
        
          
        [Test()]
        public void RegistTest()
        {
            var party = new Party(2);
            var partyProvider = NSubstitute.Substitute.For<Regulus.Project.Lockstep.Game.IPartyProvider>();
            partyProvider.Spawn(2).Returns(party);

            var matcher = new Regulus.Project.Lockstep.Game.Matcher(partyProvider);

            var result1 = matcher.Regist(2);
            var result2 = matcher.Regist(2);

            Party party1 = null;
            result1.Subscribe += (args) => party1 = args;

            Party party2 = null;
            result2.Subscribe += (args) => party2 = args;



            Assert.AreEqual(party, party1);
            Assert.AreEqual(party, party1);

            matcher.Unregist(result1);
            matcher.Unregist(result2);
        }
    }
}