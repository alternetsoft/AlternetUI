using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Alternet.UI.Integration.VisualStudio.Models;
using Alternet.UI.Integration.VisualStudio.Utils;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.ProjectSystem.Properties;
using Microsoft.VisualStudio.Shell;
using VSLangProj;

namespace Alternet.UI.Integration.VisualStudio.Services
{
    /// <summary>
    /// Queries the projects in the current solution.
    /// </summary>
    internal class SolutionService
    {
        private readonly DTE _dte;

        /// <summary>
        /// Initializes a new instance of the <see cref="SolutionService"/> class.
        /// </summary>
        /// <param name="dte">The Visual Studio DTE.</param>
        public SolutionService(DTE dte)
        {
            _dte = dte;
        }

        /// <summary>
        /// Gets a list of projects in the current solution, waiting for the projects to be
        /// fully loaded.
        /// </summary>
        /// <remarks>
        /// There is no decent way (that I can find) to wait until all projects in a solution
        /// (including their references) are full loaded, so this method uses a series of hacks
        /// to try and do this. It may or may not be sucessful...
        /// </remarks>
        /// <returns>A collection of <see cref="ProjectInfo"/> objects.</returns>
        public async Task<IReadOnlyList<ProjectInfo>> GetProjectsAsync()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var result = new Dictionary<Project, ProjectInfo>();
            var uninitialized = new Dictionary<VSProject, ProjectInfo>();
            var startupProjects =
                ((Array)_dte.Solution.SolutionBuild.StartupProjects)?.Cast<string>().ToList();

            foreach (var project in FlattenProjects(_dte.Solution))
            {
                if (project.Object is VSProject vsProject)
                {
                    if (!IsSupportedProj(project))
                    {
                        continue;
                    }

                    var projectInfo = new ProjectInfo
                    {
                        IsStartupProject = startupProjects?.Contains(project.UniqueName) ?? false,
                        Name = project.Name,
                        Project = project,
                        ProjectReferences = GetProjectReferences(vsProject),
                        References = GetReferences(vsProject),
                    };

                    result.Add(project, projectInfo);

                    // If the project is a .csproj and it has no references then we assume its
                    // references are not yet loaded. We might want to handle e.g. F# and VB
                    // projects here too.
                    if (IsCsProj(projectInfo.Project) && projectInfo.References.Count == 0)
                    {
                        uninitialized.Add(vsProject, projectInfo);
                    }
                }
            }

            if (uninitialized.Count > 0)
            {
                var tcs = new TaskCompletionSource<object>();

                foreach (var i in uninitialized)
                {
                    void Handler(Reference reference)
                    {
                        // Here we're assuming that all references will be added at once, so the
                        // fact we've got one means we've got them all. Is this guaranteed to be
                        // true? No idea, but it *seems* to be the case.
                        i.Value.ProjectReferences = GetProjectReferences(i.Key);
                        i.Value.References = GetReferences(i.Key);
                        i.Key.Events.ReferencesEvents.ReferenceAdded -= Handler;
                        uninitialized.Remove(i.Key);

                        if (uninitialized.Count == 0)
                        {
                            tcs.SetResult(null);
                        }
                    }

                    i.Key.Events.ReferencesEvents.ReferenceAdded += Handler;
                }

                await tcs.Task;
            }

            // Now everything should be loaded, loop back through the projects and add the output
            // info and recurse the project references.
            foreach (var item in result)
            {
                var outputType = TryGetProperty(item.Key?.Properties, "OutputType") ??
                    TryGetProperty(item.Key?.ConfigurationManager?.ActiveConfiguration?.Properties, "OutputType");
                var outputTypeIsExecutable = outputType == "0" || outputType == "1" ||
                    outputType?.ToLowerInvariant() == "exe" ||
                    outputType?.ToLowerInvariant() == "winexe";

                item.Value.IsExecutable = outputTypeIsExecutable;
                item.Value.Outputs = await GetOutputInfoAsync(item.Key);
                item.Value.ProjectReferences = FlattenProjectReferences(result, item.Value.ProjectReferences);
            }

            return result.Values.ToList();
        }

        public static bool ProjectHasExtrension(Project projectInfo, string ext)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (!string.IsNullOrWhiteSpace(projectInfo.FullName))
            {
                return string.Equals(
                    Path.GetExtension(projectInfo.FullName),
                    ext,
                    StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }

        public static bool IsSupportedProj(Project projectInfo)
        {
            return IsCsProj(projectInfo);
        }

        public static bool IsVbProj(Project projectInfo)
        {
            return ProjectHasExtrension(projectInfo, ".vbproj");
        }

        public static bool IsCsProj(Project projectInfo)
        {
            return ProjectHasExtrension(projectInfo, ".csproj");
        }

        private static IEnumerable<Project> FlattenProjects(IEnumerable projects)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (projects == null)
                yield break;

            object[] array = projects.Cast<object>().ToArray();

            foreach (Project project in array)
            {
                if (project.Object is VSProject)
                {
                    if (!IsSupportedProj(project))
                    {
                        continue;
                    }

                    yield return project;
                }
                else if (project.Object is SolutionFolder)
                {
                    foreach (var child in FlattenSubProjects(project.ProjectItems))
                    {
                        yield return child;
                    }
                }
            }
        }

