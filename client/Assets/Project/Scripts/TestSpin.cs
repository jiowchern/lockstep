using System.Collections;
using System.Collections.Generic;
using Regulus.Project.Lockstep.Common;
using UnityEngine;

public class TestSpin : MonoBehaviour {

    public float Cycle = 1f;


    public void LockstepUpdate(Step step)
    {
        var euler = transform.eulerAngles;
        euler.y = (euler.y + 360f * Cycle) % 360;
        transform.eulerAngles = euler;
    }
}
