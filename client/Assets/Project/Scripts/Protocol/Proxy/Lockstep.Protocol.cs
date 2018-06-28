
            using System;  
            using System.Collections.Generic;
            
            namespace Lockstep{ 
                public class Protocol : Regulus.Remoting.IProtocol
                {
                    Regulus.Remoting.InterfaceProvider _InterfaceProvider;
                    Regulus.Remoting.EventProvider _EventProvider;
                    Regulus.Remoting.MemberMap _MemberMap;
                    Regulus.Serialization.ISerializer _Serializer;
                    public Protocol()
                    {
                        var types = new Dictionary<Type, Type>();
                        types.Add(typeof(Regulus.Project.Lockstep.Common.IListenable) , typeof(Regulus.Project.Lockstep.Common.Ghost.CIListenable) );
types.Add(typeof(Regulus.Project.Lockstep.Common.IMatchable) , typeof(Regulus.Project.Lockstep.Common.Ghost.CIMatchable) );
types.Add(typeof(Regulus.Project.Lockstep.Common.IInputtable) , typeof(Regulus.Project.Lockstep.Common.Ghost.CIInputtable) );
                        _InterfaceProvider = new Regulus.Remoting.InterfaceProvider(types);

                        var eventClosures = new List<Regulus.Remoting.IEventProxyCreator>();
                        eventClosures.Add(new Regulus.Project.Lockstep.Common.Invoker.IListenable.StepEvent() );
                        _EventProvider = new Regulus.Remoting.EventProvider(eventClosures);

                        _Serializer = new Regulus.Serialization.Serializer(new Regulus.Serialization.DescriberBuilder(typeof(Regulus.Project.Lockstep.Common.InputContent),typeof(Regulus.Project.Lockstep.Common.KEY),typeof(Regulus.Project.Lockstep.Common.Record),typeof(Regulus.Project.Lockstep.Common.Record[]),typeof(Regulus.Project.Lockstep.Common.Step),typeof(Regulus.Remoting.ClientToServerOpCode),typeof(Regulus.Remoting.PackageCallMethod),typeof(Regulus.Remoting.PackageErrorMethod),typeof(Regulus.Remoting.PackageInvokeEvent),typeof(Regulus.Remoting.PackageLoadSoul),typeof(Regulus.Remoting.PackageLoadSoulCompile),typeof(Regulus.Remoting.PackageProtocolSubmit),typeof(Regulus.Remoting.PackageRelease),typeof(Regulus.Remoting.PackageReturnValue),typeof(Regulus.Remoting.PackageUnloadSoul),typeof(Regulus.Remoting.PackageUpdateProperty),typeof(Regulus.Remoting.RequestPackage),typeof(Regulus.Remoting.ResponsePackage),typeof(Regulus.Remoting.ServerToClientOpCode),typeof(System.Boolean),typeof(System.Byte[]),typeof(System.Byte[][]),typeof(System.Char),typeof(System.Char[]),typeof(System.Guid),typeof(System.Int32),typeof(System.String)));


                        _MemberMap = new Regulus.Remoting.MemberMap(new System.Reflection.MethodInfo[] {typeof(Regulus.Project.Lockstep.Common.IListenable).GetMethod("SetEnable"),typeof(Regulus.Project.Lockstep.Common.IMatchable).GetMethod("Match"),typeof(Regulus.Project.Lockstep.Common.IInputtable).GetMethod("Input")} ,new System.Reflection.EventInfo[]{ typeof(Regulus.Project.Lockstep.Common.IListenable).GetEvent("StepEvent") }, new System.Reflection.PropertyInfo[] {typeof(Regulus.Project.Lockstep.Common.IInputtable).GetProperty("Id") }, new System.Type[] {typeof(Regulus.Project.Lockstep.Common.IListenable),typeof(Regulus.Project.Lockstep.Common.IMatchable),typeof(Regulus.Project.Lockstep.Common.IInputtable)});
                    }

                    byte[] Regulus.Remoting.IProtocol.VerificationCode { get { return new byte[]{26,126,61,190,98,206,185,62,53,107,247,24,100,54,26,187};} }
                    Regulus.Remoting.InterfaceProvider Regulus.Remoting.IProtocol.GetInterfaceProvider()
                    {
                        return _InterfaceProvider;
                    }

                    Regulus.Remoting.EventProvider Regulus.Remoting.IProtocol.GetEventProvider()
                    {
                        return _EventProvider;
                    }

                    Regulus.Serialization.ISerializer Regulus.Remoting.IProtocol.GetSerialize()
                    {
                        return _Serializer;
                    }

                    Regulus.Remoting.MemberMap Regulus.Remoting.IProtocol.GetMemberMap()
                    {
                        return _MemberMap;
                    }
                    
                }
            }
            