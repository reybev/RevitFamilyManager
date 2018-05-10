using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RevitFamilyManager
{
    public class SingleInstallEvent : IExternalEventHandler
    {
        public void Execute(UIApplication uiapp)
        {
            
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Autodesk.Revit.ApplicationServices.Application app = uiapp.Application;
            Document doc = uidoc.Document;

            string FamilyPath = Properties.Settings.Default.FamilyPath;
            string FamilyType = Properties.Settings.Default.FamilyType;
            string FamilyName = Properties.Settings.Default.FamilyName;

            //TaskDialog.Show("Event", "Selected Type " + FamilyType);
            FilteredElementCollector collector = new FilteredElementCollector(doc).OfClass(typeof(Family));
            FamilySymbol symbol = collector.FirstElement() as FamilySymbol;

            Family family = FindFamilyByName(doc, typeof(Family), FamilyPath) as Family;

            if (family == null)
            {
                using (var transaction = new Transaction(doc, "InsertTransaction"))
                {
                    transaction.Start();
                    if (!doc.LoadFamily(FamilyPath, out family))
                        {
                            TaskDialog.Show("Loading", "Unable to load " + FamilyPath);
                        }
                    transaction.Commit();
                }
            }

            ISet<ElementId> familySymbolId = family.GetFamilySymbolIds();
            foreach (ElementId id in familySymbolId)
            {
                // Get name from buffer to compare
                if (family.Document.GetElement(id).Name == FamilyType && FamilyType != null)
                    symbol = family.Document.GetElement(id) as FamilySymbol;
            }
            uidoc.PostRequestForElementTypePlacement(symbol);
        }

        private Element FindFamilyByName(Document doc, Type targetType, string familyPath)
        {
            if (familyPath != null)
            {
                int indexSlash = familyPath.LastIndexOf("\\") + 1;
                string FamilyName = familyPath.Substring(indexSlash);
                string targetName = FamilyName.Substring(0, FamilyName.Length - 4);
                return
                    new FilteredElementCollector(doc).OfClass(targetType)
                        .FirstOrDefault(e => e.Name.Equals(targetName));
            }
            TaskDialog.Show("FamilyPath Error", "Directory can't be found ");
            return null;
        }

        public string GetName()
        {
            return "External Event";
        }
    }
}
