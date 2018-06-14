using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UnityEngine;
using UnityEditor;

namespace Wasp.Editor.UnityEd.VisualStudioBridge
{
	public class Configuration
	{
		public static Configuration Singleton
		{
			get
			{
				if (mSingleton == null) mSingleton = new Configuration();
				return mSingleton;
			}
		}

		Configuration()
		{
			mConfig = new Dictionary<string, HashSet<string>>();
			Refresh();
		}

		public bool Refresh()
		{
			var ProjectDirInfo = Directory.GetParent(Application.dataPath);

			if (!ProjectDirInfo.Exists) throw new InvalidOperationException("Failed to evaluate unity project path.");
			var UnityProjectPath = ProjectDirInfo.FullName;

			var ConfigFilePath = Path.Combine(UnityProjectPath, "ExternalProjects.xml");

			if (!File.Exists(ConfigFilePath)) return false;
			var CurrentWriteTime = File.GetLastWriteTime(ConfigFilePath);

			if (mLastWriteTime == CurrentWriteTime) return true;
			mLastWriteTime = CurrentWriteTime;

			var InDoc = XDocument.Load(ConfigFilePath);
			if (InDoc == null) throw new InvalidOperationException("Failed to parse external project configuration file.");
			
			XElement[] ReadElements = InDoc.Root.Elements().ToArray();
			foreach (XElement Element in ReadElements)
			{
				//Debug.Log("Add: " + Element.Name.LocalName + " = " + Element.Value);
				HashSet<string> ValueSet;
				if (!mConfig.TryGetValue(Element.Name.LocalName, out ValueSet))
				{
					ValueSet = new HashSet<string>();
					mConfig.Add(Element.Name.LocalName, ValueSet);
				}

				var ValueToAdd = Element.Value.Replace("$(UnityProjectPath)", UnityProjectPath);
				ValueSet.Add(ValueToAdd);
			}

			/*
			string[] files = Directory.GetFiles("./", "*.CSharp.csproj");

			if (files.Length > 0)
			{
				doc = XDocument.Load(files[0]);
				string ownGuid = doc.Descendants(XName.Get("ProjectGuid", @"http://schemas.microsoft.com/developer/msbuild/2003")).First().Value;
				if (ownGuid != null)
				{
					if (mConfig.ContainsKey("ownProjectGuid"))
						mConfig["ownProjectGuid"] = ownGuid;
					else
						mConfig.Add("ownProjectGuid", ownGuid);
				}

			}
			*/
			return true;
		}

		public IEnumerable<string> this[string InKey]
		{
			get
			{
				HashSet<string> ValueSet;
				return mConfig.TryGetValue(InKey, out ValueSet) ? ValueSet : Enumerable.Empty<string>();
			}
		}

		private static Configuration mSingleton;

		private Dictionary<string, HashSet<string>> mConfig;
		private DateTime mLastWriteTime;
	}
}
