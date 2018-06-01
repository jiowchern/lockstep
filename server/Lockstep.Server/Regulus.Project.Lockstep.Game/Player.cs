using System;
using Regulus.Framework;
using Regulus.Lockstep;
using Regulus.Project.Lockstep.Common;
using Regulus.Remoting;
using Regulus.Utility;

namespace Regulus.Project.Lockstep.Game
{
    internal class Player : IUpdatable , IInputtable , IListenable, ICommandProvidable<InputContent>
    {
        private readonly ISoulBinder _Binder;
        private readonly Boardcaster _Boardcaster;
        private bool _Enable;
        private InputContent _Current;
        private readonly IPlayer<InputContent> _Player;

        public Player(ISoulBinder binder,Boardcaster boardcaster)
        {
            _Enable = true;
            _Binder = binder;
            _Boardcaster = boardcaster;
            _Binder.BreakEvent += _Quit;
            _Player = _Boardcaster.Regist(this);
        }

        private void _Quit()
        {
            _Enable = false;
        }

        void IBootable.Launch()
        {

            
            
            _Binder.Bind<IInputtable>(this);
        }

        void IBootable.Shutdown()
        {
            _Boardcaster.Unregist(_Player);
            _Binder.Unbind<IInputtable>(this);
        }

        bool IUpdatable.Update()
        {
            var steps = _Player.PopSteps();
            return _Enable;
        }

        Guid IInputtable.Id { get { return _Player.Id; } }

        void IInputtable.Input(InputContent input_content)
        {
            _Current = input_content;
        }

        InputContent ICommandProvidable<InputContent>.Current { get { return _Current; } }
    }

    internal interface IListenable
    {
    }
}