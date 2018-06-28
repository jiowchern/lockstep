namespace Regulus.Unity.MonoBehaviourProxy.Regulus.Project.Lockstep.Common
{
    public abstract class IInputtable : UnityEngine.MonoBehaviour
    {

        private global::Regulus.Project.Lockstep.Common.IInputtable _Core;
        public void Initial(global::Regulus.Project.Lockstep.Common.IInputtable core)
        {
            _Core = core;
            
        }
        public void Release()
        {
            
        }
        public void Input(global::Regulus.Project.Lockstep.Common.InputContent _1)
        {
             _Core.Input(_1);
        }
        
    }
}