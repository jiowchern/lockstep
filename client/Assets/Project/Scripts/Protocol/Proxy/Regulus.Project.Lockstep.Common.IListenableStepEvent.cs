
    using System;  
    using System.Collections.Generic;
    
    namespace Regulus.Project.Lockstep.Common.Invoker.IListenable 
    { 
        public class StepEvent : Regulus.Remoting.IEventProxyCreator
        {

            Type _Type;
            string _Name;
            
            public StepEvent()
            {
                _Name = "StepEvent";
                _Type = typeof(Regulus.Project.Lockstep.Common.IListenable);                   
            
            }
            Delegate Regulus.Remoting.IEventProxyCreator.Create(Guid soul_id,int event_id, Regulus.Remoting.InvokeEventCallabck invoke_Event)
            {                
                var closure = new Regulus.Remoting.GenericEventClosure<Regulus.Project.Lockstep.Common.Step>(soul_id , event_id , invoke_Event);                
                return new Action<Regulus.Project.Lockstep.Common.Step>(closure.Run);
            }
        

            Type Regulus.Remoting.IEventProxyCreator.GetType()
            {
                return _Type;
            }            

            string Regulus.Remoting.IEventProxyCreator.GetName()
            {
                return _Name;
            }            
        }
    }
                