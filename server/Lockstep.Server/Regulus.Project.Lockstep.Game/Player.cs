using System;
using Regulus.Framework;
using Regulus.Project.Lockstep.Common;
using Regulus.Remoting;
using Regulus.Utility;

namespace Regulus.Project.Lockstep.Game
{
    internal class Player : IUpdatable , IInputtable , IListenable
    {
        private readonly ISoulBinder _Binder;
        private readonly Broadcaster _broadcaster;
        private bool _Enable;

        public Player(ISoulBinder binder,Broadcaster broadcaster)
        {
            _Enable = true;
            _Binder = binder;
            _broadcaster = broadcaster;
            _Binder.BreakEvent += _Quit;
        }

        private void _Quit()
        {
            _Enable = false;
        }

        void IBootable.Launch()
        {
            var driver = new Regulus.
            //_broadcaster.Register();
            _Binder.Bind<IInputtable>(this);
        }

        void IBootable.Shutdown()
        {
            _Binder.Unbind<IInputtable>(this);
        }

        bool IUpdatable.Update()
        {
            return _Enable;
        }

        void IInputtable.Input(InputContent inputContent)
        {
            throw new NotImplementedException();
        }
    }

    internal interface IListenable
    {
    }
}