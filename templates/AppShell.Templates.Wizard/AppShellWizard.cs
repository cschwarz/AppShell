using Microsoft.VisualStudio.TemplateWizard;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace AppShell.Templates.Wizard
{
    public class AppShellWizard : IWizard
    {
        public void BeforeOpeningFile(EnvDTE.ProjectItem projectItem)
        {
        }

        public void ProjectFinishedGenerating(EnvDTE.Project project)
        {
        }

        public void ProjectItemFinishedGenerating(EnvDTE.ProjectItem projectItem)
        {
        }

        public void RunFinished()
        {
        }

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            string safeProjectName = replacementsDictionary["$safeprojectname$"];

            string shellName = string.Empty;

            if (safeProjectName.LastIndexOf(".") == -1)
                shellName = safeProjectName;
            else
                shellName = safeProjectName.Substring(safeProjectName.LastIndexOf(".") + 1);

            replacementsDictionary.Add("$shellname$", shellName);
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }
    }
}
