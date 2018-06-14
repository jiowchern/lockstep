using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Regulus.Project.Lockstep.Common
{
    public interface IInputtable
    {
        int Id { get; }
        void Input(InputContent input_content);
    }
}
