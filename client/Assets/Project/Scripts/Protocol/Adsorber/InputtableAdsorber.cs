                    

namespace Regulus.Project.Lockstep.Common.Adsorption
{
    using System.Linq;
        
    public class InputtableAdsorber : UnityEngine.MonoBehaviour , Regulus.Remoting.Unity.Adsorber<IInputtable>
    {
        private readonly Regulus.Utility.StageMachine _Machine;        
        
        public string Agent;

        private global::Lockstep.Agent _Agent;

        [System.Serializable]
        public class UnityEnableEvent : UnityEngine.Events.UnityEvent<bool> {}
        public UnityEnableEvent EnableEvent;
        [System.Serializable]
        public class UnitySupplyEvent : UnityEngine.Events.UnityEvent<Regulus.Project.Lockstep.Common.IInputtable> {}
        public UnitySupplyEvent SupplyEvent;
        Regulus.Project.Lockstep.Common.IInputtable _Inputtable;                        
       
        public InputtableAdsorber()
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
            _Agent.Distributor.Attach<IInputtable>(this);
        }

        private void _DispatchLeave()
        {
            _Agent.Distributor.Detach<IInputtable>(this);
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

        public Regulus.Project.Lockstep.Common.IInputtable GetGPI()
        {
            return _Inputtable;
        }
        public void Supply(Regulus.Project.Lockstep.Common.IInputtable gpi)
        {
            _Inputtable = gpi;
            
            EnableEvent.Invoke(true);
            SupplyEvent.Invoke(gpi);
        }

        public void Unsupply(Regulus.Project.Lockstep.Common.IInputtable gpi)
        {
            EnableEvent.Invoke(false);
            
            _Inputtable = null;
        }
        
        public void Input(Regulus.Project.Lockstep.Common.InputContent input_content)
        {
            if(_Inputtable != null)
            {
                _Inputtable.Input(input_content);
            }
        }
        
        
    }
}
                    