        private static IEnumerable<Project> FlattenSubProjects(ProjectItems items)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (items == null)
                yield break;

            foreach (ProjectItem item in items)
            {
                var project = item.SubProject;

                if (project?.Object is VSProject)
                {
                    if (!IsSupportedProj(project))
                    {
                        continue;
                    }

                    yield return project;
                }
                else if (project?.Object is SolutionFolder)
                {
                    foreach (var child in FlattenSubProjects(project.ProjectItems))
                    {
                        yield return child;
                    }
                }
            }
        }

        private static IReadOnlyList<Project> GetProjectReferences(VSProject project)
        {
            return project.References
                .OfType<Reference>()
                .Where(x => x.SourceProject != null)
                .Select(x => x.SourceProject)
                .ToList();
        }

        private static IReadOnlyList<string> GetReferences(VSProject project)
        {
            return project.References
                .OfType<Reference>()
                .Where(x => x.SourceProject == null)
                .Select(x => x.Name).ToList();
        }

        private static async Task<IReadOnlyList<ProjectOutputInfo>> GetOutputInfoAsync(Project project)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var alternatives = new Dictionary<string, ProjectOutputInfo>();
            var unconfigured = (project as IVsBrowseObjectContext)?.UnconfiguredProject;

            if (unconfigured != null)
            {
                foreach (var loaded in unconfigured.LoadedConfiguredProjects)
                {
                    var task = loaded.GetType()
                        .GetProperty("MSBuildProject",
                            BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                        .GetMethod.Invoke(loaded, null) as Task<Microsoft.Build.Evaluation.Project>;
                    var msbuildProperties = (await task).AllEvaluatedProperties;
                    var targetPath = GetMsBuildProperty(msbuildProperties, "TargetPath");

                    if (!string.IsNullOrWhiteSpace(targetPath))
                    {
                        var hostAppDirectory = Path.Combine(Path.GetDirectoryName(typeof(SolutionService).Assembly.Location), @"UIXmlHostApp");
                        /*var hostAppDotNetFx = Path.Combine(hostAppDirectory, "DotNetFx", "Alternet.UI.Integration.UIXmlHostApp.exe");*/

                        var frameworkMoniker = GetFrameworkInfo(loaded, msbuildProperties, "TargetFramework");
                        var frameworkID = GetFrameworkInfo(loaded, msbuildProperties, "TargetFrameworkIdentifier");
                        /*var hostApp = FrameworkInfoUtils.IsNetFramework(frameworkID) ?
                            hostAppDotNetFx :
                            GetDotNetCoreHostAppPath(hostAppDirectory, frameworkMoniker);*/

                        var hostApp = GetDotNetCoreHostAppPath(hostAppDirectory, frameworkMoniker);

                        alternatives[frameworkMoniker] =
                            new ProjectOutputInfo(targetPath, frameworkMoniker, frameworkID, hostApp);
                    }
                }
            }

            return alternatives.Values.ToList();

            static string GetDotNetCoreHostAppPath(string hostAppDirectory, string frameworkMoniker)
            {
                static bool TryPath(string hostAppDirectory, string frameworkMoniker, out string path)
                {
                    path = Path.Combine(hostAppDirectory, frameworkMoniker.Replace("-windows", ""), "Alternet.UI.Integration.UIXmlHostApp.dll");
                    if (!File.Exists(path))
                        return false;
                    return true;
                }

                var pathExists = TryPath(hostAppDirectory, frameworkMoniker, out var result);
                if (!pathExists)
                {
                    pathExists = TryPath(hostAppDirectory, "net8.0", out result);
                }

                return result;
            }
        }

        private static IReadOnlyList<Project> FlattenProjectReferences(
            Dictionary<Project, ProjectInfo> projects,
            IReadOnlyList<Project> references)
        {
            var result = new HashSet<Project>();
            
            foreach (var reference in references)
            {
                FlattenProjectReferences(projects, reference, result);
            }

            return result.ToList();
        }

        private static void FlattenProjectReferences(
            Dictionary<Project, ProjectInfo> projects,
            Project reference,
            HashSet<Project> result)
        {
            result.Add(reference);

            if (projects.TryGetValue(reference, out var info))
            {
                foreach (var child in info.ProjectReferences)
                {
                    FlattenProjectReferences(projects, child, result);
                }
            }
        }

        private static string TryGetProperty(Properties props, string name)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (props == null)
                return null;

            try
            {
                return props.Item(name).Value.ToString();
            }
            catch
            {
                return null;
            }
        }

        private static string GetMsBuildProperty(
            ICollection<Microsoft.Build.Evaluation.ProjectProperty> msbuildProperties,
            string propertyName)
        {
            return msbuildProperties.FirstOrDefault(x => x.Name == propertyName)?.EvaluatedValue;
        }

        private static string GetFrameworkInfo(
            ConfiguredProject loaded,
            ICollection<Microsoft.Build.Evaluation.ProjectProperty> msbuildProperties,
            string keyTargetInfo)
        {
            return loaded.ProjectConfiguration.Dimensions.TryGetValue(keyTargetInfo, out var info)
                ? info
                : GetMsBuildProperty(msbuildProperties, keyTargetInfo) ?? "unknown";
        }
    }
}
