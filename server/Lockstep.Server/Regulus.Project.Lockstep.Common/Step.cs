using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Regulus.Project.Lockstep.Common
{
    public class Record
    {
        public int Owner;
        public InputContent Content;
    }
    public class Step
    {
        public Step()
        {
            Records = new Record[0];
        }
        public Record[] Records;
    }
}
