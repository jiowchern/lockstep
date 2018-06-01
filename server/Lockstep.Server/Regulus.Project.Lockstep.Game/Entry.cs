using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Regulus.Remoting;
using Regulus.Utility;

namespace Regulus.Project.Lockstep.Game
{
    public class Entry : Regulus.Remoting.ICore
    {
        private readonly Regulus.Utility.Updater _Updater;
        private Broadcaster _Boardcaster;
        public Entry()
        {

            
            _Boardcaster = new Broadcaster();
            _Updater = new Updater();
        }

        void IBinderProvider.AssignBinder(ISoulBinder binder)
        {
            _Updater.Add(new Player(binder, _Boardcaster));
        }

        void ICore.Launch(IProtocol protocol, ICommand command)
        {
            
        }

        bool ICore.Update()
        {
            _Updater.Working();
            return true;
        }

        void ICore.Shutdown()
        {
            _Updater.Shutdown();
        }
    }
}
