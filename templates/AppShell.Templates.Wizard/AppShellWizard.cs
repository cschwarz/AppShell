using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace AppShell.Templates.Wizard
{
    public class AppShellWizard : IWizard
    {
        private DTE dte;

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
            //Project project = dte.Solution.Projects.Cast<Project>().Where(p => p.Name == "AppShell.Desktop").Single();
            //dte.Solution.Remove(project);
        }

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            dte = automationObject as DTE;
            
            WizardWindow window = new WizardWindow();
            window.ShowDialog();

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
