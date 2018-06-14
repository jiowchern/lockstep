using System.Collections.Generic;
using System.Runtime.InteropServices;
using Regulus.Framework;
using Regulus.Lockstep;
using Regulus.Project.Lockstep.Common;
using Regulus.Utility;


namespace Regulus.Project.Lockstep.Game
{
    internal class Party :Regulus.Utility.IUpdatable
    {
        private int _Count;
        private readonly Regulus.Lockstep.Driver<InputContent> _Driver;
        private readonly TimeCounter _TimeCounter;
        private bool _Enable;

        public Party(int count)
        {
            _Count = count;
            _TimeCounter = new TimeCounter();            
            _Driver = new Driver<InputContent>(TimeCounter.SecondTicks / 10);
        }

        void IBootable.Launch()
        {
            _Enable = true;
        }

        void IBootable.Shutdown()
        {
            _Enable = false;
        }

        bool IUpdatable.Update()
        {
            var delta = _TimeCounter.Ticks;
            _Driver.Advance(delta);
            _TimeCounter.Reset();

            return _Enable;
        }

        public IPlayer<InputContent> Regist(ICommandProvidable<InputContent> provider)
        {        
            return _Driver.Regist(provider);
        }


        public void Unregist(IPlayer<InputContent> player)
        {

            _Driver.Unregist(player);
            if (--_Count == 0)
            {
                _Enable = false;
            }
            
        }

        public bool IsActivity()
        {
            return _Enable;
        }
    }

    




}
