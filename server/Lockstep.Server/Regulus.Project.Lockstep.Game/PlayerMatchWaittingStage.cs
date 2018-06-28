using System;
using Regulus.Remoting;
using Regulus.Utility;

namespace Regulus.Project.Lockstep.Game
{
    internal class PlayerMatchWaittingStage : IStage
    {
        private readonly Matcher _Matcher;
        
        private readonly Notifier<Party> _Notify;

        public System.Action<Party> DoneEvent;

        public PlayerMatchWaittingStage(Matcher matcher, int player_count)
        {
            
            _Matcher = matcher;
            _Notify =  _Matcher.Regist(player_count);
            _Notify.Subscribe += _Done;
        }

        private void _Done(Party obj)
        {
            DoneEvent(obj);
        }

        void IStage.Enter()
        {
            
        }

        void IStage.Leave()
        {
            _Matcher.Unregist(_Notify);
        }

        void IStage.Update()
        {
            
        }
    }
}