using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using SyntaxTree.VisualStudio.Unity.Bridge;
using UnityEngine;
using System.IO;

namespace Wasp.Editor.UnityEd.VisualStudioBridge
{
    public class ProjectInfo
    {
        protected string m_ProjectPath;
        protected string m_ProjectName;
        protected string m_ProjectGuid;

        protected void CleanUp(ref string Content)
        {
            Content = Content.Replace(" ", "");
            Content = Content.Replace("\"", "");
        }

        public string ProjectPath { get { return m_ProjectPath; } set { m_ProjectPath = value; CleanUp(ref m_ProjectPath); } }
        public string ProjectName { get { return m_ProjectName; } set { m_ProjectName = value; CleanUp(ref m_ProjectName); } }
        public string ProjectGuid { get { return m_ProjectGuid; } set { m_ProjectGuid = value; CleanUp(ref m_ProjectGuid); } }

        public bool Section;
        public string ProjectSection;
    }

    public class SoluationInfo
    {
        public string SoluationDir;
        public List<ProjectInfo> Projects = new List<ProjectInfo>();

        public bool LoadSoluationProjects(string SoluationPath)
        {
            if (!File.Exists(SoluationPath)) return false;

            FileStream fileStream = new FileStream(SoluationPath, FileMode.Open);
            if (!fileStream.CanRead) return false;
            SoluationDir = Path.GetDirectoryName(SoluationPath);

            StreamReader reader = new StreamReader(fileStream);

            ProjectInfo project = null;
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (project != null)
                {
                    if (line.Contains("EndProjectSection"))
                    {
                        project.Section = false;
                        project.ProjectSection += line;
                        project.ProjectSection += "\n";
                        continue;
                    }
                    else if (line.Contains("ProjectSection"))
                    {
                        project.Section = true;
                        project.ProjectSection += line;
                        project.ProjectSection += "\n";
                        continue;
                    }
                    else if (line.StartsWith("EndProject"))
                    {
                        Projects.Add(project);
                        project = null;
                        continue;
                    }

                    if (project.Section == true)
                    {
                        project.ProjectSection += line;
                        project.ProjectSection += "\n";
                        continue;
                    }
                }

                if (line.StartsWith("Project"))
                {
                    string[] splits = line.Split(',');

                    project = new ProjectInfo();
                    project.ProjectName = splits[0].Split('=')[1];
                    project.ProjectPath = splits[1];
                    project.ProjectGuid = splits[2];
                }
            }

            return true;
        }
    }

    public struct SoluationImport
    {
        public string Path;
        public string Filter;
        public string Guid;
    }

	internal static class SolutionFileHandler
	{
		private const string RegexGuidPattern = "[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}";
		private const string ExternalProjectFolderGuid = "{AAD8BF4E-4DA4-4466-991B-33A37E5DABD4}";

		//!!!! DO NOT modify below two strings
		private const string SolutionFolderGuid = "{2150E333-8FDC-42A3-9474-1A3956D46DE8}";
		private const string CSharpProjectGuid = "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}";

		public static void Register(SoluationImport Soluation)
		{
			ProjectFilesGenerator.SolutionFileGeneration += (InName, InContent) =>
			{
				try
				{
					var NewContent = AddFolderToSolution(InContent, Soluation.Filter, Soluation.Guid);
                        
                    SoluationInfo soluation = ParseSoluation(Soluation.Path);
				    foreach (ProjectInfo project in soluation.Projects)
				    {
				        NewContent = AddProjectToSolution(NewContent, Path.Combine(soluation.SoluationDir, project.ProjectPath), project.ProjectSection, ExternalProjectFolderGuid);
				    }

                    InContent = NewContent;
				}
				catch (Exception e)
				{
					Debug.LogException(e);
				}
				return InContent;
			};

			ProjectFileHandler.Register();
		}

        private static SoluationInfo ParseSoluation(string SoluationPath)
        {
            var ProjectDirInfo = Directory.GetParent(Application.dataPath);

            if (!ProjectDirInfo.Exists)
                throw new InvalidOperationException("Failed to evaluate unity project path.");
            var UnityProjectPath = ProjectDirInfo.FullName;

            var SoluationFilePath = Path.Combine(UnityProjectPath, SoluationPath);

            if (!File.Exists(SoluationFilePath))
            {
                throw new SystemException($"No path found {SoluationFilePath}");
                
            }
                

            SoluationInfo soluation = new SoluationInfo();
            if (!soluation.LoadSoluationProjects(SoluationFilePath)) return null;

            return soluation;
        }

        private static string AddFolderToSolution(string InContent, string InFolderName, string InFolderGuid)
		{
			var Signature = new StringBuilder();
			Signature.AppendLine($"Project(\"{SolutionFolderGuid}\") = \"{InFolderName}\", \"{InFolderName}\", \"{InFolderGuid}\"");
			Signature.AppendLine("EndProject");
			Signature.Append("Global");
			return Regex.Replace(InContent, "^Global", Signature.ToString(), RegexOptions.Multiline);
		}

		private static string AddProjectToSolution(string InContent, string InProjectFile, string InProjectSection, string InFolderGuid = null)
		{
			string InProjectName, InProjectGuid;
			if (!ProjectFileHandler.GetProjectInfo(InProjectFile, out InProjectName, out InProjectGuid)) return InContent;

			var Signature = new StringBuilder();
            Match match = Regex.Match(InContent, $@"^Project\(""\{{{RegexGuidPattern}\}}""\)\s+=\s+""{InProjectName}"",\s+", RegexOptions.Multiline);

            // Project is exists
            if (match.Success)
            {
                if (InProjectSection != null)
                {
                    int start = InContent.IndexOf(match.Value);
                    int end = InContent.IndexOf("EndProject", start);
                    InContent = InContent.Insert(end - 1, InProjectSection);
                }
            }
            else
			{
				Signature.AppendLine($"Project(\"{CSharpProjectGuid}\") = \"{InProjectName}\", \"{InProjectFile}\", \"{InProjectGuid}\"");
                if (InProjectSection != null) Signature.Append(InProjectSection);
                Signature.AppendLine("EndProject");
				Signature.Append("Global");

				InContent = Regex.Replace(InContent, "^Global", Signature.ToString(), RegexOptions.Multiline);
			}
			
			if (InFolderGuid != null)
			{
				Signature.Length = 0;

				var Expr = new Regex(@"^\t+GlobalSection\(NestedProjects\)\s+=\s+preSolution", RegexOptions.Multiline);
				var ScopeStart = Expr.Match(InContent);
				if (ScopeStart.Success
					&& InContent.IndexOf($"{InProjectGuid} =", ScopeStart.Index + ScopeStart.Length) < 0)
				{
					Signature.AppendLine("\tGlobalSection(NestedProjects) = preSolution");
					Signature.Append($"\t\t{InProjectGuid} = {InFolderGuid}");
					InContent = Expr.Replace(InContent, Signature.ToString());
				}
				else
				{
					Signature.AppendLine("\tGlobalSection(NestedProjects) = preSolution");
					Signature.AppendLine($"\t\t{InProjectGuid} = {InFolderGuid}");
					Signature.AppendLine("\tEndGlobalSection");
					Signature.AppendLine("EndGlobal");
					InContent = Regex.Replace(InContent, "^EndGlobal", Signature.ToString(), RegexOptions.Multiline);
				}
			}
			return InContent;
		}
	}
}
