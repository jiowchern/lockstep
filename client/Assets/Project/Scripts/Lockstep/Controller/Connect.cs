using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Regulus.Project.Lockstep.Unity.Controller
{
    public class Connect : MonoBehaviour
    {
        public global::Lockstep.Agent Agent;
        // Use this for initialization
        public UnityEngine.GameObject Panel; 

        public void Launch()
        {
            Panel.SetActive(false);
            Agent.Connect("127.0.0.1" , 12345);            
        }

        public void ConnectResult(bool success)
        {
            Panel.SetActive(!success);
        }
    }

}
