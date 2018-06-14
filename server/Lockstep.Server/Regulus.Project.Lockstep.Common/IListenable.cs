using System;

namespace Regulus.Project.Lockstep.Common
{
    public interface IListenable
    {
        void SetEnable(bool enable);

        event Action<Step> StepEvent;
    }
}