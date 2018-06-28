using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Regulus.Unity.Editor
{
    public class ProtocolGenerator : EditorWindow

    {
        private string _SourcePath;
        private string _TargetPath;
        private string _AgentName;

        public ProtocolGenerator()
        {
            _SourcePath = string.Empty;
            _TargetPath = string.Empty;
            _AgentName = string.Empty;
        }

        [MenuItem("Regulus/Protocol/Generator")]
        public static void Open()
        {
            var window = (ProtocolGenerator)GetWindow(typeof(ProtocolGenerator));
            window.Show();
        }

        public void OnGUI()
        {
            EditorGUILayout.BeginVertical();
                EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(_SourcePath );
                    if(GUILayout.Button("Open Input Name"))
                    {
                        _SourcePath = EditorUtility.OpenFilePanel("Select Assembly", "", "dll");                        
                    }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(_TargetPath);
                    if (GUILayout.Button("Open Output Folder"))
                    {
                        _TargetPath = EditorUtility.OpenFolderPanel("Select Folder", "", "");
                    }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Agent Name");
                    _AgentName = EditorGUILayout.TextField(_AgentName);
                
                EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Generate"))
                {
                    _Generate(_SourcePath , _TargetPath,_AgentName);
                }
            EditorGUILayout.EndVertical();

        }

        private void _Generate(string source_path, string target_path ,string agent_name)
        {
            var sourceAsm = System.Reflection.Assembly.LoadFile(source_path);
            var builder = new Regulus.Remoting.Unity.AssemblyOutputer(sourceAsm , agent_name);            
            builder.OutputDir(target_path);
        }
    }

}
