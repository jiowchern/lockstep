using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Regulus.Utility.WindowConsoleAppliction;
namespace Regulus.Project.Lockstep.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new Regulus.Remoting.ConsoleRunner.Application(args);
            app.Run();
        }
    }
}
