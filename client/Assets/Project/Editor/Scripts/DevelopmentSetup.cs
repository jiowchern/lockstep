using System;
using UnityEngine;
using UnityEditor;

namespace Wasp.Editor.UnityEd.Launch
{
	[InitializeOnLoad]
	public class DevelopmentSetup
	{
		static DevelopmentSetup()
		{
			//EditorApplication.update -= PreInit;
			//EditorApplication.playmodeStateChanged -= PlayModeChanged;

			if (!EditorApplication.isPlaying && !EditorApplication.isPlayingOrWillChangePlaymode)
			{
				//RegisterSolutionGenerationHooks();
				//EditorApplication.update += PreInit;
			}
			//EditorApplication.playmodeStateChanged += PlayModeChanged;
		}

        private static void RegisterSolutionGenerationHooks()
        {
            VisualStudioBridge.SoluationImport sln = new VisualStudioBridge.SoluationImport()
            {
                Path = "../server/Lockstep.Server/Lockstep.Server.sln",
                Filter = "Projects",
                Guid = "{AAD8BF4E-4DA4-4466-991B-33A37E5DABD4}"
            };
            VisualStudioBridge.SolutionFileHandler.Register(sln);
		}

		private static void PreInit()
		{
			EditorApplication.update -= PreInit;
		}

		private static void PlayModeChanged()
		{
		}
	}
}
