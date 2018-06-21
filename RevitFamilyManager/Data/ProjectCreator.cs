using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;

namespace RevitFamilyManager.Data
{
    [Transaction(TransactionMode.Manual)]
    class ProjectCreator : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;


            var TypeList = GetAllTypes();
            //TestTypes(TypeList);
            foreach (var type in TypeList)
            {
                CreateTypeProject(uidoc, app,  type);
            }
            
            return Result.Succeeded;
        }

        private void CreateTypeProject(UIDocument uidoc, Application app, FamilyTypeData Type)
        {
            var NewDoc = app.NewProjectDocument(UnitSystem.Metric);
            string Path = "D:\\TypesForWeb\\" + Type.Name + ".rvt";
            if (!File.Exists(Path))
            {
                PutTypeIntoProject(uidoc, NewDoc, Type);
                try
                {
                    NewDoc.SaveAs(Path);
                }
                catch (Exception e)
                {
                    TaskDialog.Show("File not created", Type.Name + e.Message);
                }
                
            }
        }

        private void PutTypeIntoProject(UIDocument uidoc, Document doc, FamilyTypeData type)
        {
            Autodesk.Revit.DB.View view = uidoc.ActiveView;
            Family family = null;
            FamilySymbol symbol = null;
            using (var transaction = new Transaction(doc, "Load Family"))
            {
                transaction.Start();
                doc.LoadFamily(type.Path, out family);
                transaction.Commit();
            }

            ISet<ElementId> familySymbolId = family.GetFamilySymbolIds();
            foreach (ElementId id in familySymbolId)
            {
                if (family.Document.GetElement(id).Name == type.Name)
                {
                    symbol = family.Document.GetElement(id) as FamilySymbol;
                }
            }

            using (var transact = new Transaction(doc, "Insert Symbol"))
            {
                transact.Start();
                XYZ point = new XYZ(0, 0, 0);
                if (symbol != null)
                {
                    symbol.Activate();
                    Level level = view.GenLevel;
                    Element host = level as Element;
                    doc.Create.NewFamilyInstance(point, symbol, host, StructuralType.NonStructural);

                }
                transact.Commit();
            }
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

        private List<FamilyTypeData> GetAllTypes()
        {
            List<FamilyTypeData> TypeList = new List<FamilyTypeData>();
            foreach (var item in ReadXML())
            {
                if (item != null)
                {
                    TypeList.AddRange(item.FamilyTypeDatas);
                }
            }
            return TypeList;
        }

        private void TestTypes(List<FamilyTypeData> TypeList)
        {
            string temp = string.Empty;
            foreach (var item  in TypeList)
            {
                temp += item.Name + "\n";
            }

            TaskDialog.Show("Test", temp);
        }
    }
}
