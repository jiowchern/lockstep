using System.Collections.Generic;
using Regulus.Framework;
using Regulus.Lockstep;
using Regulus.Project.Lockstep.Common;
using Regulus.Utility;


namespace Regulus.Project.Lockstep.Game
{
    internal class Boardcaster :Regulus.Utility.IUpdatable
    {
        private Regulus.Lockstep.Driver<InputContent> _Driver;
        private readonly TimeCounter _TimeCounter;
        public Boardcaster()
        {
            _TimeCounter = new TimeCounter();            
            _Driver = new Driver<InputContent>(TimeCounter.SecondTicks / 10);
        }

        void IBootable.Launch()
        {
            
        }

        void IBootable.Shutdown()
        {
            
        }

        bool IUpdatable.Update()
        {
            
            _Driver.Advance(_TimeCounter.Ticks);
            _TimeCounter.Reset();

            return true;
        }

        public IPlayer<InputContent> Regist(ICommandProvidable<InputContent> provider)
        {        
            return _Driver.Regist(provider);
        }


        public void Unregist(IPlayer<InputContent> player)
        {
            _Driver.Unregist(player);
        }
    }

    




}
