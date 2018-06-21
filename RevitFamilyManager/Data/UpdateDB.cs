using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private const string emptyParameter = " --- ";
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

                    string paramDescription = string.Empty;
                    //FamilyParameter paramDescription;
                    try
                    {
                        paramDescription = type.AsString(familyManager.get_Parameter("Description"));
                        if (string.IsNullOrEmpty(paramDescription))
                        {
                            try
                            {
                                paramDescription = type.AsString(familyManager.get_Parameter("Beschreibung"));
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                    string paramMountType = string.Empty;
                    try
                    {
                        paramMountType = type.AsString(familyManager.get_Parameter("Installationsart"));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                    string paramPlacement = string.Empty;
                    try
                    {
                        paramPlacement = type.AsString(familyManager.get_Parameter("Installationsort"));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                    string paramInstallationMedium = string.Empty;
                    try
                    {
                        paramInstallationMedium = type.AsString(familyManager.get_Parameter("Installations Medium"));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                    string paramDiameter = string.Empty;
                    try
                    {
                        paramDiameter = type.AsString(familyManager.get_Parameter("E_Durchmesser"));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                    string paramWidth = string.Empty;
                    try
                    {
                         paramWidth = type.AsString(familyManager.get_Parameter("E_Breite"));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                    string paramHeight = string.Empty;
                    try
                    {
                        paramHeight = type.AsString(familyManager.get_Parameter("E_Hohe"));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                    string paramDepth = string.Empty;
                    try
                    {
                        paramDepth = type.AsString(familyManager.get_Parameter("E_Tiefe"));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                    string param_eBKP_H = string.Empty;
                    try
                    {
                        param_eBKP_H = type.AsString(familyManager.get_Parameter("eBKP-H"));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                    string paramBKP = string.Empty;
                    try
                    {
                        paramBKP = type.AsString(familyManager.get_Parameter("BKP"));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                    string paramManufacturer = string.Empty;
                    try
                    {
                        paramManufacturer = type.AsString(familyManager.get_Parameter("Fabrikat"));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                    string paramProduct = string.Empty;
                    try
                    {
                        paramProduct = type.AsString(familyManager.get_Parameter("Produkt"));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                    string paramProductNumber = string.Empty;
                    try
                    {
                        paramProductNumber = type.AsString(familyManager.get_Parameter("Produkte-Nr."));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                    string paramE_Number = string.Empty;
                    try
                    {
                        paramE_Number = type.AsString(familyManager.get_Parameter("E-Nummer"));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                    string paramRevitCategory = String.Empty;
                    try
                    {
                        paramRevitCategory = type.AsString(familyManager.get_Parameter("Revit Kategorie"));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                    string paramOmniClass = string.Empty;
                    try
                    {
                        paramOmniClass = type.AsString(familyManager.get_Parameter("OmniClass-Nummer"));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                    if (true/*type != null && paramMountType!= null && paramPlacement!= null && paramInstallationMedium!=null*/)
                    {
                        FamilyTypeData typeData = new FamilyTypeData
                        {
                            Name = type.Name,
                            Description = string.IsNullOrEmpty(paramDescription)? emptyParameter: paramDescription,
                            MountType = string.IsNullOrEmpty(paramMountType)? emptyParameter: paramMountType,
                            Placement = string.IsNullOrEmpty(paramPlacement)? emptyParameter :paramPlacement,
                            InstallationMedium = string.IsNullOrEmpty(paramInstallationMedium) ? emptyParameter : paramInstallationMedium,
                            Path = path,
                            CombinedTypeData = paramDescription + "\n" + type.Name,
                            Diameter = string.IsNullOrEmpty(paramDiameter)? emptyParameter: paramDiameter,
                            Width = string.IsNullOrEmpty(paramWidth) ? emptyParameter : paramWidth,
                            Hight = string.IsNullOrEmpty(paramHeight) ? emptyParameter : paramHeight,
                            Depth = string.IsNullOrEmpty(paramDepth) ? emptyParameter : paramDepth,
                            eBKP_H = string.IsNullOrEmpty(param_eBKP_H) ? emptyParameter : param_eBKP_H,
                            BKP = string.IsNullOrEmpty(paramBKP) ? emptyParameter : paramBKP,
                            Manufacturer = string.IsNullOrEmpty(paramManufacturer) ? emptyParameter : paramManufacturer,
                            Product = string.IsNullOrEmpty(paramProduct) ? emptyParameter : paramProduct,
                            ProductNumber = string.IsNullOrEmpty(paramProductNumber) ? emptyParameter : paramProductNumber,
                            E_Number = string.IsNullOrEmpty(paramE_Number) ? emptyParameter : paramE_Number,
                            RevitCategory = string.IsNullOrEmpty(paramRevitCategory) ? emptyParameter : paramRevitCategory,
                            OmniClass = string.IsNullOrEmpty(paramOmniClass) ? emptyParameter : paramOmniClass
                        };
                        types.Add(typeData);
                    }
                    trans.Commit();
                }
            }

            return types;
        }

        private void WriteToXML(List<FamilyData> item)
        {
            XmlSerializer ser = new XmlSerializer(typeof(List<FamilyData>));

            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string xmlFileNameLocal = Path.Combine(assemblyFolder, "FamilyData.xml");
            TextWriter writerLocal = new StreamWriter(xmlFileNameLocal);
            ser.Serialize(writerLocal, item);
            writerLocal.Close();
        }
    }
}
