using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using SyntaxTree.VisualStudio.Unity.Bridge;
using UnityEngine;

namespace Wasp.Editor.UnityEd.VisualStudioBridge
{
	internal static class ProjectFileHandler
	{
		public static void Register()
		{
			/*
			ProjectFilesGenerator.ProjectFileGeneration += (InName, InContent) =>
			{
				Debug.Log($"ProjectFile: {InName}");
				return InContent;
			};
			*/
		}

		public static bool GetProjectInfo(string InProjectFile, out string OutProjectName, out string OutProjectGuid)
		{
			OutProjectName = null;
			OutProjectGuid = null;
			try
			{
				var InDoc = XDocument.Load(InProjectFile);
				OutProjectGuid = InDoc.Descendants(XName.Get("ProjectGuid", @"http://schemas.microsoft.com/developer/msbuild/2003")).First().Value;

				OutProjectName = Path.GetFileNameWithoutExtension(InProjectFile);
				if (String.IsNullOrEmpty(OutProjectName))
				{
					var FoundElements = InDoc.Descendants(XName.Get("RootNamespace", @"http://schemas.microsoft.com/developer/msbuild/2003"));
					if (FoundElements.Count() < 0)
					{
						throw new ArgumentNullException("InProjectFile");
					}
					OutProjectName = FoundElements.First().Value;
				}
				return true;
			}
			catch (Exception e)
			{
				Debug.LogWarning(e);
			}
			return false;
		}
	}
}

/*
//[InitializeOnLoad]
public class ProjectGenerationHandler
{
	//static ProjectGenerationHandler()
	public static void DoWork()
	{
        ProjectFilesGenerator.ProjectFileGeneration += (name, content) =>
        {
            try
            {
                Debug.Log("ProjectGenerationHook:" + name);
                var regex = new Regex("\\.CSharp\\.Editor\\.csproj", RegexOptions.RightToLeft);
                if(regex.IsMatch(name))
                    return content;

                string assemblyName = ExternalProjectConfiguration.Instance["assemblyName"];
                string projectFilePath = ExternalProjectConfiguration.Instance["projectFilePath"];
                string projectGuid = ExternalProjectConfiguration.Instance["projectGuid"];

                content = RemoveAssemblyReferenceFromProject(content, assemblyName);
                content = AddProjectReferenceToProject(content, assemblyName, projectFilePath, projectGuid);
                content = AddCopyAssemblyToAssetsPostBuildEvent(content, assemblyName);

                return content;
            } catch (Exception e)
            {
                Debug.LogException(e);
            }
            return content;
        };
    }

    private static string AddCopyAssemblyToAssetsPostBuildEvent(string content, string assemblyName)
    {
		if (content.Contains("PostBuildEvent"))
            return content; // already added

        var signature = new StringBuilder();

        string[] pbe = new string[] {
            string.Format(@"copy /Y ""{0}{1}.*"" ""$(TargetDir)""", ExternalProjectConfiguration.Instance["projectOutputPath"], assemblyName),
            string.Format(@"copy /Y ""$(TargetDir){0}.*"" ""$(ProjectDir)Assets\""", assemblyName),
            string.Format(@"""{0}"" ""$(ProjectDir)Assets\{1}.dll""", ExternalProjectConfiguration.Instance["monoConvertExe"], assemblyName)
        };

        signature.AppendLine("  <PropertyGroup>");
        signature.AppendLine("    <RunPostBuildEvent>Always</RunPostBuildEvent>");
        signature.AppendLine(string.Format(@"    <PostBuildEvent>{0}</PostBuildEvent>", string.Join("\r\n", pbe)));
        signature.AppendLine("  </PropertyGroup>");
        signature.AppendLine("</Project>");

        var regex = new Regex("^</Project>", RegexOptions.Multiline);
        return regex.Replace(content, signature.ToString());
    }

    private static string RemoveAssemblyReferenceFromProject(string content, string assemblyName)
    {
        var regex = new Regex(string.Format(@"^\s*<Reference Include=""{0}"">\r\n\s*<HintPath>.*{0}.dll</HintPath>\r\n\s*</Reference>\r\n", assemblyName), RegexOptions.Multiline);
        return regex.Replace(content, string.Empty);
    }

    private static string AddProjectReferenceToProject(string content, string projectName, string projectFilePath, string projectGuid)
    {
        if (content.Contains(">" + projectName + "<"))
            return content; // already added

        var signature = new StringBuilder();
        signature.AppendLine("  <ItemGroup>");
        signature.AppendLine(string.Format("    <ProjectReference Include=\"{0}\">", projectFilePath));
        signature.AppendLine(string.Format("      <Project>{0}</Project>", projectGuid));
        signature.AppendLine(string.Format("      <Name>{0}</Name>", projectName));
        signature.AppendLine("    </ProjectReference>");
        signature.AppendLine("  </ItemGroup>");
        signature.AppendLine("</Project>");

        var regex = new Regex("^</Project>", RegexOptions.Multiline);
        return regex.Replace(content, signature.ToString());
    }

}
*/
