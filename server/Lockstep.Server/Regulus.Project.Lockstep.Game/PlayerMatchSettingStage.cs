using System;
using Regulus.Project.Lockstep.Common;
using Regulus.Remoting;
using Regulus.Utility;

namespace Regulus.Project.Lockstep.Game
{
    internal class PlayerMatchSettingStage : IStage , IMatchable
    {
        private readonly ISoulBinder _Binder;
        
        


        public event Action<int> DoneEvent;
        public PlayerMatchSettingStage(ISoulBinder binder)
        {
            _Binder = binder;        
        }

        

        void IStage.Enter()
        {
            _Binder.Bind<IMatchable>(this);
        }

        void IStage.Leave()
        {
            _Binder.Unbind<IMatchable>(this);
            
        }

        void IStage.Update()
        {
            
        }


        void IMatchable.Match(int player_amount)
        {
            DoneEvent(player_amount);
        }
    }
}