                    

namespace Regulus.Project.Lockstep.Common.Adsorption
{
    using System.Linq;
        
    public class ListenableAdsorber : UnityEngine.MonoBehaviour , Regulus.Remoting.Unity.Adsorber<IListenable>
    {
        private readonly Regulus.Utility.StageMachine _Machine;        
        
        public string Agent;

        private global::Lockstep.Agent _Agent;

        [System.Serializable]
        public class UnityEnableEvent : UnityEngine.Events.UnityEvent<bool> {}
        public UnityEnableEvent EnableEvent;
        [System.Serializable]
        public class UnitySupplyEvent : UnityEngine.Events.UnityEvent<Regulus.Project.Lockstep.Common.IListenable> {}
        public UnitySupplyEvent SupplyEvent;
        Regulus.Project.Lockstep.Common.IListenable _Listenable;                        
       
        public ListenableAdsorber()
        {
            _Machine = new Regulus.Utility.StageMachine();
        }

        void Start()
        {
            _Machine.Push(new Regulus.Utility.SimpleStage(_ScanEnter, _ScanLeave, _ScanUpdate));
        }

        private void _ScanUpdate()
        {
            var agents = UnityEngine.GameObject.FindObjectsOfType<global::Lockstep.Agent>();
            _Agent = agents.FirstOrDefault(d => string.IsNullOrEmpty(d.Name) == false && d.Name == Agent);
            if(_Agent != null)
            {
                _Machine.Push(new Regulus.Utility.SimpleStage(_DispatchEnter, _DispatchLeave));
            }            
        }

        private void _DispatchEnter()
        {
            _Agent.Distributor.Attach<IListenable>(this);
        }

        private void _DispatchLeave()
        {
            _Agent.Distributor.Detach<IListenable>(this);
        }

        private void _ScanLeave()
        {

        }


        private void _ScanEnter()
        {

        }

        void Update()
        {
            _Machine.Update();
        }

        void OnDestroy()
        {
            _Machine.Termination();
        }

        public Regulus.Project.Lockstep.Common.IListenable GetGPI()
        {
            return _Listenable;
        }
        public void Supply(Regulus.Project.Lockstep.Common.IListenable gpi)
        {
            _Listenable = gpi;
            _Listenable.StepEvent += _OnStepEvent;
            EnableEvent.Invoke(true);
            SupplyEvent.Invoke(gpi);
        }

        public void Unsupply(Regulus.Project.Lockstep.Common.IListenable gpi)
        {
            EnableEvent.Invoke(false);
            _Listenable.StepEvent -= _OnStepEvent;
            _Listenable = null;
        }
        
        public void SetEnable(System.Boolean enable)
        {
            if(_Listenable != null)
            {
                _Listenable.SetEnable(enable);
            }
        }
        
        
        [System.Serializable]
        public class UnityStepEvent : UnityEngine.Events.UnityEvent<Regulus.Project.Lockstep.Common.Step> { }
        public UnityStepEvent StepEvent;
        
        
        private void _OnStepEvent(Regulus.Project.Lockstep.Common.Step arg0)
        {
            StepEvent.Invoke(arg0);
        }
        
    }
}
                    