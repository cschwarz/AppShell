using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.TemplateWizard;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AppShell.Templates.Wizard
{
    public class AppShellWizard : IWizard
    {
        private DTE dte;
        private WizardWindow wizardWindow;
        private Dictionary<string, string> replacementsDictionary;
        private string templatePath;

        public void BeforeOpeningFile(ProjectItem projectItem)
        {
        }

        public void ProjectFinishedGenerating(Project project)
        {
        }

        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
        }

        public void RunFinished()
        {
            if (!wizardWindow.DesktopCheckBox.IsChecked.Value)
                dte.Solution.Remove(dte.Solution.Projects.Cast<Project>().Where(p => p.Name.EndsWith(".Desktop")).Single());
            if (!wizardWindow.AndroidCheckBox.IsChecked.Value)
            {
                dte.Solution.Remove(dte.Solution.Projects.Cast<Project>().Where(p => p.Name.EndsWith(".Mobile")).Single());
                dte.Solution.Remove(dte.Solution.Projects.Cast<Project>().Where(p => p.Name.EndsWith(".Mobile.Android")).Single());
            }

            AddNugetSolutionFolder();            
            SetStartupProject();
            RemoveKeepFiles();
        }

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            /*
            StringBuilder builder = new StringBuilder();

            foreach (var temp in replacementsDictionary)
                builder.AppendLine(string.Format("{0}: {1}", temp.Key, temp.Value));

            MessageBox.Show(builder.ToString());*/

            dte = automationObject as DTE;
            this.replacementsDictionary = replacementsDictionary;
            this.templatePath = customParams[0] as string;

            string destinationDirectory = replacementsDictionary["$destinationdirectory$"];

            try
            {
                wizardWindow = new WizardWindow();
                wizardWindow.Title = string.Format("New AppShell Project - {0}", replacementsDictionary["$projectname$"]);
                bool? result = wizardWindow.ShowDialog();

                if (result.HasValue && !result.Value)
                    throw new WizardBackoutException();

                AddReplacementVariables();
            }
            catch (Exception)
            {
                if (Directory.Exists(destinationDirectory))
                    Directory.Delete(destinationDirectory, true);

                throw;
            }
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }

        private void AddReplacementVariables()
        {
            string safeProjectName = replacementsDictionary["$safeprojectname$"];

            string shellName = string.Empty;

            if (safeProjectName.LastIndexOf(".") == -1)
                shellName = safeProjectName;
            else
                shellName = safeProjectName.Substring(safeProjectName.LastIndexOf(".") + 1);

            replacementsDictionary.Add("$shellname$", shellName);
        }

        private void AddNugetSolutionFolder()
        {
            string sourceDirectory = Path.Combine(Path.GetDirectoryName(templatePath), ".nuget");
            string targetDirectory = Path.Combine(Directory.GetParent(replacementsDictionary["$destinationdirectory$"]).FullName, ".nuget");

            if (!Directory.Exists(targetDirectory))
                Directory.CreateDirectory(targetDirectory);

            Project solutionFolderProject = ((Solution2)dte.Solution).Projects.Cast<Project>().Single(p => p.Name == ".nuget");

            foreach (string file in Directory.EnumerateFiles(sourceDirectory))
            {
                string targetFile = Path.Combine(targetDirectory, Path.GetFileName(file));

                File.Copy(file, targetFile);
                solutionFolderProject.ProjectItems.AddFromFile(targetFile);
            }
        }
        
        private void SetStartupProject()
        {
            dte.Solution.Properties.Item("StartupProject").Value = dte.Solution.Projects.Cast<Project>().Where(p => p.Name.EndsWith(".Desktop")).Single().Name;
        }

        private void RemoveKeepFiles()
        {
            foreach (Project project in dte.Solution.Projects)
                RemoveKeepFiles(project.ProjectItems);
        }

        private void RemoveKeepFiles(ProjectItems projectItems)
        {
            if (projectItems == null)
                return;

            foreach (ProjectItem projectItem in projectItems)
            {
                if (projectItem.Name == ".keep")
                {
                    projectItem.Remove();
                    File.Delete(projectItem.Properties.Item("FullPath").Value);
                }

                RemoveKeepFiles(projectItem.ProjectItems);
            }
        }
    }
}
