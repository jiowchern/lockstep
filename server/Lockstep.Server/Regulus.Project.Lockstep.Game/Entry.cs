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
        private readonly PartySet _Partys;
        private readonly Matcher _Matcher;
        public Entry()
        {
            _Partys = new PartySet();
            _Matcher = new Matcher(_Partys);
            
            _Updater = new Updater();
        }

        void IBinderProvider.AssignBinder(ISoulBinder binder)
        {
            _Updater.Add(new Player(binder, _Matcher ));
        }

        void ICore.Launch(IProtocol protocol, ICommand command)
        {
            _Updater.Add(_Partys);
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
