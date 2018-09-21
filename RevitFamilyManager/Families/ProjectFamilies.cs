using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Xml;
using System.Xml.Serialization;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitFamilyManager.Data;

namespace RevitFamilyManager.Families
{
    [Transaction(TransactionMode.Manual)]
    class ProjectFamilies : IExternalCommand
    {
        public string CategoryName { get; set; }

        public ProjectFamilies()
        {
            CategoryName = "Projectfamilien";
        }

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var projectTypes = GetProjectTypes(commandData);
            var familyData = ConvertFamilyData(projectTypes, commandData);

            //FamilyFolderProcess folderProcess = new FamilyFolderProcess();
            //List<FamilyData> familyData = folderProcess.GetFamilyTypes();
            SetPanelData(commandData, familyData);
            return Result.Succeeded;
        }

        private void SetPanelData(ExternalCommandData commandData, List<FamilyData> familyData)
        {
            DockablePaneId dpid = new DockablePaneId(new Guid("209923d1-7cdc-4a1c-a4ad-1e2f9aae1dc5"));
            DockablePane dp = commandData.Application.GetDockablePane(dpid);
            FamilyManagerDockable.WPFpanel.CategoryName.Content = " " + CategoryName + " ";
            FamilyManagerDockable.WPFpanel.GenerateGrid(familyData);
            dp.Show();
        }

        private List<FamilySymbol> GetProjectTypes(ExternalCommandData commandData)
        {
            List<FamilySymbol> listFamilySymbol = new List<FamilySymbol>();
            Document doc = commandData.Application.ActiveUIDocument.Document;
            List<Element> symbol = new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol)).ToList();
            var types = string.Empty;
            foreach (var item in symbol)
            {
                var sym = item as FamilySymbol;
                types += sym.Name + "|-|" + sym.FamilyName + "\n\n";
                listFamilySymbol.Add(sym);
            }

            return listFamilySymbol;
        }

        private List<FamilyData> ConvertFamilyData(List<FamilySymbol> listFamilySymbol,ExternalCommandData commandData)
        {
            List<FamilyData> listFamilyData = new List<FamilyData>();
            foreach (FamilySymbol item in listFamilySymbol)
            {
                FamilyData familyData = GetFamilyData(item);
                //MessageBox.Show(familyData.FamilyName);
                familyData.FamilyTypeDatas = new List<FamilyTypeData>();
               
                

                FamilyTypeData typeData = new FamilyTypeData();
                typeData.Name = item.Name;
                typeData.Description = item.Name + "\n" + item.Family;
                typeData.Image = null;
                
                typeData.MountType = GetMountType(item);
                typeData.Placement = GetPlacement(item);
                typeData.InstallationMedium = " --- ";
                typeData.Path = GetFamilyPath(item);
                
                typeData.CombinedTypeData = item.Name + "\n" + familyData.Category;

                //List<FamilyTypeData> listFamilyTypeData = new List<FamilyTypeData>();
                //listFamilyTypeData.Add(typeData);
                
                familyData.FamilyTypeDatas.Clear();

                if(FilterFamilyInstance(commandData, item))
                familyData.FamilyTypeDatas.Add(typeData);

                if (!string.IsNullOrEmpty(familyData.FamilyName))
                    listFamilyData.Add(familyData);
            }
            return listFamilyData;
        }

        private string GetMountType(FamilySymbol symbol)
        {
            string mountType = " --- ";
            var familyName = symbol.Family.Name;
            int indexMount = familyName.LastIndexOf("_");
            if (indexMount > 0)
            {
                if (familyName.Substring(indexMount + 1).Length == 3)
                    mountType = familyName.Substring(indexMount + 2);
            }
            return mountType;
        }

        private string GetPlacement(FamilySymbol symbol)
        {
            string placement = " --- ";
            var familyName = symbol.Family.Name;
            int indexPlacement = familyName.LastIndexOf("_");
            if (indexPlacement > 0)
            {
                if (familyName.Substring(indexPlacement + 1).Length == 3)
                    placement = familyName.Substring(indexPlacement + 1, 1);
                switch (placement)
                {
                    case "W":
                        placement = "Wand";
                        break;
                    case "D":
                        placement = "Decke";
                        break;
                    case "B":
                        placement = "Boden";
                        break;
                }
            }

            return placement;
        }

        private string GetFamilyPath(FamilySymbol symbol)
        {
            string familyPath = string.Empty;
            foreach (var item in ReadXML())
            {
                try
                {
                    if (item.FamilyName.Contains(symbol.Family.Name))
                        familyPath = item.FamilyPath;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            return familyPath;
        }

        private FamilyData GetFamilyData(FamilySymbol symbol)
        {
            FamilyData familyData = new FamilyData();
            foreach (var item in ReadXML())
            {
                try
                {
                    if (item.FamilyName.Contains(symbol.Family.Name))
                        familyData = item;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            return familyData;
        }

        private List<FamilyData> ReadXML()
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string xmlFileName = Path.Combine(assemblyFolder, "FamilyData.xml");

            XmlSerializer serializer = new XmlSerializer(typeof(List<FamilyData>));
            FileStream fs = new FileStream(xmlFileName, FileMode.Open);
            XmlReader reader = XmlReader.Create(fs);

            var familyList = (List<FamilyData>)serializer.Deserialize(reader);
            fs.Close();
            return familyList;
        }

        private bool FilterFamilyInstance(ExternalCommandData commandData, FamilySymbol symbol)
        {
            var collector = new FilteredElementCollector(commandData.Application.ActiveUIDocument.Document).OfClass(typeof(FamilyInstance));

            foreach (var item in collector)
            {
                Element el = commandData.Application.ActiveUIDocument.Document.GetElement(item.GetTypeId());
                if (symbol.Name.Contains(el.Name) || item.Name.Contains(symbol.Name))
                    return true;
            }

            return false;
        }
    }
}
