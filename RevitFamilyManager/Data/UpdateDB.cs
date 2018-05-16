using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RevitFamilyManager.Data
{
    [Transaction(TransactionMode.Manual)]
    class UpdateDB : IExternalCommand
    {
        public List<FamilyData> Families { get; set; }

        public UpdateDB()
        {
            Families = new List<FamilyData>();
        }

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Autodesk.Revit.ApplicationServices.Application app = uiapp.Application;
            Document doc = uidoc.Document;

            //TaskDialog.Show("Database Update", "It may take up to 5 minutes for Data Base update");
            FamilyFolderProcess folderProcess = new FamilyFolderProcess();
            string[] allPaths = Directory.GetDirectories(Properties.Settings.Default.RootFolder);
           
            foreach (var path in allPaths)
            {
                var devicesList = folderProcess.GetFamilyData(path);
                Families.AddRange(devicesList);
            }

            List<FamilyData> supplementedFamilyList = new List<FamilyData>();
            foreach (var item in Families)
            {
                FamilyData element = GetInternalFamilyData(item, doc);
                supplementedFamilyList.Add(element);
            }
            WriteToXML(supplementedFamilyList);
            
            return Result.Succeeded;
        }

        private FamilyData GetInternalFamilyData(FamilyData familyData, Document doc)
        {
            Family family = null;
            using (Transaction transaction = new Transaction(doc))
            {
                transaction.Start("GetFamily");
                if (doc.LoadFamily(familyData.FamilyPath, out family))
                {
                    //TaskDialog.Show("Received Family", family.Name + " * " + family.FamilyCategory);
                }
                else
                {
                    TaskDialog.Show("Warning", "Can't load family into project or family alredy exists");
                    return null;
                }
                //TODO Resolve extra families in project
                transaction.Commit();
            }
            familyData.FamilyTypeDatas = GetTypes(family, doc, familyData.FamilyPath);
            return familyData;
        }

        private List<FamilyTypeData> GetTypes(Family family, Document doc, string path)
        {
            Document familyDoc = doc.EditFamily(family);
            FamilyManager familyManager = familyDoc.FamilyManager;
            FamilyTypeSet familyTypes = familyManager.Types;
            FamilyTypeSetIterator iterator = familyTypes.ForwardIterator();
            iterator.Reset();
            
            List<FamilyTypeData> types = new List<FamilyTypeData>();
            while (iterator.MoveNext())
            {
                using (Transaction trans = new Transaction(familyDoc, "Getting Parameter"))
                {
                    trans.Start();
                    familyManager.CurrentType = iterator.Current as FamilyType;
                    FamilyType type = familyManager.CurrentType;
                    
                    FamilyParameter paramDescription;
                    if (familyManager.get_Parameter("Beschreibung") == null)
                    {
                        paramDescription = familyManager.get_Parameter("Description");
                    }
                    else
                    {
                        paramDescription = familyManager.get_Parameter("Beschreibung");//Description
                    }
                   
                    FamilyParameter paramMountType = familyManager.get_Parameter("Installationsart");
                    FamilyParameter paramPlacement = familyManager.get_Parameter("Installationsort");
                    FamilyParameter paramInstallationMedium = familyManager.get_Parameter("Installations Medium");

                    if (type != null && paramMountType!= null && paramPlacement!= null && paramInstallationMedium!=null)
                    {
                        FamilyTypeData typeData = new FamilyTypeData
                        {
                            Name = type.Name,
                            Description = type.AsString(paramDescription),
                            MountType = string.IsNullOrEmpty(type.AsString(paramMountType))? " --- ": type.AsString(paramMountType),
                            Placement = string.IsNullOrEmpty(type.AsString(paramPlacement))? " --- " :type.AsString(paramPlacement),
                            InstallationMedium = string.IsNullOrEmpty(type.AsString(paramInstallationMedium)) ? " --- " : type.AsString(paramInstallationMedium),
                            Path = path
                        };
                        types.Add(typeData);
                    }

                    //TaskDialog.Show("Params", paramDescription.Definition.Name + " * " + type.AsString(paramDescription));
                    //TaskDialog.Show("Params", paramMountType.Definition.Name + " * " + type.AsString(paramMountType));
                    //TaskDialog.Show("Params", paramPlacement.Definition.Name + " * " + type.AsString(paramPlacement));
                    //TaskDialog.Show("Params", paramInstallationMedium.Definition.Name + " * " + type.AsString(paramInstallationMedium));
                    trans.Commit();
                }
            }

            return types;
        }

        private void WriteToXML(List<FamilyData> item)
        {
            XmlSerializer ser = new XmlSerializer(typeof(List<FamilyData>));
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string xmlFileName = Path.Combine(assemblyFolder, "FamilyData.xml");
            TextWriter writer = new StreamWriter(xmlFileName);
            ser.Serialize(writer, item);
            writer.Close();
        }

        private void ReadXML()
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string xmlFileName = Path.Combine(assemblyFolder, "FamilyData.xml");

            XmlSerializer serializer = new XmlSerializer(typeof(List<FamilyData>));
            FileStream fs = new FileStream(xmlFileName, FileMode.Open);
            XmlReader reader = XmlReader.Create(fs);

            List<FamilyData> familyList;

            familyList = (List<FamilyData>) serializer.Deserialize(reader);
            fs.Close();

            TaskDialog.Show("Readed Data" , familyList[0].FamilyName + "||" + familyList[0].FamilyTypeDatas.Count);

        }

       
    }
}
