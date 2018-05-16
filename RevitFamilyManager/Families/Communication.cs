using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Media;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;
using RevitFamilyManager.Data;

namespace RevitFamilyManager.Families
{
    [Transaction(TransactionMode.Manual)]
    class Communication : IExternalCommand
    {
        private string CategoryName { get; set; }

        public Communication()
        {
            CategoryName = "Kommunikationsgeräte";
        }

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            FamilyFolderProcess folderProcess = new FamilyFolderProcess();
            List<FamilyData> familyData = folderProcess.GetCategoryTypes(CategoryName);
            SetPanelData(commandData, familyData);
            return Result.Succeeded;
        }
        
        private void SetPanelData(ExternalCommandData commandData, List<FamilyData> familyData)
        {
            DockablePaneId dpid = new DockablePaneId(new Guid("209923d1-7cdc-4a1c-a4ad-1e2f9aae1dc5"));
            DockablePane dp = commandData.Application.GetDockablePane(dpid);
            FamilyManagerDockable.WPFpanel.CategoryName.Content = CategoryName;
            FamilyManagerDockable.WPFpanel.ListFamilies = familyData;
            FamilyManagerDockable.WPFpanel.GenerateGrid(familyData);

            dp.Show();
        }

        private void MessageCategory(List<FamilyData> familyData)
        {
            string temp = string.Empty;
            foreach (var item in familyData)
            {
                FamilyData fd = item;
                temp += fd.Category + "| |" + fd.FamilyName + "\n";
            }

            TaskDialog.Show("Elements", temp);
        }

        private List<FamilyData> ReadXML()
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string xmlFileName = Path.Combine(assemblyFolder, "FamilyData.xml");

            XmlSerializer serializer = new XmlSerializer(typeof(List<FamilyData>));
            FileStream fs = new FileStream(xmlFileName, FileMode.Open);
            XmlReader reader = XmlReader.Create(fs);

            List<FamilyData> familyList;

            familyList = (List<FamilyData>)serializer.Deserialize(reader);
            fs.Close();

            TaskDialog.Show("Readed Data", familyList[0].FamilyName + "||" + familyList[0].FamilyTypeDatas.Count);
            return familyList;
        }

        private List<FamilyData> GetCategoryTypes(List<FamilyData> families, string categoryName)
        {
            string temp = string.Empty;
            List<FamilyData> filteredList = new List<FamilyData>();
            foreach (var item in families)
            {
                if (item != null)
                {
                    if (item.Category == categoryName)
                    {
                        filteredList.Add(item);
                        
                        temp += item.Category + " || " + item.FamilyName + "\n";
                    }
                }
            }

            TaskDialog.Show("Filtered", temp);
            return filteredList;
        }
    }
}

