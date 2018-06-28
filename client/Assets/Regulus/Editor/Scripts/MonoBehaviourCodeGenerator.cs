using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Regulus.Unity.Editor
{
    public class MonoBehaviourCodeGenerator
    {
        public readonly string Code;
        public readonly string Name;

        public MonoBehaviourCodeGenerator(System.Type type)
        {
            var tup = _Generate(type);
            Code = tup.Item1;
            Name = tup.Item2;
        }

        

        private Tuple<string,string> _Generate(Type type)
        {
            var typeName = _GetTypeName(type);
            var code = $@"namespace Regulus.Unity.MonoBehaviourProxy.{type.Namespace}
{{
    public class {typeName} : UnityEngine.MonoBehaviour
    {{

        private {_GetFullName(type)} _Core;
        public void Initial({_GetFullName(type)} core)
        {{
            _Core = core;
            {_GetBindEvents(type, "+=")}
        }}
        public void Release()
        {{
            {_GetBindEvents(type, "-=")}
        }}
        {
                    string.Join("\r\n",
                        _BuildMethods(type.GetMethods()
                            .Where(m => m.IsPublic && !m.IsStatic && !m.IsSpecialName && m.DeclaringType == type)))
         }
        {string.Join("\r\n", _BuildEvents(type.GetEvents()))}
    }}
}}";
            return new Tuple<string, string>(code ,typeName );
        }//.Where(m => !m.IsSpecialName && m.DeclaringType == type)

        private IEnumerable<string> _BuildEvents(IEnumerable<EventInfo> eventInfos)
        {
            foreach (var eventInfo in eventInfos)
            {
                yield return _GenerateEventDefine(eventInfo);
                yield return _GenerateEventFunction(eventInfo);
            }
        }

        private IEnumerable<string> _BuildMethods(IEnumerable<MethodInfo> methods)
        {
            foreach (var method in methods)
            {
                int paramId = 0;
                var paramCode = string.Join(",", (from p in method.GetParameters() select $"{ _GetFullName(p.ParameterType)} {"_" + ++paramId}").ToArray());

                
                var returnType = _GetFullName(method.ReturnType);                
                yield return $@"public {returnType} {method.Name}({paramCode})
        {{
            {_BuildReturn(method.ReturnType)} _Core.{method.Name}({_BuildAddParams(method.GetParameters())});
        }}";
            }
        }


        private string _BuildAddParams(ParameterInfo[] infos)
        {
            var parameters = infos;

            List<string> addParams = new List<string>();

            for (int i = 0; i < parameters.Length; i++)
            {
                addParams.Add("_" + (i + 1));
            }
            return string.Join(" ,", addParams.ToArray());
        }

        private string _BuildReturn(Type method_return_type)
        {
            return method_return_type == typeof(void)? "" : "return";
        }

        private string _GetTypeName(Type type)
        {
            return type.Name.Replace("+", "."); ;
        }

        private string _GetFullName(Type type)
        {
            if (type == typeof(void))
                return "void";
            return "global::" + string.Join("." , new []{ type.Namespace, type.Name.Replace("+", ".") }); ;
        }


        

        private string _GenerateEventFunction(EventInfo event_info)
        {

            var argTypes = event_info.EventHandlerType.GetGenericArguments();
            var argNames = _GetArgNames(argTypes);
            var argDefines = _GetArgDefines(argTypes);
            string code = String.Format(@"        
        private void _On{0}({1})
        {{
            {0}.Invoke({2});
        }}
        ", event_info.Name, argDefines, argNames);
            return code;
        }

        private string _GetArgNames(Type[] arg_types)
        {
            return string.Join(",", _GenArgNumber(arg_types.Length).ToArray());
        }

        private IEnumerable<string> _GenArgNumber(int length)
        {
            for (int i = 0; i < length; i++)
            {
                yield return "arg" + i;
            }
        }

        private string _GenerateEventDefine(EventInfo event_info)
        {
            var argTypes = _GetArgTypes(event_info.EventHandlerType.GetGenericArguments());

            string code = String.Format(@"
        [System.Serializable]
        public class Unity{0} : UnityEngine.Events.UnityEvent{1} {{}}
        public Unity{0} {0};
        ", event_info.Name, argTypes);
            return code;
        }

        private string _GetArgTypes(Type[] generic_arguments)
        {
            if (generic_arguments.Any())
                return "<" + string.Join(",", (from arg in generic_arguments select _GetFullName(arg)).ToArray()) + ">";
            return String.Empty;
        }
        private string _GetArgDefines(Type[] generic_arguments)
        {
            var types = new List<string>();
            for (int i = 0; i < generic_arguments.Length; i++)
            {
                var type = generic_arguments[i];
                types.Add(_GetFullName(type) + " arg" + i);
            }

            return string.Join(",", types.ToArray());
        }

        private object _GetBindEvents(Type type, string op_code)
        {
            var codes = new List<string>();
            foreach (var eventInfo in type.GetEvents())
            {
                codes.Add(_GetBindEvent(type, eventInfo, op_code));
            }
            return string.Join("\n", codes.ToArray());
        }
        private string _GetBindEvent(Type type, EventInfo event_info, string op_code)
        {

            return string.Format("_Core.{0} {1} _On{0};", event_info.Name, op_code);
        }

        
    }
}