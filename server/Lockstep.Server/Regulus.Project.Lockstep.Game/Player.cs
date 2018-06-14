using System;
using System.Linq;
using Regulus.Framework;
using Regulus.Lockstep;
using Regulus.Project.Lockstep.Common;
using Regulus.Remoting;
using Regulus.Utility;

namespace Regulus.Project.Lockstep.Game
{
    internal class Player : IUpdatable  
    {
        private readonly ISoulBinder _Binder;
        private readonly Matcher _Matcher;        
        private bool _Enable;
        
        

        private readonly Regulus.Utility.StageMachine _Machine;
        

        public Player(ISoulBinder binder,Matcher matcher)
        {
            _Machine = new StageMachine();
            _Enable = true;
            _Binder = binder;
            _Matcher = matcher;
            
            _Binder.BreakEvent += _Quit;
           
        }

        private void _Quit()
        {
            _Enable = false;
        }

        void IBootable.Launch()
        {


            _ToMatch();

            
            
        }

        private void _ToMatch()
        {
            var stage = new PlayerMatchStage(_Binder, _Matcher);
            stage.DoneEvent += _ToGame;
            _Machine.Push(stage);
        }

        private void _ToGame(Party party )
        {
            var stage = new PlayerPlayStage(_Binder, party );
            stage.DoneEvent += () => _Enable = false;
            _Machine.Push(stage);
        }

        void IBootable.Shutdown()
        {
            _Machine.Termination();
        }

        bool IUpdatable.Update()
        {
            
            
            return _Enable;
        }
        

                
    }
}