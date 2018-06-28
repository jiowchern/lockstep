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
            _ToMatchSetting();
        }

        private void _ToMatchSetting()
        {
            var stage = new PlayerMatchSettingStage(_Binder);
            stage.DoneEvent += _ToMatchWaitting;
            _Machine.Push(stage);
        }

        private void _ToMatchWaitting(int player_count)
        {
            var stage = new PlayerMatchWaittingStage(_Matcher , player_count);
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