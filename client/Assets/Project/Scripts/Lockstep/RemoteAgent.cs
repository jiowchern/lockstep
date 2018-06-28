using System.Collections;
using System.Collections.Generic;
using Regulus.Network.Rudp;
using Regulus.Remoting;
using UnityEngine;


namespace Regulus.Project.Lockstep
{

    public class RemoteAgent : global::Lockstep.Agent
    {
        private readonly IAgent _Agent;

        public RemoteAgent()
        {
            var protocol = new global::Lockstep.Protocol();
            _Agent = Remoting.Ghost.Native.Agent.Create(protocol, new Client(new UdpSocket()));
        }
        // Use this for initialization
        public override IAgent _GetAgent()
        {
            return _Agent;
        }
    }

}
