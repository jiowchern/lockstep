   
    using System;  
    
    using System.Collections.Generic;
    
    namespace Regulus.Project.Lockstep.Common.Ghost 
    { 
        public class CIInputtable : Regulus.Project.Lockstep.Common.IInputtable , Regulus.Remoting.IGhost
        {
            readonly bool _HaveReturn ;
            
            readonly Guid _GhostIdName;
            
            
            
            public CIInputtable(Guid id, bool have_return )
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
            
            
                void Regulus.Project.Lockstep.Common.IInputtable.Input(Regulus.Project.Lockstep.Common.InputContent _1)
                {                    

                    Regulus.Remoting.IValue returnValue = null;
                    var info = typeof(Regulus.Project.Lockstep.Common.IInputtable).GetMethod("Input");
                    _CallMethodEvent(info , new object[] {_1} , returnValue);                    
                    
                }

                


                System.Int32 _Id;
                System.Int32 Regulus.Project.Lockstep.Common.IInputtable.Id { get{ return _Id;} }

            
        }
    }
