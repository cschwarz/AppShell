using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;
using System.Collections.Generic;
using System.Linq;

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
            dte = automationObject as DTE;
            
            wizardWindow = new WizardWindow();
            wizardWindow.Title = string.Format("New AppShell Project - {0}", replacementsDictionary["$projectname$"]);
            wizardWindow.ShowDialog();

            AddVariables(replacementsDictionary);            
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
