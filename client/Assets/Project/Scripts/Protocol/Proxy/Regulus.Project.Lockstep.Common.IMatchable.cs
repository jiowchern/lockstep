   
    using System;  
    
    using System.Collections.Generic;
    
    namespace Regulus.Project.Lockstep.Common.Ghost 
    { 
        public class CIMatchable : Regulus.Project.Lockstep.Common.IMatchable , Regulus.Remoting.IGhost
        {
            readonly bool _HaveReturn ;
            
            readonly Guid _GhostIdName;
            
            
            
            public CIMatchable(Guid id, bool have_return )
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
            
            
                void Regulus.Project.Lockstep.Common.IMatchable.Match(System.Int32 _1)
                {                    

                    Regulus.Remoting.IValue returnValue = null;
                    var info = typeof(Regulus.Project.Lockstep.Common.IMatchable).GetMethod("Match");
                    _CallMethodEvent(info , new object[] {_1} , returnValue);                    
                    
                }

                



            
        }
    }
