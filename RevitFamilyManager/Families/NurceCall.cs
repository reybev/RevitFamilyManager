using System;
using System.Collections.Generic;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitFamilyManager.Data;

namespace RevitFamilyManager.Families
{
    [Transaction(TransactionMode.Manual)]
    class NurceCall : IExternalCommand
    {

        private string CategoryName { get; set; }

        public NurceCall()
        {
            CategoryName = "Notrufgeräte";
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
            FamilyManagerDockable.WPFpanel.CategoryName.Content = " " + CategoryName + " ";
            FamilyManagerDockable.WPFpanel.GenerateGrid(familyData);
            dp.Show();
        }

    }
}
