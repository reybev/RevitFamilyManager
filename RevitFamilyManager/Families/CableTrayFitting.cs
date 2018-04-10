using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RevitFamilyManager.Families
{
    [Transaction(TransactionMode.Manual)]
    class CableTrayFitting : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            string path = GetDeviceFolder("Kabeltrassenformteile");
            string files = GetFamilyNames(path);
            TaskDialog.Show("Electrical Fixture Data", files);

            return Result.Succeeded;
        }

        private string GetDeviceFolder(string deviceType)
        {
            string[] allPaths = Directory.GetDirectories(@"D:\2016.10.10 Familien Gianfranco");
            string path = string.Empty;
            foreach (string folder in allPaths)
            {
                if (folder.Contains(deviceType))
                {
                    path = folder;
                }
            }
            return path;
        }

        private string GetFamilyNames(string path)
        {
            var files = Directory.GetFiles(path);
            string fileNames = string.Empty;
            foreach (var file in files)
            {
                fileNames += FileNameCut(file) + "\n";
            }
            return fileNames;
        }

        string FileNameCut(string file)
        {
            int lastSlash = file.LastIndexOf("\\", StringComparison.Ordinal);
            int lastDot = file.LastIndexOf(".", StringComparison.Ordinal);
            string fileName = file.Substring(++lastSlash, lastDot - lastSlash);
            return fileName;
        }

    }
}
