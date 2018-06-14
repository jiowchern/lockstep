using System;
using System.Linq;
using Regulus.Lockstep;
using Regulus.Project.Lockstep.Common;
using Regulus.Remoting;
using Regulus.Utility;

namespace Regulus.Project.Lockstep.Game
{
    internal class PlayerPlayStage : IStage , IInputtable, IListenable, ICommandProvidable<InputContent>
    {
        private readonly ISoulBinder _Binder;
        private readonly Party _Party;
        
        private InputContent _Current;
        private bool _EnableStepBoardcast;
        private readonly IPlayer<InputContent> _Player;
        public event Action DoneEvent;
        public PlayerPlayStage(ISoulBinder binder, Party party)
        {
            _Binder = binder;
            _Party = party;
            _Player = _Party.Regist(this);
        }

        void IStage.Enter()
        {
            _Binder.Bind<IInputtable>(this);
            _Binder.Bind<IListenable>(this);
        }

        void IStage.Leave()
        {
            _Binder.Unbind<IInputtable>(this);
            _Binder.Unbind<IListenable>(this);
            _Party.Unregist(_Player);
        }

        void IStage.Update()
        {
            if (_EnableStepBoardcast)
            {
                var steps = _Player.PopSteps();

                foreach (var step1 in steps)
                {
                    var step = new Step
                    {
                        Records = (from r in step1.Records
                            select new Common.Record() { Owner = r.Id, Content = r.Command }).ToArray()
                    };
                    _StepEvent.Invoke(step);
                }
            }

            if (!_Party.IsActivity())
            {
                DoneEvent();
            }
        }

        int IInputtable.Id { get { return _Player.Id; } }

        void IInputtable.Input(InputContent input_content)
        {
            _Current = input_content;
        }

        InputContent ICommandProvidable<InputContent>.Current { get { return _Current; } }

        void IListenable.SetEnable(bool enable)
        {
            _EnableStepBoardcast = enable;
        }

        private event Action<Step> _StepEvent;
        event Action<Step> IListenable.StepEvent
        {
            add { _StepEvent += value; }
            remove { _StepEvent -= value; }
        }
    }
}