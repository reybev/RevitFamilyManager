using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Ookii.Dialogs.Wpf;
using RevitFamilyManager.Properties;
using TaskDialog = Autodesk.Revit.UI.TaskDialog;

namespace RevitFamilyManager
{
    [Transaction(TransactionMode.Manual)]
    class UserSettings : IExternalCommand
    {
        public string RootFolder { get; set; }

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            GetStartFolder();
            return Result.Succeeded;
        }

        public string GetStartFolder()
        {
            VistaFolderBrowserDialog fbd = new VistaFolderBrowserDialog();
            if (fbd.ShowDialog() == true)
            {
                Properties.Settings.Default.RootFolder = fbd.SelectedPath;
                Properties.Settings.Default.Save();
            }

            return fbd.SelectedPath;
        }
    }
}
