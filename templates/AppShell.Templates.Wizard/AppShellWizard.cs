using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace AppShell.Templates.Wizard
{
    public class AppShellWizard : IWizard
    {
        private DTE dte;
        private WizardWindow wizardWindow;

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
        }

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            /*
            StringBuilder builder = new StringBuilder();

            foreach (var temp in replacementsDictionary)
                builder.AppendLine(string.Format("{0}: {1}", temp.Key, temp.Value));

            MessageBox.Show(builder.ToString());*/

            dte = automationObject as DTE;

            string destinationDirectory = replacementsDictionary["$destinationdirectory$"];

            try
            {
                wizardWindow = new WizardWindow();
                wizardWindow.Title = string.Format("New AppShell Project - {0}", replacementsDictionary["$projectname$"]);
                bool? result = wizardWindow.ShowDialog();

                if (result.HasValue && !result.Value)
                    throw new WizardBackoutException();

                AddVariables(replacementsDictionary);
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

        private void AddVariables(Dictionary<string, string> replacementsDictionary)
        {
            string safeProjectName = replacementsDictionary["$safeprojectname$"];

            string shellName = string.Empty;

            if (safeProjectName.LastIndexOf(".") == -1)
                shellName = safeProjectName;
            else
                shellName = safeProjectName.Substring(safeProjectName.LastIndexOf(".") + 1);

            replacementsDictionary.Add("$shellname$", shellName);
        }
    }
}
