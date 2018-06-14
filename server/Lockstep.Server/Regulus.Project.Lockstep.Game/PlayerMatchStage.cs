using System;
using Regulus.Project.Lockstep.Common;
using Regulus.Remoting;
using Regulus.Utility;

namespace Regulus.Project.Lockstep.Game
{
    internal class PlayerMatchStage : IStage 
    {
        private readonly Matcher _Matcher;
        private readonly Notifier<Party> _Notifier;


        public event Action<Party > DoneEvent;
        public PlayerMatchStage(ISoulBinder binder, Matcher matcher)
        {
            _Matcher = matcher;

            _Notifier = matcher.Regist(2);
            _Notifier.Subscribe += _Done;
        }

        private void _Done(Party info)
        {
            DoneEvent(info);
        }

        void IStage.Enter()
        {            
            
        }

        void IStage.Leave()
        {
            _Matcher.Unregist(_Notifier);
        }

        void IStage.Update()
        {
            
        }

        
    }
}