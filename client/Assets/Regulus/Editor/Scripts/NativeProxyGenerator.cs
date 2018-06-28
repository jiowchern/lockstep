using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Regulus.Unity.Editor
{
    public class NativeProxyGenerator : EditorWindow
    {
        private string _TypeFullName;


        public NativeProxyGenerator()
        {
            _TypeFullName = string.Empty;
        }

        [MenuItem("Regulus/NativeProxy/Generator")]
        public static void Open()
        {
            var window = (NativeProxyGenerator) GetWindow(typeof(NativeProxyGenerator));
            window.Show();
        }


        public void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            _TypeFullName = EditorGUILayout.TextField(_TypeFullName);
            if (GUILayout.Button("Generate"))
            {
                var savePath = EditorUtility.SaveFolderPanel("save to folder", "", "");
                System.Type type = _FindType(_TypeFullName);
                if(type == null)
                    throw new SystemException($"not found {_TypeFullName}");

                var codeGenerator = new MonoBehaviourCodeGenerator(type);
                
                System.IO.File.WriteAllText(System.IO.Path.Combine(savePath, codeGenerator.Name+".cs"), codeGenerator.Code);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }

        private Type _FindType(string type_full_name)
        {
            return (from asm in AppDomain.CurrentDomain.GetAssemblies()
                let type = asm.GetType(type_full_name)
                where type != null
                select type).FirstOrDefault();                
        }
    }
}