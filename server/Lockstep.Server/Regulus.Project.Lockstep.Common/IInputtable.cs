using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Regulus.Project.Lockstep.Common
{
    public interface IInputtable
    {
        Guid Id { get; }
        void Input(InputContent inputContent);
    }
}
