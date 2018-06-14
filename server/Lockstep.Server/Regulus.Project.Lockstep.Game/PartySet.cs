using Regulus.Framework;
using Regulus.Utility;

namespace Regulus.Project.Lockstep.Game
{
    internal class PartySet : IPartyProvider , Regulus.Utility.IUpdatable
    {
        private readonly Regulus.Utility.Updater _Partys;

        public PartySet()
        {
            _Partys = new Updater();
        }
        Party IPartyProvider.Spawn(int count)
        {
            var party = new Party(count);
            _Partys.Add(party);
            return party;
        }

        void IBootable.Launch()
        {
            
        }

        void IBootable.Shutdown()
        {
            _Partys.Shutdown();
        }

        bool IUpdatable.Update()
        {
            _Partys.Working();
            return true;
        }
    }
}