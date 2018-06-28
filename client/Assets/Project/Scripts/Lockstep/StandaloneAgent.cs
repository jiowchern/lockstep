using System.Collections;
using System.Collections.Generic;
using Regulus.Network.Rudp;
using Regulus.Remoting;
using UnityEngine;

namespace Regulus.Project.Lockstep
{
    public class StandaloneAgent : global::Lockstep.Agent
    {
        
        private readonly IAgent _Agent;

        public StandaloneAgent()
        {            
            var protocol = new global::Lockstep.Protocol();            
            _Agent = new Regulus.Remoting.Standalone.Agent(protocol);            
        }
        // Use this for initialization
        public override IAgent _GetAgent()
        {
            return _Agent;
        }
    }

}

