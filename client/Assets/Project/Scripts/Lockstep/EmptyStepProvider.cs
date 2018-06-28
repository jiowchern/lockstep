using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Regulus.Project.Lockstep.Unity
{
    public class EmptyStepProvider : MonoBehaviour
    {
        public float Interval;
        float _Delta;
        public Driver Target;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void FixedUpdate()
        {
            _Delta += UnityEngine.Time.deltaTime;
            if (_Delta > Interval)
            {
                _Delta -= Interval;
                var step = new Regulus.Project.Lockstep.Common.Step();
                Target.Push(step);
            }
            
        }
    }

}
