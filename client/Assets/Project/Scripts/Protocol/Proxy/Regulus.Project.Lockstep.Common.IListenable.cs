   
    using System;  
    
    using System.Collections.Generic;
    
    namespace Regulus.Project.Lockstep.Common.Ghost 
    { 
        public class CIListenable : Regulus.Project.Lockstep.Common.IListenable , Regulus.Remoting.IGhost
        {
            readonly bool _HaveReturn ;
            
            readonly Guid _GhostIdName;
            
            
            
            public CIListenable(Guid id, bool have_return )
            {
                _HaveReturn = have_return ;
                _GhostIdName = id;            
            }
            

            Guid Regulus.Remoting.IGhost.GetID()
            {
                return _GhostIdName;
            }

            bool Regulus.Remoting.IGhost.IsReturnType()
            {
                return _HaveReturn;
            }
            object Regulus.Remoting.IGhost.GetInstance()
            {
                return this;
            }

            private event Regulus.Remoting.CallMethodCallback _CallMethodEvent;

            event Regulus.Remoting.CallMethodCallback Regulus.Remoting.IGhost.CallMethodEvent
            {
                add { this._CallMethodEvent += value; }
                remove { this._CallMethodEvent -= value; }
            }
            
            
                void Regulus.Project.Lockstep.Common.IListenable.SetEnable(System.Boolean _1)
                {                    

                    Regulus.Remoting.IValue returnValue = null;
                    var info = typeof(Regulus.Project.Lockstep.Common.IListenable).GetMethod("SetEnable");
                    _CallMethodEvent(info , new object[] {_1} , returnValue);                    
                    
                }

                



                System.Action<Regulus.Project.Lockstep.Common.Step> _StepEvent;
                event System.Action<Regulus.Project.Lockstep.Common.Step> Regulus.Project.Lockstep.Common.IListenable.StepEvent
                {
                    add { _StepEvent += value;}
                    remove { _StepEvent -= value;}
                }
                
            
        }
    }
