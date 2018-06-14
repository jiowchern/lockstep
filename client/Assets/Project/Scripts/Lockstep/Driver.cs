using System.Collections;
using System.Collections.Generic;
using Regulus.Lockstep;
using Regulus.Project.Lockstep.Common;
using Regulus.Utility;
using UnityEngine;

namespace Regulus.Project.Lockstep.Unity
{
    [System.Serializable]
    public class UnityStepEvent : UnityEngine.Events.UnityEvent<Step>
    {
        
    }
    

    public class Driver : MonoBehaviour
    {
        private Propeller<Regulus.Project.Lockstep.Common.Step> _Propeller;
        private readonly Regulus.Utility.TimeCounter _TimeCounter;

        public float Interval;
        public int KeyFrame;
        public int Buffer;
        
        
        public UnityStepEvent AdvanceEvent;

        public Driver()
        {
            
            _TimeCounter = new TimeCounter();
        }
        // Use this for initialization
        void Start()
        {
            
            var interval = Regulus.Utility.TimeCounter.SecondTicks * Interval;
            _Propeller = new Regulus.Lockstep.Propeller<Regulus.Project.Lockstep.Common.Step>((long)interval, KeyFrame, Buffer);


        }

        // Update is called once per frame
        void FixedUpdate()
        {
            var ticks = _TimeCounter.Ticks;
            _TimeCounter.Reset();

            Step step;
            if (_Propeller.Advance(ticks, out step))
            {                
                AdvanceEvent.Invoke(step);
            }

        }

        public void Push(Step step)
        {
            _Propeller.Push(step);
        }
    }



}
